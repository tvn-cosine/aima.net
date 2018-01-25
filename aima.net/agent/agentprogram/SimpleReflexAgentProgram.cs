using aima.net.agent.api;
using aima.net.agent.agentprogram.simplerule;
using aima.net.collections.api;
using aima.net.util;

namespace aima.net.agent.agentprogram
{
     /**
     * Artificial Intelligence A Modern Approach (3rd Edition): Figure 2.10, page
     * 49.
     *  
     * function SIMPLE-RELEX-AGENT(percept) returns an action
     *   persistent: rules, a set of condition-action rules
     *   
     *   state  <- INTERPRET-INPUT(percept);
     *   rule   <- RULE-MATCH(state, rules);
     *   action <- rule.ACTION;
     *   return action
     * 
     * Figure 2.10 A simple reflex agent. It acts according to a rule whose
     * condition matches the current state, as defined by the percept. 
     */
    public class SimpleReflexAgentProgram : IAgentProgram
    { 
        // persistent: rules, a set of condition-action rules
        private ISet<Rule> rules;
         
        /// <summary>
        /// Constructs a SimpleReflexAgentProgram with a set of condition-action rules.
        /// </summary>
        /// <param name="ruleSet">a set of condition-action rules</param>
        public SimpleReflexAgentProgram(ISet<Rule> ruleSet)
        {
            rules = ruleSet;
        }

        // function SIMPLE-RELEX-AGENT(percept) returns an action 
        public IAction Execute(IPercept percept)
        {

            // state <- INTERPRET-INPUT(percept);
            ObjectWithDynamicAttributes state = interpretInput(percept);
            // rule <- RULE-MATCH(state, rules);
            Rule rule = ruleMatch(state, rules);
            // action <- rule.ACTION;
            // return action
            return ruleAction(rule);
        }
         
        protected ObjectWithDynamicAttributes interpretInput(IPercept p)
        {
            return (DynamicPercept)p;
        }

        protected Rule ruleMatch(ObjectWithDynamicAttributes state, ISet<Rule> rulesSet)
        {
            foreach (Rule r in rulesSet)
            {
                if (r.evaluate(state))
                {
                    return r;
                }
            }
            return null;
        }

        protected IAction ruleAction(Rule r)
        {
            return null == r ? DynamicAction.NO_OP : r.getAction();
        }
    } 
}
