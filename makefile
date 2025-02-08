build: 
	dotnet build

src: 
	dotnet build .\src\Falcon.CosmosDb.Gremlin\Falcon.CosmosDb.Gremlin.csproj

samples:
	dotnet build ./samples/Falcon.CosmosDb.Gremlin.Samples