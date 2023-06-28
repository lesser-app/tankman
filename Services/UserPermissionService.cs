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
    var isWildCard = resourceId.EndsWith("*");

    if (isWildCard)
    {
      resourceId = resourceId.Substring(0, resourceId.Length - 1);
    }

    var normalizedResourceId = Paths.Normalize(resourceId);

    var dbContext = new TankmanDbContext();

    if (!isWildCard)
    {
      return await dbContext.UserPermissions.Where((x) => x.ResourceId == normalizedResourceId && x.OrgId == orgId).ToListAsync();
    }
    else
    {

      return await dbContext.UserPermissions.Where((x) => EF.Functions.ILike(x.ResourceId, $"{normalizedResourceId}%") && x.OrgId == orgId).ToListAsync();
    }
  }
}
