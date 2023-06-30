using System.CommandLine;
using System.Text.Json.Serialization;
using tankman.Db;
using tankman.RequestHandlers;
using tankman.Utils;

var rootCommand = new RootCommand("Tankman user management and access control");

var dbHostOption = new Option<string>(
            name: "--dbhost",
            description: "Database hostname.",
            getDefaultValue: () => "localhost");

var dbPortOption = new Option<int>(
            name: "--dbport",
            description: "Database port.",
            getDefaultValue: () => 5432);

var dbNameOption = new Option<string>(
            name: "--dbname",
            description: "Database name.",
            getDefaultValue: () => "tankmandb");

var dbUserOption = new Option<string?>(
            name: "--dbuser",
            description: "Database username",
            getDefaultValue: () => null);

var dbPassOption = new Option<string?>(
            name: "--dbpass",
            description: "Database username",
            getDefaultValue: () => null);

var wildcardOption = new Option<string?>(
            name: "--wildcard",
            description: "Wildcard character to be used in matches.",
            getDefaultValue: () => "~");

var separatorOption = new Option<string?>(
            name: "--separator",
            description: "Separator character to be used in the url.",
            getDefaultValue: () => ",");

var maxResultsOptions = new Option<int>(
            name: "--max-results",
            description: "Maximum number of results to return.",
            getDefaultValue: () => 10000);

var safetyKeyArg = new Option<string?>(
  name: "--safety-key",
  description: "Require this key to be supplied as the querystring parameter 'safetyKey' for org deletion."
);

rootCommand.AddOption(dbHostOption);
rootCommand.AddOption(dbPortOption);
rootCommand.AddOption(dbNameOption);
rootCommand.AddOption(dbUserOption);
rootCommand.AddOption(dbPassOption);
rootCommand.AddOption(wildcardOption);
rootCommand.AddOption(separatorOption);
rootCommand.AddOption(safetyKeyArg);
rootCommand.AddOption(maxResultsOptions);

rootCommand.SetHandler((context) =>
{
  var host = context.ParseResult.GetValueForOption(dbHostOption);
  var port = context.ParseResult.GetValueForOption(dbPortOption);
  var dbName = context.ParseResult.GetValueForOption(dbNameOption);
  var user = context.ParseResult.GetValueForOption(dbUserOption);
  var password = context.ParseResult.GetValueForOption(dbPassOption);
  var wildcardCharacter = context.ParseResult.GetValueForOption(wildcardOption);
  var separatorCharacter = context.ParseResult.GetValueForOption(separatorOption);
  var safetyKey = context.ParseResult.GetValueForOption(safetyKeyArg);
  var maxResults = context.ParseResult.GetValueForOption(maxResultsOptions);

  Settings.MaxResults = maxResults;
  Settings.Wildcard = wildcardCharacter!;
  Settings.Separator = separatorCharacter!;

  if (safetyKey != null)
  {
    Settings.SafetyKey = safetyKey;
  }

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

// ORGS
orgsApi.MapGet("/{orgId?}", OrgHandlers.GetOrgsAsync).WithOpenApi();
orgsApi.MapPost("/", OrgHandlers.CreateOrgAsync).WithOpenApi();
orgsApi.MapPut("/{orgId}", OrgHandlers.UpdateOrgAsync).WithOpenApi();
orgsApi.MapDelete("/{orgId}", OrgHandlers.DeleteOrgAsync).WithOpenApi();
// ORG PROPERTIES
orgsApi.MapPut("/{orgId}/properties/{name}", OrgHandlers.UpdatePropertyAsync).WithOpenApi();
orgsApi.MapDelete("/{orgId}/properties/{name}", OrgHandlers.DeletePropertyAsync).WithOpenApi();

// TODO: support the following
// orgsApi.MapGet("/{orgId}?fields=name,location,country", ...
// orgsApi.MapDelete("/{orgId}/fields/name,location", ...

// RESOURCES
orgsApi.MapGet("/{orgId}/resources/{*resourceId}", ResourceHandlers.GetResourcesAsync).WithOpenApi();
orgsApi.MapPost("/{orgId}/resources", ResourceHandlers.CreateResourceAsync).WithOpenApi();
orgsApi.MapPut("/{orgId}/resources/{*resourceId}", ResourceHandlers.UpdateResourceAsync).WithOpenApi();
orgsApi.MapDelete("/{orgId}/resources/{*resourceId}", ResourceHandlers.DeleteResourceAsync).WithOpenApi();

// TODO: support the following
// orgsApi.MapGet("/{orgId}/resources?fields=name,location,country", ...
// orgsApi.MapDelete("/{orgId}/resources/fields/name,location", ...

// USERS
orgsApi.MapGet("/{orgId}/users/{userId?}", UserHandlers.GetUsersAsync).WithOpenApi();
orgsApi.MapPost("/{orgId}/users", UserHandlers.CreateUserAsync).WithOpenApi();
orgsApi.MapPut("/{orgId}/users/{userId}", UserHandlers.UpdateUserAsync).WithOpenApi();
orgsApi.MapDelete("/{orgId}/users/{userId}", UserHandlers.DeleteUserAsync).WithOpenApi();
// USER ROLES
orgsApi.MapPost("/{orgId}/users/{userId}/roles", UserHandlers.AssignRoleAsync).WithOpenApi();
orgsApi.MapDelete("/{orgId}/users/{userId}/roles/{roleId}", UserHandlers.UnassignRoleAsync).WithOpenApi();
// USER PERMISSIONS
orgsApi.MapGet("/{orgId}/users/{userId}/permissions/{action?}/{*resourceId}", UserPermissionHandlers.GetUserPermissionsAsync).WithOpenApi();
orgsApi.MapPost("/{orgId}/users/{userId}/permissions", UserPermissionHandlers.CreateUserPermissionAsync).WithOpenApi();
orgsApi.MapDelete("/{orgId}/users/{userId}/permissions/{action}/{*resourceId}", UserPermissionHandlers.DeleteUserPermissionAsync).WithOpenApi();
// COMBINED USER/ROLE
orgsApi.MapGet("/{orgId}/users/{userId}/effective-permissions/{action?}/{*resourceId}", UserPermissionHandlers.GetEffectivePermissionsAsync).WithOpenApi();
// ROLE PROPERTIES
orgsApi.MapPut("/{orgId}/users/{userId}/properties/{name}", UserHandlers.UpdatePropertyAsync).WithOpenApi();
orgsApi.MapDelete("/{orgId}/users/{userId}/properties/{name}", UserHandlers.DeletePropertyAsync).WithOpenApi();

// TODO: support the following
// orgsApi.MapGet("/{orgId}/users?fields=name,location,country", ...
// orgsApi.MapDelete("/{orgId}/users/fields/name,location", ...

// ROLES
orgsApi.MapGet("/{orgId}/roles/{roleId?}", RoleHandlers.GetRolesAsync).WithOpenApi();
orgsApi.MapPost("/{orgId}/roles", RoleHandlers.CreateRoleAsync).WithOpenApi();
orgsApi.MapPut("/{orgId}/roles/{roleId}", RoleHandlers.UpdateRoleAsync).WithOpenApi();
orgsApi.MapDelete("/{orgId}/roles/{roleId}", RoleHandlers.DeleteRoleAsync).WithOpenApi();
// ROLE PERMISSIONS
orgsApi.MapGet("/{orgId}/roles/{roleId}/permissions/{action?}/{*resourceId}", RolePermissionHandlers.GetRolePermissionsAsync).WithOpenApi();
orgsApi.MapPost("/{orgId}/roles/{roleId}/permissions", RolePermissionHandlers.CreateRolePermissionAsync).WithOpenApi();
orgsApi.MapDelete("/{orgId}/roles/{roleId}/permissions/{action}/{*resourceId}", RolePermissionHandlers.DeleteRolePermissionAsync).WithOpenApi();
// ROLE PROPERTIES
orgsApi.MapPut("/{orgId}/roles/{roleId}/properties/{name}", RoleHandlers.UpdatePropertyAsync).WithOpenApi();
orgsApi.MapDelete("/{orgId}/roles/{roleId}/properties/{name}", RoleHandlers.DeletePropertyAsync).WithOpenApi();

// TODO: support the following
// orgsApi.MapPost("/{orgId}/roles?fields=name,location,country", ...
// orgsApi.MapDelete("/{orgId}/roles/fields/name,location", ...

var rolesApi = app.MapGroup("roles");
var resourcesApi = app.MapGroup("resources");

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.Run();

