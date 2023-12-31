using System.CommandLine;
using System.Text.Json.Serialization;
using tankman.Db;
using tankman.RequestHandlers;
using tankman.Utils;

var rootCommand = new RootCommand("Tankman user management and access control");

var hostOption = new Option<string>(
            name: "--host",
            description: "Host name.",
            getDefaultValue: () => "localhost");

var portOption = new Option<int>(
            name: "--port",
            description: "Port.",
            getDefaultValue: () => 1989);

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
            description: "Database password",
            getDefaultValue: () => null);

var wildcardOption = new Option<string?>(
            name: "--wildcard",
            description: "Wildcard character to be used in matches.",
            getDefaultValue: () => "~");

var separatorOption = new Option<string?>(
            name: "--separator",
            description: "Separator character to be used in the url.",
            getDefaultValue: () => ",");

var maxResultsOption = new Option<int>(
            name: "--max-results",
            description: "Maximum number of results to return.",
            getDefaultValue: () => 10000);

var initDbOption = new Option<bool>(
            name: "--initdb",
            description: "Initialize the database.");

var safetyKeyArg = new Option<string?>(
  name: "--safety-key",
  description: "Require this key to be supplied as the querystring parameter 'safetyKey' for org deletion."
);

rootCommand.AddOption(hostOption);
rootCommand.AddOption(portOption);
rootCommand.AddOption(dbHostOption);
rootCommand.AddOption(dbPortOption);
rootCommand.AddOption(dbNameOption);
rootCommand.AddOption(dbUserOption);
rootCommand.AddOption(dbPassOption);
rootCommand.AddOption(wildcardOption);
rootCommand.AddOption(separatorOption);
rootCommand.AddOption(safetyKeyArg);
rootCommand.AddOption(maxResultsOption);
rootCommand.AddOption(initDbOption);

string host = Environment.GetEnvironmentVariable("TANKMAN_HOST") ?? "localhost";
int port = !String.IsNullOrEmpty(Environment.GetEnvironmentVariable("TANKMAN_PORT"))
  ? int.Parse(Environment.GetEnvironmentVariable("TANKMAN_PORT")!)
  : 1989;

bool initDb = false;

rootCommand.SetHandler((context) =>
{
  var hostOptionResult = context.ParseResult.FindResultFor(hostOption)!;
  var portOptionResult = context.ParseResult.FindResultFor(portOption)!;

  if (!hostOptionResult.IsImplicit)
  {
    host = hostOptionResult.GetValueOrDefault<string>()!;
  }

  if (!portOptionResult.IsImplicit)
  {
    port = portOptionResult.GetValueOrDefault<int>();
  }

  var dbHost = context.ParseResult.GetValueForOption(dbHostOption);
  var dbPort = context.ParseResult.GetValueForOption(dbPortOption);
  var dbName = context.ParseResult.GetValueForOption(dbNameOption);
  var dbUser = context.ParseResult.GetValueForOption(dbUserOption);
  var dbPassword = context.ParseResult.GetValueForOption(dbPassOption);
  var wildcardCharacter = context.ParseResult.GetValueForOption(wildcardOption);
  var separatorCharacter = context.ParseResult.GetValueForOption(separatorOption);
  var safetyKey = context.ParseResult.GetValueForOption(safetyKeyArg);
  var maxResults = context.ParseResult.GetValueForOption(maxResultsOption);
  initDb = context.ParseResult.GetValueForOption(initDbOption);

  Settings.MaxResults = maxResults;
  Settings.Wildcard = wildcardCharacter!;
  Settings.Separator = separatorCharacter!;

  if (safetyKey != null)
  {
    Settings.SafetyKey = safetyKey;
  }

  if (dbUser != null && dbPassword != null)
  {
    TankmanDbContext.SetConnectionStringFromArgs($"Server={dbHost};Port={dbPort};Database={dbName};User Id={dbUser};Password={dbPassword}");
  }


  if (!initDb)
  {

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
    orgsApi.MapGet("/{orgId}/properties/{name?}", OrgHandlers.GetPropertiesAsync).WithOpenApi();
    orgsApi.MapPut("/{orgId}/properties/{name}", OrgHandlers.UpdatePropertyAsync).WithOpenApi();
    orgsApi.MapDelete("/{orgId}/properties/{name}", OrgHandlers.DeletePropertyAsync).WithOpenApi();


    // RESOURCES
    orgsApi.MapGet("/{orgId}/resources/{*resourceId}", ResourceHandlers.GetResourcesAsync).WithOpenApi();
    orgsApi.MapPost("/{orgId}/resources", ResourceHandlers.CreateResourceAsync).WithOpenApi();
    orgsApi.MapPut("/{orgId}/resources/{*resourceId}", ResourceHandlers.UpdateResourceAsync).WithOpenApi();
    orgsApi.MapDelete("/{orgId}/resources/{*resourceId}", ResourceHandlers.DeleteResourceAsync).WithOpenApi();


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
    // COMBINED USER+ROLE PERMISSIONS
    orgsApi.MapGet("/{orgId}/users/{userId}/effective-permissions/{action?}/{*resourceId}", UserPermissionHandlers.GetEffectivePermissionsAsync).WithOpenApi();
    // ROLE PROPERTIES
    orgsApi.MapGet("/{orgId}/users/{userId}/properties/{name?}", UserHandlers.GetPropertiesAsync).WithOpenApi();
    orgsApi.MapPut("/{orgId}/users/{userId}/properties/{name}", UserHandlers.UpdatePropertyAsync).WithOpenApi();
    orgsApi.MapDelete("/{orgId}/users/{userId}/properties/{name}", UserHandlers.DeletePropertyAsync).WithOpenApi();


    // ROLES
    orgsApi.MapGet("/{orgId}/roles/{roleId?}", RoleHandlers.GetRolesAsync).WithOpenApi();
    orgsApi.MapPost("/{orgId}/roles", RoleHandlers.CreateRoleAsync).WithOpenApi();
    orgsApi.MapPut("/{orgId}/roles/{roleId}", RoleHandlers.UpdateRoleAsync).WithOpenApi();
    orgsApi.MapDelete("/{orgId}/roles/{roleId}", RoleHandlers.DeleteRoleAsync).WithOpenApi();
    // ROLE USERS
    orgsApi.MapGet("/{orgId}/roles/{roleId}/users", RoleHandlers.GetRoleUsersAsync).WithOpenApi();
    // ROLE PERMISSIONS
    orgsApi.MapGet("/{orgId}/roles/{roleId}/permissions/{action?}/{*resourceId}", RolePermissionHandlers.GetRolePermissionsAsync).WithOpenApi();
    orgsApi.MapPost("/{orgId}/roles/{roleId}/permissions", RolePermissionHandlers.CreateRolePermissionAsync).WithOpenApi();
    orgsApi.MapDelete("/{orgId}/roles/{roleId}/permissions/{action}/{*resourceId}", RolePermissionHandlers.DeleteRolePermissionAsync).WithOpenApi();
    // ROLE PROPERTIES
    orgsApi.MapGet("/{orgId}/roles/{roleId}/properties/{name?}", RoleHandlers.GetPropertiesAsync).WithOpenApi();
    orgsApi.MapPut("/{orgId}/roles/{roleId}/properties/{name}", RoleHandlers.UpdatePropertyAsync).WithOpenApi();
    orgsApi.MapDelete("/{orgId}/roles/{roleId}/properties/{name}", RoleHandlers.DeletePropertyAsync).WithOpenApi();

    var rolesApi = app.MapGroup("roles");
    var resourcesApi = app.MapGroup("resources");

    if (app.Environment.IsDevelopment())
    {
      app.UseSwagger();
      app.UseSwaggerUI();
    }

    app.Run($"http://{host}:{port}");

  }
  // initDb = true. Initialize the database.
  else
  {
    InitializeDb.Init();
  }

});

rootCommand.Invoke(args);
