using tankman.Models;
using Microsoft.EntityFrameworkCore;
using tankman.Db;
using tankman.Utils;
using OneOf;
using tankman.Types;

namespace tankman.Services;

public class RolePermissionResourcePathJoin : IPathSearchHelperEntityInJoin<ResourcePath>
{
  public required RolePermission RolePermission { get; set; }
  public required ResourcePath ResourcePath { get; set; }
}

public static class RolePermissionService
{
  public static async Task<OneOf<RolePermission, Error<string>>> CreateRolePermissionAsync(string roleId, string resourceId, string action, string orgId)
  {
    var (normalizedResourceId, isWildcard) = Paths.Normalize(resourceId);

    var dbContext = new TankmanDbContext();
    var rolePermission = new RolePermission
    {
      RoleId = roleId,
      ResourceId = normalizedResourceId,
      Action = action,
      OrgId = orgId,
      CreatedAt = DateTime.UtcNow,
    };
    dbContext.RolePermissions.Add(rolePermission);
    await dbContext.SaveChangesAsync();
    return rolePermission;
  }

  public static async Task<OneOf<List<RolePermission>, Error<string>>> GetRolePermissionsAsync(string roleId, string resourceId, string action, string orgId, int? from, int? limit)
  {
    var dbContext = new TankmanDbContext();
    var (normalizedResourceId, isWildcard) = Paths.Normalize(resourceId);
    var parts = normalizedResourceId.Split("/").Skip(1).ToList();

    if (isWildcard)
    {
      if (normalizedResourceId == "/")
      {
        return await dbContext.RolePermissions
                  .FilterByOrg(orgId)
                  .FilterByRolePattern(roleId)
                  .FilterByActionPattern(action)
                  .ApplySkip(from)
                  .ApplyLimit(limit)
                  .ToListAsync();
      }
      else
      {
        var query = dbContext.RolePermissions
                  .FilterByOrg(orgId)
                  .FilterByRolePattern(roleId)
                  .FilterByActionPattern(action);

        var rolePermissionsQuery = query.Join(
            dbContext.ResourcePaths,
            (x) => x.ResourceId,
            (x) => x.ResourceId,
            (perm, path) => new RolePermissionResourcePathJoin { RolePermission = perm, ResourcePath = path }
          )
          .FilterWithPathScanOptimization<RolePermissionResourcePathJoin, ResourcePath>(normalizedResourceId, parts.Count)
          .Select((x) => x.RolePermission);

        return await rolePermissionsQuery
          .ApplySkip(from)
          .ApplyLimit(limit)
          .ToListAsync();
      }
    }
    else
    {
      var permissions = await dbContext.RolePermissions
        .FilterByOrg(orgId)
        .FilterByRolePattern(roleId)
        .FilterByActionPattern(action)
        .FilterByResource(normalizedResourceId)
        .ApplyLimit()
        .ToListAsync();

      return permissions;
    }
  }

  public static async Task<OneOf<bool, Error<string>>> DeleteRolePermissionsAsync(string roleId, string resourceId, string action, string orgId)
  {
    var (normalizedResourceId, isWildcard) = Paths.Normalize(resourceId);
    var dbContext = new TankmanDbContext();

    var permission = await dbContext.RolePermissions
      .SingleAsync((x) => x.OrgId == orgId && x.RoleId == roleId && x.ResourceId == normalizedResourceId && x.Action == action);

    dbContext.RolePermissions.Remove(permission);
    await dbContext.SaveChangesAsync();

    return true;
  }

}
