namespace tankman.Types;

public class ApiResult<T>
{
  public T Data { get; set; }

  public ApiResult(T data)
  {
    this.Data = data;
  }
}