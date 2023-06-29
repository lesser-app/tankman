using tankman.Models;
using Microsoft.EntityFrameworkCore;
using tankman.Db;
using tankman.Utils;
using OneOf;
using tankman.Types;

namespace tankman.Services;

public static class ResourceService
{
  public static async Task<OneOf<List<Resource>, Error<string>>> GetResourcesAsync(string resourceId, string orgId)
  {
    var dbContext = new TankmanDbContext();

    var resources = await dbContext.Resources
      .ApplyOrgFilter(orgId)
      .ApplyPathedIdFilter(resourceId)
      .ToListAsync();

    return resources;
  }

  public static async Task<OneOf<Resource, Error<string>>> CreateResourceAsync(string resourceId, string orgId)
  {
    var normalizedResourceId = Paths.Normalize(resourceId);

    var dbContext = new TankmanDbContext();
    var resource = new Resource
    {
      Id = normalizedResourceId,
      CreatedAt = DateTime.UtcNow,
      OrgId = orgId,
    };
    dbContext.Resources.Add(resource);
    await dbContext.SaveChangesAsync();
    return resource;
  }

  public static async Task<OneOf<bool, Error<string>>> DeleteResourceAsync(string resourceId, string orgId)
  {
    var dbContext = new TankmanDbContext();

    var normalizedResourceId = Paths.Normalize(resourceId);
    var resource = await dbContext.Resources.SingleAsync((x) => x.OrgId == orgId && x.Id == normalizedResourceId);

    dbContext.Resources.Remove(resource);
    await dbContext.SaveChangesAsync();

    return true;
  }
}
