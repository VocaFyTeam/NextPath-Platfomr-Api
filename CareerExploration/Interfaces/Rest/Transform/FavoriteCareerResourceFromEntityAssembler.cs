using VocaFy.NextPath.Platform.CareerExploration.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.CareerExploration.Interfaces.Rest.Resources;

namespace VocaFy.NextPath.Platform.CareerExploration.Interfaces.Rest.Transform;

public static class FavoriteCareerResourceFromEntityAssembler
{
    public static FavoriteCareerResource ToResourceFromEntity(FavoriteCareer entity)
    {
        return new FavoriteCareerResource(entity.Id, entity.StudentId, entity.CareerId);
    }
}
