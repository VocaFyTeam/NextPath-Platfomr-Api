using VocaFy.NextPath.Platform.Shared.Domain.Model.Errors;

namespace VocaFy.NextPath.Platform.Monitoring.Domain.Model.Errors;

public static class MonitoringErrors
{
    public static readonly Error HistoricalProfileNotFound =
        Error.NotFound("Monitoring.HistoricalProfileNotFound", "The specified student profile was not found.");

    public static readonly Error HistoricalProfileAlreadyExists =
        Error.Conflict("Monitoring.HistoricalProfileAlreadyExists",
            "The student already has a historical profile.");

    public static readonly Error StudentGroupNotFound =
        Error.NotFound("Monitoring.StudentGroupNotFound", "The specified student group was not found.");

    public static readonly Error DuplicateStudentGroupName =
        Error.Conflict("Monitoring.DuplicateStudentGroupName", "A group with the same name already exists.");

    public static readonly Error VocationalReportNotFound =
        Error.NotFound("Monitoring.VocationalReportNotFound", "The specified vocational report was not found.");

    public static readonly Error GroupComparisonReportNotFound =
        Error.NotFound("Monitoring.GroupComparisonReportNotFound",
            "The specified group comparison report was not found.");

    public static readonly Error UserNotFound =
        Error.NotFound("Monitoring.UserNotFound", "The specified student or psychologist does not exist.");

    public static readonly Error PsychologistNotAuthorized =
        Error.Forbidden("Monitoring.PsychologistNotAuthorized",
            "The authenticated psychologist is not authorized to access or modify this resource.");

    public static readonly Error InvalidObservation =
        Error.Validation("Monitoring.InvalidObservation", "The observation text is required and must not exceed the allowed length.");

    public static readonly Error InvalidStudentGroup =
        Error.Validation("Monitoring.InvalidStudentGroup",
            "The group name is required and the name or description exceed the allowed length.");

    public static readonly Error InvalidReportContent =
        Error.Validation("Monitoring.InvalidReportContent",
            "The report content is required and must not exceed the allowed length.");

    public static readonly Error InvalidFlag =
        Error.Validation("Monitoring.InvalidFlag", "One or more student flags are not valid.");

    public static readonly Error InvalidReportType =
        Error.Validation("Monitoring.InvalidReportType", "The report type is not valid.");

    public static readonly Error AtLeastTwoGroupsRequired =
        Error.Validation("Monitoring.AtLeastTwoGroupsRequired", "At least two different groups are required to compare.");

    public static readonly Error ReportAlreadyValidated =
        Error.Conflict("Monitoring.ReportAlreadyValidated", "The report is already validated and cannot be modified.");

    public static Error HistoricalProfileAlreadyExistsForStudent(int studentId) =>
        Error.Conflict("Monitoring.HistoricalProfileAlreadyExists",
            $"The student with id {studentId} already has a historical profile.");

    public static Error DuplicateStudentGroupNameWithName(string name) =>
        Error.Conflict("Monitoring.DuplicateStudentGroupName", $"A group named '{name}' already exists.");
}
