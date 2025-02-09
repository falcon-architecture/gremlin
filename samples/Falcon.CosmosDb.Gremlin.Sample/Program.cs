using Falcon.CosmosDb.Gremlin.Sample.OpenApi;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers()
                .AddNewtonsoftJson(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });

builder.Services.AddGremlinModule(builder.Configuration, "Cosmos:Gremlin")
                .AddOpenApi();

builder.Logging.AddSimpleConsole((options) =>
{
    options.SingleLine = true;
    options.TimestampFormat = "hh:mm:ss ";
});

var app = builder.Build();
app.UseHttpsRedirection();
app.MapGet("/", () => "BinaryTree API Service");
app.UseFalconOpenApi("Falcon.CosmosDb.Gremlin.Sample");
app.UseFalconHealthChecks();
app.Services.UseGremlinModule();


app.MapPost("/user/list", async ([FromServices] IGremlinQuerySource g) =>
{
    // // Loop
    // var result = await g.V<User>()
    //                 .Loop(l => l
    //                         .Repeat(__ => __.Out<Right>().OfType<User>())
    //                         .Until(__ => __.Not(__ => __.Out<Right>()))
    //                 ).FirstAsync();

    // // List All from root node
    // var result = await g.V<User>().OfType<User>().ToArrayAsync();

    // // List All from some node
    var result = await g.V<User>("john-5").ToArrayAsync();
    return Results.Ok(result);
});

app.MapPost("/user/{introducerPosition}", UserApi.AddUser);
await app.RunAsync();
