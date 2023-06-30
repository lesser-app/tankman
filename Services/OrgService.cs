using Microsoft.EntityFrameworkCore;
using tankman.Db;
using tankman.Models;
using OneOf;
using tankman.Types;
using tankman.Utils;

namespace tankman.Services;

public static class OrgService
{
  public static async Task<OneOf<List<Org>, Error<string>>> GetOrgsAsync(string orgId)
  {
    var dbContext = new TankmanDbContext();
    return await dbContext.Orgs
      .ApplyIdFilter(orgId)
      .ApplyLimit(null)
      .ToListAsync();
  }

  public static async Task<OneOf<Org, Error<string>>> CreateOrgAsync(string orgId, string data)
  {
    if (String.IsNullOrWhiteSpace(orgId))
    {
      return new Error<string>("name should not be empty.");
    }

    var dbContext = new TankmanDbContext();

    var org = new Org
    {
      Id = orgId,
      Data = data,
      CreatedAt = DateTime.UtcNow,
    };

    dbContext.Orgs.Add(org);
    await dbContext.SaveChangesAsync();

    return org;
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