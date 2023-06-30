using tankman.Models;
using Microsoft.EntityFrameworkCore;
using tankman.Db;
using tankman.Utils;
using OneOf;
using tankman.Types;

namespace tankman.Services;


public class UserPermissionResourcePathJoin : IPathSearchHelperEntityInJoin<ResourcePath>
{
  public required UserPermission UserPermission { get; set; }
  public required ResourcePath ResourcePath { get; set; }
}

public static class UserPermissionService
{
  public static async Task<OneOf<UserPermission, Error<string>>> CreateUserPermissionAsync(string userId, string resourceId, string action, string orgId)
  {
    var (normalizedResourceId, isWildcard) = Paths.Normalize(resourceId);

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
    var (normalizedResourceId, isWildcard) = Paths.Normalize(resourceId);
    var parts = normalizedResourceId.Split("/").Skip(1).ToList();

    if (isWildcard)
    {
      if (normalizedResourceId == "/")
      {
        return await dbContext.UserPermissions
                   .ApplyOrgFilter(orgId)
                   .ApplyUsersFilter(userId)
                   .ApplyActionsFilter(action).ToListAsync();
      }
      else
      {
        var baseQuery = dbContext.UserPermissions
            .ApplyOrgFilter(orgId)
            .ApplyUsersFilter(userId)
            .ApplyActionsFilter(action);

        var userPermissionsQuery = baseQuery.Join(
            dbContext.ResourcePaths,
            (x) => x.ResourceId,
            (x) => x.ResourceId,
            (perm, path) => new UserPermissionResourcePathJoin { UserPermission = perm, ResourcePath = path }
          )
          .UsePathScanOptimization<UserPermissionResourcePathJoin, ResourcePath>(normalizedResourceId, parts.Count)
          .Select((x) => x.UserPermission);

        return await userPermissionsQuery.ToListAsync();
      }
    }
    else
    {
      var permissions = await dbContext.UserPermissions
        .ApplyOrgFilter(orgId)
        .ApplyUsersFilter(userId)
        .ApplyActionsFilter(action)
        .ApplyExactResourceFilter(resourceId)
        .ToListAsync();

      return permissions;
    }
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
