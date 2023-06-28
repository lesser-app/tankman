using Microsoft.EntityFrameworkCore;

namespace tankman.Models;

[PrimaryKey(nameof(Id))]
public class Org
{
  public required string Id { get; set; }
  
  public required DateTime CreatedAt { get; set; } = DateTime.UtcNow;
  
  public required bool Active { get; set; } = true;
  
  public List<User> Users { get; set; } = new List<User>();
  
  public List<Resource> Resources { get; set; } = new List<Resource>();
  
  public List<Role> Roles { get; set; } = new List<Role>();
}