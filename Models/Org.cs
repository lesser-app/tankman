using Microsoft.EntityFrameworkCore;

namespace tankman.Models;

public class OrgJson
{
  public required string Id { get; set; }
  public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  public required string Data { get; set; }
  public required Dictionary<string, string> Properties { get; set; }
}

[PrimaryKey(nameof(Id))]
public class Org : IEntity
{
  public required string Id { get; set; }

  public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;

  public required string Data { get; set; }

  public List<User> Users { get; set; } = new List<User>();

  public List<Resource> Resources { get; set; } = new List<Resource>();

  public List<Role> Roles { get; set; } = new List<Role>();

  public List<OrgProperty> Properties { get; set; } = new List<OrgProperty>();

  public static OrgJson ToJson(Org entity)
  {
    var properties = new Dictionary<string, string>();

    foreach (var property in entity.Properties)
    {
      properties[property.Name] = property.Value;
    }

    return new OrgJson
    {
      Id = entity.Id,
      CreatedAt = entity.CreatedAt,
      Data = entity.Data,
      Properties = properties
    };
  }
}