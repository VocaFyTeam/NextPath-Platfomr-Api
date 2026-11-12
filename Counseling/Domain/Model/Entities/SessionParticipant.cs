using VocaFy.NextPath.Platform.Counseling.Domain.Model.ValueObjects;

namespace VocaFy.NextPath.Platform.Counseling.Domain.Model.Entities;

/// <summary>
///     A student invited to a group session (participantStudentIds of the diagram).
///     Owned by GroupSession and stored in table session_participants (session_id, student_id).
/// </summary>
public class SessionParticipant
{
    public SessionParticipant()
    {
    }

    public SessionParticipant(StudentId participantStudentId)
    {
        StudentId = participantStudentId;
    }

    public StudentId StudentId { get; private set; }
}
