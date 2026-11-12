using VocaFy.NextPath.Platform.VocationalResources.Application.CommandServices;
using VocaFy.NextPath.Platform.VocationalResources.Application.Internal.OutboundServices;
using VocaFy.NextPath.Platform.VocationalResources.Domain.Model;
using VocaFy.NextPath.Platform.VocationalResources.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.VocationalResources.Domain.Model.Commands;
using VocaFy.NextPath.Platform.VocationalResources.Domain.Model.Errors;
using VocaFy.NextPath.Platform.VocationalResources.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.VocationalResources.Domain.Repositories;
using VocaFy.NextPath.Platform.Shared.Application.Model;
using VocaFy.NextPath.Platform.Shared.Domain.Repositories;

namespace VocaFy.NextPath.Platform.VocationalResources.Application.Internal.CommandServices;

public class VocationalResourceCommandService(
    IVocationalResourceRepository vocationalResourceRepository,
    ExternalIamService externalIamService,
    IUnitOfWork unitOfWork)
    : IVocationalResourceCommandService
{
    public async Task<Result<VocationalResource>> Handle(CreateVocationalResourceCommand command,
        CancellationToken cancellationToken)
    {
        // Rule 1: only psychologists publish (asked to IAM through the ACL)
        var psychologistId = await externalIamService.FetchPsychologistIdByUserId(command.RequesterUserId,
            cancellationToken);
        if (psychologistId is null)
            return Result<VocationalResource>.Failure(VocationalResourcesErrors.NotAPsychologist,
                VocationalResourcesError.NotAPsychologist);

        // Rule 2: a psychologist cannot repeat a title
        if (await vocationalResourceRepository.ExistsByTitleAndPsychologistIdAsync(command.Title,
                psychologistId.Value, null, cancellationToken))
            return Result<VocationalResource>.Failure(VocationalResourcesErrors.DuplicateResourceTitle(command.Title),
                VocationalResourcesError.DuplicateResourceTitle);

        var resource = new VocationalResource(psychologistId.Value, command.Title, command.Category,
            command.Description, command.CoverUrl, command.FileUrl);
        await vocationalResourceRepository.AddAsync(resource, cancellationToken);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<VocationalResource>.Success(resource);
    }

    public async Task<Result<VocationalResource>> Handle(UpdateVocationalResourceCommand command,
        CancellationToken cancellationToken)
    {
        var owned = await FindOwnedResourceAsync(command.VocationalResourceId, command.RequesterUserId,
            cancellationToken);
        if (owned.IsFailure) return owned;
        var resource = owned.Value!;

        if (await vocationalResourceRepository.ExistsByTitleAndPsychologistIdAsync(command.Title,
                resource.PsychologistId, resource.Id, cancellationToken))
            return Result<VocationalResource>.Failure(VocationalResourcesErrors.DuplicateResourceTitle(command.Title),
                VocationalResourcesError.DuplicateResourceTitle);

        resource.UpdateDetails(command.Title, command.Category, command.Description, command.CoverUrl,
            command.FileUrl);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result<VocationalResource>.Success(resource);
    }

    public async Task<Result> Handle(DeleteVocationalResourceCommand command, CancellationToken cancellationToken)
    {
        var owned = await FindOwnedResourceAsync(command.VocationalResourceId, command.RequesterUserId,
            cancellationToken);
        if (owned.IsFailure) return Result.Failure(owned.DomainError, owned.Error!);

        vocationalResourceRepository.Remove(owned.Value!);
        await unitOfWork.CompleteAsync(cancellationToken);
        return Result.Success();
    }

    /// <summary>Loads the resource (tracked) and checks that the requester is the psychologist who owns it.</summary>
    private async Task<Result<VocationalResource>> FindOwnedResourceAsync(Guid resourceId, Guid requesterUserId,
        CancellationToken cancellationToken)
    {
        var resource = await vocationalResourceRepository.FindByIdAsync(new VocationalResourceId(resourceId),
            cancellationToken);
        if (resource is null)
            return Result<VocationalResource>.Failure(VocationalResourcesErrors.VocationalResourceNotFound,
                VocationalResourcesError.VocationalResourceNotFound);

        var psychologistId = await externalIamService.FetchPsychologistIdByUserId(requesterUserId,
            cancellationToken);
        if (psychologistId is null || !resource.IsOwnedBy(psychologistId.Value))
            return Result<VocationalResource>.Failure(VocationalResourcesErrors.NotResourceOwner,
                VocationalResourcesError.NotResourceOwner);

        return Result<VocationalResource>.Success(resource);
    }
}
