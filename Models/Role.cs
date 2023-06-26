using Microsoft.EntityFrameworkCore;

namespace tankman.Models;

[PrimaryKey(nameof(Id))]
public class Role
{
  public string Id { get; set; }
  public string OrgId { get; set; } 
  public DateTime CreatedAt { get; set; }
}