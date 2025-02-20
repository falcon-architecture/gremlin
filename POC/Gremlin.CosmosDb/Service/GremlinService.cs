namespace Gremlin.CosmosDb.Service;

public class GremlinService : GremlinServiceAbstraction
{
    private const string NEW_USER = "newuser";
    private const string NEW_ORDER = "neworder";
    public GremlinService(GraphTraversalSource g, GremlinClient client) : base(g, client) { }

    public Task<ResultSet<dynamic>> VertexLevelQueryAsync(string userId, int level, CancellationToken cancellationToken = default)
    {
        var traversal = g.V(userId).HasLabel(USER_VERTEX_LABEL)
                         .Union<GraphTraversal<Vertex, Vertex>>(
                             __.Identity(),
                            __.Repeat(__.Out()).Emit().Times(level).HasLabel(USER_VERTEX_LABEL)
                         )
                         .Dedup()
                         .Limit<Vertex>((int)Math.Pow(2, level) - 1)
                         .Project<object>("Id", "Name", "PartitionKey")
                            .By(__.Values<string>("id"))
                            .By(__.Values<string>("Name"))
                            .By(__.Values<string>("PartitionKey"));
        return traversal.ExecuteAsync<dynamic>(Client, cancellationToken);
    }

    public Task<ResultSet<dynamic>> GetById(string userId, CancellationToken cancellationToken = default)
    {
        var traversal = g.V(userId).HasLabel(USER_VERTEX_LABEL)
                            .Project<object>("Vertex", "InEdge", "OutEdge", "InVertex", "OutVertex")
                            .By(__.ValueMap<object, object>())
                            .By(__.InE().ValueMap<object, object>().Fold())
                            .By(__.OutE().ValueMap<object, object>().Fold())
                            .By(__.In().ValueMap<object, object>().Fold())
                            .By(__.Out().ValueMap<object, object>().Fold());
        return traversal.ExecuteAsync<dynamic>(Client, cancellationToken);
    }

    public async Task<dynamic> AddAsync(UserVertex user, string introducerPosition, CancellationToken cancellationToken = default)
    {
        var count = await g.V()
                            .Count()
                            .ExecuteSingleAsync<long>(Client, cancellationToken);
        if (count > 0 && string.IsNullOrWhiteSpace(user.IntroducerId))
        {
            throw new ArgumentException("Introducer Id is required");
        }
        if (string.IsNullOrWhiteSpace(user.IntroducerId))
        {
            return await g.AddV(nameof(UserVertex))
                            .Property(user)
                            .ExecuteDynamicAsync(Client, cancellationToken);
        }
        else
        {
            var traversal = introducerPosition.ToLower() switch
            {
                "left" => await AddVertexInExtream<LeftEdge>(user, cancellationToken),
                "right" => await AddVertexInExtream<RightEdge>(user, cancellationToken),
                _ => throw new ArgumentException("Invalid introducer position")
            };
            return await traversal.ExecuteDynamicAsync(Client, cancellationToken);
        }
    }

    private async Task<GraphTraversal<Vertex, Vertex>> AddVertexInExtream<E>(UserVertex user, CancellationToken cancellationToken) where E : Edge
    {
        var parentId = await ExtremeVertexId<E>(user.IntroducerId).ExecuteSingleAsync<object>(Client, cancellationToken);
        var edgeName = typeof(E).Name;
        return g.V(parentId).HasLabel(USER_VERTEX_LABEL)
                .AddV().Property(user).Property(nameof(UserVertex.ParentId), parentId).As(NEW_USER)
                .AddE(edgeName).From(__.V(parentId)).To(NEW_USER)
                .AddE(INTRODUCED_EDGE_LABEL).From(__.V(user.IntroducerId)).To(NEW_USER)
                .Select<Vertex>(NEW_USER);
    }

    public async Task<dynamic> AddOrderAsync(object userId, OrderVertex orderVertex, CancellationToken cancellationToken = default)
    {
        var edgeProperties = new OrderedEdgeProperty { OrderDate = orderVertex.OrderDate };
        return await g.V(userId).HasLabel(USER_VERTEX_LABEL)
                        .AddV().Property(orderVertex).As(NEW_ORDER)
                        .AddE(ORDERED_EDGE_LABEL).Property(edgeProperties).From(__.V(userId)).To(NEW_ORDER)
                        .Select<Vertex>(NEW_ORDER)
                        .ExecuteDynamicAsync(Client, cancellationToken);
    }
}
