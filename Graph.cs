using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GraphApp
{
    public class Graph : GraphIF
    {
        private int graph_ID;
        private List<Vertex> vertices;
        private List<Edge> edges;

        public Graph(int id)
        {
            graph_ID = id;
            vertices = new List<Vertex>();
            edges = new List<Edge>();
        }

        public int GetGraphID() => graph_ID;
        public void SetGraphID(int id) => graph_ID = id;

        public List<Vertex> GetVertices() => vertices;
        public List<Edge> GetEdges() => edges;

        public void AddVertex(Vertex v) => vertices.Add(v);
        public void AddEdge(Edge e) => edges.Add(e);

        public Vertex? FindVertex(int id) => vertices.Find(v => v.GetVertexID() == id);
        public Edge? FindEdge(int id) => edges.Find(e => e.GetEdgeID() == id);

        public void UpdateVertex(int id, int x, int y)
        {
            var v = FindVertex(id);
            if (v != null) v.SetXY(x, y);
        }

        public void UpdateEdge(int id, Vertex from, Vertex to)
        {
            var e = FindEdge(id);
            if (e != null) { e.SetFrom(from); e.SetTo(to); }
        }

        // display the graph by drawing all vertices and edges onto the given panel
        public void Display(Panel panel)
        {
            panel.Refresh();
            using Graphics g = panel.CreateGraphics();
            g.Clear(Color.White);

            foreach (var e in edges)
                e.Drawing(g);

            foreach (var v in vertices)
                v.Drawing(g);

            // draw graph ID label
            g.DrawString($"Graph ID: {graph_ID}", new Font("Arial", 10, FontStyle.Bold),
                Brushes.Black, new PointF(5, 5));
        }

        // Prototype: deep copy 
        public object Clone()
        {
            var newGraph = new Graph(graph_ID);

            // clone vertices
            var vertexMap = new Dictionary<int, Vertex>();
            foreach (var v in vertices)
            {
                var cloned = (Vertex)v.Clone();
                newGraph.AddVertex(cloned);
                vertexMap[cloned.GetVertexID()] = cloned;
            }

            // clone edges
            foreach (var e in edges)
            {
                var newFrom = vertexMap[e.GetFrom().GetVertexID()];
                var newTo = vertexMap[e.GetTo().GetVertexID()];
                newGraph.AddEdge(new Edge(e.GetEdgeID(), newFrom, newTo));
            }

            return newGraph;
        }

        public override string ToString() =>
            $"Graph {graph_ID} | Vertices: {vertices.Count} | Edges: {edges.Count}";
    }
}
