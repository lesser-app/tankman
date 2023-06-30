using Microsoft.EntityFrameworkCore;

namespace tankman.Models;

public class OrgJson
{
  public required string Id { get; set; }
  public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  public required string Data { get; set; }
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

  public static OrgJson ToJson(Org org)
  {
    return new OrgJson
    {
      Id = org.Id,
      CreatedAt = org.CreatedAt,
      Data = org.Data,
    };
  }
}