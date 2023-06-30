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
  public static async Task<IResult> GetOrgsAsync(string? orgId)
  {
    return ApiResult.ToResult(await OrgService.GetOrgsAsync(orgId: orgId ?? Settings.Wildcard), (List<Org> orgs) => orgs.Select(Org.ToJson));
  }

  public static async Task<IResult> CreateOrgAsync(CreateOrg createOrg)
  {
    return ApiResult.ToResult(await OrgService.CreateOrgAsync(orgId: createOrg.Id, data: createOrg.Data), Org.ToJson);
  }

  public static async Task<IResult> UpdateOrgAsync(string orgId, UpdateOrg updateOrg)
  {
    return ApiResult.ToResult(await OrgService.UpdateOrgAsync(orgId: orgId, data: updateOrg.Data), Org.ToJson);
  }

  public static async Task<IResult> DeleteOrgAsync(string orgId, string? safetyKey)
  {
    if (Settings.SafetyKey != null && safetyKey != Settings.SafetyKey)
    {
      return TypedResults.BadRequest("Missing org deletion key.");
    }
    return ApiResult.ToResult(await OrgService.DeleteOrgAsync(orgId: orgId));
  }
}