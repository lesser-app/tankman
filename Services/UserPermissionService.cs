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

  public static async Task<OneOf<List<UserPermission>, Error<string>>> GetUserPermissionsAsync(string userId, string resourceId, string action, string orgId, int? from, int? limit)
  {
    var dbContext = new TankmanDbContext();
    var (normalizedResourceId, isWildcard) = Paths.Normalize(resourceId);
    var parts = normalizedResourceId.Split("/").Skip(1).ToList();

    if (isWildcard)
    {
      if (normalizedResourceId == "/")
      {
        return await dbContext.UserPermissions
                  .FilterByOrg(orgId)
                  .FilterByUserPattern(userId)
                  .FilterByActionPattern(action)
                  .ApplySkip(from)
                  .ApplyLimit(limit)
                  .ToListAsync();
      }
      else
      {
        var baseQuery = dbContext.UserPermissions
            .FilterByOrg(orgId)
            .FilterByUserPattern(userId)
            .FilterByActionPattern(action);

        var userPermissionsQuery = baseQuery.Join(
            dbContext.ResourcePaths,
            (x) => x.ResourceId,
            (x) => x.ResourceId,
            (perm, path) => new UserPermissionResourcePathJoin { UserPermission = perm, ResourcePath = path }
          )
          .FilterWithPathScanOptimization<UserPermissionResourcePathJoin, ResourcePath>(normalizedResourceId, parts.Count)
          .Select((x) => x.UserPermission);

        return await userPermissionsQuery
          .ApplySkip(from)
          .ApplyLimit(limit)
          .ToListAsync();
      }
    }
    else
    {
      var permissions = await dbContext.UserPermissions
        .FilterByOrg(orgId)
        .FilterByUserPattern(userId)
        .FilterByActionPattern(action)
        .FilterByResource(normalizedResourceId)
        .ApplyLimit()
        .ToListAsync();

      return permissions;
    }
  }

  public static async Task<OneOf<List<object>, Error<string>>> GetEffectivePermissionsAsync(string userId, string resourceId, string action, string orgId, int? from, int? limit)
  {
    var dbContext = new TankmanDbContext();
    var userPermissions = await GetUserPermissionsAsync(userId: userId, resourceId: resourceId, action: action, orgId: orgId, from: from, limit: limit);

    if (userPermissions.IsT0)
    {
      var roles = await dbContext.RoleAssignments
        .Where(x => x.UserId == userId && x.OrgId == orgId)
        .ToListAsync();
      var rolePermissions = await RolePermissionService.GetRolePermissionsAsync(String.Join(",", roles.Select(x => x.RoleId)), resourceId, action, orgId, from, limit);
      if (rolePermissions.IsT0)
      {
        var allPermissions = new List<object>();
        allPermissions.AddRange(userPermissions.AsT0);
        allPermissions.AddRange(rolePermissions.AsT0);
        return allPermissions;
      }
      else
      {
        return userPermissions.AsT1;
      }
    }
    else
    {
      return userPermissions.AsT1;
    }
  }

  public static async Task<OneOf<bool, Error<string>>> DeleteUserPermissionsAsync(string userId, string resourceId, string action, string orgId)
  {
    var (normalizedResourceId, isWildcard) = Paths.Normalize(resourceId);
    var dbContext = new TankmanDbContext();

    var permission = await dbContext.UserPermissions
      .SingleAsync((x) => x.OrgId == orgId && x.UserId == userId && x.ResourceId == normalizedResourceId && x.Action == action);

    dbContext.UserPermissions.Remove(permission);
    await dbContext.SaveChangesAsync();

    return true;
  }

}
