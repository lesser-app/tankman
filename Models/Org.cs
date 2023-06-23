using Microsoft.EntityFrameworkCore;

namespace tankman.Models;

[PrimaryKey(nameof(Id))]
public class Org
{
  public required string Id { get; set; }
  public required DateTime CreatedAt { get; set; }
}