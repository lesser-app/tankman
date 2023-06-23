namespace tankman.Services;
using Microsoft.EntityFrameworkCore;
using tankman.Models;

public static class OrgService
{

  public static async Task<Org> CreateOrgAsync(string id)
  {
    var dbContext = new TankmanDbContext();
    var org = new Org
    {
      Id = id,
      CreatedAt = DateTime.Now,
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