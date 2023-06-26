using tankman.Services;

namespace tankman.RequestHandlers;

public class CreateOrg
{
  public string Id { get; set; }
}

public static class OrgHandlers
{
  public async static Task<Models.Org> CreateOrg(CreateOrg org)
  {
    var savedOrg = await OrgService.CreateOrgAsync(org.Id);
    return savedOrg;
  }
}