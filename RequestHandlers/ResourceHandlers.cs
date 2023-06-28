using tankman.Http;
using tankman.Models;
using tankman.Services;
using tankman.Types;

namespace tankman.RequestHandlers;

public class CreateResource
{
  public required string Id { get; set; }
}

public static class ResourceHandlers
{
  public static async Task<IResult> GetResourcesAsync(string orgId)
  {
    return ApiResult.ToResult(await ResourceService.GetResourcesAsync(orgId));
  }

  public static async Task<IResult> CreateResourceAsync(string orgId, CreateResource createResource)
  {
    return ApiResult.ToResult(await ResourceService.CreateResourceAsync(createResource.Id, orgId));
  }
}