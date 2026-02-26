using System;
using System.Drawing;

namespace GraphApp
{
    public class Edge : ICloneable
    {
        private int edge_ID;
        private Vertex from_vertex;
        private Vertex to_vertex;

        public Edge(int id, Vertex from, Vertex to)
        {
            edge_ID = id;
            from_vertex = from;
            to_vertex = to;
        }

        public int GetEdgeID() => edge_ID;
        public Vertex GetFrom() => from_vertex;
        public void SetFrom(Vertex v) => from_vertex = v;
        public Vertex GetTo() => to_vertex;
        public void SetTo(Vertex v) => to_vertex = v;

        // draw a directed edge (line + arrowhead) between the two vertices.
        public void Drawing(Graphics g)
        {
            int r = 20; // vertex radius

            double dx = to_vertex.GetX() - from_vertex.GetX();
            double dy = to_vertex.GetY() - from_vertex.GetY();
            double len = Math.Sqrt(dx * dx + dy * dy);
            if (len == 0) return;

            double ux = dx / len;
            double uy = dy / len;

            // start point: edge of from-vertex circle
            float x1 = (float)(from_vertex.GetX() + ux * r);
            float y1 = (float)(from_vertex.GetY() + uy * r);

            // end point: edge of to-vertex circle
            float x2 = (float)(to_vertex.GetX() - ux * r);
            float y2 = (float)(to_vertex.GetY() - uy * r);

            using var pen = new Pen(Color.DarkSlateGray, 2);
            g.DrawLine(pen, x1, y1, x2, y2);

            // arrowhead
            double angle = Math.PI / 6;
            PointF a1 = ComputeArrow(dx, dy, x2, y2, angle);
            PointF a2 = ComputeArrow(dx, dy, x2, y2, -angle);
            g.DrawLine(pen, x2, y2, a1.X, a1.Y);
            g.DrawLine(pen, x2, y2, a2.X, a2.Y);

            // origin dot
            g.FillEllipse(Brushes.DarkSlateGray, x1 - 4, y1 - 4, 8, 8);
        }

        private PointF ComputeArrow(double dx, double dy, float tipX, float tipY, double angle)
        {
            double len = Math.Sqrt(dx * dx + dy * dy);
            double x = -20 * dx / len;
            double y = -20 * dy / len;
            double nx = x * Math.Cos(angle) - y * Math.Sin(angle);
            double ny = x * Math.Sin(angle) + y * Math.Cos(angle);
            return new PointF(tipX + (float)nx, tipY + (float)ny);
        }

        public object Clone()
        {
            // shallow clone vertices by reference (deep copy handled by Graph)
            return new Edge(edge_ID, from_vertex, to_vertex);
        }

        public override string ToString() =>
            $"Edge({edge_ID}): Vertex {from_vertex.GetVertexID()} -> Vertex {to_vertex.GetVertexID()}";
    }
}
