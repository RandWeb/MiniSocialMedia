﻿using CQRS.Core.Events;

namespace CQRS.Core.Domain;

public interface IEventStoreRepository
{
    Task<List<EventModel>> FinAllAsync();
    Task SaveAsync(EventModel eventModel);
    Task<List<EventModel>> GetByAggregateIdAsync(Guid aggregateId);
}