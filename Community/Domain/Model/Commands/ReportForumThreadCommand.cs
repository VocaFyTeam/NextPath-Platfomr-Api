namespace VocaFy.NextPath.Platform.Community.Domain.Model.Commands;

public record ReportForumThreadCommand(Guid ForumThreadId, string Reason);
