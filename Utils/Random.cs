namespace tankman.Utils;

public static class TokenGenerator
{
  private static readonly Random random = new();

  public static string RandomString(int length)
  {
    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    return new string(Enumerable.Repeat(chars, length)
        .Select(s => s[random.Next(s.Length)]).ToArray());
  }
}