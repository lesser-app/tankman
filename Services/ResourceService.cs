using tankman.Models;
using Microsoft.EntityFrameworkCore;
using tankman.Db;
using tankman.Utils;
using OneOf;
using tankman.Types;

namespace tankman.Services;

public class ResourceResourcePathJoin : IPathScanEntityInJoin<ResourcePath>
{
  public required Resource Resource { get; set; }
  public required ResourcePath ResourcePath { get; set; }
}

public static class ResourceService
{
  public static async Task<OneOf<List<Resource>, Error<string>>> GetResourcesAsync(string resourceId, string orgId)
  {
    var dbContext = new TankmanDbContext();
    var normalizedResourceId = Paths.Normalize(resourceId);
    var parts = normalizedResourceId.Split("/").Skip(1).ToList();

    if (Paths.IsWildcard(resourceId))
    {
      var baseQuery = dbContext.Resources.ApplyOrgFilter(orgId);
      var resourcesQuery = baseQuery.Join(
        dbContext.ResourcePaths,
        x => x.Id,
        x => x.ResourceId,
        (res, resPath) => new ResourceResourcePathJoin { Resource = res, ResourcePath = resPath }
      )
      .ApplyPathScanFilterToJoin<ResourceResourcePathJoin, ResourcePath>(normalizedResourceId, parts.Count)
      .Select(x => x.Resource);

      return await resourcesQuery.ToListAsync();
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

    // We need to create the ResourcePath as well for fast retrieval.
    var partsArr = normalizedResourceId.Split("/");
    var parentParts = partsArr.Take(partsArr.Length - 1).Skip(1).ToList();

    var makeRootId = (int count) => parentParts.Count >= count ? ("/" + String.Join("/", parentParts.Take(count))) : "";

    var root1Id = parentParts.Count >= 1 ? ("/" + parentParts[0]) : "";
    var root2Id = makeRootId(2);
    var root3Id = makeRootId(3);
    var root4Id = makeRootId(4);
    var root5Id = makeRootId(5);
    var root6Id = makeRootId(6);
    var root7Id = makeRootId(7);
    var root8Id = makeRootId(8);
    var root9Id = makeRootId(9);
    var root10Id = makeRootId(10);
    var root11Id = makeRootId(11);
    var root12Id = makeRootId(12);
    var root13Id = makeRootId(13);
    var root14Id = makeRootId(14);
    var root15Id = makeRootId(15);
    var root16Id = makeRootId(16);

    var parentId = "/" + string.Join("/", parentParts);

    var resourcePath = new ResourcePath
    {
      ResourceId = normalizedResourceId,
      ParentId = parentId,
      Root1Id = root1Id,
      Root2Id = root2Id,
      Root3Id = root3Id,
      Root4Id = root4Id,
      Root5Id = root5Id,
      Root6Id = root6Id,
      Root7Id = root7Id,
      Root8Id = root8Id,
      Root9Id = root9Id,
      Root10Id = root10Id,
      Root11Id = root11Id,
      Root12Id = root12Id,
      Root13Id = root13Id,
      Root14Id = root14Id,
      Root15Id = root15Id,
      Root16Id = root16Id,
      Depth = parentParts.Count + 1,
      OrgId = orgId
    };

    dbContext.ResourcePaths.Add(resourcePath);

    await dbContext.SaveChangesAsync();
    return resource;
  }

  public static async Task<OneOf<bool, Error<string>>> DeleteResourceAsync(string resourceId, string orgId)
  {
    var dbContext = new TankmanDbContext();

    var normalizedResourceId = Paths.Normalize(resourceId);

    var resource = await dbContext.Resources.SingleAsync((x) => x.OrgId == orgId && x.Id == normalizedResourceId);
    dbContext.Resources.Remove(resource);

    var resourcePath = await dbContext.ResourcePaths.SingleAsync((x) => x.OrgId == orgId && x.ResourceId == normalizedResourceId);
    dbContext.ResourcePaths.Remove(resourcePath);

    await dbContext.SaveChangesAsync();

    return true;
  }
}
