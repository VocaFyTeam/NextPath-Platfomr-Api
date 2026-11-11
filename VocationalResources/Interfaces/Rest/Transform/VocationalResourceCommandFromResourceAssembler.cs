using VocaFy.NextPath.Platform.VocationalResources.Domain.Model.Commands;
using VocaFy.NextPath.Platform.VocationalResources.Interfaces.Rest.Resources;

namespace VocaFy.NextPath.Platform.VocationalResources.Interfaces.Rest.Transform;

public static class VocationalResourceCommandFromResourceAssembler
{
    public static CreateVocationalResourceCommand ToCreateCommand(Guid requesterUserId,
        SaveVocationalResourceResource r) =>
        new(requesterUserId, r.Title, r.Category, r.Description, r.CoverUrl, r.FileUrl);

    public static UpdateVocationalResourceCommand ToUpdateCommand(Guid resourceId, Guid requesterUserId,
        SaveVocationalResourceResource r) =>
        new(resourceId, requesterUserId, r.Title, r.Category, r.Description, r.CoverUrl, r.FileUrl);
}
