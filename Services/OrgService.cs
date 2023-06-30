using Microsoft.EntityFrameworkCore;
using tankman.Db;
using tankman.Models;
using OneOf;
using tankman.Types;
using tankman.Utils;

namespace tankman.Services;

public static class OrgService
{
  public static async Task<OneOf<List<Org>, Error<string>>> GetOrgsAsync(string orgId, string? properties)
  {
    var dbContext = new TankmanDbContext();

    var results = await dbContext.Orgs
      .ApplyIdFilter(orgId)
      .Include(x => x.Properties)
      .ApplyLimit(null)
      .ToListAsync();

    var shownProps = properties != null ? properties.Split(",") : new string[] { };

    foreach (var result in results)
    {
      result.Properties = result.Properties.Where(x => !x.Hidden || shownProps.Contains(x.Name)).ToList();
    }

    return results;
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

  public static async Task<OneOf<Org, Error<string>>> UpdateOrgAsync(string orgId, string data)
  {
    var dbContext = new TankmanDbContext();

    var org = await dbContext.Orgs.SingleAsync(x => x.Id == orgId);
    org.Data = data;
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

  public static async Task<OneOf<bool, Error<string>>> UpdatePropertyAsync(string orgId, string name, string value, bool hidden)
  {
    var dbContext = new TankmanDbContext();

    var property = await dbContext.OrgProperties.SingleOrDefaultAsync(x => x.Name == name && x.OrgId == orgId);

    if (property != null)
    {
      property.Value = value;
    }
    else
    {
      var newProperty = new OrgProperty
      {
        Name = name,
        Value = value,
        Hidden = hidden,
        OrgId = orgId,
        CreatedAt = DateTime.UtcNow
      };
      dbContext.OrgProperties.Add(newProperty);
    }

    await dbContext.SaveChangesAsync();
    return true;
  }

  public static async Task<OneOf<bool, Error<string>>> DeletePropertyAsync(string orgId, string name)
  {
    var dbContext = new TankmanDbContext();

    var property = await dbContext.OrgProperties.SingleOrDefaultAsync(x => x.Name == name && x.OrgId == orgId);

    if (property != null)
    {
      dbContext.OrgProperties.Remove(property);
      await dbContext.SaveChangesAsync();
    }

    return true;
  }
}