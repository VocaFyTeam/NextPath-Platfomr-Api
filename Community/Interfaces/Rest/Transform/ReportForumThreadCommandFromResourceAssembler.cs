using VocaFy.NextPath.Platform.Community.Domain.Model.Commands;
using VocaFy.NextPath.Platform.Community.Interfaces.Rest.Resources;

namespace VocaFy.NextPath.Platform.Community.Interfaces.Rest.Transform;

public static class ReportForumThreadCommandFromResourceAssembler
{
    public static ReportForumThreadCommand ToCommandFromResource(Guid threadId, ReportThreadResource resource)
    {
        ArgumentNullException.ThrowIfNull(resource);
        return new ReportForumThreadCommand(threadId, resource.Reason);
    }
}
