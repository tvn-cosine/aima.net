using aima.net;
using aima.net.api;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.text;
using aima.net.text.api;

namespace aima.net.nlp.parsing.grammars
{
    /**
     * Represents the most general grammatical formalism,
     * the Unrestricted (or Recrusively Enumerable) Grammar.
     * All other grammars can derive from this grammar, imposing extra
     * restrictions.
     * @author Jonathon
     *
     */
    public class ProbUnrestrictedGrammar : ProbabilisticGrammar
    {
        // types of grammars
        public const int UNRESTRICTED = 0;
        public const int CONTEXT_SENSITIVE = 1;
        public const int CONTEXT_FREE = 2;
        public const int REGULAR = 3;
        public const int CNFGRAMMAR = 4;
        public const int PROB_CONTEXT_FREE = 5;

        public ICollection<Rule> rules;
        public ICollection<string> vars;
        public ICollection<string> terminals;
        public int type;

        // default constructor. has no rules
        public ProbUnrestrictedGrammar()
        {
            type = 0;
            rules = CollectionFactory.CreateQueue<Rule>();
            vars = CollectionFactory.CreateQueue<string>();
            terminals = CollectionFactory.CreateQueue<string>();
        }

        /**
         * Add a number of rules at once, testing each in turn
         * for validity, and then testing the batch for probability validity.
         * @param ruleList
         * @return true if rules are valid and incorporated into the grammar. false, otherwise
         */
        public virtual bool addRules(ICollection<Rule> ruleList)
        {
            foreach (Rule aRuleList in ruleList)
            {
                if (!validRule(aRuleList))
                    return false;
            }
            if (!validateRuleProbabilities(ruleList))
                return false;
            rules = ruleList;
            updateVarsAndTerminals();
            return true;
        }

        /**
         * Add a single rule the grammar, testing it for structural 
         * and probability validity.
         * @param rule
         * @return true if rule is incorporated. false, otherwise
         */
        // TODO: More sophisticated probability distribution management
        public virtual bool addRule(Rule rule)
        {
            if (validRule(rule))
            {
                rules.Add(rule);
                updateVarsAndTerminals(rule);
                return true;
            }
            else
            {
                return false;
            }
        }

        /**
         * For a set of rules, test whether each batch of rules with the same 
         * LHS have their probabilities sum to exactly 1.0
         * @param ruleList
         * @return true if the probabilities are valid. false, otherwise
         */
        public virtual bool validateRuleProbabilities(ICollection<Rule> ruleList)
        {
            float probTotal = 0;
            foreach (string var in vars)
            {
                for (int j = 0; j < ruleList.Size(); j++)
                {
                    // reset probTotal at start
                    if (j == 0)
                        probTotal = (float)0.0;
                    if (ruleList.Get(j).lhs.Get(0).Equals(var))
                        probTotal += ruleList.Get(j).PROB;
                    // check probTotal hasn't exceed max
                    if (probTotal > 1.0)
                        return false;
                    // check we have correct probability total
                    if (j == ruleList.Size() - 1 && probTotal != (float)1.0)
                        return false;
                }
            }
            return true;
        }

        /**
         * Test validity of the LHS and RHS of grammar rule.
         * In unrestricted grammar, the only invalid rule type is
         * a rule with a null LHS.
         * @param r ( a rule )
         * @return true, if rule has valid form. false, otherwise
         */
        public virtual bool validRule(Rule r)
        {
            return r.lhs != null && r.lhs.Size() > 0;
        }

        /** 
         * Whenever a new rule is added to the grammar, we want to 
         * update the list of variables and terminals with any new grammar symbols
         */
        public virtual void updateVarsAndTerminals()
        {
            if (rules == null)
            {
                vars = CollectionFactory.CreateQueue<string>();
                terminals = CollectionFactory.CreateQueue<string>();
                return;
            }
            foreach (Rule r in rules)
                updateVarsAndTerminals(r);    // update the variables and terminals for this rule
        }

        /**
         * Update variable and terminal lists with a single rule's symbols,
         * if there a new symbols
         * @param r
         */
        public virtual void updateVarsAndTerminals(Rule r)
        {
            // check lhs for new terminals or variables
            for (int j = 0; j < r.lhs.Size(); j++)
            {
                if (isVariable(r.lhs.Get(j)) && !vars.Contains(r.lhs.Get(j)))
                    vars.Add(r.lhs.Get(j));
                else if (isTerminal(r.lhs.Get(j)) && !terminals.Contains(r.lhs.Get(j)))
                    terminals.Add(r.lhs.Get(j));
            }
            // for rhs we must check that this isn't a null-rule
            if (r.rhs != null)
            {
                // check rhs for new terminals or variables
                for (int j = 0; j < r.rhs.Size(); j++)
                {
                    if (isVariable(r.rhs.Get(j)) && !vars.Contains(r.rhs.Get(j)))
                        vars.Add(r.rhs.Get(j));
                    else if (isTerminal(r.rhs.Get(j)) && !terminals.Contains(r.rhs.Get(j)))
                        terminals.Add(r.rhs.Get(j));
                }
            }
            // maintain sorted lists
            vars.Sort(new List<string>.Comparer());
            terminals.Sort(new List<string>.Comparer());
        }


        /**
         * Check if we have a variable, as they are uppercase strings.
         * @param s
         * @return
         */
        public static bool isVariable(string s)
        {
            for (int i = 0; i < s.Length;++i)
            {
                if (!char.IsUpper(s[i]))
                    return false;
            }
            return true;
        }

        /** 
         * Check if we have a terminal, as they are lowercase strings
         * @param s
         * @return true, if string must be a terminal. false, otherwise
         */
        public static bool isTerminal(string s)
        {
            for (int i = 0; i < s.Length;++i)
            {
                if (!char.IsLower(s[i]))
                    return false;
            }
            return true;
        }
         
        public override string ToString()
        {
            IStringBuilder output = TextFactory.CreateStringBuilder();

            output.Append("Variables:  ");
            foreach (var var in vars)
            {
                output.Append(var).Append(", ");
            }

            output.Append('\n');
            output.Append("Terminals:  ");
            foreach (var terminal in terminals)
            {
                output.Append(terminal).Append(", ");
            }

            output.Append('\n');
            foreach (var rule in rules)
            {
                output.Append(rule.ToString()).Append('\n');
            }
            return output.ToString();
        }
    }
}
