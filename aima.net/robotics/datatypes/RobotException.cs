using aima.net.exceptions;

namespace aima.net.robotics.datatypes
{
    /// <summary>
    /// A RobotException may be thrown by a class implementing IMclRobot 
    /// during any actions invoked on the robot in case something has gone 
    /// wrong and the localization should be halted.
    /// </summary>
    public class RobotException : Exception
    {
        public RobotException(string message)
            : base(message)
        { }
    }
}
