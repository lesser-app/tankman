using tankman.Models;
using Microsoft.EntityFrameworkCore;
using tankman.Db;
using OneOf;
using tankman.Types;
using tankman.Utils;

namespace tankman.Services;

public static class RoleService
{
  public static async Task<OneOf<List<Role>, Error<string>>> GetRolesAsync(string roleId, string orgId)
  {
    var dbContext = new TankmanDbContext();
    return await dbContext.Roles
      .ApplyOrgFilter(orgId)
      .ApplyIdFilter(roleId)
      .ApplyLimit()
      .ToListAsync();
  }

  public static async Task<OneOf<Role, Error<string>>> CreateRoleAsync(string roleId, string orgId)
  {
    var dbContext = new TankmanDbContext();

    var role = new Role
    {
      Id = roleId,
      CreatedAt = DateTime.UtcNow,
      OrgId = orgId
    };

    dbContext.Roles.Add(role);
    await dbContext.SaveChangesAsync();
    return role;
  }

  public static async Task<OneOf<bool, Error<string>>> DeleteRoleAsync(string roleId, string orgId)
  {
    var dbContext = new TankmanDbContext();
    var role = await dbContext.Roles.SingleAsync((x) => x.Id == roleId && x.OrgId == orgId);
    dbContext.Roles.Remove(role);
    await dbContext.SaveChangesAsync();
    return true;
  }
}
