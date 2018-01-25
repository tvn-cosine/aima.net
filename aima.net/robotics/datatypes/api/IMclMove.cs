namespace aima.net.robotics.datatypes.api
{  
    /// <summary>
    /// This interface represents a movement or a sequence of movements that the robot performed.<para />
    /// In addition it describes a method for using a movement noise model on the move.
    /// </summary>
    /// <typeparam name="M">the class that is implementing this interface.</typeparam>
    public interface IMclMove<M>
        where M : IMclMove<M>
    { 
        /// <summary>
        /// Generates noise onto the move to mask errors in measuring the performed 
        /// movements and to localize successfully with a smaller number of particles than without noise.
        /// </summary>
        /// <returns>a new move onto that noise has been added.</returns>
        M GenerateNoise();
    } 
}
