namespace VocaFy.NextPath.Platform.VocationalResources.Domain.Model.Commands;

public record DeleteVocationalResourceCommand(Guid VocationalResourceId, Guid RequesterUserId);
