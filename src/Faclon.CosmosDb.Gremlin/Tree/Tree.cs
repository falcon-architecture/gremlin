namespace Faclon.CosmosDb.Gremlin.Tree;

public abstract class Tree<V> where V : IVertex
{
    protected readonly IGremlinQuerySource g;
    protected Tree(IGremlinQuerySource querySource) { g = querySource; }

    public IVertexGremlinQuery<V> AddRootVertex(V vertex)
    {
        return g.AddV(vertex);
    }

    public IVertexGremlinQuery<V> GetVertex(V vertex)
    {
        return g.V<V>(vertex);
    }
    public IVertexGremlinQuery<V> GetAllVertices()
    {
        return g.V<V>();
    }

    public IEdgeGremlinQuery<E> GetAllEdges<E>() where E : IEdge
    {
        return g.E<E>();
    }

    public IGremlinQuery<long> GetVertexCount<E>(V vertex) where E : IEdge
    {
        // return g
        //     .V<V>(vertex)
        //     .Out<E>()
        //     .OfType<V>()
        //     .Loop(l => l
        //         .Repeat(__ => __.Out().OfType<V>())
        //         .Emit()
        //     )
        //     .Count();

        return g
            .V<V>(vertex)
            .Out<E>()
            .OfType<V>()
            .Tree<V>()
            .Count();
    }

    public IVertexGremlinQuery<V> GetParent<E>(V child) where E : IEdge
    {
        return g.V(child).In<E>().OfType<V>().Limit(1);
    }

    public IVertexGremlinQuery<V> GetRoot<E>() where E : IEdge
    {
        return g.V<V>().Not(__ => __.In<E>()).OfType<V>().Limit(1);
    }
}
