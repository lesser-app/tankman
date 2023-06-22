namespace tankman.Models;

public class Resource
{
  public required string Path { get; set; }
  public required string OrgId { get; set; }
  public required DateTime CreatedAt { get; set; }
}