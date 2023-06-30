using tankman.Http;
using tankman.Utils;
using tankman.Services;

namespace tankman.RequestHandlers;

public class CreateResource
{
  public required string Id { get; set; }
}

public static class ResourceHandlers
{
  public static async Task<IResult> GetResourcesAsync(string? resourceId, string orgId, int? depth, int? from, int? limit)
  {
    return ApiResult.ToResult(await ResourceService.GetResourcesAsync(resourceId: resourceId ?? Settings.Wildcard, orgId: orgId, from: from, limit: limit));
  }

  public static async Task<IResult> CreateResourceAsync(string orgId, CreateResource createResource)
  {
    return ApiResult.ToResult(await ResourceService.CreateResourceAsync(resourceId: createResource.Id, orgId: orgId));
  }

  public static async Task<IResult> DeleteResourceAsync(string resourceId, string orgId)
  {
    return ApiResult.ToResult(await ResourceService.DeleteResourceAsync(resourceId: resourceId, orgId: orgId));
  }
}