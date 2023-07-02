using tankman.Services;
using tankman.Http;
using tankman.Utils;
using tankman.Models;

namespace tankman.RequestHandlers;

public class CreateOrg
{
  public required string Id { get; set; }
  public required string Data { get; set; }
}

public class UpdateOrg
{
  public required string Data { get; set; }
}

public static class OrgHandlers
{
  public static async Task<IResult> GetOrgsAsync(string? orgId, string? properties, string? sort, string? order, int? from, int? limit, HttpContext context)
  {
    var matchProperties = QueryStringUtils.GetPrefixedQueryDictionary("properties.", context);

    SortUserBy? sortBy = sort switch
    {
      "id" => SortUserBy.UserId,
      "createdAt" => SortUserBy.CreatedAt,
      _ => null
    };

    SortOrder? sortOrder = order switch
    {
      "asc" => SortOrder.Ascending,
      "desc" => SortOrder.Descending,
      _ => null
    };

    return ApiResult.ToResult(
      await OrgService.GetOrgsAsync(
        orgId: orgId ?? Settings.Wildcard,
        properties: properties,
        sortBy: sortBy,
        sortOrder: sortOrder,
        from: from,
        limit: limit,
        matchProperties: matchProperties
      ),
      Org.ToJson
    );
  }

  public static async Task<IResult> CreateOrgAsync(CreateOrg create)
  {
    return ApiResult.ToResult(await OrgService.CreateOrgAsync(orgId: create.Id, data: create.Data), Org.ToJson);
  }

  public static async Task<IResult> UpdateOrgAsync(string orgId, UpdateOrg update)
  {
    return ApiResult.ToResult(await OrgService.UpdateOrgAsync(orgId: orgId, data: update.Data), Org.ToJson);
  }

  public static async Task<IResult> DeleteOrgAsync(string orgId, string? safetyKey)
  {
    if (Settings.SafetyKey != null && safetyKey != Settings.SafetyKey)
    {
      return TypedResults.BadRequest("Missing org deletion key.");
    }
    return ApiResult.ToResult(await OrgService.DeleteOrgAsync(orgId: orgId));
  }

  public static async Task<IResult> GetPropertiesAsync(string orgId, string? name)
  {
    return ApiResult.ToResult(await OrgService.GetPropertiesAsync(orgId: orgId, name: name ?? Settings.Wildcard), DynamicProperty.ToJson);
  }

  public static async Task<IResult> UpdatePropertyAsync(string orgId, string name, UpdateProperty update)
  {
    return ApiResult.ToResult(await OrgService.UpdatePropertyAsync(orgId: orgId, name: name, value: update.Value, hidden: update.Hidden), DynamicProperty.ToJson);
  }

  public static async Task<IResult> DeletePropertyAsync(string orgId, string name)
  {
    return ApiResult.ToResult(await OrgService.DeletePropertyAsync(orgId: orgId, name: name));
  }
}