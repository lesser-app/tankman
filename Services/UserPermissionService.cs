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
}
