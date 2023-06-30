using tankman.Models;
using Microsoft.EntityFrameworkCore;
using tankman.Db;
using tankman.Utils;
using OneOf;
using tankman.Types;

namespace tankman.Services;

public class ResourceResourcePathJoin : IPathSearchHelperEntityInJoin<ResourcePath>
{
  public required Resource Resource { get; set; }
  public required ResourcePath ResourcePath { get; set; }
}

public static class ResourceService
{
  public static async Task<OneOf<List<Resource>, Error<string>>> GetResourcesAsync(string resourceId, string orgId, int? from, int? limit)
  {
    var dbContext = new TankmanDbContext();
    var (normalizedResourceId, isWildcard) = Paths.Normalize(resourceId);
    var parts = normalizedResourceId.Split("/").Skip(1).ToList();

    if (isWildcard)
    {
      if (normalizedResourceId == "/")
      {
        return await dbContext.Resources
          .ApplyOrgFilter(orgId)
          .ApplySkip(from)
          .ApplyLimit(limit)
          .ToListAsync();
      }
      else
      {
        var baseQuery = dbContext.Resources.ApplyOrgFilter(orgId);
        var resourcesQuery = baseQuery.Join(
          dbContext.ResourcePaths,
          x => x.Id,
          x => x.ResourceId,
          (res, resPath) => new ResourceResourcePathJoin { Resource = res, ResourcePath = resPath }
        )
        .UsePathScanOptimization<ResourceResourcePathJoin, ResourcePath>(normalizedResourceId, parts.Count)
        .Select(x => x.Resource);

        return await resourcesQuery
          .ApplySkip(from)
          .ApplyLimit(limit)
          .ToListAsync();
      }

    }
    else
    {
      var resources = await dbContext.Resources
        .ApplyOrgFilter(orgId)
        .Where(x => x.Id == normalizedResourceId)
        .ToListAsync();

      return resources;
    }
  }

  public static async Task<OneOf<Resource, Error<string>>> CreateResourceAsync(string resourceId, string data, string orgId)
  {
    var (normalizedResourceId, isWildcard) = Paths.Normalize(resourceId);

    var dbContext = new TankmanDbContext();
    var resource = new Resource
    {
      Id = normalizedResourceId,
      Data = data,
      CreatedAt = DateTime.UtcNow,
      OrgId = orgId,
    };
    dbContext.Resources.Add(resource);

    // We need to create the ResourcePath as well for fast retrieval.
    var partsArr = normalizedResourceId.Split("/");
    var parentParts = partsArr.Take(partsArr.Length - 1).Skip(1).ToList();

    var makeParentId = (int count) => parentParts.Count >= count ? ("/" + String.Join("/", parentParts.Take(count))) : "";

    var parentId = "/" + string.Join("/", parentParts);

    var resourcePath = new ResourcePath
    {
      ResourceId = normalizedResourceId,
      ParentId = parentId,
      Parent1Id = makeParentId(1),
      Parent2Id = makeParentId(2),
      Parent3Id = makeParentId(3),
      Parent4Id = makeParentId(4),
      Parent5Id = makeParentId(5),
      Parent6Id = makeParentId(6),
      Parent7Id = makeParentId(7),
      Parent8Id = makeParentId(8),
      Parent9Id = makeParentId(9),
      Parent10Id = makeParentId(10),
      Parent11Id = makeParentId(11),
      Parent12Id = makeParentId(12),
      Parent13Id = makeParentId(13),
      Parent14Id = makeParentId(14),
      Parent15Id = makeParentId(15),
      Parent16Id = makeParentId(16),
      Depth = parentParts.Count + 1,
      OrgId = orgId
    };

    dbContext.ResourcePaths.Add(resourcePath);

    await dbContext.SaveChangesAsync();

    // Clear this.
    resource.ResourcePath = null;
    return resource;
  }

  public static async Task<OneOf<bool, Error<string>>> DeleteResourceAsync(string resourceId, string orgId)
  {
    var dbContext = new TankmanDbContext();

    var (normalizedResourceId, isWildcard) = Paths.Normalize(resourceId);

    var resource = await dbContext.Resources.SingleAsync((x) => x.OrgId == orgId && x.Id == normalizedResourceId);
    dbContext.Resources.Remove(resource);

    var resourcePath = await dbContext.ResourcePaths.SingleAsync((x) => x.OrgId == orgId && x.ResourceId == normalizedResourceId);
    dbContext.ResourcePaths.Remove(resourcePath);

    await dbContext.SaveChangesAsync();

    return true;
  }
}
