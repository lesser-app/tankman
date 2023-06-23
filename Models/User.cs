using Microsoft.EntityFrameworkCore;

namespace tankman.Models;

[PrimaryKey(nameof(Id))]
public class User
{
  public required string Id { get; set; }
  public required string IdentityProvider { get; set; }
  public required string IdentityProviderUserId { get; set; }
  public required string OrgId { get; set; }
  public required DateTimeOffset CreatedAt { get; set; }
  public required bool Active { get; set; }
  public List<RoleAssignment> RoleAssignments { get; set; } = new List<RoleAssignment>();
}