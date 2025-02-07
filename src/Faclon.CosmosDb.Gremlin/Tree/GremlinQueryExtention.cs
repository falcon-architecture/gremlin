namespace Gremlin.Tree;

public static class GremlinQueryExtention
{
    public static IVertexGremlinQuery<V> GetExtreamVertex<V, E>(this IVertexGremlinQuery<V> query) where V : IVertex where E : IEdge
    {
        return query.Union(
            __ => __.Identity(), // Add root node
            __ => __.Loop(l => l // Find right node
                        .Repeat(__ => __.Out<E>().OfType<V>())
                        .Emit()
                        .Until(__ => __.Not(__ => __.Out<E>()))
                    )
            )
            .Tail(1); // Get last node
    }

    public static IVertexGremlinQuery<V> GetVertex<V, E>(this IVertexGremlinQuery<V> query, V parent) where V : IVertex where E : IEdge
    {
        return query.V(parent).Out<E>().OfType<V>().Limit(1); // TODO: Test this
    }

    // public static IEdgeGremlinQuery<Left, V, V> AddLeftChild<V>(this IVertexGremlinQuery<V> query, V child) where V : IVertex
    // {
    //     return query.AddE<Left>().To(__ => __.AddV(child));
    // }

    // public static IEdgeGremlinQuery<Right, V, V> AddRightChild<V>(this IVertexGremlinQuery<V> query, V child) where V : IVertex
    // {
    //     return query.AddE<Right>().To(__ => __.AddV(child));
    // }
}