using aima.net.agent.api;
using aima.net.collections;
using aima.net.collections.api;

namespace aima.net.agent
{
    public abstract class EnvironmentBase : IEnvironment
    {
        // Note: Use LinkedHashSet's in order to ensure order is respected as provide access to these elements via List interface.
        protected ICollection<IEnvironmentObject> envObjects = CollectionFactory.CreateFifoQueueNoDuplicates<IEnvironmentObject>();
        protected ISet<IAgent> agents = CollectionFactory.CreateSet<IAgent>();
        protected ISet<IEnvironmentView> views = CollectionFactory.CreateSet<IEnvironmentView>();
        protected IMap<IAgent, double> performanceMeasures = CollectionFactory.CreateInsertionOrderedMap<IAgent, double>();

        // Methods to be implemented by subclasses. 
        public abstract void executeAction(IAgent agent, IAction action);
        public abstract IPercept getPerceptSeenBy(IAgent anAgent);

        /// <summary> 
        /// Method for implementing dynamic environments in which not all changes are
        /// directly caused by agent action execution. The default implementation
        /// does nothing.
        /// </summary>
        public virtual void CreateExogenousChange()
        { }

        public virtual ICollection<IAgent> GetAgents()
        {
            // Return as a List but also ensures the caller cannot modify
            return CollectionFactory.CreateReadOnlyQueue<IAgent>(agents);
        }

        public virtual void AddAgent(IAgent a)
        {
            AddEnvironmentObject(a);
        }

        public virtual void RemoveAgent(IAgent a)
        {
            RemoveEnvironmentObject(a);
        }

        public virtual ICollection<IEnvironmentObject> GetEnvironmentObjects()
        {
            // Return as a List but also ensures the caller cannot modify
            return CollectionFactory.CreateReadOnlyQueue<IEnvironmentObject>(envObjects);
        }

        public virtual void AddEnvironmentObject(IEnvironmentObject eo)
        {
            envObjects.Add(eo);
            if (eo is IAgent)
            {
                IAgent a = (IAgent)eo;
                if (!agents.Contains(a))
                {
                    agents.Add(a);
                    this.notifyEnvironmentViews(a);
                }
            }
        }

        public virtual void RemoveEnvironmentObject(IEnvironmentObject eo)
        {
            envObjects.Remove(eo);
            agents.Remove(eo as IAgent);
        }

        /// <summary>
        /// Central template method for controlling agent simulation. The concrete
        /// behavior is determined by the primitive operations
        /// #getPerceptSeenBy(Agent), #executeAction(Agent, Action),
        /// and #createExogenousChange().
        /// </summary>
        public virtual void Step()
        {
            foreach (IAgent agent in agents)
            {
                if (agent.IsAlive())
                {
                    IPercept percept = getPerceptSeenBy(agent);
                    IAction anAction = agent.Execute(percept);
                    executeAction(agent, anAction);
                    NotifyEnvironmentViews(agent, percept, anAction);
                }
            }
            CreateExogenousChange();
        }

        public virtual void Step(int n)
        {
            for (int i = 0; i < n; ++i)
            {
                Step();
            }
        }

        public virtual void StepUntilDone()
        {
            while (!IsDone())
            {
                Step();
            }
        }

        public virtual bool IsDone()
        {
            foreach (IAgent agent in agents)
            {
                if (agent.IsAlive())
                {
                    return false;
                }
            }
            return true;
        }

        public virtual double GetPerformanceMeasure(IAgent forAgent)
        {
            if (!performanceMeasures.ContainsKey(forAgent))
            {
                performanceMeasures.Put(forAgent, 0.0D);
            }

            return performanceMeasures.Get(forAgent);
        }

        public virtual void AddEnvironmentView(IEnvironmentView ev)
        {
            views.Add(ev);
        }

        public virtual void RemoveEnvironmentView(IEnvironmentView ev)
        {
            views.Remove(ev);
        }

        public virtual void NotifyViews(string message)
        {
            foreach (IEnvironmentView ev in views)
            {
                ev.Notify(message);
            }
        }

        protected virtual void updatePerformanceMeasure(IAgent forAgent, double addTo)
        {
            performanceMeasures.Put(forAgent, GetPerformanceMeasure(forAgent) + addTo);
        }

        protected virtual void notifyEnvironmentViews(IAgent agent)
        {
            foreach (IEnvironmentView view in views)
            {
                view.AgentAdded(agent, this);
            }
        }

        protected virtual void NotifyEnvironmentViews(IAgent agent, IPercept percept, IAction action)
        {
            foreach (IEnvironmentView view in views)
            {
                view.AgentActed(agent, percept, action, this);
            }
        }
    }
}
