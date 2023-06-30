namespace tankman.Utils;

public static class Paths
{
  public static (string, bool) Normalize(string path)
  {
    var isWildcard = IsWildcard(path);
    path = path.EndsWith(Settings.Wildcard) ? path.Substring(0, path.Length - 1) : path;
    path = !path.StartsWith("/") ? "/" + path : path;
    path = path.Length > 1 ? path.TrimEnd('/') : path;
    return (path, isWildcard);
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