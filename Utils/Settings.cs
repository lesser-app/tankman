namespace tankman.Utils;

public static class Settings
{
  public static int MaxResults { get; set; } = 10000;
  public static string Wildcard { get; set; } = "~";
  public static string Separator { get; set; } = ",";
  public static string? SafetyKey { get; set; } = null;
}