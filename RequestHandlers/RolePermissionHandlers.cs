using tankman.Http;
using tankman.Models;
using tankman.Services;
using tankman.Types;

namespace tankman.RequestHandlers;

public class CreateRolePermission
{
  public required string ResourceId { get; set; }
  public required string RoleId { get; set; }
  public required string Action { get; set; }
}

public static class RolePermissionHandlers
{
  public static async Task<IResult> GetRolePermissionsAsync(string orgId)
  {
    return ApiResult.ToResult(await RolePermissionService.GetPermissionsAsync(orgId));
  }

  public static async Task<IResult> CreateRolePermissionAsync(string orgId, CreateRolePermission createPermission)
  {
    return ApiResult.ToResult(await RolePermissionService.CreateRolePermissionAsync(createPermission.RoleId, createPermission.ResourceId, createPermission.Action, orgId));
  }

  public static async Task<IResult> GetRolePermissionsForResourceAsync(string resourceId, string orgId)
  {
    return ApiResult.ToResult(await RolePermissionService.GetRolePermissionsForResourceAsync(resourceId, orgId));
  }
}