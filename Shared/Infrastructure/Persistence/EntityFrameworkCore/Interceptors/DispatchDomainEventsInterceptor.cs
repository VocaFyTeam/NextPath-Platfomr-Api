using Cortex.Mediator;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using VocaFy.NextPath.Platform.Shared.Domain.Model.Aggregates;

namespace VocaFy.NextPath.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Interceptors;

public class DispatchDomainEventsInterceptor(IMediator mediator) : SaveChangesInterceptor
{
    public override async ValueTask<int> SavedChangesAsync(
        SaveChangesCompletedEventData eventData,
        int result,
        CancellationToken cancellationToken = default)
    {
        await DispatchDomainEvents(eventData.Context, cancellationToken);

        return await base.SavedChangesAsync(eventData, result, cancellationToken);
    }

    private async Task DispatchDomainEvents(
        DbContext? context,
        CancellationToken cancellationToken)
    {
        if (context is null)
            return;

        var aggregates = context.ChangeTracker
            .Entries<IAggregateRoot>()
            .Select(entry => entry.Entity)
            .Where(aggregate => aggregate.DomainEvents.Any())
            .ToList();

        var domainEvents = aggregates
            .SelectMany(aggregate => aggregate.DomainEvents)
            .ToList();

        foreach (var domainEvent in domainEvents)
        {
            await mediator.PublishAsync(domainEvent, cancellationToken);
        }

        foreach (var aggregate in aggregates)
        {
            aggregate.ClearDomainEvents();
        }
    }
}