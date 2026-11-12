using VocaFy.NextPath.Platform.VocationalResources.Domain.Model.Events;
using VocaFy.NextPath.Platform.VocationalResources.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Shared.Domain.Model.Aggregates.Internal;

namespace VocaFy.NextPath.Platform.VocationalResources.Domain.Model.Aggregates;

/// <summary>
///     A vocational resource (guide, recommended reading, worksheet) that a psychologist publishes
///     in the resource library (table vocational_resources).
/// </summary>
public partial class VocationalResource : AggregateRoot
{
    public VocationalResource()
    {
        Title = string.Empty;
        Category = string.Empty;
        Description = string.Empty;
        CoverUrl = string.Empty;
        FileUrl = string.Empty;
    }

    // Parameter names differ from property names so EF Core uses the parameterless constructor on load.
    public VocationalResource(PsychologistId ownerId, string resourceTitle, string resourceCategory,
        string? resourceDescription, string? resourceCoverUrl, string resourceFileUrl) : this()
    {
        Id = VocationalResourceId.New();
        PsychologistId = ownerId;
        UpdateDetails(resourceTitle, resourceCategory, resourceDescription, resourceCoverUrl, resourceFileUrl);
        AddDomainEvent(new VocationalResourceCreatedEvent(Id, PsychologistId, Title));
    }

    public VocationalResourceId Id { get; private set; }
    public PsychologistId PsychologistId { get; private set; }
    public string Title { get; private set; }
    public string Category { get; private set; }
    public string Description { get; private set; }
    public string CoverUrl { get; private set; }
    public string FileUrl { get; private set; }

    public bool IsOwnedBy(PsychologistId psychologistId) => PsychologistId == psychologistId;

    public VocationalResource UpdateDetails(string title, string category, string? description, string? coverUrl,
        string fileUrl)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(title);
        ArgumentException.ThrowIfNullOrWhiteSpace(category);
        ArgumentException.ThrowIfNullOrWhiteSpace(fileUrl);
        Title = title.Trim();
        Category = category.Trim();
        Description = description?.Trim() ?? string.Empty;
        CoverUrl = coverUrl?.Trim() ?? string.Empty;
        FileUrl = fileUrl.Trim();
        return this;
    }
}
