using NextPath.Platform.Tasks.Domain.Model.ValueObjects;

namespace NextPath.Platform.Tasks.Interfaces.Rest.Transform;

/// <summary>Converts between the JSON values "pending"/"completed" and <see cref="ETaskStatus"/>.</summary>
public static class TaskStatusMapper
{
    public static bool TryParse(string? value, out ETaskStatus status)
    {
        // Enum.TryParse also accepts numbers ("1"), which we do not want in the public API.
        status = default;
        return !string.IsNullOrWhiteSpace(value)
               && Enum.GetNames<ETaskStatus>().Contains(value.Trim(), StringComparer.OrdinalIgnoreCase)
               && Enum.TryParse(value.Trim(), true, out status);
    }

    public static string ToResourceValue(ETaskStatus status) => status.ToString().ToLowerInvariant();
}
