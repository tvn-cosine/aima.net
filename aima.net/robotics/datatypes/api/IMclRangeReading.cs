namespace aima.net.robotics.datatypes.api
{ 
    /// <summary>
    /// This interface defines a data type for a range measurement.
    /// It has to contain an MclVector by which the robot was rotated 
    /// from its original facing direction to receive the measurement.<para />
    /// In addition it describes a method for using a range sensor noise model on the measurement.
    /// </summary>
    /// <typeparam name="R">the class that is implementing this interface.</typeparam>
    /// <typeparam name="V">
    /// a n-1-dimensional vector implementing IMclVector, 
    /// where n is the dimensionality of the environment.</typeparam>
    public interface IMclRangeReading<R, V>
        where R : IMclRangeReading<R, V>
        where V : IMclVector
    {
        /// <summary>
        /// Returns the vector by which the robot was rotated for this range reading.
        /// </summary>
        /// <returns>the vector by which the robot was rotated for this range reading.</returns>
        V GetAngle();
        
        /// <summary>
        /// The range sensor noise model.
        /// Calculates a weight between 0 and 1 that specifies how similar the 
        /// given range readings is to this range reading.
        /// </summary>
        /// <param name="secondRange">the second range to be weighted against.</param>
        /// <returns>a weight between 0 and 1.</returns>
        double CalculateWeight(R secondRange);
    }

}
