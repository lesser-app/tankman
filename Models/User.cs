using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace tankman.Models;

[PrimaryKey(nameof(Id), nameof(OrgId))]
public class User
{
  public required string Id { get; set; }

  public required string IdentityProvider { get; set; }

  public required string IdentityProviderUserId { get; set; }

  public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;

  public required bool Active { get; set; }

  public required string OrgId { get; set; }
  [ForeignKey(nameof(OrgId))]
  public Org? Org { get; set; } = null;

  [NotMapped]
  public List<string> Roles { get; set; } = new List<string>();

  public List<RoleAssignment> RoleAssignments { get; set; } = new List<RoleAssignment>();

  public List<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();
}