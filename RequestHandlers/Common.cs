namespace tankman.RequestHandlers;

public class UpdateProperty
{
  public required string Value { get; set; }
  public bool Hidden { get; set; } = false;
}