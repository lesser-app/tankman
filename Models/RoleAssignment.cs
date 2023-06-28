using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace tankman.Models;

[PrimaryKey(nameof(Id), nameof(OrgId))]
public class RoleAssignment
{
  public required string Id { get; set; }

  public required string UserId { get; set; }
  [ForeignKey(nameof(UserId) + "," + nameof(OrgId))]
  public User? User { get; set; }

  public required string RoleId { get; set; }
  [ForeignKey(nameof(RoleId) + "," + nameof(OrgId))]
  public Role? Role { get; set; }

  public DateTime CreatedAt { get; set; }

  public required string OrgId { get; set; }
  [ForeignKey(nameof(OrgId))]
  public Org? Org { get; set; } = null;
}