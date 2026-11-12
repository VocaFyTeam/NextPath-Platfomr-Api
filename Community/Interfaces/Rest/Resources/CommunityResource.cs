namespace VocaFy.NextPath.Platform.Community.Interfaces.Rest.Resources;

public record CommunityResource(Guid Id, string Name, string Description, IEnumerable<Guid> MemberIds);
