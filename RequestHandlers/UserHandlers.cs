using tankman.Models;
using tankman.Services;

namespace tankman.RequestHandlers;

public static class UserHandlers
{
  public static async Task<List<User>> GetUsersAsync(string orgId)
  {
    return await UserService.GetUsersAsync(orgId);
  }

  public static async Task<List<User>> CreateUserAsync(string orgId)
  {
    return await UserService.GetUsersAsync(orgId);
  }
}