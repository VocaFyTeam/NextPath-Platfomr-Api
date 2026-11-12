using VocaFy.NextPath.Platform.Assessments.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Assessments.Interfaces.Rest.Resources;

namespace VocaFy.NextPath.Platform.Assessments.Interfaces.Rest.Transform;

public static class VocationalTestResourceFromEntityAssembler
{
    public static VocationalTestResource ToResourceFromEntity(VocationalTest entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        return new VocationalTestResource(
            entity.Id.Value,
            entity.Title,
            entity.ShortName,
            entity.Headline,
            entity.Description,
            entity.Instructions,
            AssessmentsEnumParser.ToText(entity.Type),
            entity.Featured,
            entity.Order,
            entity.Version,
            entity.DurationMinutes);
    }
}
