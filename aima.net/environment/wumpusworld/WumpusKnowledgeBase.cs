using aima.net;
using aima.net.agent.api;
using aima.net.collections;
using aima.net.collections.api;
using aima.net.exceptions;
using aima.net.environment.wumpusworld.action;
using aima.net.logic.propositional.inference;
using aima.net.logic.propositional.kb;
using aima.net.logic.propositional.parsing.ast;
using aima.net.api;
using aima.net.text.api;
using aima.net.text;

namespace aima.net.environment.wumpusworld
{
    /// <summary>
    /// A Knowledge base tailored to the Wumpus World environment. 
    /// </summary>
    public class WumpusKnowledgeBase : KnowledgeBase
    {
        public const string LOCATION = "L";
        public const string BREEZE = "B";
        public const string STENCH = "S";
        public const string PIT = "P";
        public const string WUMPUS = "W";
        public const string WUMPUS_ALIVE = "WumpusAlive";
        public const string HAVE_ARROW = "HaveArrow";
        public static readonly string FACING_NORTH = AgentPosition.Orientation.FACING_NORTH.ToString();
        public static readonly string FACING_SOUTH = AgentPosition.Orientation.FACING_SOUTH.ToString();
        public static readonly string FACING_EAST = AgentPosition.Orientation.FACING_EAST.ToString();
        public static readonly string FACING_WEST = AgentPosition.Orientation.FACING_WEST.ToString();
        public const string PERCEPT_STENCH = "Stench";
        public const string PERCEPT_BREEZE = "Breeze";
        public const string PERCEPT_GLITTER = "Glitter";
        public const string PERCEPT_BUMP = "Bump";
        public const string PERCEPT_SCREAM = "Scream";
        public const string ACTION_FORWARD = Forward.FORWARD_ACTION_NAME;
        public const string ACTION_SHOOT = Shoot.SHOOT_ACTION_NAME;
        public const string ACTION_TURN_LEFT = TurnLeft.TURN_LEFT_ACTION_NAME;
        public const string ACTION_TURN_RIGHT = TurnRight.TURN_RIGHT_ACTION_NAME;
        public const string OK_TO_MOVE_INTO = "OK";
       
        private int caveXDimension;
        private int caveYDimension;
        private DPLL dpll;

        public WumpusKnowledgeBase(int caveXandYDimensions)
            : this(new OptimizedDPLL(), caveXandYDimensions)
        { }

        /**
         * Create a Knowledge Base that contains the atemporal "wumpus physics" and
         * temporal rules with time zero.
         * 
         * @param dpll
         *        the dpll implementation to use for answering 'ask' queries.
         * @param caveXandYDimensions
         *            x and y dimensions of the wumpus world's cave.
         * 
         */
        public WumpusKnowledgeBase(DPLL dpll, int caveXandYDimensions)
            : base()
        {
            this.dpll = dpll;

            this.caveXDimension = caveXandYDimensions;
            this.caveYDimension = caveXandYDimensions;

            //
            // 7.7.1 - The current state of the World
            // The agent knows that the starting square contains no pit
            tell(new ComplexSentence(Connective.NOT, newSymbol(PIT, 1, 1)));
            // and no wumpus.
            tell(new ComplexSentence(Connective.NOT, newSymbol(WUMPUS, 1, 1)));

            // Atemporal rules about breeze and stench
            // For each square, the agent knows that the square is breezy
            // if and only if a neighboring square has a pit; and a square
            // is smelly if and only if a neighboring square has a wumpus.
            for (int y = 1; y <= caveYDimension; y++)
            {
                for (int x = 1; x <= caveXDimension; x++)
                {
                    ICollection<Sentence> pitsIn = CollectionFactory.CreateQueue<Sentence>();
                    ICollection<Sentence> wumpsIn = CollectionFactory.CreateQueue<Sentence>();

                    if (x > 1)
                    { // West room exists
                        pitsIn.Add(newSymbol(PIT, x - 1, y));
                        wumpsIn.Add(newSymbol(WUMPUS, x - 1, y));
                    }
                    if (y < caveYDimension)
                    { // North room exists
                        pitsIn.Add(newSymbol(PIT, x, y + 1));
                        wumpsIn.Add(newSymbol(WUMPUS, x, y + 1));
                    }
                    if (x < caveXDimension)
                    { // East room exists
                        pitsIn.Add(newSymbol(PIT, x + 1, y));
                        wumpsIn.Add(newSymbol(WUMPUS, x + 1, y));
                    }
                    if (y > 1)
                    { // South room exists
                        pitsIn.Add(newSymbol(PIT, x, y - 1));
                        wumpsIn.Add(newSymbol(WUMPUS, x, y - 1));
                    }

                    tell(new ComplexSentence(newSymbol(BREEZE, x, y), Connective.BICONDITIONAL, Sentence.newDisjunction(pitsIn)));
                    tell(new ComplexSentence(newSymbol(STENCH, x, y), Connective.BICONDITIONAL, Sentence.newDisjunction(wumpsIn)));
                }
            }

            // The agent also knows there is exactly one wumpus. This is represented
            // in two parts. First, we have to say that there is at least one wumpus
            ICollection<Sentence> wumpsAtLeast = CollectionFactory.CreateQueue<Sentence>();
            for (int x = 1; x <= caveXDimension; x++)
            {
                for (int y = 1; y <= caveYDimension; y++)
                {
                    wumpsAtLeast.Add(newSymbol(WUMPUS, x, y));
                }
            }
            tell(Sentence.newDisjunction(wumpsAtLeast));

            // Then, we have to say that there is at most one wumpus.
            // For each pair of locations, we add a sentence saying
            // that at least one of them must be wumpus-free.
            int numRooms = (caveXDimension * caveYDimension);
            for (int i = 0; i < numRooms; i++)
            {
                for (int j = i + 1; j < numRooms; j++)
                {
                    tell(new ComplexSentence(Connective.OR,
                            new ComplexSentence(Connective.NOT, newSymbol(WUMPUS, (i / caveXDimension) + 1, (i % caveYDimension) + 1)),
                            new ComplexSentence(Connective.NOT, newSymbol(WUMPUS, (j / caveXDimension) + 1, (j % caveYDimension) + 1))));
                }
            }
        }

        public AgentPosition askCurrentPosition(int t)
        {
            int locX = -1, locY = -1;
            for (int x = 1; x <= getCaveXDimension() && locX == -1; x++)
            {
                for (int y = 1; y <= getCaveYDimension() && locY == -1; y++)
                {
                    if (ask(newSymbol(LOCATION, t, x, y)))
                    {
                        locX = x;
                        locY = y;
                    }
                }
            }
            if (locX == -1 || locY == -1)
            {
                throw new IllegalStateException("Inconsistent KB, unable to determine current room position.");
            }
            AgentPosition current = null;
            if (ask(newSymbol(FACING_NORTH, t)))
            {
                current = new AgentPosition(locX, locY, AgentPosition.Orientation.FACING_NORTH);
            }
            else if (ask(newSymbol(FACING_SOUTH, t)))
            {
                current = new AgentPosition(locX, locY, AgentPosition.Orientation.FACING_SOUTH);
            }
            else if (ask(newSymbol(FACING_EAST, t)))
            {
                current = new AgentPosition(locX, locY, AgentPosition.Orientation.FACING_EAST);
            }
            else if (ask(newSymbol(FACING_WEST, t)))
            {
                current = new AgentPosition(locX, locY, AgentPosition.Orientation.FACING_WEST);
            }
            else
            {
                throw new IllegalStateException("Inconsistent KB, unable to determine current room orientation.");
            }

            return current;
        }

        // safe <- {[x, y] : ASK(KB, OK<sup>t</sup><sub>x,y</sub>) = true}
        public ISet<Room> askSafeRooms(int t)
        {
            ISet<Room> safe = CollectionFactory.CreateSet<Room>();
            for (int x = 1; x <= getCaveXDimension(); x++)
            {
                for (int y = 1; y <= getCaveYDimension(); y++)
                {
                    if (ask(newSymbol(OK_TO_MOVE_INTO, t, x, y)))
                    {
                        safe.Add(new Room(x, y));
                    }
                }
            }
            return safe;
        }

        public bool askGlitter(int t)
        {
            return ask(newSymbol(PERCEPT_GLITTER, t));
        }

        // unvisited <- {[x, y] : ASK(KB, L<sup>t'</sup><sub>x,y</sub>) = false for all t' &le; t}
        public ISet<Room> askUnvisitedRooms(int t)
        {
            ISet<Room> unvisited = CollectionFactory.CreateSet<Room>();

            for (int x = 1; x <= getCaveXDimension(); x++)
            {
                for (int y = 1; y <= getCaveYDimension(); y++)
                {
                    for (int tPrime = 0; tPrime <= t; tPrime++)
                    {
                        if (ask(newSymbol(LOCATION, tPrime, x, y)))
                        {
                            break; // i.e. is not false for all t' <= t
                        }
                        if (tPrime == t)
                        {
                            unvisited.Add(new Room(x, y)); // i.e. is false for all t' <= t
                        }
                    }
                }
            }

            return unvisited;
        }

        public bool askHaveArrow(int t)
        {
            return ask(newSymbol(HAVE_ARROW, t));
        }

        // possible_wumpus <- {[x, y] : ASK(KB, ~W<sub>x,y</sub>) = false}
        public ISet<Room> askPossibleWumpusRooms(int t)
        {
            ISet<Room> possible = CollectionFactory.CreateSet<Room>();

            for (int x = 1; x <= getCaveXDimension(); x++)
            {
                for (int y = 1; y <= getCaveYDimension(); y++)
                {
                    if (!ask(new ComplexSentence(Connective.NOT, newSymbol(WUMPUS, x, y))))
                    {
                        possible.Add(new Room(x, y));
                    }
                }
            }

            return possible;
        }

        // not_unsafe <- {[x, y] : ASK(KB, ~OK<sup>t</sup><sub>x,y</sub>) = false}
        public ISet<Room> askNotUnsafeRooms(int t)
        {
            ISet<Room> notUnsafe = CollectionFactory.CreateSet<Room>();

            for (int x = 1; x <= getCaveXDimension(); x++)
            {
                for (int y = 1; y <= getCaveYDimension(); y++)
                {
                    if (!ask(new ComplexSentence(Connective.NOT, newSymbol(OK_TO_MOVE_INTO, t, x, y))))
                    {
                        notUnsafe.Add(new Room(x, y));
                    }
                }
            }

            return notUnsafe;
        }

        public bool askOK(int t, int x, int y)
        {
            return ask(newSymbol(OK_TO_MOVE_INTO, t, x, y));
        }

        public bool ask(Sentence query)
        {
            return dpll.isEntailed(this, query);
        }

        public int getCaveXDimension()
        {
            return caveXDimension;
        }

        public void setCaveXDimension(int caveXDimension)
        {
            this.caveXDimension = caveXDimension;
        }

        public int getCaveYDimension()
        {
            return caveYDimension;
        }

        public void setCaveYDimension(int caveYDimension)
        {
            this.caveYDimension = caveYDimension;
        }

        /**
         * Add to KB sentences that describe the action a
         * 
         * @param a
         *            action that must be added to KB
         * @param time
         *            current time
         */
        public void makeActionSentence(IAction a, int t)
        {
            if (a is Climb)
            {
                tell(newSymbol(Climb.CLIMB_ACTION_NAME, t));
            }
            else
            {
                tell(new ComplexSentence(Connective.NOT, newSymbol(Climb.CLIMB_ACTION_NAME, t)));
            }
            if (a is Forward)
            {
                tell(newSymbol(Forward.FORWARD_ACTION_NAME, t));
            }
            else
            {
                tell(new ComplexSentence(Connective.NOT, newSymbol(Forward.FORWARD_ACTION_NAME, t)));
            }
            if (a is Grab)
            {
                tell(newSymbol(Grab.GRAB_ACTION_NAME, t));
            }
            else
            {
                tell(new ComplexSentence(Connective.NOT, newSymbol(Grab.GRAB_ACTION_NAME, t)));
            }
            if (a is Shoot)
            {
                tell(newSymbol(Shoot.SHOOT_ACTION_NAME, t));
            }
            else
            {
                tell(new ComplexSentence(Connective.NOT, newSymbol(Shoot.SHOOT_ACTION_NAME, t)));
            }
            if (a is TurnLeft)
            {
                tell(newSymbol(TurnLeft.TURN_LEFT_ACTION_NAME, t));
            }
            else
            {
                tell(new ComplexSentence(Connective.NOT, newSymbol(TurnLeft.TURN_LEFT_ACTION_NAME, t)));
            }
            if (a is TurnRight)
            {
                tell(newSymbol(TurnRight.TURN_RIGHT_ACTION_NAME, t));
            }
            else
            {
                tell(new ComplexSentence(Connective.NOT, newSymbol(TurnRight.TURN_RIGHT_ACTION_NAME, t)));
            }
        }

        /**
         * Add to KB sentences that describe the perception p
         * (only about the current time).
         * 
         * @param p
         *            perception that must be added to KB
         * @param time
         *            current time
         */
        public void makePerceptSentence(AgentPercept p, int time)
        {
            if (p.isStench())
            {
                tell(newSymbol(PERCEPT_STENCH, time));
            }
            else
            {
                tell(new ComplexSentence(Connective.NOT, newSymbol(PERCEPT_STENCH, time)));
            }

            if (p.isBreeze())
            {
                tell(newSymbol(PERCEPT_BREEZE, time));
            }
            else
            {
                tell(new ComplexSentence(Connective.NOT, newSymbol(PERCEPT_BREEZE, time)));
            }

            if (p.isGlitter())
            {
                tell(newSymbol(PERCEPT_GLITTER, time));
            }
            else
            {
                tell(new ComplexSentence(Connective.NOT, newSymbol(PERCEPT_GLITTER, time)));
            }

            if (p.isBump())
            {
                tell(newSymbol(PERCEPT_BUMP, time));
            }
            else
            {
                tell(new ComplexSentence(Connective.NOT, newSymbol(PERCEPT_BUMP, time)));
            }

            if (p.isScream())
            {
                tell(newSymbol(PERCEPT_SCREAM, time));
            }
            else
            {
                tell(new ComplexSentence(Connective.NOT, newSymbol(PERCEPT_SCREAM, time)));
            }
        }

        /**
         * TELL the KB the temporal "physics" sentences for time t
         * 
         * @param t
         *            current time step.
         */
        public void tellTemporalPhysicsSentences(int t)
        {
            if (t == 0)
            {
                // temporal rules at time zero
                tell(newSymbol(LOCATION, 0, 1, 1));
                tell(newSymbol(FACING_EAST, 0));
                tell(newSymbol(HAVE_ARROW, 0));
                tell(newSymbol(WUMPUS_ALIVE, 0));
            }

            // We can connect stench and breeze percepts directly
            // to the properties of the squares where they are experienced
            // through the location fluent as follows. For any time step t
            // and any square [x,y], we assert
            for (int x = 1; x <= caveXDimension; x++)
            {
                for (int y = 1; y <= caveYDimension; y++)
                {
                    tell(new ComplexSentence(
                            newSymbol(LOCATION, t, x, y),
                            Connective.IMPLICATION,
                            new ComplexSentence(newSymbol(PERCEPT_BREEZE, t), Connective.BICONDITIONAL, newSymbol(BREEZE, x, y))));

                    tell(new ComplexSentence(
                            newSymbol(LOCATION, t, x, y),
                            Connective.IMPLICATION,
                            new ComplexSentence(newSymbol(PERCEPT_STENCH, t), Connective.BICONDITIONAL, newSymbol(STENCH, x, y))));
                }
            }

            //
            // Successor state axioms (dependent on location)	
            for (int x = 1; x <= caveXDimension; x++)
            {
                for (int y = 1; y <= caveYDimension; y++)
                {

                    // Location
                    ICollection<Sentence> locDisjuncts = CollectionFactory.CreateQueue<Sentence>();
                    locDisjuncts.Add(new ComplexSentence(
                                            newSymbol(LOCATION, t, x, y),
                                            Connective.AND,
                                            new ComplexSentence(
                                                    new ComplexSentence(Connective.NOT, newSymbol(ACTION_FORWARD, t)),
                                                    Connective.OR,
                                                    newSymbol(PERCEPT_BUMP, t + 1))));
                    if (x > 1)
                    { // West room is possible
                        locDisjuncts.Add(new ComplexSentence(
                                                newSymbol(LOCATION, t, x - 1, y),
                                                Connective.AND,
                                                new ComplexSentence(
                                                        newSymbol(FACING_EAST, t),
                                                        Connective.AND,
                                                        newSymbol(ACTION_FORWARD, t))));
                    }
                    if (y < caveYDimension)
                    { // North room is possible
                        locDisjuncts.Add(new ComplexSentence(
                                                newSymbol(LOCATION, t, x, y + 1),
                                                Connective.AND,
                                                new ComplexSentence(
                                                        newSymbol(FACING_SOUTH, t),
                                                        Connective.AND,
                                                        newSymbol(ACTION_FORWARD, t))));
                    }
                    if (x < caveXDimension)
                    { // East room is possible	
                        locDisjuncts.Add(new ComplexSentence(
                                                newSymbol(LOCATION, t, x + 1, y),
                                                Connective.AND,
                                                new ComplexSentence(
                                                        newSymbol(FACING_WEST, t),
                                                        Connective.AND,
                                                        newSymbol(ACTION_FORWARD, t))));
                    }
                    if (y > 1)
                    { // South room is possible
                        locDisjuncts.Add(new ComplexSentence(
                                                newSymbol(LOCATION, t, x, y - 1),
                                                Connective.AND,
                                                new ComplexSentence(
                                                        newSymbol(FACING_NORTH, t),
                                                        Connective.AND,
                                                        newSymbol(ACTION_FORWARD, t))));
                    }

                    tell(new ComplexSentence(
                                newSymbol(LOCATION, t + 1, x, y),
                                Connective.BICONDITIONAL,
                                Sentence.newDisjunction(locDisjuncts)));

                    // The most important question for the agent is whether
                    // a square is OK to move into, that is, the square contains
                    // no pit nor live wumpus.
                    tell(new ComplexSentence(
                                newSymbol(OK_TO_MOVE_INTO, t, x, y),
                                Connective.BICONDITIONAL,
                                new ComplexSentence(
                                        new ComplexSentence(Connective.NOT, newSymbol(PIT, x, y)),
                                        Connective.AND,
                                        new ComplexSentence(Connective.NOT,
                                                new ComplexSentence(
                                                        newSymbol(WUMPUS, x, y),
                                                        Connective.AND,
                                                        newSymbol(WUMPUS_ALIVE, t))))));
                }
            }

            //
            // Successor state axioms (independent of location)

            // Rules about current orientation
            // Facing North
            tell(new ComplexSentence(
                        newSymbol(FACING_NORTH, t + 1),
                        Connective.BICONDITIONAL,
                        Sentence.newDisjunction(
                                new ComplexSentence(newSymbol(FACING_WEST, t), Connective.AND, newSymbol(ACTION_TURN_RIGHT, t)),
                                new ComplexSentence(newSymbol(FACING_EAST, t), Connective.AND, newSymbol(ACTION_TURN_LEFT, t)),
                                new ComplexSentence(newSymbol(FACING_NORTH, t),
                                        Connective.AND,
                                        new ComplexSentence(
                                                    new ComplexSentence(Connective.NOT, newSymbol(ACTION_TURN_LEFT, t)),
                                                    Connective.AND,
                                                    new ComplexSentence(Connective.NOT, newSymbol(ACTION_TURN_RIGHT, t))))
                                )));
            // Facing South
            tell(new ComplexSentence(
                    newSymbol(FACING_SOUTH, t + 1),
                    Connective.BICONDITIONAL,
                    Sentence.newDisjunction(
                            new ComplexSentence(newSymbol(FACING_WEST, t), Connective.AND, newSymbol(ACTION_TURN_LEFT, t)),
                            new ComplexSentence(newSymbol(FACING_EAST, t), Connective.AND, newSymbol(ACTION_TURN_RIGHT, t)),
                            new ComplexSentence(newSymbol(FACING_SOUTH, t),
                                    Connective.AND,
                                    new ComplexSentence(
                                            new ComplexSentence(Connective.NOT, newSymbol(ACTION_TURN_LEFT, t)),
                                            Connective.AND,
                                            new ComplexSentence(Connective.NOT, newSymbol(ACTION_TURN_RIGHT, t))))
                            )));
            // Facing East
            tell(new ComplexSentence(
                    newSymbol(FACING_EAST, t + 1),
                    Connective.BICONDITIONAL,
                    Sentence.newDisjunction(
                            new ComplexSentence(newSymbol(FACING_NORTH, t), Connective.AND, newSymbol(ACTION_TURN_RIGHT, t)),
                            new ComplexSentence(newSymbol(FACING_SOUTH, t), Connective.AND, newSymbol(ACTION_TURN_LEFT, t)),
                            new ComplexSentence(newSymbol(FACING_EAST, t),
                                    Connective.AND,
                                    new ComplexSentence(
                                            new ComplexSentence(Connective.NOT, newSymbol(ACTION_TURN_LEFT, t)),
                                            Connective.AND,
                                            new ComplexSentence(Connective.NOT, newSymbol(ACTION_TURN_RIGHT, t))))
                            )));
            // Facing West
            tell(new ComplexSentence(
                    newSymbol(FACING_WEST, t + 1),
                    Connective.BICONDITIONAL,
                    Sentence.newDisjunction(
                            new ComplexSentence(newSymbol(FACING_NORTH, t), Connective.AND, newSymbol(ACTION_TURN_LEFT, t)),
                            new ComplexSentence(newSymbol(FACING_SOUTH, t), Connective.AND, newSymbol(ACTION_TURN_RIGHT, t)),
                            new ComplexSentence(newSymbol(FACING_WEST, t),
                                    Connective.AND,
                                    new ComplexSentence(
                                            new ComplexSentence(Connective.NOT, newSymbol(ACTION_TURN_LEFT, t)),
                                            Connective.AND,
                                            new ComplexSentence(Connective.NOT, newSymbol(ACTION_TURN_RIGHT, t))))
                            )));

            // Rule about the arrow
            tell(new ComplexSentence(
                        newSymbol(HAVE_ARROW, t + 1),
                        Connective.BICONDITIONAL,
                        new ComplexSentence(
                                newSymbol(HAVE_ARROW, t),
                                Connective.AND,
                                new ComplexSentence(Connective.NOT, newSymbol(ACTION_SHOOT, t)))));

            // Rule about wumpus (dead or alive)
            tell(new ComplexSentence(
                    newSymbol(WUMPUS_ALIVE, t + 1),
                    Connective.BICONDITIONAL,
                    new ComplexSentence(
                            newSymbol(WUMPUS_ALIVE, t),
                            Connective.AND,
                            new ComplexSentence(Connective.NOT, newSymbol(PERCEPT_SCREAM, t + 1)))));
        }

        public override string ToString()
        {
            ICollection<Sentence> sentences = getSentences();
            if (sentences.Size() == 0)
            {
                return "";
            }
            else
            {
                bool first = true;
                IStringBuilder sb = TextFactory.CreateStringBuilder();
                foreach (Sentence s in sentences)
                {
                    if (!first)
                    {
                        sb.Append("\n");
                    }
                    sb.Append(s.ToString());
                    first = false;
                }
                return sb.ToString();
            }
        }

        public PropositionSymbol newSymbol(string prefix, int timeStep)
        {
            return new PropositionSymbol(prefix + "_" + timeStep);
        }

        public PropositionSymbol newSymbol(string prefix, int x, int y)
        {
            return new PropositionSymbol(prefix + "_" + x + "_" + y);
        }

        public PropositionSymbol newSymbol(string prefix, int timeStep, int x, int y)
        {
            return newSymbol(newSymbol(prefix, timeStep).ToString(), x, y);
        }
    }

}
