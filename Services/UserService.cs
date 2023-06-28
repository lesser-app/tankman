using tankman.Models;
using Microsoft.EntityFrameworkCore;
using tankman.Db;
using OneOf;
using tankman.Types;

namespace tankman.Services;

public static class UserService
{
  public static async Task<OneOf<User, Error<string>>> CreateUserAsync(string id, string identityProviderUserId, string identityProvider, string orgId)
  {
    var dbContext = new TankmanDbContext();
    var user = new User
    {
      Id = id,
      IdentityProviderUserId = identityProviderUserId,
      IdentityProvider = identityProvider,
      Active = true,
      CreatedAt = DateTime.UtcNow,
      OrgId = orgId
    };
    dbContext.Users.Add(user);
    await dbContext.SaveChangesAsync();
    return user;
  }

  public static async Task<OneOf<List<User>, Error<string>>> GetUsersAsync(string orgId)
  {
    var dbContext = new TankmanDbContext();
    return await dbContext.Users.Where((x) => x.OrgId == orgId).ToListAsync();
  }

  public static async Task<OneOf<User, Error<string>>> DeactivateUserAsync(string userId)
  {
    var dbContext = new TankmanDbContext();
    var user = dbContext.Users.Single((x) => x.Id == userId);
    user.Active = false;
    await dbContext.SaveChangesAsync();
    return user;
  }

  public static async Task<OneOf<List<Role>, Error<string>>> GetRolesAsync(string userId, string orgId)
  {
    var dbContext = new TankmanDbContext();
    return await (from user in dbContext.Users
                  join assignment in dbContext.RoleAssignments on user.Id equals assignment.UserId
                  join role in dbContext.Roles on assignment.RoleId equals role.Id
                  where user.Id == userId
                  where user.OrgId == orgId
                  select role).ToListAsync();

  }

  public static async Task<OneOf<List<UserPermission>, Error<string>>> GetUserPermissionsAsync(string userId)
  {
    var dbContext = new TankmanDbContext();
    return await dbContext.UserPermissions.Where((x) => x.UserId == userId).ToListAsync();
  }

}