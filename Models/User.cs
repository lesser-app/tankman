using Microsoft.EntityFrameworkCore;

namespace tankman.Models;

[PrimaryKey(nameof(Id))]
public class User
{
  public string Id { get; set; }
  public string IdentityProvider { get; set; }
  public string IdentityProviderUserId { get; set; }
  public DateTime CreatedAt { get; set; }
  public bool Active { get; set; }

  public Org Org { get; set; }
  public List<RoleAssignment> RoleAssignments { get; set; }
  public List<UserPermission> UserPermissions { get; set; }
}