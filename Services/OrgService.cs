using Microsoft.EntityFrameworkCore;
using tankman.Db;
using tankman.Models;

namespace tankman.Services;

public static class OrgService
{
  public static async Task<Org> CreateOrgAsync(string id)
  {
    var dbContext = new TankmanDbContext();

    var org = new Org
    {
      Id = id,
      CreatedAt = DateTime.UtcNow,
    };

    dbContext.Orgs.Add(org);
    await dbContext.SaveChangesAsync();

    return org;
  }

  public static async Task<List<Org>> GetOrgsAsync()
  {
    var dbContext = new TankmanDbContext();
    return await dbContext.Orgs.ToListAsync();
  }
}