using NextPath.Platform.Shared.Domain.Model.ValueObjects;

namespace NextPath.Platform.CareerExploration.Domain.Model.ValueObjects;

/// <summary>Strongly typed identifier of a <c>FavoriteCareer</c>.</summary>
public readonly record struct FavoriteCareerId(int Value) : IEntityId<int>
{
    public static implicit operator int(FavoriteCareerId id) => id.Value;
    public static implicit operator FavoriteCareerId(int value) => new(value);

    public override string ToString() => Value.ToString();
}
