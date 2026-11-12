using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace NextPath.Platform.CareerExploration.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

/// <summary>
///     Stores a value object (or a list of them) as JSON in a single column. It works with any
///     EF Core provider, so the code does not depend on <c>ToJson()</c> support of the MySQL provider.
/// </summary>
internal static class JsonPropertyExtensions
{
    private static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web);

    public static PropertyBuilder<T> HasJsonConversion<T>(this PropertyBuilder<T> property) where T : class
    {
        var converter = new ValueConverter<T, string>(
            value => JsonSerializer.Serialize(value, Options),
            json => JsonSerializer.Deserialize<T>(json, Options)!);

        // Without a comparer EF cannot detect changes inside the list/object.
        var comparer = new ValueComparer<T>(
            (left, right) => JsonSerializer.Serialize(left, Options) == JsonSerializer.Serialize(right, Options),
            value => JsonSerializer.Serialize(value, Options).GetHashCode(),
            value => JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(value, Options), Options)!);

        property.HasConversion(converter);
        property.Metadata.SetValueComparer(comparer);
        property.HasColumnType("json");
        return property;
    }
}
