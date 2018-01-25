using aima.net.robotics.datatypes;

namespace aima.net.robotics.map.api
{
    /// <summary>
    /// This interface defines a factory for the 
    /// class extending AbstractRangeReading in the context of the MclCartesianPlot2D.
    /// </summary>
    /// <typeparam name="R">a range reading extending AbstractRangeReading.</typeparam>
    public interface IRangeReadingFactory<R>
        where R : AbstractRangeReading
    {
        /// <summary>
        /// Creates a new instance of {@code <R>} for the given parameters.
        /// </summary>
        /// <param name="value">the value of the new range reading.</param>
        /// <returns>the new range reading.</returns>
        R getRangeReading(double value);
    }
}
