namespace NextPath.Platform.Monitoring.Interfaces.Rest.Transform;

public static class EnumResourceMapper
{
    public static string ToResource<TEnum>(TEnum value) where TEnum : struct, Enum
    {
        var name = value.ToString();
        return char.ToLowerInvariant(name[0]) + name[1..];
    }

    public static bool TryParse<TEnum>(string? value, out TEnum result) where TEnum : struct, Enum
    {
        result = default;
        return !string.IsNullOrWhiteSpace(value)
               && !int.TryParse(value, out _)
               && Enum.TryParse(value, true, out result)
               && Enum.IsDefined(result);
    }

    public static bool TryParseMany<TEnum>(IEnumerable<string> values, out List<TEnum> results)
        where TEnum : struct, Enum
    {
        results = new List<TEnum>();
        foreach (var value in values)
        {
            if (!TryParse<TEnum>(value, out var parsed)) return false;
            results.Add(parsed);
        }

        return true;
    }
}
