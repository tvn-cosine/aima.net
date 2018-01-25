using aima.net.agent.api;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.datastructures;

namespace aima.net.agent.agentprogram
{
    /**
    * Artificial Intelligence A Modern Approach (3rd Edition): Figure 2.7, page 47. 
    *  
    * function TABLE-DRIVEN-AGENT(percept) returns an action
    *   persistent: percepts, a sequence, initially empty
    *               table, a table of actions, indexed by percept sequences, initially fully specified
    *           
    *   append percept to end of percepts
    *   action <- LOOKUP(percepts, table)
    *   return action
    *   
    * Figure 2.7 The TABLE-DRIVEN-AGENT program is invoked for each new percept and
    * returns an action each time. It retains the complete percept sequence in
    * memory. 
    */
    public class TableDrivenAgentProgram : IAgentProgram
    {
        private ICollection<IPercept> percepts = CollectionFactory.CreateQueue<IPercept>();
        private Table<ICollection<IPercept>, string, IAction> table;

        private const string ACTION = "action";

        // persistent: percepts, a sequence, initially empty table, a table 
        // of actions, indexed by percept sequences, initially fully specified

        /// <summary>
        /// Constructs a TableDrivenAgentProgram with a table of actions, indexed by percept sequences.
        /// </summary>
        /// <param name="perceptSequenceActions">a table of actions, indexed by percept sequences</param>
        public TableDrivenAgentProgram(IMap<ICollection<IPercept>, IAction> perceptSequenceActions)
        {
            ICollection<ICollection<IPercept>> rowHeaders
                = CollectionFactory.CreateQueue<ICollection<IPercept>>(perceptSequenceActions.GetKeys());

            ICollection<string> colHeaders = CollectionFactory.CreateFifoQueue<string>();
            colHeaders.Add(ACTION);

            table = new Table<ICollection<IPercept>, string, IAction>(rowHeaders, colHeaders);

            foreach (ICollection<IPercept> row in rowHeaders)
            {
                table.Set(row, ACTION, perceptSequenceActions.Get(row));
            }
        }

        // function TABLE-DRIVEN-AGENT(percept) returns an action
        public IAction Execute(IPercept percept)
        {
            // append percept to end of percepts
            percepts.Add(percept);

            // action <- LOOKUP(percepts, table)
            // return action
            return lookupCurrentAction();
        }

        private IAction lookupCurrentAction()
        {
            IAction action = null;

            action = table.Get(percepts, ACTION);
            if (null == action)
            {
                action = DynamicAction.NO_OP;
            }

            return action;
        }
    }
}
