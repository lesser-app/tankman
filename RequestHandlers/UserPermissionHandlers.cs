using tankman.Http;
using tankman.Models;
using tankman.Services;
using tankman.Types;

namespace tankman.RequestHandlers;

public class CreateUserPermission
{
  public required string ResourceId { get; set; }
  public required string UserId { get; set; }
  public required string Action { get; set; }
}

public static class UserPermissionHandlers
{
  public static async Task<IResult> GetUserPermissionsAsync(string orgId)
  {
    return ApiResult.ToResult(await UserPermissionService.GetPermissionsAsync(orgId));
  }

  public static async Task<IResult> CreateUserPermissionAsync(string orgId, CreateUserPermission createPermission)
  {
    return ApiResult.ToResult(await UserPermissionService.CreateUserPermissionAsync(createPermission.UserId, createPermission.ResourceId, createPermission.Action, orgId));
  }
}