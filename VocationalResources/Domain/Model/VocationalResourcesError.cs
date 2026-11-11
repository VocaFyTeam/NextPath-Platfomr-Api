namespace VocaFy.NextPath.Platform.VocationalResources.Domain.Model;

/// <summary>
///     Each name must exist as a key in Shared/Resources/Errors/ErrorMessages.resx.
/// </summary>
public enum VocationalResourcesError
{
    None,
    VocationalResourceNotFound,
    NotAPsychologist,
    NotResourceOwner,
    DuplicateResourceTitle,
    OperationCancelled,
    DatabaseError,
    InternalServerError
}
