using VocaFy.NextPath.Platform.Shared.Domain.Model.Errors;

namespace VocaFy.NextPath.Platform.VocationalResources.Domain.Model.Errors;

public static class VocationalResourcesErrors
{
    public static readonly Error VocationalResourceNotFound =
        Error.NotFound("VocationalResources.VocationalResourceNotFound", "The specified resource was not found.");

    public static readonly Error NotAPsychologist =
        Error.Forbidden("VocationalResources.NotAPsychologist", "Only psychologists can publish resources.");

    public static readonly Error NotResourceOwner =
        Error.Forbidden("VocationalResources.NotResourceOwner", "Only the psychologist who published it can change it.");

    public static Error DuplicateResourceTitle(string title) =>
        Error.Conflict("VocationalResources.DuplicateResourceTitle",
            $"You already published a resource titled '{title}'.");
}
