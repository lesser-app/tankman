using tankman.Models;
using tankman.Utils;
using Microsoft.EntityFrameworkCore;
using tankman.Db;

namespace tankman.Services;

public static class RoleService
{
  public static async Task<List<Role>> GetRolesAsync(string orgId)
  {
    var dbContext = new TankmanDbContext();
    return await dbContext.Roles.Where((x) => x.OrgId == orgId).ToListAsync();
  }

  public static async Task<Role> CreateRoleAsync(string id, string orgId)
  {
    var dbContext = new TankmanDbContext();

    var role = new Role
    {
      Id = id,
      CreatedAt = DateTime.UtcNow,
      OrgId = orgId
    };

    dbContext.Roles.Add(role);
    await dbContext.SaveChangesAsync();
    return role;
  }

}
