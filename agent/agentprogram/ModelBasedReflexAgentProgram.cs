using aima.net.agent.api;
using aima.net.agent.agentprogram.simplerule;
using aima.net.collections.api;

namespace aima.net.agent.agentprogram
{
    /**
    * Artificial Intelligence A Modern Approach (3rd Edition): Figure 2.12, page
    * 51.
    * 
    * function MODEL-BASED-REFLEX-AGENT(percept) returns an action
    *   persistent: state, the agent's current conception of the world state
    *               model, a description of how the next state depends on current state and action
    *               rules, a set of condition-action rules
    *               action, the most recent action, initially none
    *               
    *   state  <- UPDATE-STATE(state, action, percept, model)
    *   rule   <- RULE-MATCH(state, rules)
    *   action <- rule.ACTION
    *   return action
    *  
    * Figure 2.12 A model-based reflex agent. It keeps track of the current state
    * of the world using an internal model. It then chooses an action in the same
    * way as the reflex agent.  
    */
    public abstract class ModelBasedReflexAgentProgram<MODEL> : IAgentProgram
    {
        // persistent: state, the agent's current conception of the world state
        private DynamicState state;

        // model, a description of how the next state depends on current state and action
        private MODEL model;

        // rules, a set of condition-action rules
        private ISet<Rule> rules;

        // action, the most recent action, initially none
        private IAction action;

        public ModelBasedReflexAgentProgram()
        {
            init();
        }
         
        /// <summary>
        /// Set the agent's current conception of the world state.
        /// </summary>
        /// <param name="state">the agent's current conception of the world state.</param>
        public void SetState(DynamicState state)
        {
            this.state = state;
        }

        /// <summary>
        /// Set the program's description of how the next state depends on the state and action.
        /// </summary>
        /// <param name="model">a description of how the next state depends on the current state and action.</param>
        public void SetModel(MODEL model)
        {
            this.model = model;
        }
     
        /// <summary>
        /// Set the program's condition-action rules
        /// </summary>
        /// <param name="ruleSet">a set of condition-action rules</param>
        public void setRules(ISet<Rule> ruleSet)
        {
            rules = ruleSet;
        }

        // function MODEL-BASED-REFLEX-AGENT(percept) returns an action
        public IAction Execute(IPercept percept)
        {
            // state <- UPDATE-STATE(state, action, percept, model)
            state = updateState(state, action, percept, model);
            // rule <- RULE-MATCH(state, rules)
            Rule rule = ruleMatch(state, rules);
            // action <- rule.ACTION
            action = ruleAction(rule);
            // return action
            return action;
        }

        /// <summary>
        /// Realizations of this class should implement the init() method so that it 
        /// calls the setState(), setModel(), and setRules() method.
        /// </summary>
        protected abstract void init();

        protected abstract DynamicState updateState(DynamicState state, IAction action, IPercept percept, MODEL model);

        protected Rule ruleMatch(DynamicState state, ISet<Rule> rules)
        {
            foreach (Rule r in rules)
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
