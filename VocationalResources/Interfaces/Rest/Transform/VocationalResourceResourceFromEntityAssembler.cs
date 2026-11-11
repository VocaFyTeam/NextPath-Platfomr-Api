using VocaFy.NextPath.Platform.VocationalResources.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.VocationalResources.Interfaces.Rest.Resources;

namespace VocaFy.NextPath.Platform.VocationalResources.Interfaces.Rest.Transform;

public static class VocationalResourceResourceFromEntityAssembler
{
    public static VocationalResourceResource ToResourceFromEntity(VocationalResource entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        var lastChange = entity.UpdatedAt ?? entity.CreatedAt;
        return new VocationalResourceResource(entity.Id.Value, entity.PsychologistId.Value, entity.Title,
            entity.Category, entity.Description, entity.CoverUrl, entity.FileUrl,
            lastChange?.ToString("yyyy-MM-dd") ?? string.Empty);
    }
}
