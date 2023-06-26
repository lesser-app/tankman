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
      Active = true
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

  public static async Task<Org> GetOrgAsync(string id)
  {
    var dbContext = new TankmanDbContext();
    return await dbContext.Orgs.SingleAsync((x) => x.Id == id);
  }

  public static async Task<Org> ActivateOrgAsync(string id)
  {
    var dbContext = new TankmanDbContext();
    var org = dbContext.Orgs.Single((x) => x.Id == id);
    org.Active = true;
    await dbContext.SaveChangesAsync();
    return org;
  }

  public static async Task<Org> DeactivateOrgAsync(string id)
  {
    var dbContext = new TankmanDbContext();
    var org = dbContext.Orgs.Single((x) => x.Id == id);
    org.Active = false;
    await dbContext.SaveChangesAsync();
    return org;
  }
}