namespace NextPath.Platform.Tasks.Interfaces.Rest.Resources;

/// <summary>Same JSON shape the frontend uses (<c>StudentTask</c>): status is "pending" or "completed".</summary>
public record StudentTaskResource(int Id, int StudentId, string Title, string Status, DateOnly? DueDate, int Order);
