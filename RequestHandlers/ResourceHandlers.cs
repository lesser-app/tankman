using tankman.Http;
using tankman.Utils;
using tankman.Services;
using tankman.Models;

namespace tankman.RequestHandlers;

public class CreateResource
{
  public required string Id { get; set; }
  public required string Data { get; set; }
}

public class UpdateResource
{
  public required string Data { get; set; }
}

public static class ResourceHandlers
{
  public static async Task<IResult> GetResourcesAsync(string? resourceId, string orgId, int? depth, int? from, int? limit)
  {
    return ApiResult.ToResult(
      await ResourceService.GetResourcesAsync(
        resourceId: resourceId ?? Settings.Wildcard,
        orgId: orgId,
        from: from,
        limit: limit
      ),
      (List<Resource> entities) => entities.Select(Resource.ToJson)
    );
  }

  public static async Task<IResult> CreateResourceAsync(string orgId, CreateResource create)
  {
    return ApiResult.ToResult(await ResourceService.CreateResourceAsync(resourceId: create.Id, data: create.Data, orgId: orgId), Resource.ToJson);
  }

  public static async Task<IResult> UpdateResourceAsync(string resourceId, string orgId, UpdateResource update)
  {
    return ApiResult.ToResult(await ResourceService.UpdateResourceAsync(resourceId: resourceId, data: update.Data, orgId: orgId), Resource.ToJson);
  }

  public static async Task<IResult> DeleteResourceAsync(string resourceId, string orgId)
  {
    return ApiResult.ToResult(await ResourceService.DeleteResourceAsync(resourceId: resourceId, orgId: orgId));
  }
}