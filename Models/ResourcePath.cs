using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace tankman.Models;

[PrimaryKey(nameof(ResourceId), nameof(OrgId))]
public class ResourcePath : IPathScanEntity, IOrgAssociated
{
  public required string ResourceId { get; set; }
  [ForeignKey(nameof(ResourceId) + "," + nameof(OrgId))]
  public Resource? Resource { get; set; } = null;

  public required string ParentId { get; set; }
  
  public required string Root1Id { get; set; }
  public required string Root2Id { get; set; }
  public required string Root3Id { get; set; }
  public required string Root4Id { get; set; }
  public required string Root5Id { get; set; }
  public required string Root6Id { get; set; }
  public required string Root7Id { get; set; }
  public required string Root8Id { get; set; }
  public required string Root9Id { get; set; }
  public required string Root10Id { get; set; }
  public required string Root11Id { get; set; }
  public required string Root12Id { get; set; }
  public required string Root13Id { get; set; }
  public required string Root14Id { get; set; }
  public required string Root15Id { get; set; }
  public required string Root16Id { get; set; }

  // How deep is the resource identifiers?
  // eg: /a/b/c = 3
  public required int Depth { get; set; }

  public required string OrgId { get; set; }
  [ForeignKey(nameof(OrgId))]
  public Org? Org { get; set; } = null;
}