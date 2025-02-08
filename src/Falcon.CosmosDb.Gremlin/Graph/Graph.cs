namespace Falcon.CosmosDb.Gremlin.Graph;

public abstract class Graph<V> where V : IVertex
{
    protected readonly IGremlinQuerySource g;
    protected Graph(IGremlinQuerySource querySource) { g = querySource; }

    public IVertexGremlinQuery<V> AddRootVertex(V vertex)
    {
        return g.AddV(vertex);
    }

    public IVertexGremlinQuery<V> Vertex(object vertex)
    {
        return g.V<V>(vertex);
    }

    public IVertexGremlinQuery<V> AllVertices()
    {
        return g.V<V>();
    }

    public IEdgeGremlinQuery<E> AllEdges<E>() where E : IEdge
    {
        return g.E<E>();
    }

    public IGremlinQuery<long> VertexCount<E>(V vertex) where E : IEdge
    {
        return g
            .V<V>(vertex)
            .Out<E>().OfType<V>()
            .Tree<V>()
            .Count();
    }

    public IVertexGremlinQuery<V> Parent<E>(V vertex) where E : IEdge
    {
        return g.V(vertex)
                .In<E>().OfType<V>()
                .Limit(1);
    }
}
