using Microsoft.EntityFrameworkCore;

namespace tankman.Models;

[PrimaryKey(nameof(Id))]
public class Org
{
  public string Id { get; set; }
  public DateTime CreatedAt { get; set; }
}