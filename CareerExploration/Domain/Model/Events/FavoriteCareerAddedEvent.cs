using NextPath.Platform.CareerExploration.Domain.Model.ValueObjects;
using NextPath.Platform.Shared.Domain.Model.Events;

namespace NextPath.Platform.CareerExploration.Domain.Model.Events;

public class FavoriteCareerAddedEvent(StudentId studentId, CareerId careerId) : IEvent
{
    public StudentId StudentId { get; } = studentId;
    public CareerId CareerId { get; } = careerId;
}
