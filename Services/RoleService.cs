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
    return await dbContext.Roles.Where((x) => x.Org.Id == orgId).ToListAsync();
  }

  public static async Task<Role> CreateRoleAsync(string id, string orgId)
  {
    var org = new Org { Id = orgId };

    var role = new Role
    {
      Id = id,
      CreatedAt = DateTime.UtcNow,
      Org = org
    };

    var dbContext = new TankmanDbContext();
    dbContext.Roles.Add(role);
    dbContext.Entry(org).State = EntityState.Detached;
    await dbContext.SaveChangesAsync();
    return role;
  }

}
