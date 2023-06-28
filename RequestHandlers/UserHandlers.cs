using tankman.Http;
using tankman.Models;
using tankman.Services;
using tankman.Types;

namespace tankman.RequestHandlers;

public class CreateUser
{
  public required string Id { get; set; }
  public required string IdentityProviderUserId { get; set; }
  public required string IdentityProvider { get; set; }
}

public class AssignRole
{
  public required string RoleId { get; set; }
}

public static class UserHandlers
{
  public static async Task<IResult> GetUsersAsync(string orgId)
  {
    return ApiResult.ToResult(await UserService.GetUsersAsync(orgId));
  }

  public static async Task<IResult> GetUserAsync(string id, string orgId)
  {
    return ApiResult.ToResult(await UserService.GetUserAsync(id, orgId));
  }

  public static async Task<IResult> CreateUserAsync(string orgId, CreateUser createUser)
  {
    return ApiResult.ToResult(await UserService.CreateUserAsync(createUser.Id, createUser.IdentityProviderUserId, createUser.IdentityProvider, orgId));
  }

  public static async Task<IResult> AssignRoleAsync(string userId, string orgId, AssignRole assignRole)
  {
    return ApiResult.ToResult(await UserService.AssignRoleAsync(assignRole.RoleId, userId, orgId));
  }

  public static async Task<IResult> UnassignRoleAsync(string roleId, string userId, string orgId)
  {
    return ApiResult.ToResult(await UserService.UnassignRoleAsync(roleId, userId, orgId));
  }

  public static async Task<IResult> DeleteUserAsync( string userId, string orgId)
  {
    return ApiResult.ToResult(await UserService.DeleteUserAsync(userId, orgId));
  }
}