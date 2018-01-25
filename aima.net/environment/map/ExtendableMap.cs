using aima.net.collections;
using aima.net.collections.api;
using aima.net.datastructures;
using aima.net.util;
using aima.net.util.math.geom.shapes;

namespace aima.net.environment.map
{
    /**
     * Implements a map with locations, distance labeled links between the
     * locations, straight line distances, and 2d-placement positions of locations.
     * Locations are represented by strings and travel distances by double values.
     * Locations and links can be added dynamically and removed after creation. This
     * enables to read maps from file or to modify them with respect to newly
     * obtained knowledge.
     * 
     * @author Ruediger Lunde
     */
    public class ExtendableMap : Map
    {

        /**
         * Stores map data. Locations are represented as vertices and connections
         * (links) as directed edges labeled with corresponding travel distances.
         */
        private readonly LabeledGraph<string, double?> links;

        /** Stores xy-coordinates for each location. */
        private readonly IMap<string, Point2D> locationPositions;

        /** Creates an empty map. */
        public ExtendableMap()
        {
            links = new LabeledGraph<string, double?>();
            locationPositions = CollectionFactory.CreateInsertionOrderedMap<string, Point2D>();
        }

        /** Removes everything. */
        public void clear()
        {
            links.Clear();
            locationPositions.Clear();
        }

        /** Clears all connections but keeps location position informations. */
        public void clearLinks()
        {
            links.Clear();
        }

        /** Returns a list of all locations. */
        public ICollection<string> getLocations()
        {
            return links.GetVertexLabels();
        }

        /** Checks whether the given string is the name of a location. */
        public bool isLocation(string str)
        {
            return links.IsVertexLabel(str);
        }

        /**
         * Answers to the question: Where can I get, following one of the
         * connections starting at the specified location?
         */
        public ICollection<string> getPossibleNextLocations(string location)
        {
            ICollection<string> result = links.GetSuccessors(location);
            result.Sort(new List<string>.Comparer());
            return result;
        }

        /**
         * Answers to the question: From where can I reach a specified location,
         * following one of the map connections? This implementation just calls
         * {@link #getPossibleNextLocations(string )} as the underlying graph structure
         * cannot be traversed efficiently in reverse order.
         */
        public  ICollection<string> getPossiblePrevLocations(string  location)
        {
            return getPossibleNextLocations(location);
        }

        /**
         * Returns the travel distance between the two specified locations if they
         * are linked by a connection and null otherwise.
         */
        public double? getDistance(string  fromLocation, string toLocation)
        {
            return links.Get(fromLocation, toLocation);
        }

        /** Adds a one-way connection to the map. */
        public void addUnidirectionalLink(string  fromLocation, string toLocation, double distance)
        {
            links.Set(fromLocation, toLocation, distance);
        }

        /**
         * Adds a connection which can be traveled in both direction. Internally,
         * such a connection is represented as two one-way connections.
         */
        public void addBidirectionalLink(string  fromLocation, string toLocation, double distance)
        {
            links.Set(fromLocation, toLocation, distance);
            links.Set(toLocation, fromLocation, distance);
        }

        /**
         * Returns a location which is selected by random.
         */
        public string randomlyGenerateDestination()
        {
            return Util.selectRandomlyFromList(getLocations());
        }

        /** Removes a one-way connection. */
        public void removeUnidirectionalLink(string  fromLocation, string toLocation)
        {
            links.Remove(fromLocation, toLocation);
        }

        /** Removes the two corresponding one-way connections. */
        public void removeBidirectionalLink(string  fromLocation, string toLocation)
        {
            links.Remove(fromLocation, toLocation);
            links.Remove(toLocation, fromLocation);
        }

        /**
         * Defines the position of a location as with respect to an orthogonal
         * coordinate system.
         */
        public void setPosition(string  loc, double x, double y)
        {
            locationPositions.Put(loc, new Point2D(x, y));
        }

        /**
         * Defines the position of a location within the map. Using this method, one
         * location should be selected as reference position (<code>dist=0</code>
         * and <code>dir=0</code>) and all the other location should be placed
         * relative to it.
         * 
         * @param loc
         *            location name
         * @param dist
         *            distance to a reference position
         * @param dir
         *            bearing (compass direction) in which the location is seen from
         *            the reference position
         */
        public void setDistAndDirToRefLocation(string  loc, double dist, int dir)
        {
            Point2D coords = new Point2D(-System.Math.Sin(dir * System.Math.PI / 180.0) * dist, 
                System.Math.Cos(dir * System.Math.PI / 180.0) * dist);
            links.AddVertex(loc);
            locationPositions.Put(loc, coords);
        }

        /**
         * Returns the position of the specified location as with respect to an
         * orthogonal coordinate system.
         */
        public Point2D getPosition(string  loc)
        {
            return locationPositions.Get(loc);
        }
    }
}
