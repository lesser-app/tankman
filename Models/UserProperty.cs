using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace tankman.Models;

[PrimaryKey(nameof(Name), nameof(UserId), nameof(OrgId))]
public class UserProperty : IUserAssociated, IOrgAssociated
{
  public required string Name { get; set; }
  public required string Value { get; set; }

  public required string UserId { get; set; }
  [ForeignKey(nameof(UserId) + "," + nameof(OrgId))]
  public User? User { get; set; }

  public required string OrgId { get; set; }
  [ForeignKey(nameof(OrgId))]
  public Org? Org { get; set; } = null;

  public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}