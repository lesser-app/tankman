namespace tankman.Utils;

public static class Paths
{
  public static string Normalize(string path)
  {
    path = !path.StartsWith("/") ? "/" + path : path;
    path = path.EndsWith("/") ? path.Substring(0, path.Length - 1) : path;
    return path;
  }

  public static string StripFromEnd(string path, string text)
  {
    return path.Substring(0, path.Length - text.Length);
  }

  public static bool IsWildcard(string path)
  {
    return path.EndsWith(Settings.Wildcard);
  }
}