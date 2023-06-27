namespace tankman.Models;

public interface IPermission
{
  public abstract Resource Resource { get; set; }
  public string Action { get; set; }
  public DateTime CreatedAt { get; set; }
}