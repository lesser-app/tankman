using Microsoft.EntityFrameworkCore;

namespace tankman.Models;

[PrimaryKey(nameof(Id))]
public class Role
{
  public required string Id { get; set; }
  public required string OrgId { get; set; } 
  public required DateTime CreatedAt { get; set; }
}