namespace GraphApp
{
    // Factory Method Pattern
    public abstract class GraphFactory
    {
        public abstract Graph CreateGraph(int id);
    }

    // Concrete creator
    public class ConcreteGraphFactory : GraphFactory
    {
        public override Graph CreateGraph(int id)
        {
            return new Graph(id);
        }
    }
}
