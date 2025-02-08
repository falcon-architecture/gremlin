namespace Gremlin.WebApi;

public static class UserApi
{
    private const string RIGHT = "right";

    public static async Task<IResult> AddUser(string introducerPosition, [FromBody] User user, [FromServices] IGremlinQuerySource g)
    {
        if (await g.V().Count().FirstAsync() != 0 && string.IsNullOrWhiteSpace(user.IntroducerId))
        {
            return Results.BadRequest("Parent id is required");
        }
        if (string.IsNullOrWhiteSpace(user.IntroducerId))
        {
            await g.AddV(user).FirstAsync();
        }
        else
        {
            var introducerVertex = g.V<User>(user.IntroducerId);
            if (string.Equals(introducerPosition, RIGHT, StringComparison.OrdinalIgnoreCase))
            {
                await AddVertexInExtreamRightEdge(introducerVertex, user).FirstAsync();
            }
            else
            {
                await AddVertexInExtreamLeftEdge(introducerVertex, user).FirstAsync();
            }
        }
        return Results.Ok(user);
    }

    private static IEdgeGremlinQuery<Right, User, User> AddVertexInExtreamRightEdge(IVertexGremlinQuery<User> vertexQuery, User user)
    {
        var extreamVertex = GetRightExtreamVertex(vertexQuery);
        return AddVertexInRightEdge(extreamVertex, user);
    }

    private static IEdgeGremlinQuery<Left, User, User> AddVertexInExtreamLeftEdge(IVertexGremlinQuery<User> vertexQuery, User user)
    {
        var extreamVertex = GetLeftExtreamVertex(vertexQuery);
        return AddVertexInLeftEdge(extreamVertex, user);
    }

    private static IEdgeGremlinQuery<Right, User, User> AddVertexInRightEdge(IVertexGremlinQuery<User> vertexQuery, User user)
    {
        return vertexQuery.AddE<Right>().To(__ => __.AddV(user));
    }
    private static IEdgeGremlinQuery<Left, User, User> AddVertexInLeftEdge(IVertexGremlinQuery<User> vertexQuery, User user)
    {
        return vertexQuery.AddE<Left>().To(__ => __.AddV(user));
    }

    private static IVertexGremlinQuery<User> GetLeftExtreamVertex(IVertexGremlinQuery<User> query)
    {
        return GetExtreamVertex<Left>(query);
    }

    private static IVertexGremlinQuery<User> GetRightExtreamVertex(IVertexGremlinQuery<User> query)
    {
        return GetExtreamVertex<Right>(query);
    }
    private static IVertexGremlinQuery<User> GetExtreamVertex<E>(IVertexGremlinQuery<User> query) where E : IEdge
    {
        return query.Union(
            __ => __.Identity(), // Add root node
            __ => __.Loop(l => l // Find right node
                        .Repeat(__ => __.Out<E>().OfType<User>())
                        .Emit()
                        .Until(__ => __.Not(__ => __.Out<E>()))
                    )
            )
            .Tail(1); // Get last node
    }
}