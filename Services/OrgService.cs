using Microsoft.EntityFrameworkCore;
using tankman.Db;
using tankman.Models;
using OneOf;
using tankman.Types;

namespace tankman.Services;

public static class OrgService
{
  public static async Task<OneOf<Org, Error<string>>> CreateOrgAsync(string orgId)
  {
    if (String.IsNullOrWhiteSpace(orgId))
    {
      return new Error<string>("name should not be empty.");
    }

    var dbContext = new TankmanDbContext();

    var org = new Org
    {
      Id = orgId,
      CreatedAt = DateTime.UtcNow,
    };

    dbContext.Orgs.Add(org);
    await dbContext.SaveChangesAsync();

    return org;
  }

  public static async Task<OneOf<List<Org>, Error<string>>> GetOrgsAsync()
  {
    var dbContext = new TankmanDbContext();
    return await dbContext.Orgs.ToListAsync();
  }

  public static async Task<OneOf<Org, Error<string>>> GetOrgAsync(string orgId)
  {
    var dbContext = new TankmanDbContext();
    return await dbContext.Orgs.SingleAsync((x) => x.Id == orgId);
  }

  public static async Task<OneOf<bool, Error<string>>> DeleteOrgAsync(string orgId)
  {
    var dbContext = new TankmanDbContext();
    var org = await dbContext.Orgs.SingleAsync((x) => x.Id == orgId);
    dbContext.Orgs.Remove(org);
    await dbContext.SaveChangesAsync();
    return true;
  }
}