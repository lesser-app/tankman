using tankman.Models;
using Microsoft.EntityFrameworkCore;
using tankman.Db;
using OneOf;
using tankman.Types;
using tankman.Utils;

namespace tankman.Services;

public enum SortUserBy
{
  UserId,
  CreatedAt
}

public static class UserService
{
  public static async Task<OneOf<List<User>, Error<string>>> GetUsersAsync(string userId, string orgId, SortUserBy? sortBy, SortOrder? sortOrder, int? from, int? limit)
  {
    var dbContext = new TankmanDbContext();

    var query = dbContext.Users
      .ApplyOrgFilter(orgId)
      .ApplyIdFilter(userId);

    if (sortBy.HasValue)
    {
      query =
        sortBy == SortUserBy.UserId ?
          !sortOrder.HasValue || sortOrder.Value == SortOrder.Ascending ?
            query.OrderBy((x) => x.Id) :
            query.OrderByDescending((x) => x.Id) :
        sortBy == SortUserBy.CreatedAt ?
          !sortOrder.HasValue || sortOrder.Value == SortOrder.Ascending ?
            query.OrderBy((x) => x.CreatedAt) :
            query.OrderByDescending((x) => x.CreatedAt)
        : query;
    }

    var users = await query
      .Include((x) => x.RoleAssignments)
      .Include(x => x.Properties)      
      .ApplySkip(from)
      .ApplyLimit(limit)
      .ToListAsync();

    foreach (var user in users)
    {
      user.Roles = user.RoleAssignments.Select((x) => x.RoleId).ToList();
      user.RoleAssignments = new List<RoleAssignment>();
    }

    return users;
  }

  public static async Task<OneOf<User, Error<string>>> CreateUserAsync(string userId, string identityProviderUserId, string identityProvider, string data, string orgId)
  {
    var dbContext = new TankmanDbContext();
    var user = new User
    {
      Id = userId,
      Data = data,
      IdentityProviderUserId = identityProviderUserId,
      IdentityProvider = identityProvider,
      CreatedAt = DateTime.UtcNow,
      OrgId = orgId
    };
    dbContext.Users.Add(user);
    await dbContext.SaveChangesAsync();
    return user;
  }

  public static async Task<OneOf<User, Error<string>>> UpdateUserAsync(string userId, string identityProviderUserId, string identityProvider, string data, string orgId)
  {
    var dbContext = new TankmanDbContext();
    var user = await dbContext.Users.SingleAsync((x) => x.Id == userId && x.OrgId == orgId);
    user.IdentityProviderUserId = identityProviderUserId;
    user.IdentityProvider = identityProvider;
    user.Data = data;
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

  public static async Task<OneOf<bool, Error<string>>> UpdatePropertyAsync(string userId, string name, string value, string orgId)
  {
    var dbContext = new TankmanDbContext();

    var property = await dbContext.UserProperties.SingleOrDefaultAsync(x => x.Name == name && x.UserId == userId && x.OrgId == orgId);

    if (property != null)
    {
      property.Value = value;
    }
    else
    {
      var newProperty = new UserProperty
      {
        Name = name,
        Value = value,
        UserId = userId,
        OrgId = orgId,
        CreatedAt = DateTime.UtcNow
      };
      dbContext.UserProperties.Add(newProperty);
    }

    await dbContext.SaveChangesAsync();
    return true;
  }

  public static async Task<OneOf<bool, Error<string>>> DeletePropertyAsync(string userId, string name, string orgId)
  {
    var dbContext = new TankmanDbContext();

    var property = await dbContext.UserProperties.SingleOrDefaultAsync(x => x.Name == name && x.UserId == userId && x.OrgId == orgId);

    if (property != null)
    {
      dbContext.UserProperties.Remove(property);
      await dbContext.SaveChangesAsync();
    }

    return true;
  }
}