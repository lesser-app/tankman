using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace tankman.Models;

public class RoleJson
{
  public required string Id { get; set; }
  public required string Data { get; set; }
  public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  public required string OrgId { get; set; }
  public required Dictionary<string, string> Properties { get; set; }
}

[PrimaryKey(nameof(Id), nameof(OrgId))]
public class Role : IEntity, IOrgAssociated, IHasDynamicProperties<RoleProperty>
{
  public required string Id { get; set; }

  public required string Data { get; set; }

  public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;

  public required string OrgId { get; set; }
  [ForeignKey(nameof(OrgId))]
  public Org? Org { get; set; }

  public List<RoleAssignment> RoleAssignments { get; set; } = new List<RoleAssignment>();

  public List<RolePermission> RolePermissions { get; set; } = new List<RolePermission>();

  public List<RoleProperty> Properties { get; set; } = new List<RoleProperty>();

  public static RoleJson ToJson(Role entity)
  {
    var properties = new Dictionary<string, string>();

    foreach (var property in entity.Properties)
    {
      properties[property.Name] = property.Value;
    }

    return new RoleJson
    {
      Id = entity.Id,
      CreatedAt = entity.CreatedAt,
      Data = entity.Data,
      OrgId = entity.OrgId,
      Properties = properties
    };
  }
}