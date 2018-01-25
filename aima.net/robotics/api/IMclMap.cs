using aima.net.robotics.datatypes.api;

namespace aima.net.robotics.api
{ 
    /// <summary>
    /// This interface defines functionality for a map of an environment for a robot (agent) to perform Monte-Carlo-Localization in.
    /// </summary>
    /// <typeparam name="P">a pose implementing IMclPose.</typeparam>
    /// <typeparam name="V">
    /// an n-1-dimensional vector implementing IMclVector, 
    /// where n is the dimensionality of the environment.
    /// </typeparam>
    /// <typeparam name="M">
    /// a movement (or sequence of movements) of the robot, implementing IMclMove.
    /// </typeparam>
    /// <typeparam name="R">a range measurement, implementing IMclRangeReading.</typeparam>
    public interface IMclMap<P, V, M, R>
        where P : IMclPose<P, V, M>
        where V : IMclVector
        where M : IMclMove<M>
        where R : IMclRangeReading<R, V>
    {
        /**
         * Generates a random valid pose on the map.
         * @return a random valid pose on the map.
         */
        P randomPose();
        /**
         * Calculates the length of a ray in a direction defined by a pose.
         * @param pose the pose from which the ray is to be cast.
         * @return the length of the ray as a range reading.
         */
        R rayCast(P pose);
        /**
         * Verifies whether a pose is valid, that is inside the map boundaries and not within an obstacle.
         * @param pose the pose which is to be evaluated.
         * @return true if the pose is valid.
         */
        bool isPoseValid(P pose);
    }
}
