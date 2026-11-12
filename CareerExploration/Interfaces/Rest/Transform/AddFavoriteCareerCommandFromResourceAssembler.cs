using VocaFy.NextPath.Platform.CareerExploration.Domain.Model.Commands;
using VocaFy.NextPath.Platform.CareerExploration.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.CareerExploration.Interfaces.Rest.Resources;

namespace VocaFy.NextPath.Platform.CareerExploration.Interfaces.Rest.Transform;

public static class AddFavoriteCareerCommandFromResourceAssembler
{
    public static AddFavoriteCareerCommand ToCommandFromResource(CreateFavoriteCareerResource resource)
    {
        return new AddFavoriteCareerCommand(new StudentId(resource.StudentId), new CareerId(resource.CareerId));
    }
}
