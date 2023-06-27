using tankman.Models;
using tankman.Utils;
using Microsoft.EntityFrameworkCore;
using tankman.Db;

namespace tankman.Services;

public static class UserService
{

  public static async Task<User> CreateUserAsync(string id, string identityProviderUserId, string identityProvider, string orgId)
  {
    var org = new Org { Id = orgId };
    var user = new User
    {
      Id = id,
      IdentityProviderUserId = identityProviderUserId,
      IdentityProvider = identityProvider,
      Active = true,
      CreatedAt = DateTime.UtcNow,
      Org = org
    };
    var dbContext = new TankmanDbContext();
    dbContext.Users.Add(user);
    dbContext.Entry(org).State = EntityState.Detached;
    await dbContext.SaveChangesAsync();
    return user;
  }

  public static async Task<List<User>> GetUsersAsync(string orgId)
  {
    var dbContext = new TankmanDbContext();
    return await dbContext.Users.Where((x) => x.Org.Id == orgId).ToListAsync();
  }

  public static async Task DeactivateUserAsync(string userId)
  {
    var dbContext = new TankmanDbContext();
    var user = dbContext.Users.Single((x) => x.Id == userId);
    user.Active = false;
    await dbContext.SaveChangesAsync();
  }

  public static async Task<List<Role>> GetRolesAsync(string userId, string orgId)
  {
    var dbContext = new TankmanDbContext();
    return await (from user in dbContext.Users
                  join assignment in dbContext.RoleAssignments on user.Id equals assignment.User.Id
                  join role in dbContext.Roles on assignment.Role.Id equals role.Id
                  where user.Id == userId
                  where user.Org.Id == orgId
                  select role).ToListAsync();

  }

  public static async Task<List<UserPermission>> GetUserPermissionsAsync(string userId)
  {
    var dbContext = new TankmanDbContext();
    return await dbContext.UserPermissions.Where((x) => x.User.Id == userId).ToListAsync();
  }

}