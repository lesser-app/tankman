namespace tankman.Http;

public static class ApiResult
{
  public static IResult ToResult<T0, T1>(OneOf.OneOf<T0, T1> result)
  {
    return result.IsT0 ? TypedResults.Ok(result.AsT0) : TypedResults.BadRequest(result.AsT1);
  }

  public static IResult ToResult<T0, T1, TResult>(OneOf.OneOf<T0, T1> result, Func<T0, TResult> jsonTransform)
  {
    return result.IsT0 ? TypedResults.Ok(jsonTransform(result.AsT0)) : TypedResults.BadRequest(result.AsT1);
  }
}