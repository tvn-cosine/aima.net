using aima.net.collections;
using aima.net.collections.api;

namespace aima.net.datastructures
{
    /// <summary>
    /// Represents a directed labeled graph. Vertices are represented by their unique
    /// labels and labeled edges by means of nested hashtables. Variant of class
    /// #tvn.cosine.ai.util.Table. This version is more dynamic, it requires no
    /// initialization and can add new items whenever needed.
    /// </summary>
    /// <typeparam name="VertexLabelType"></typeparam>
    /// <typeparam name="EdgeLabelType"></typeparam>
    public class LabeledGraph<VertexLabelType, EdgeLabelType>
    {
        /// <summary>
        /// Lookup for edge label information. Contains an entry for every vertex label.
        /// </summary>
        private readonly IMap<VertexLabelType, IMap<VertexLabelType, EdgeLabelType>> globalEdgeLookup;
        /// <summary>
        /// List of the labels of all vertices within the graph.
        /// </summary>
        private readonly ICollection<VertexLabelType> vertexLabels;

        /// <summary>
        /// Creates a new empty graph.
        /// </summary>
        public LabeledGraph()
        {
            globalEdgeLookup = CollectionFactory.CreateInsertionOrderedMap<VertexLabelType, IMap<VertexLabelType, EdgeLabelType>>();
            vertexLabels = CollectionFactory.CreateFifoQueue<VertexLabelType>();
        }

        /// <summary>
        /// Adds a new vertex to the graph if it is not already present.
        /// </summary>
        /// <param name="v">the vertex to add</param>
        public void AddVertex(VertexLabelType v)
        {
            checkForNewVertex(v);
        }

        /// <summary>
        /// Adds a directed labeled edge to the graph. The end points of the edge are
        /// specified by vertex labels. New vertices are automatically identified and
        /// added to the graph.
        /// </summary>
        /// <param name="from">the first vertex of the edge</param>
        /// <param name="to">the second vertex of the edge</param>
        /// <param name="el">an edge label</param>
        public void Set(VertexLabelType from, VertexLabelType to, EdgeLabelType el)
        {
            IMap<VertexLabelType, EdgeLabelType> localEdgeLookup = checkForNewVertex(from);
            localEdgeLookup.Put(to, el);
            checkForNewVertex(to);
        }

        /// <summary>
        /// Handles new vertices.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        private IMap<VertexLabelType, EdgeLabelType> checkForNewVertex(VertexLabelType v)
        {
            IMap<VertexLabelType, EdgeLabelType> result = globalEdgeLookup.Get(v);
            if (result == null)
            {
                result = CollectionFactory.CreateInsertionOrderedMap<VertexLabelType, EdgeLabelType>();
                globalEdgeLookup.Put(v, result);
                vertexLabels.Add(v);
            }
            return result;
        }
         
        /// <summary>
        /// Removes an edge from the graph.
        /// </summary>
        /// <param name="from">the first vertex of the edge</param>
        /// <param name="to">the second vertex of the edge</param>
        public void Remove(VertexLabelType from, VertexLabelType to)
        {
            IMap<VertexLabelType, EdgeLabelType> localEdgeLookup = globalEdgeLookup.Get(from);
            if (localEdgeLookup != null)
                localEdgeLookup.Remove(to);
        }
         
        /// <summary>
        /// Returns the label of the edge between the specified vertices, or null if
        /// there is no edge between them.
        /// </summary>
        /// <param name="from">the first vertex of the ege</param>
        /// <param name="to">the second vertex of the edge</param>
        /// <returns>
        /// the label of the edge between the specified vertices, or null if
        /// there is no edge between them.
        /// </returns>
        public EdgeLabelType Get(VertexLabelType from, VertexLabelType to)
        {
            IMap<VertexLabelType, EdgeLabelType> localEdgeLookup = globalEdgeLookup.Get(from);
            return localEdgeLookup == null ? default(EdgeLabelType) : localEdgeLookup.Get(to);
        }

        /// <summary>
        /// Returns the labels of those vertices which can be obtained by following
        /// the edges starting at the specified vertex.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public ICollection<VertexLabelType> GetSuccessors(VertexLabelType v)
        {
            ICollection<VertexLabelType> result = CollectionFactory.CreateQueue<VertexLabelType>();
            IMap<VertexLabelType, EdgeLabelType> localEdgeLookup = globalEdgeLookup.Get(v);
            if (localEdgeLookup != null)
            {
                result.AddAll(localEdgeLookup.GetKeys());
            }
            return result;
        }

        /// <summary>
        /// Returns the labels of all vertices within the graph.
        /// </summary>
        /// <returns></returns>
        public ICollection<VertexLabelType> GetVertexLabels()
        {
            return vertexLabels;
        }

        /// <summary>
        /// Checks whether the given label is the label of one of the vertices.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public bool IsVertexLabel(VertexLabelType v)
        {
            return globalEdgeLookup.Get(v) != null;
        }

        /// <summary>
        /// Removes all vertices and all edges from the graph.
        /// </summary>
        public void Clear()
        {
            vertexLabels.Clear();
            globalEdgeLookup.Clear();
        }
    }
}
