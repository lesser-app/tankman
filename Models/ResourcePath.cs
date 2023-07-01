using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace tankman.Models;

[PrimaryKey(nameof(ResourceId), nameof(OrgId))]
public class ResourcePath : IPathSearchHelperEntity, IResourceAssociated, IOrgAssociated
{
  public required string ResourceId { get; set; }
  [ForeignKey(nameof(ResourceId) + "," + nameof(OrgId))]
  public Resource? Resource { get; set; } = null;

  public required string ParentId { get; set; }
  
  public required string Parent1Id { get; set; }
  public required string Parent2Id { get; set; }
  public required string Parent3Id { get; set; }
  public required string Parent4Id { get; set; }
  public required string Parent5Id { get; set; }
  public required string Parent6Id { get; set; }
  public required string Parent7Id { get; set; }
  public required string Parent8Id { get; set; }
  public required string Parent9Id { get; set; }
  public required string Parent10Id { get; set; }
  public required string Parent11Id { get; set; }
  public required string Parent12Id { get; set; }
  public required string Parent13Id { get; set; }
  public required string Parent14Id { get; set; }
  public required string Parent15Id { get; set; }
  public required string Parent16Id { get; set; }

  // How deep is the resource identifiers?
  // eg: /a/b/c = 3
  public required int Depth { get; set; }

  public required string OrgId { get; set; }
  [ForeignKey(nameof(OrgId))]
  public Org? Org { get; set; } = null;
}