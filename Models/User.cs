using Microsoft.EntityFrameworkCore;

namespace tankman.Models;

[PrimaryKey(nameof(Id))]
public class User
{
  public string Id { get; set; }
  public string IdentityProvider { get; set; }
  public string IdentityProviderUserId { get; set; }
  public string OrgId { get; set; }
  public DateTime CreatedAt { get; set; }
  public bool Active { get; set; }
  public List<RoleAssignment> RoleAssignments { get; set; } = new List<RoleAssignment>();
}