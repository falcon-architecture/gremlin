global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Builder;
global using Microsoft.AspNetCore.Diagnostics.HealthChecks;
global using Microsoft.Extensions.DependencyInjection;
global using Microsoft.Extensions.Hosting;

global using System.Linq;
global using System.Text.Json;
global using System.Text.Json.Serialization;

global using ExRam.Gremlinq.Core;
global using Newtonsoft.Json.Serialization;

global using Falcon.CosmosDb.Gremlin;
global using Falcon.CosmosDb.Gremlin.Configurations;
global using Falcon.CosmosDb.Gremlin.HealthChecks;
global using Falcon.CosmosDb.Gremlin.Graph;

global using Gremlin.BinaryTree;
global using Gremlin.WebApi;