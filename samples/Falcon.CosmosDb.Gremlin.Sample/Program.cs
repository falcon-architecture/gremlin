var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
    });

builder.Services.AddGremlinModule(builder.Configuration, "Cosmos:Gremlin");
builder.Services.AddOpenApi();
builder.Logging.AddSimpleConsole((options) =>
{
    options.SingleLine = true;
    options.TimestampFormat = "hh:mm:ss ";
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Gremline Graph Db POC");
    });
}
app.MapGet("/", () => "BinaryTree API Service");
app.UseHttpsRedirection();
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
