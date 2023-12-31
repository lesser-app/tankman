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
  public class MatchFields
  {
    public required string? identityProvider { get; set; }
    public required string? identityProviderUserId { get; set; }
  }

  public static async Task<OneOf<List<User>, Error<string>>> GetUsersAsync(
    string userId,
    string orgId,
    string? properties,
    SortUserBy? sortBy,
    SortOrder? sortOrder,
    int? from,
    int? limit,
    MatchFields matchFields,
    Dictionary<string, string> matchProperties
  )
  {
    var dbContext = new TankmanDbContext();

    var query = dbContext.Users
      .FilterByOrg(orgId)
      .FilterByIdPattern(userId)
      .FilterByProperties<User, UserProperty>(matchProperties);

    if (matchFields.identityProvider != null)
    {
      query = query.Where(x => x.IdentityProvider == matchFields.identityProvider);
    }

    if (matchFields.identityProviderUserId != null)
    {
      query = query.Where(x => x.IdentityProviderUserId == matchFields.identityProviderUserId);
    }

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

    var results = await query
      .Include((x) => x.RoleAssignments)
      .Include(x => x.Properties)
      .ApplySkip(from)
      .ApplyLimit(limit)
      .ToListAsync();

    var shownProps = properties != null ? properties.Split(",") : new string[] { };

    foreach (var result in results)
    {
      result.Roles = result.RoleAssignments.Select((x) => x.RoleId).ToList();
      result.RoleAssignments = new List<RoleAssignment>();
      result.Properties = result.Properties.Where(x => !x.Hidden || shownProps.Contains(x.Name)).ToList();
    }

    return results;
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

  public static async Task<OneOf<List<UserProperty>, Error<string>>> GetPropertiesAsync(string userId, string orgId, string name)
  {
    var dbContext = new TankmanDbContext();

    var query = dbContext.UserProperties
      .FilterByOrg(orgId)
      .FilterByUser(userId)
      .SelectProperties(name);

    return await query.ToListAsync();
  }


  public static async Task<OneOf<UserProperty, Error<string>>> UpdatePropertyAsync(string userId, string name, string value, bool hidden, string orgId)
  {
    var dbContext = new TankmanDbContext();

    var property = await dbContext.UserProperties.SingleOrDefaultAsync(x => x.Name == name && x.OrgId == orgId);

    if (property != null)
    {
      property.Value = value;
      property.Hidden = hidden;
      await dbContext.SaveChangesAsync();
      return property;
    }
    else
    {
      var newProperty = new UserProperty
      {
        Name = name,
        Value = value,
        Hidden = hidden,
        UserId = userId,
        OrgId = orgId,
        CreatedAt = DateTime.UtcNow
      };
      dbContext.UserProperties.Add(newProperty);
      await dbContext.SaveChangesAsync();
      return newProperty;
    }
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