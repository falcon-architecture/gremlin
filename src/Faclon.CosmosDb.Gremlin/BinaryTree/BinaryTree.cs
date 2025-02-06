namespace Faclon.CosmosDb.Gremlin.BinaryTree;

public abstract class BinaryTree<V, E> where V : IVertex where E : IEdge
{
    protected readonly IGremlinQuerySource g;
    protected BinaryTree(IGremlinQuerySource querySource) { g = querySource; }

    public IVertexGremlinQuery<V> AddRootVertex(V vertex)
    {
        return g.AddV(vertex);
    }

    public IGremlinQuery<long> Count()
    {
        return g.V().Count();
    }

    public ValueTask<Left> AddLeftChild(V parent, V child)
    {
        return g.V(parent).AddE<Left>().To(__ => __.AddV(child)).FirstAsync();
    }

    public IEdgeGremlinQuery<Right, object, V> AddRightChild(V parent, V child)
    {
        return g.V(parent).AddE<Right>().To(__ => __.AddV(child));
    }

    public IVertexGremlinQuery<V> GetLeftChild(V parent)
    {
        return g.V(parent).Out<E>().OfType<V>().Limit(1);
    }

    public IVertexGremlinQuery<V> GetRightChild(V parent)
    {
        return g.V(parent).Out<E>().OfType<V>().Skip(1).Limit(1);
    }

    public IVertexGremlinQuery<V> GetParent(V child)
    {
        return g.V(child).In<E>().OfType<V>().Limit(1);
    }

    public IVertexGremlinQuery<V> GetRoot()
    {
        return g.V().Not(__ => __.In<E>()).OfType<V>().Limit(1);
    }

    public IVertexGremlinQuery<V> GetAllVertices()
    {
        return g.V().OfType<V>();
    }

    public IEdgeGremlinQuery<E> GetAllEdges()
    {
        return g.E().OfType<E>();
    }
}
