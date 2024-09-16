using BrightStar.Services.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightStar.Services.Application.BrightServices.Abstraction
{
    public interface IEventService
    {
        bool CheckEventExists(string eventCode);
        void  CreateEvent(Event model);
        bool DeleteEvent(int id);
        IEnumerable<Event> GetAllEvents();
        Event GetEventById(int id);
        Event GetOnlyEventById(int id);
        void UpdateEvent(Event model);
        Task<bool> CreateSubscriptionAsync(EventSubscription subscription);

        Task<bool> DeleteEventSubscriptionAsync(EventSubscription subscription);
        Task<EventSubscription> GetEventSubscriptionAsync(int eventId, string userId);
        IEnumerable<EventSubscription> GetAllSubscriptions();

        Task<IEnumerable<EventSubscription>> GetUserSubscriptionsAsync(string userId);
    }
}
