namespace VocaFy.NextPath.Platform.Monitoring.Domain.Model;

public enum MonitoringError
{
    None,
    HistoricalProfileNotFound,
    HistoricalProfileAlreadyExists,
    StudentGroupNotFound,
    DuplicateStudentGroupName,
    VocationalReportNotFound,
    GroupComparisonReportNotFound,
    UserNotFound,
    PsychologistNotAuthorized,
    InvalidObservation,
    InvalidStudentGroup,
    InvalidReportContent,
    InvalidFlag,
    InvalidReportType,
    AtLeastTwoGroupsRequired,
    ReportAlreadyValidated,
    OperationCancelled,
    DatabaseError,
    InternalServerError
}
