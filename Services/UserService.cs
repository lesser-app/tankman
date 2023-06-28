using tankman.Models;
using Microsoft.EntityFrameworkCore;
using tankman.Db;
using OneOf;
using tankman.Types;

namespace tankman.Services;

public static class UserService
{
  public static async Task<OneOf<List<User>, Error<string>>> GetUsersAsync(string orgId)
  {
    var dbContext = new TankmanDbContext();
    return await dbContext.Users.Where((x) => x.OrgId == orgId).ToListAsync();
  }

  public static async Task<OneOf<User, Error<string>>> GetUserAsync(string id, string orgId)
  {
    var dbContext = new TankmanDbContext();
    var userDetails = await dbContext.Users.Include((x) => x.RoleAssignments).SingleAsync((x) => x.Id == id && x.OrgId == orgId);
    userDetails.Roles = userDetails.RoleAssignments.Select((x) => x.RoleId).ToList();
    userDetails.RoleAssignments = new List<RoleAssignment>();
    return userDetails;
  }

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

  public static async Task<OneOf<User, Error<string>>> DeactivateUserAsync(string userId)
  {
    var dbContext = new TankmanDbContext();
    var user = dbContext.Users.Single((x) => x.Id == userId);
    user.Active = false;
    await dbContext.SaveChangesAsync();
    return user;
  }

  public static async Task<OneOf<RoleAssignment, Error<string>>> AssignRoleAsync(string roleId, string userId, string orgId)
  {
    var dbContext = new TankmanDbContext();
    var roleAssignment = new RoleAssignment
    {
      RoleId = roleId,
      UserId = userId,
      CreatedAt = DateTime.UtcNow,
      OrgId = orgId
    };
    dbContext.RoleAssignments.Add(roleAssignment);
    await dbContext.SaveChangesAsync();
    return roleAssignment;
  }

  public static async Task<OneOf<bool, Error<string>>> UnassignRoleAsync(string roleId, string userId, string orgId)
  {
    var dbContext = new TankmanDbContext();
    var roleAssignment = dbContext.RoleAssignments.Single((x) => x.RoleId == roleId && x.UserId == userId && x.OrgId == orgId);
    dbContext.RoleAssignments.Remove(roleAssignment);
    await dbContext.SaveChangesAsync();
    return true;
  }

  public static async Task<OneOf<List<UserPermission>, Error<string>>> GetUserPermissionsAsync(string userId)
  {
    var dbContext = new TankmanDbContext();
    return await dbContext.UserPermissions.Where((x) => x.UserId == userId).ToListAsync();
  }
}