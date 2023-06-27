using System.CommandLine;
using System.Text.Json.Serialization;
using tankman.Db;
using tankman.RequestHandlers;

var rootCommand = new RootCommand("Tankman user management and access control");

var dbhost = new Argument<string>(
            name: "--dbhost",
            description: "Database hostname",
            getDefaultValue: () => "localhost");

var dbport = new Argument<int>(
            name: "--dbport",
            description: "Database port",
            getDefaultValue: () => 5432);

var dbname = new Argument<string>(
            name: "--dbname",
            description: "Database name",
            getDefaultValue: () => "tankmandb");

var dbuser = new Argument<string?>(
            name: "--dbuser",
            description: "Database username",
            getDefaultValue: () => null);

var dbpass = new Argument<string?>(
            name: "--dbpass",
            description: "Database username",
            getDefaultValue: () => null);

rootCommand.AddArgument(dbhost);
rootCommand.AddArgument(dbport);
rootCommand.AddArgument(dbname);
rootCommand.AddArgument(dbuser);
rootCommand.AddArgument(dbpass);

rootCommand.SetHandler((context) =>
{
    var host = context.ParseResult.GetValueForArgument(dbhost);
    var port = context.ParseResult.GetValueForArgument(dbport);
    var dbName = context.ParseResult.GetValueForArgument(dbname);
    var user = context.ParseResult.GetValueForArgument(dbuser);
    var password = context.ParseResult.GetValueForArgument(dbpass);

    if (user != null && password != null)
    {
        TankmanDbContext.SetConnectionStringFromArgs($"Server={host};Port={port};Database={dbName};User Id={user};Password={password}");
    }
});

rootCommand.Invoke(args);

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNameCaseInsensitive = false;
    options.SerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    options.SerializerOptions.DictionaryKeyPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    options.SerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

var orgsApi = app.MapGroup("orgs");
orgsApi.MapGet("/", OrgHandlers.GetOrgsAsync).WithOpenApi();
orgsApi.MapPost("/", OrgHandlers.CreateOrgAsync).WithOpenApi();
orgsApi.MapGet("/{orgId}", OrgHandlers.GetOrgAsync).WithOpenApi();
orgsApi.MapPatch("/{orgId}", OrgHandlers.PatchOrgAsync).WithOpenApi();
orgsApi.MapGet("/{orgId}/users", UserHandlers.GetUsersAsync).WithOpenApi();
orgsApi.MapPost("/{orgId}/users", UserHandlers.CreateUserAsync).WithOpenApi();

var rolesApi = app.MapGroup("roles");
var resourcesApi = app.MapGroup("resources");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();

