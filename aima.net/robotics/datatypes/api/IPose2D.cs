namespace aima.net.robotics.datatypes.api
{
    /// <summary>
    /// This interface describes functionality for a pose in a two-dimensional Cartesian plot.
    /// </summary>
    /// <typeparam name="P">the pose implementing IPose2D.</typeparam>
    /// <typeparam name="M">a movement (or sequence of movements) of the robot, implementing IMclMove.</typeparam>
    public interface IPose2D<P, M> : IMclPose<P, Angle, M>
        where P : IPose2D<P, M>
        where M : IMclMove<M>
    {
        /// <summary>
        /// Return the X coordinate of the pose.
        /// </summary>
        /// <returns>the X coordinate of the pose.</returns>
        double GetX();

        /// <summary>
        /// Return the Y coordinate of the pose.
        /// </summary>
        /// <returns>the Y coordinate of the pose.</returns>
        double GetY();

        /// <summary>
        /// Return the heading of the pose in radians.
        /// </summary>
        /// <returns>the heading of the pose in radians.</returns>
        double GetHeading();
    }

}
