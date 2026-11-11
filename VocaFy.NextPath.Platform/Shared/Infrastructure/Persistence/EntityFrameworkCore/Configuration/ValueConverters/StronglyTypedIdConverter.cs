using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VocaFy.NextPath.Platform.Shared.Domain.Model.ValueObjects;

namespace VocaFy.NextPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.ValueConverters;

public class StronglyTypedIdConverter<TId, TProvider>(
    Expression<Func<TId, TProvider>> convertToProviderExpression,
    Expression<Func<TProvider, TId>> convertFromProviderExpression)
    : ValueConverter<TId, TProvider>(
        convertToProviderExpression,
        convertFromProviderExpression)
    where TId : struct, IEntityId<TProvider>;