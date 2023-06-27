using Microsoft.EntityFrameworkCore;

namespace tankman.Models;

[PrimaryKey(nameof(Id))]
public class Org
{
  public string Id { get; set; }
  public DateTime CreatedAt { get; set; }
  public bool Active { get; set; }
  public List<User> Users { get; set; }
  public List<Resource> Resources { get; set; }
  public List<Role> Roles { get; set; }
}