using VocaFy.NextPath.Platform.CareerExploration.Domain.Model.Commands;
using VocaFy.NextPath.Platform.CareerExploration.Domain.Model.Events;
using VocaFy.NextPath.Platform.CareerExploration.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Shared.Domain.Model.Aggregates.Internal;
using VocaFy.NextPath.Platform.Shared.Domain.Model.Entities;

namespace VocaFy.NextPath.Platform.CareerExploration.Domain.Model.Aggregates;

/// <summary>A career a student saved in the favorites list (HU19). Unique per (student, career).</summary>
public class FavoriteCareer : AggregateRoot, IAuditableEntity
{
    // Required by EF Core.
    protected FavoriteCareer()
    {
    }

    public FavoriteCareer(StudentId studentId, CareerId careerId)
    {
        StudentId = studentId;
        CareerId = careerId;
        AddDomainEvent(new FavoriteCareerAddedEvent(studentId, careerId));
    }

    public FavoriteCareer(AddFavoriteCareerCommand command) : this(command.StudentId, command.CareerId)
    {
    }

    public FavoriteCareerId Id { get; private set; }
    public StudentId StudentId { get; private set; }
    public CareerId CareerId { get; private set; }

    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
