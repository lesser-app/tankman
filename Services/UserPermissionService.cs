using tankman.Models;
using Microsoft.EntityFrameworkCore;
using tankman.Db;
using tankman.Utils;
using OneOf;
using tankman.Types;

namespace tankman.Services;

public static class UserPermissionService
{
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

  public static async Task<OneOf<List<UserPermission>, Error<string>>> GetUserPermissionsAsync(string userId, string resourceId, string action, string orgId)
  {
    var dbContext = new TankmanDbContext();

    var permissions = await dbContext.UserPermissions
      .ApplyOrgFilter(orgId)
      .ApplyUsersFilter(userId)
      .ApplyActionsFilter(action)
      .ApplyResourceFilter(resourceId)
      .ToListAsync();

    return permissions;
  }

  public static async Task<OneOf<bool, Error<string>>> DeleteUserPermissionsAsync(string userId, string resourceId, string action, string orgId)
  {
    var dbContext = new TankmanDbContext();

    var permission = await dbContext.UserPermissions
      .SingleAsync((x) => x.OrgId == orgId && x.UserId == userId && x.ResourceId == resourceId && x.Action == action);

    dbContext.UserPermissions.Remove(permission);
    await dbContext.SaveChangesAsync();

    return true;
  }

}
