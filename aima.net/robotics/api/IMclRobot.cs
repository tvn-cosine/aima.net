using aima.net.robotics.datatypes.api;

namespace aima.net.robotics.api
{
    /**
     * This interface defines functionality a robotic agent has to implement in order to localize itself via {@link MonteCarloLocalization}. 
     * 
     * @author Arno von Borries
     * @author Jan Phillip Kretzschmar
     * @author Andreas Walscheid
     *
     * @param <V> an n-1-dimensional vector implementing {@link IMclVector}, where n is the dimensionality of the environment.
     * @param <M> a movement (or sequence of movements) of the robot, implementing {@link IMclMove}. 
     * @param <R> a range measurement, implementing {@link IMclRangeReading}.
     */
    public interface IMclRobot<V, M, R>
        where V : IMclVector
        where M : IMclMove<M>
        where R : IMclRangeReading<R, V>
    {
        /**
         * Causes a series of sensor measurements to determine the distance to various obstacles within the environment.
         * @return an array of range measurements {@code <R>}. 
         * @throws RobotException thrown if the range reading was not successful.
         */
        R[] getRangeReadings();
        /**
         * Causes the robot to perform a movement.
         * @return the move the robot performed.
         * @throws RobotException thrown if the move was not successful.
         */
        M performMove();
    } 
}
