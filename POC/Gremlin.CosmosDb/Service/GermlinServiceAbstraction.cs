namespace Gremlin.CosmosDb.Service;

public abstract class GremlinServiceAbstraction
{
    public readonly GraphTraversalSource g;
    public readonly GremlinClient Client;
    
    protected const string USER_VERTEX_LABEL = nameof(UserVertex);
    protected const string ORDER_VERTEX_LABEL = nameof(OrderVertex);

    protected const string INTRODUCED_EDGE_LABEL = nameof(IntroducedEdge);
    protected const string ORDERED_EDGE_LABEL = nameof(OrderedEdge);
    protected const string LEFT_EDGE_LABEL = nameof(LeftEdge);
    protected const string RIGHT_EDGE_LABEL = nameof(RightEdge);

    protected GremlinServiceAbstraction(GraphTraversalSource gS, GremlinClient client)
    {
        g = gS;
        Client = client;
    }

    public GraphTraversal<Vertex, GraphTraversal<Vertex, Vertex>> ExtremeVertex<E>(object? id) where E : Edge
    {
        var edgeName = typeof(E).Name;
        return g.V(id)
                .Union<GraphTraversal<Vertex, Vertex>>(
                    __.Identity(),
                    __.Repeat(__.Out(edgeName)).Until(__.OutE(edgeName).Count().Is(0))
                )
                .Tail<GraphTraversal<Vertex, Vertex>>(1);
    }

    public GraphTraversal<Vertex, object> ExtremeVertexId<E>(object? id) where E : Edge
    {
        return ExtremeVertex<E>(id).Id();
    }
}