using tankman.Models;
using tankman.Services;

namespace tankman.RequestHandlers;

public class CreateUser
{
  public string Id { get; set; }
  public string IdentityProviderUserId { get; set; }
  public string IdentityProvider { get; set; }
}

public static class UserHandlers
{
  public static async Task<List<User>> GetUsersAsync(string orgId)
  {
    return await UserService.GetUsersAsync(orgId);
  }

  public static async Task<User> CreateUserAsync(string orgId, CreateUser createUser)
  {
    return await UserService.CreateUserAsync(createUser.Id, createUser.IdentityProviderUserId, createUser.IdentityProvider, orgId);
  }
}