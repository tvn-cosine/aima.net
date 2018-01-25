using aima.net.agent.api;

namespace aima.net.agent
{
    public delegate STATE PerceptToStateFunction<PERCEPT, STATE>(PERCEPT percept) where PERCEPT : IPercept;
}
