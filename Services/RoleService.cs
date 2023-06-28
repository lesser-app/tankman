using tankman.Models;
using Microsoft.EntityFrameworkCore;
using tankman.Db;
using OneOf;
using tankman.Types;

namespace tankman.Services;

public static class RoleService
{
  public static async Task<OneOf<List<Role>, Error<string>>> GetRolesAsync(string orgId)
  {
    var dbContext = new TankmanDbContext();
    return await dbContext.Roles.Where((x) => x.OrgId == orgId).ToListAsync();
  }

  public static async Task<OneOf<Role, Error<string>>> CreateRoleAsync(string id, string orgId)
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
