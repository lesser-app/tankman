using tankman.Models;
using Microsoft.EntityFrameworkCore;
using tankman.Db;
using tankman.Utils;
using OneOf;
using tankman.Types;

namespace tankman.Services;

public static class RolePermissionService
{
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

  public static async Task<OneOf<List<RolePermission>, Error<string>>> GetRolePermissionsAsync(string roleId, string resourceId, string action, string orgId)
  {
    var dbContext = new TankmanDbContext();

    var permissions = await dbContext.RolePermissions
      .ApplyOrgFilter(orgId)
      .ApplyRolesFilter(roleId)
      .ApplyActionsFilter(action)
      .ApplyResourceFilter(resourceId)
      .ToListAsync();

    return permissions;
  }

  public static async Task<OneOf<bool, Error<string>>> DeleteRolePermissionsAsync(string roleId, string resourceId, string action, string orgId)
  {
    var dbContext = new TankmanDbContext();

    var permission = await dbContext.RolePermissions
      .SingleAsync((x) => x.OrgId == orgId && x.RoleId == roleId && x.ResourceId == resourceId && x.Action == action);

    dbContext.RolePermissions.Remove(permission);
    await dbContext.SaveChangesAsync();

    return true;
  }

}
