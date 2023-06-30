namespace tankman.Http;

public class ValidResult
{
  public required object Data { get; set; }
}

public class ErrorResult
{
  public required object Error { get; set; }
}

public static class ApiResult
{
  public static IResult ToResult<T0, T1>(OneOf.OneOf<T0, T1> result)
  {
    return result.IsT0 ? TypedResults.Ok(new ValidResult { Data = result.AsT0! }) : TypedResults.BadRequest(new ErrorResult { Error = result.AsT1! });
  }

  public static IResult ToResult<T0, T1, TResult>(OneOf.OneOf<T0, T1> result, Func<T0, TResult> jsonTransform)
  {
    return result.IsT0 ? TypedResults.Ok(new ValidResult { Data = jsonTransform(result.AsT0)! }) : TypedResults.BadRequest(new ErrorResult { Error = result.AsT1! });
  }
}