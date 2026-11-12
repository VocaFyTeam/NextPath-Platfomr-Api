using NextPath.Platform.CareerExploration.Domain.Model.Aggregates;
using NextPath.Platform.CareerExploration.Interfaces.Rest.Resources;

namespace NextPath.Platform.CareerExploration.Interfaces.Rest.Transform;

public static class FavoriteCareerResourceFromEntityAssembler
{
    public static FavoriteCareerResource ToResourceFromEntity(FavoriteCareer entity)
    {
        return new FavoriteCareerResource(entity.Id, entity.StudentId, entity.CareerId);
    }
}
