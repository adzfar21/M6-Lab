using System;
using System.Drawing;

namespace GraphApp
{
    public class Vertex : ICloneable
    {
        private int vertex_ID;
        private int x_coordinate;
        private int y_coordinate;

        public Vertex(int id, int x, int y)
        {
            vertex_ID = id;
            x_coordinate = x;
            y_coordinate = y;
        }

        public int GetVertexID() => vertex_ID;

        public void SetXY(int x, int y)
        {
            x_coordinate = x;
            y_coordinate = y;
        }

        public int GetX() => x_coordinate;
        public int GetY() => y_coordinate;

        // Draw the vertex as a circle on the given Graphics context
        public void Drawing(Graphics g)
        {
            int radius = 20;
            int cx = x_coordinate - radius;
            int cy = y_coordinate - radius;
            g.FillEllipse(Brushes.LightSteelBlue, cx, cy, radius * 2, radius * 2);
            g.DrawEllipse(Pens.DarkBlue, cx, cy, radius * 2, radius * 2);
            g.DrawString(vertex_ID.ToString(), new Font("Arial", 9, FontStyle.Bold),
                Brushes.DarkBlue, new PointF(x_coordinate - 7, y_coordinate - 7));
        }

        public object Clone()
        {
            return new Vertex(vertex_ID, x_coordinate, y_coordinate);
        }

        public override string ToString() => $"Vertex({vertex_ID}) @ ({x_coordinate},{y_coordinate})";
    }
}
