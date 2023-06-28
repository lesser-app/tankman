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

public static class UserHandlers
{
  public static async Task<IResult> GetUsersAsync(string orgId)
  {
    return ApiResult.ToResult(await UserService.GetUsersAsync(orgId));
  }

  public static async Task<IResult> CreateUserAsync(string orgId, CreateUser createUser)
  {
    return ApiResult.ToResult(await UserService.CreateUserAsync(createUser.Id, createUser.IdentityProviderUserId, createUser.IdentityProvider, orgId));
  }
}