namespace tankman.Types;

public class Error<T>
{
  public T Value { get; set; }

  public Error(T value)
  {
    this.Value = value;
  }
}