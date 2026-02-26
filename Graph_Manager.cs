using System;
using System.Collections.Generic;

namespace GraphApp
{
    // Singleton Pattern
    public class Graph_Manager
    {
        private static Graph_Manager? instance;
        private static readonly object lockObj = new object();

        private List<Graph> graphs;
        private GraphFactory factory;
        private int nextGraphID = 1;

        private Graph_Manager()
        {
            graphs = new List<Graph>();
            factory = new ConcreteGraphFactory();
        }

        // singleton accessor
        public static Graph_Manager GetInstance()
        {
            if (instance == null)
            {
                lock (lockObj)
                {
                    if (instance == null)
                        instance = new Graph_Manager();
                }
            }
            return instance;
        }

        // create a blank graph with a unique ID using Factory Method
        public Graph CreateGraph()
        {
            var g = factory.CreateGraph(nextGraphID++);
            graphs.Add(g);
            return g;
        }

        // find graph by ID and let the caller modify it
        public Graph? GetGraphByID(int id)
        {
            return graphs.Find(g => g.GetGraphID() == id);
        }

        // add vertex to a specific graph (unique vertex ID enforced by caller/UI)
        public bool AddVertexToGraph(int graphID, Vertex v)
        {
            var g = GetGraphByID(graphID);
            if (g == null) return false;
            g.AddVertex(v);
            return true;
        }

        // add edge to a specific graph
        public bool AddEdgeToGraph(int graphID, Edge e)
        {
            var g = GetGraphByID(graphID);
            if (g == null) return false;
            g.AddEdge(e);
            return true;
        }

        // update vertex coordinates in a graph
        public bool UpdateVertex(int graphID, int vertexID, int x, int y)
        {
            var g = GetGraphByID(graphID);
            if (g == null) return false;
            g.UpdateVertex(vertexID, x, y);
            return true;
        }

        // update edge endpoints in a graph
        public bool UpdateEdge(int graphID, int edgeID, Vertex from, Vertex to)
        {
            var g = GetGraphByID(graphID);
            if (g == null) return false;
            g.UpdateEdge(edgeID, from, to);
            return true;
        }

        // deep copy via Prototype pattern, assign new ID
        public Graph? CopyGraph(int id)
        {
            var original = GetGraphByID(id);
            if (original == null) return null;

            var copy = (Graph)original.Clone();
            copy.SetGraphID(nextGraphID++);
            graphs.Add(copy);
            return copy;
        }

        public List<Graph> GetAllGraphs() => graphs;
    }
}
