using System;
using System.Collections.Generic;
using aima.net.text.patternmatching.api;

namespace aima.net.text.patternmatching.ahocorasick
{
    public class PatternMatchingMachine : IPatternMatchingMachine
    {
        #region Private Members
        private readonly Node root = new Node('ϵ', true);
        #endregion

        #region Ctors
        public PatternMatchingMachine(ISet<IPattern> patterns)
        {
            if (patterns == null)
            {
                throw new ArgumentNullException("Keywords cannot be null.");
            }
            if (patterns.Count == 0)
            {
                throw new ArgumentNullException("Keywords count cannot be 0.");
            }

            constructGotoFunction(patterns);
            constructFailureFunction();
        }
        #endregion

        #region Aho Corasic State Machine
        private Node g(Node state, char a)
        {
            if (!state.GotoStateDictionary.ContainsKey(a))
            {
                if (state.IsRootNode)
                {
                    return state;
                }

                return null;
            }

            return state.GotoStateDictionary[a];
        }

        private Node f(Node state)
        {
            return state.Failure;
        }

        private ISet<IPattern> output(Node state)
        {
            return state.Output;
        }

        private void constructGotoFunction(ICollection<IPattern> k)
        {
            foreach (var y in k) enter(y);
        }

        private void constructFailureFunction()
        {
            Queue<Node> queue = new Queue<Node>();
            foreach (var a in root.GotoStateDictionary.Values)
            {
                queue.Enqueue(a);
                a.Failure = root;
            }

            while (queue.Count > 0)
            {
                Node r = queue.Dequeue();
                foreach (var s in r.GotoStateDictionary.Values)
                {
                    var a = s.Value;
                    queue.Enqueue(s);
                    var state = r.Failure;
                    while (g(state, a) == null) state = state.Failure;
                    s.Failure = g(state, a);

                    foreach (var o in s.Failure.Output)
                    {
                        if (!s.Output.Contains(o))
                        {
                            s.Output.Add(o);
                        }
                    }
                }
            }
        }

        private void enter(IPattern pattern)
        {
            Node current = root;
            foreach (char a in pattern)
            {
                Node newNode = g(current, a);
                if (newNode == null ||
                    newNode == root)
                {
                    newNode = new Node(a, false);
                    if (!current.GotoStateDictionary.ContainsKey(newNode.Value))
                    {
                        current.GotoStateDictionary[newNode.Value] = newNode;
                    }
                }
                current = newNode;
            }

            if (!current.Output.Contains(pattern))
            {
                current.Output.Add(pattern);
            }
        }
        #endregion

        public ICollection<IPattern> Match(IEnumerable<char> input)
        {
            ICollection<IPattern> allPatternsFound = new HashSet<IPattern>();
            Match(input, (sender, patternsFound, position) =>
            {
                foreach (var pattern in patternsFound)
                {
                    allPatternsFound.Add(pattern);
                }
            });

            return allPatternsFound;
        }

        public void Match(IEnumerable<char> input, PatternFoundDelegate patternMatchFound)
        {
            uint position = 0;
            Node state = root;
            foreach (char a in input)
            {
                while (g(state, a) == null) state = f(state);
                state = g(state, a);

                if (patternMatchFound != null && output(state).Count > 0)
                {
                    patternMatchFound(this, output(state), position);
                }

                ++position;
            }
        }
    }
}
