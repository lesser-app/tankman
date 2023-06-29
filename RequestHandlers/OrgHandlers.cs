using tankman.Services;
using tankman.Http;
using tankman.Utils;

namespace tankman.RequestHandlers;

public class CreateOrg
{
  public required string Id { get; set; }
}

public class PatchOrg
{
  public required string Op { get; set; }
}

public static class OrgHandlers
{
  public static async Task<IResult> GetOrgsAsync(string? orgId)
  {
    return ApiResult.ToResult(await OrgService.GetOrgsAsync(orgId: orgId ?? Settings.Wildcard));
  }

  public static async Task<IResult> CreateOrgAsync(CreateOrg org)
  {
    return ApiResult.ToResult(await OrgService.CreateOrgAsync(orgId: org.Id));
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