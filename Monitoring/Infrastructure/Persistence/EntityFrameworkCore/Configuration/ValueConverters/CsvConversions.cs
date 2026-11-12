using NextPath.Platform.Monitoring.Domain.Model.ValueObjects;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace NextPath.Platform.Monitoring.Infrastructure.Persistence.EntityFrameworkCore.Configuration.ValueConverters;

public static class CsvConversions
{
    public static string FlagsToCsv(List<EStudentFlag>? flags)
    {
        return flags is null ? string.Empty : string.Join(',', flags.Select(flag => (int)flag));
    }

    public static List<EStudentFlag> FlagsFromCsv(string? csv)
    {
        if (string.IsNullOrWhiteSpace(csv)) return new List<EStudentFlag>();
        return csv.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(value => (EStudentFlag)int.Parse(value))
            .ToList();
    }

    public static bool FlagsEqual(List<EStudentFlag>? first, List<EStudentFlag>? second)
    {
        if (ReferenceEquals(first, second)) return true;
        if (first is null || second is null) return false;
        return first.SequenceEqual(second);
    }

    public static int FlagsHash(List<EStudentFlag> flags)
    {
        var hash = new HashCode();
        foreach (var flag in flags) hash.Add(flag);
        return hash.ToHashCode();
    }

    public static ValueConverter<List<EStudentFlag>, string> CreateFlagsConverter()
    {
        return new ValueConverter<List<EStudentFlag>, string>(flags => FlagsToCsv(flags), csv => FlagsFromCsv(csv));
    }

    public static ValueComparer<List<EStudentFlag>> CreateFlagsComparer()
    {
        return new ValueComparer<List<EStudentFlag>>(
            (first, second) => FlagsEqual(first, second),
            flags => FlagsHash(flags),
            flags => flags.ToList());
    }
    
    public static string GroupIdsToCsv(List<StudentGroupId>? groupIds)
    {
        return groupIds is null ? string.Empty : string.Join(',', groupIds.Select(groupId => groupId.Value));
    }

    public static List<StudentGroupId> GroupIdsFromCsv(string? csv)
    {
        if (string.IsNullOrWhiteSpace(csv)) return new List<StudentGroupId>();
        return csv.Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(value => new StudentGroupId(int.Parse(value)))
            .ToList();
    }

    public static bool GroupIdsEqual(List<StudentGroupId>? first, List<StudentGroupId>? second)
    {
        if (ReferenceEquals(first, second)) return true;
        if (first is null || second is null) return false;
        return first.SequenceEqual(second);
    }

    public static int GroupIdsHash(List<StudentGroupId> groupIds)
    {
        var hash = new HashCode();
        foreach (var groupId in groupIds) hash.Add(groupId.Value);
        return hash.ToHashCode();
    }

    public static ValueConverter<List<StudentGroupId>, string> CreateGroupIdsConverter()
    {
        return new ValueConverter<List<StudentGroupId>, string>(groupIds => GroupIdsToCsv(groupIds),
            csv => GroupIdsFromCsv(csv));
    }

    public static ValueComparer<List<StudentGroupId>> CreateGroupIdsComparer()
    {
        return new ValueComparer<List<StudentGroupId>>(
            (first, second) => GroupIdsEqual(first, second),
            groupIds => GroupIdsHash(groupIds),
            groupIds => groupIds.ToList());
    }
}
