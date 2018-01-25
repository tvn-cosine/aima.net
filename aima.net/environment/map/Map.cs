using aima.net.collections.api;
using aima.net.util.math.geom.shapes;

namespace aima.net.environment.map
{
    /**
     * Provides a general interface for maps.
     * 
     * @author Ruediger Lunde
     */
    public interface Map
    {
        /** Returns a list of all locations. */
        ICollection<string> getLocations();

        /**
         * Answers to the question: Where can I get, following one of the
         * connections starting at the specified location?
         */
        ICollection<string> getPossibleNextLocations(string location);

        /**
         * Answers to the question: From where can I reach a specified location,
         * following one of the map connections?
         */
        ICollection<string> getPossiblePrevLocations(string location);

        /**
         * Returns the travel distance between the two specified locations if they
         * are linked by a connection and null otherwise.
         */
        double? getDistance(string fromLocation, string toLocation);

        /**
         * Returns the position of the specified location. The position is
         * represented by two coordinates, e.g. latitude and longitude values.
         */
        Point2D getPosition(string loc);

        /**
         * Returns a location which is selected by random.
         */
        string randomlyGenerateDestination();
    }
}
