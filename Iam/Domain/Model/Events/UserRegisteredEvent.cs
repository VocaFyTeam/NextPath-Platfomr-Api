using VocaFy.NextPath.Platform.Iam.Domain.Model.ValueObjects;
using VocaFy.NextPath.Platform.Shared.Domain.Model.Events;

namespace VocaFy.NextPath.Platform.Iam.Domain.Model.Events;

public record UserRegisteredEvent(Guid UserId, string Email, EUserRole Role) : IEvent;
