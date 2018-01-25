using aima.net.collections;
using aima.net.collections.api;

namespace aima.net.nlp.parsing.grammars
{
    /**
     * The CYK algorithm exploits the structure of grammars in 
     * Chomsky Normal Form in order to parse efficiently. 
     * 
     * A context-free grammar is in Chomsky Normal Form (CNF) if each
     * rule has one of the following forms:
     * 
     * 1. A -> BC,
     * 2. A -> a,
     * 3. S -> null, where B,C are non-start variables (V - {S}), 
     * and a is a terminal.
     * 
     * @author Jonathon
     *
     */
    public class ProbCNFGrammar : ProbContextFreeGrammar
    { 
        // TODO: Implement conversion from ContextFree to ChomskyNormalForm

        // default constructor
        public ProbCNFGrammar()
        {
            type = 4;
            rules = CollectionFactory.CreateQueue<Rule>();
        }

        public ProbCNFGrammar(ProbCNFGrammar grammar)
        {
            type = 4;
            rules = grammar.rules;
        }

        /**
         * Add a ruleList as the grammar's rule list if all rules in it pass
         * both the restrictions of the Context-free grammar, and all rules 
         * or in Chomsky-Normal-Form
         */
        public override bool addRules(ICollection<Rule> ruleList)
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
         * Add a rule to the grammar's rule list if it passes
         * both the restrictions of the Context-free grammar, and the rule is
         * in Chomsky Normal Form.
         */
        public override bool addRule(Rule r)
        {
            if (!validRule(r))
                return false;
            rules.Add(r);
            updateVarsAndTerminals(r);
            return true;
        }

        /**
         * For a grammar rule to be valid in a context-free grammar, 
         * all the restrictions of the context-free grammar must hold,
         * and the rule must be in one of three forms.
         * 
         * 1. A -> BC,
         * 2. A -> a,
         * 3. S -> null, where B,C are non-start variables (V - {S}), 
         * and a is a terminal.
         * 
         * @param r,  a rule
         * @return true, if rule is in CNF. false, otherwise
         */
        public override bool validRule(Rule r)
        {
            if (!base.validRule(r))
                return false;
            // 3. rhs is null
            if (r.rhs == null || r.rhs.Size() == 0)
                return true;
            // 2. rhs is a terminal (a)
            else if (r.rhs.Size() == 1 && isTerminal(r.rhs.Get(0)))
                return true;
            // 1. rhs is 2 variables (BC)
            else if (r.rhs.Size() == 2 && isVariable(r.rhs.Get(0)) && isVariable(r.rhs.Get(1)))
                return true;
            // rule is not in one of the 3 CNF forms
            return false;
        }

    } // end of CNFGrammar()
}
