using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace tankman.Models;

[PrimaryKey(nameof(Name), nameof(OrgId))]
public class OrgProperty : DynamicProperty, IOrgAssociated
{
  public required string OrgId { get; set; }
  [ForeignKey(nameof(OrgId))]
  public Org? Org { get; set; } = null;
}