using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace tankman.Models;

[PrimaryKey(nameof(Name), nameof(RoleId), nameof(OrgId))]
public class RoleProperty : IRoleAssociated, IOrgAssociated
{
  public required string Name { get; set; }
  public required string Value { get; set; }
  public required bool Hidden { get; set; }

  public required string RoleId { get; set; }
  [ForeignKey(nameof(RoleId) + "," + nameof(OrgId))]
  public Role? Role { get; set; }

  public required string OrgId { get; set; }
  [ForeignKey(nameof(OrgId))]
  public Org? Org { get; set; } = null;

  public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}