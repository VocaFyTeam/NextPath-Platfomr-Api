namespace VocaFy.NextPath.Platform.Assessments.Domain.Model.ValueObjects;

/// <summary>
///     Lifecycle of a test result: in progress (answers saved) or completed (profile calculated).
/// </summary>
public enum ETestResultStatus
{
    Pending,
    Completed
}
