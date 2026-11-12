using VocaFy.NextPath.Platform.Community.Domain.Model.Aggregates;
using VocaFy.NextPath.Platform.Community.Interfaces.Rest.Resources;

namespace VocaFy.NextPath.Platform.Community.Interfaces.Rest.Transform;

public static class CommunityResourceFromEntityAssembler
{
    public static CommunityResource ToResourceFromEntity(InterestCommunity entity)
    {
        ArgumentNullException.ThrowIfNull(entity);
        return new CommunityResource(entity.Id.Value, entity.Name, entity.Description,
            entity.Members.Select(member => member.StudentId));
    }
}
