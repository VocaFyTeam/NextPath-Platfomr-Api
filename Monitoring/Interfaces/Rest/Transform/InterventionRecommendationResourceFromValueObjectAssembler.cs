using NextPath.Platform.Monitoring.Domain.Model.ValueObjects;
using NextPath.Platform.Monitoring.Interfaces.Rest.Resources;

namespace NextPath.Platform.Monitoring.Interfaces.Rest.Transform;

public static class InterventionRecommendationResourceFromValueObjectAssembler
{
    public static InterventionRecommendationResource ToResourceFromValueObject(InterventionRecommendation valueObject)
    {
        ArgumentNullException.ThrowIfNull(valueObject);
        return new InterventionRecommendationResource(EnumResourceMapper.ToResource(valueObject.Type),
            EnumResourceMapper.ToResource(valueObject.Reason));
    }
}
