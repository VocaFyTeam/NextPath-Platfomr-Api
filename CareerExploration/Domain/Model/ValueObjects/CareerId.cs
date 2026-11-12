using NextPath.Platform.Shared.Domain.Model.ValueObjects;

namespace NextPath.Platform.CareerExploration.Domain.Model.ValueObjects;

/// <summary>Strongly typed identifier of a <c>Career</c>.</summary>
public readonly record struct CareerId(int Value) : IEntityId<int>
{
    public static implicit operator int(CareerId id) => id.Value;
    public static implicit operator CareerId(int value) => new(value);

    public override string ToString() => Value.ToString();
}
