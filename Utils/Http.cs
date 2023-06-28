namespace tankman.Http;

public static class ApiResult
{
  public static IResult ToResult<T1, T2>(OneOf.OneOf<T1, T2> result)
  {
    return result.IsT0 ? TypedResults.Ok(result.AsT0) : TypedResults.BadRequest(result.AsT1);
  }
}