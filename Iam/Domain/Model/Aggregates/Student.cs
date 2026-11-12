using VocaFy.NextPath.Platform.Iam.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Shared.Domain.Model.Aggregates.Internal;

namespace VocaFy.NextPath.Platform.Iam.Domain.Model.Aggregates;

public class Student : AggregateRoot
{
    public Student()
    {
        SchoolOrUniversity = string.Empty;
        CurrentCycle = string.Empty;
    }

    public Student(UserId ownerUserId, string school, string cycle, string? psychologistCode) : this()
    {
        Id = StudentId.New();
        UserId = ownerUserId;
        UpdateProfile(school, cycle);
        LinkToPsychologist(psychologistCode);
    }

    public StudentId Id { get; private set; }
    public UserId UserId { get; private set; }
    public string SchoolOrUniversity { get; private set; }
    public string CurrentCycle { get; private set; }
    public string? LinkedPsychologistCode { get; private set; }

    public Student UpdateProfile(string schoolOrUniversity, string currentCycle)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(schoolOrUniversity);
        ArgumentException.ThrowIfNullOrWhiteSpace(currentCycle);
        SchoolOrUniversity = schoolOrUniversity.Trim();
        CurrentCycle = currentCycle.Trim();
        return this;
    }

    public Student LinkToPsychologist(string? psychologistCode)
    {
        LinkedPsychologistCode = Psychologist.NormalizeLinkingCode(psychologistCode);
        return this;
    }
}
