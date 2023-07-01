using tankman.Http;
using tankman.Utils;
using tankman.Services;
using tankman.Models;

namespace tankman.RequestHandlers;

public class CreateRole
{
  public required string Id { get; set; }
  public required string Data { get; set; }
}

public class UpdateRole
{
  public required string Data { get; set; }
}

public static class RoleHandlers
{
  public static async Task<IResult> GetRolesAsync(string? roleId, string orgId, string? properties, HttpContext context)
  {
    var matchProperties = QueryStringUtils.GetPrefixedQueryDictionary("properties.", context);
    return ApiResult.ToResult(await RoleService.GetRolesAsync(roleId ?? Settings.Wildcard, orgId: orgId, properties: properties, matchProperties: matchProperties), (List<Role> entities) => entities.Select(Role.ToJson));
  }

  public static async Task<IResult> CreateRoleAsync(string orgId, CreateRole create)
  {
    return ApiResult.ToResult(await RoleService.CreateRoleAsync(roleId: create.Id, data: create.Data, orgId: orgId), Role.ToJson);
  }

  public static async Task<IResult> UpdateRoleAsync(string roleId, string orgId, UpdateRole update)
  {
    return ApiResult.ToResult(await RoleService.UpdateRoleAsync(roleId: roleId, data: update.Data, orgId: orgId), Role.ToJson);
  }

  public static async Task<IResult> DeleteRoleAsync(string roleId, string orgId)
  {
    return ApiResult.ToResult(await RoleService.DeleteRoleAsync(roleId: roleId, orgId: orgId));
  }

  public static async Task<IResult> UpdatePropertyAsync(string orgId, string roleId, string name, UpdateProperty update)
  {
    return ApiResult.ToResult(await RoleService.UpdatePropertyAsync(orgId: orgId, roleId: roleId, name: name, value: update.Value, hidden: update.Hidden));
  }

  public static async Task<IResult> DeletePropertyAsync(string orgId, string roleId, string name)
  {
    return ApiResult.ToResult(await RoleService.DeletePropertyAsync(orgId: orgId, roleId: roleId, name: name));
  }
}