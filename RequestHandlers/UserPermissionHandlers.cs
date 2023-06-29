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
  public static async Task<IResult> GetUserPermissionsAsync(string orgId, string userId, string action, string resourceId)
  {
    return ApiResult.ToResult(await UserPermissionService.GetUserPermissionsAsync(
      resourceId: resourceId,
      action: action,
      userId: userId,
      orgId: orgId
    ));
  }

  public static async Task<IResult> CreateUserPermissionAsync(string orgId, CreateUserPermission createPermission)
  {
    return ApiResult.ToResult(await UserPermissionService.CreateUserPermissionAsync(
      userId: createPermission.UserId,
      resourceId: createPermission.ResourceId,
      action: createPermission.Action,
      orgId: orgId
    ));
  }

  public static async Task<IResult> DeleteUserPermissionAsync(string orgId, string userId, string action, string resourceId)
  {
    return ApiResult.ToResult(await UserPermissionService.DeleteUserPermissionsAsync(
      resourceId: resourceId,
      action: action,
      userId: userId,
      orgId: orgId
    ));
  }
}