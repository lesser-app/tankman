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
  public static async Task<IResult> GetRolesAsync(string? roleId, string orgId)
  {
    return ApiResult.ToResult(await RoleService.GetRolesAsync(roleId ?? Settings.Wildcard, orgId: orgId), (List<Role> roles) => roles.Select(Role.ToJson));
  }

  public static async Task<IResult> CreateRoleAsync(string orgId, CreateRole createRole)
  {
    return ApiResult.ToResult(await RoleService.CreateRoleAsync(roleId: createRole.Id, data: createRole.Data, orgId: orgId), Role.ToJson);
  }

  public static async Task<IResult> UpdateRoleAsync(string roleId, string orgId, UpdateRole updateRole)
  {
    return ApiResult.ToResult(await RoleService.UpdateRoleAsync(roleId: roleId, data: updateRole.Data, orgId: orgId), Role.ToJson);
  }


  public static async Task<IResult> DeleteRoleAsync(string roleId, string orgId)
  {
    return ApiResult.ToResult(await RoleService.DeleteRoleAsync(roleId: roleId, orgId: orgId));
  }
}