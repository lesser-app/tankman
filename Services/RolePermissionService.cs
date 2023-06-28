using tankman.Models;
using Microsoft.EntityFrameworkCore;
using tankman.Db;
using tankman.Utils;
using OneOf;
using tankman.Types;

namespace tankman.Services;

public static class RolePermissionService
{
  public static async Task<OneOf<List<RolePermission>, Error<string>>> GetPermissionsAsync(string orgId)
  {
    var dbContext = new TankmanDbContext();
    return await dbContext.RolePermissions.Where((x) => x.OrgId == orgId).ToListAsync();
  }

  public static async Task<OneOf<RolePermission, Error<string>>> CreateRolePermissionAsync(string roleId, string resourceId, string action, string orgId)
  {
    var normalizedResourceId = Paths.Normalize(resourceId);

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

  public static async Task<OneOf<List<RolePermission>, Error<string>>> GetRolePermissionsForResourceAsync(string resourceId, string orgId)
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
      return await dbContext.RolePermissions.Where((x) => x.ResourceId == normalizedResourceId && x.OrgId == orgId).ToListAsync();
    }
    else
    {

      return await dbContext.RolePermissions.Where((x) => EF.Functions.ILike(x.ResourceId, $"{normalizedResourceId}%") && x.OrgId == orgId).ToListAsync();
    }
  }
}
