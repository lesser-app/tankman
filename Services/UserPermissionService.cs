using tankman.Models;
using Microsoft.EntityFrameworkCore;
using tankman.Db;
using tankman.Utils;
using OneOf;
using tankman.Types;

namespace tankman.Services;

public static class UserPermissionService
{
  public static async Task<OneOf<List<UserPermission>, Error<string>>> GetPermissionsAsync(string orgId)
  {
    var dbContext = new TankmanDbContext();
    return await dbContext.UserPermissions.Where((x) => x.OrgId == orgId).ToListAsync();
  }

  public static async Task<OneOf<UserPermission, Error<string>>> CreateUserPermissionAsync(string userId, string resourceId, string action, string orgId)
  {
    var normalizedResourceId = Paths.Normalize(resourceId);

    var dbContext = new TankmanDbContext();
    var userPermission = new UserPermission
    {
      UserId = userId,
      ResourceId = normalizedResourceId,
      Action = action,
      OrgId = orgId,
      CreatedAt = DateTime.UtcNow,
    };
    dbContext.UserPermissions.Add(userPermission);
    await dbContext.SaveChangesAsync();
    return userPermission;
  }

  public static async Task<OneOf<List<UserPermission>, Error<string>>> GetUserPermissionsForResourceAsync(string resourceId, string orgId)
  {
    var isWildCard = resourceId.EndsWith(Settings.Wildcard);

    if (isWildCard)
    {
      resourceId = Paths.StripFromEnd(resourceId, Settings.Wildcard);
    }

    var normalizedResourceId = Paths.Normalize(resourceId);

    var dbContext = new TankmanDbContext();

    var userPermissionsExact = await dbContext.UserPermissions.Where((x) => x.ResourceId == normalizedResourceId && x.OrgId == orgId).ToListAsync();

    if (!isWildCard)
    {
      return userPermissionsExact;
    }
    else
    {
      var childUserPermissions = await dbContext.UserPermissions.Where((x) => EF.Functions.ILike(x.ResourceId, $"{normalizedResourceId}/%") && x.OrgId == orgId).ToListAsync();
      return userPermissionsExact.Concat(childUserPermissions).ToList();
    }
  }

  public static async Task<OneOf<bool, Error<string>>> DeleteUserPermissionAsync(string userId, string resourceId, string action, string orgId)
  {
    var normalizedResourceId = Paths.Normalize(resourceId);
    var dbContext = new TankmanDbContext();
    if (action != Settings.Wildcard)
    {
      var userPermission = await dbContext.UserPermissions.SingleAsync((x) => x.UserId == userId && x.ResourceId == normalizedResourceId && x.Action == action && x.OrgId == orgId);
      dbContext.UserPermissions.Remove(userPermission);
    }
    else
    {
      var userPermissions = await dbContext.UserPermissions.Where((x) => x.UserId == userId && x.ResourceId == normalizedResourceId && x.Action == action && x.OrgId == orgId).ToListAsync();
      foreach (var userPermission in userPermissions)
      {
        dbContext.UserPermissions.Remove(userPermission);
      }
    }
    await dbContext.SaveChangesAsync();
    return true;
  }
}
