namespace GraphApp
{
    public interface GraphIF : ICloneable
    {
        void Display(System.Windows.Forms.Panel panel);
        void AddVertex(Vertex v);
        void AddEdge(Edge e);
        object Clone();
        int GetGraphID();
    }
}
