using System.CommandLine;
using System.Text.Json.Serialization;
using tankman.Db;
using tankman.RequestHandlers;
using tankman.Utils;

var rootCommand = new RootCommand("Tankman user management and access control");

var dbHostOption = new Option<string>(
            name: "--dbhost",
            description: "Database hostname",
            getDefaultValue: () => "localhost");

var dbPortOption = new Option<int>(
            name: "--dbport",
            description: "Database port",
            getDefaultValue: () => 5432);

var dbNameOption = new Option<string>(
            name: "--dbname",
            description: "Database name",
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
            description: "Wildcard character",
            getDefaultValue: () => "~");

var safetyKeyArg = new Option<string?>(
  name: "--safety-key",
  description: "Protect destructive operations with a key."
);

rootCommand.AddOption(dbHostOption);
rootCommand.AddOption(dbPortOption);
rootCommand.AddOption(dbNameOption);
rootCommand.AddOption(dbUserOption);
rootCommand.AddOption(dbPassOption);
rootCommand.AddOption(wildcardOption);
rootCommand.AddOption(safetyKeyArg);

rootCommand.SetHandler((context) =>
{
  var host = context.ParseResult.GetValueForOption(dbHostOption);
  var port = context.ParseResult.GetValueForOption(dbPortOption);
  var dbName = context.ParseResult.GetValueForOption(dbNameOption);
  var user = context.ParseResult.GetValueForOption(dbUserOption);
  var password = context.ParseResult.GetValueForOption(dbPassOption);
  var wildcardCharacter = context.ParseResult.GetValueForOption(wildcardOption);
  var safetyKey = context.ParseResult.GetValueForOption(safetyKeyArg);

  Settings.Wildcard = wildcardCharacter!;

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
orgsApi.MapGet("/", OrgHandlers.GetOrgsAsync).WithOpenApi();
orgsApi.MapPost("/", OrgHandlers.CreateOrgAsync).WithOpenApi();
orgsApi.MapGet("/{orgId}", OrgHandlers.GetOrgAsync).WithOpenApi();
orgsApi.MapDelete("/{orgId}", OrgHandlers.DeleteOrgAsync).WithOpenApi();

// USERS
orgsApi.MapGet("/{orgId}/users", UserHandlers.GetUsersAsync).WithOpenApi();
orgsApi.MapGet("/{orgId}/users/{userId}", UserHandlers.GetUserAsync).WithOpenApi();
orgsApi.MapDelete("/{orgId}/users/{userId}", UserHandlers.DeleteUserAsync).WithOpenApi();
orgsApi.MapPost("/{orgId}/users/{userId}/roles", UserHandlers.AssignRoleAsync).WithOpenApi();
orgsApi.MapPost("/{orgId}/users", UserHandlers.CreateUserAsync).WithOpenApi();
orgsApi.MapDelete("/{orgId}/users/{userId}/roles/{roleId}", UserHandlers.UnassignRoleAsync).WithOpenApi();
orgsApi.MapDelete("/{orgId}/users/{userId}/permissions/{action}/{*resourceId}", UserPermissionHandlers.DeleteUserPermissionAsync).WithOpenApi();

// ROLES
orgsApi.MapGet("/{orgId}/roles", RoleHandlers.GetRolesAsync).WithOpenApi();
orgsApi.MapPost("/{orgId}/roles", RoleHandlers.CreateRoleAsync).WithOpenApi();
orgsApi.MapDelete("/{orgId}/roles/{roleId}", RoleHandlers.DeleteRoleAsync).WithOpenApi();
orgsApi.MapDelete("/{orgId}/roles/{roleId}/permissions/{action}/{*resourceId}", RolePermissionHandlers.DeleteRolePermissionAsync).WithOpenApi();

// RESOURCES
orgsApi.MapGet("/{orgId}/resources", ResourceHandlers.GetResourcesAsync).WithOpenApi();
orgsApi.MapPost("/{orgId}/resources", ResourceHandlers.CreateResourceAsync).WithOpenApi();
orgsApi.MapDelete("/{orgId}/resources/{*resourceId}", ResourceHandlers.DeleteResourceAsync).WithOpenApi();

// USER PERMISSIONS
orgsApi.MapGet("/{orgId}/user-permissions", UserPermissionHandlers.GetUserPermissionsAsync).WithOpenApi();
orgsApi.MapGet("/{orgId}/user-permissions/{*resourceId}", UserPermissionHandlers.GetUserPermissionsForResourceAsync).WithOpenApi();
orgsApi.MapPost("/{orgId}/user-permissions", UserPermissionHandlers.CreateUserPermissionAsync).WithOpenApi();


// ROLE PERMISSIONS
orgsApi.MapGet("/{orgId}/role-permissions", RolePermissionHandlers.GetRolePermissionsAsync).WithOpenApi();
orgsApi.MapGet("/{orgId}/role-permissions/{*resourceId}", RolePermissionHandlers.GetRolePermissionsForResourceAsync).WithOpenApi();
orgsApi.MapPost("/{orgId}/role-permissions", RolePermissionHandlers.CreateRolePermissionAsync).WithOpenApi();

var rolesApi = app.MapGroup("roles");
var resourcesApi = app.MapGroup("resources");

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.Run();

