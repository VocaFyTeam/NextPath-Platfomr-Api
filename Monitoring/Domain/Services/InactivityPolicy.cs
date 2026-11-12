namespace VocaFy.NextPath.Platform.Monitoring.Domain.Services;

public static class InactivityPolicy
{

    public const int ProlongedInactivityDays = 30;
    
    public static int DaysInactive(DateTimeOffset lastActivityAt, DateTimeOffset now)
    {
        var days = (int)Math.Floor((now - lastActivityAt).TotalDays);
        return Math.Max(0, days);
    }
    public static bool IsProlonged(DateTimeOffset lastActivityAt, DateTimeOffset now)
    {
        return DaysInactive(lastActivityAt, now) > ProlongedInactivityDays;
    }
}
