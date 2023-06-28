using tankman.Services;
using tankman.Http;

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
  public static async Task<IResult> GetOrgsAsync()
  {
    return ApiResult.ToResult(await OrgService.GetOrgsAsync());
  }

  public static async Task<IResult> CreateOrgAsync(CreateOrg org)
  {
    return ApiResult.ToResult(await OrgService.CreateOrgAsync(org.Id));


  }

  public static async Task<IResult> GetOrgAsync(string orgId)
  {
    return ApiResult.ToResult(await OrgService.GetOrgAsync(orgId));
  }

  public static async Task<IResult> PatchOrgAsync(string orgId, PatchOrg patchOrg)
  {
    if (patchOrg.Op.ToLower() == "activate")
    {
      return ApiResult.ToResult(await OrgService.ActivateOrgAsync(orgId));
    }
    else if (patchOrg.Op.ToLower() == "deactivate")
    {
      return ApiResult.ToResult(await OrgService.DeactivateOrgAsync(orgId));
    }
    else
    {
      return TypedResults.BadRequest("Invalid operation " + patchOrg.Op.ToLower());
    }
  }
}