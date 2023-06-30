using tankman.Models;
using Microsoft.EntityFrameworkCore;
using tankman.Db;
using OneOf;
using tankman.Types;
using tankman.Utils;

namespace tankman.Services;

public static class UserService
{
  public static async Task<OneOf<List<User>, Error<string>>> GetUsersAsync(string userId, string orgId)
  {
    var dbContext = new TankmanDbContext();
    var users = await dbContext.Users.ApplyIdFilter(userId)
      .Include((x) => x.RoleAssignments)
      .Take(Settings.MaxResults)
      .ToListAsync();

    foreach (var user in users)
    {
      user.Roles = user.RoleAssignments.Select((x) => x.RoleId).ToList();
      user.RoleAssignments = new List<RoleAssignment>();
    }

    return users;
  }

  public static async Task<OneOf<User, Error<string>>> CreateUserAsync(string userId, string identityProviderUserId, string identityProvider, string orgId)
  {
    var dbContext = new TankmanDbContext();
    var user = new User
    {
      Id = userId,
      IdentityProviderUserId = identityProviderUserId,
      IdentityProvider = identityProvider,
      CreatedAt = DateTime.UtcNow,
      OrgId = orgId
    };
    dbContext.Users.Add(user);
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
    var roleAssignment = await dbContext.RoleAssignments.SingleAsync((x) => x.OrgId == orgId && x.UserId == userId && x.RoleId == roleId);
    dbContext.RoleAssignments.Remove(roleAssignment);
    await dbContext.SaveChangesAsync();
    return true;
  }

  public static async Task<OneOf<bool, Error<string>>> DeleteUserAsync(string userId, string orgId)
  {
    var dbContext = new TankmanDbContext();
    var user = await dbContext.Users.SingleAsync((x) => x.Id == userId && x.OrgId == orgId);
    dbContext.Users.Remove(user);
    await dbContext.SaveChangesAsync();
    return true;
  }
}