using tankman.Http;
using tankman.Utils;
using tankman.Services;

namespace tankman.RequestHandlers;

public class CreateRole
{
  public required string Id { get; set; }
}

public static class RoleHandlers
{
  public static async Task<IResult> GetRolesAsync(string? roleId, string orgId)
  {
    return ApiResult.ToResult(await RoleService.GetRolesAsync(roleId ?? Settings.Wildcard, orgId: orgId));
  }

  public static async Task<IResult> CreateRoleAsync(string orgId, CreateRole createRole)
  {
    return ApiResult.ToResult(await RoleService.CreateRoleAsync(roleId: createRole.Id, orgId: orgId));
  }

  public static async Task<IResult> DeleteRoleAsync(string roleId, string orgId)
  {
    return ApiResult.ToResult(await RoleService.DeleteRoleAsync(roleId: roleId, orgId: orgId));
  }
}