using BrightStar.Services.Application.BrightServices.Abstraction;
using BrightStar.Services.Application.Common.Interfaces;
using BrightStar.Services.Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightStar.Services.Application.BrightServices.Implementation
{
    public class EventService : IEventService
    {
        private readonly IUnitOfWork _unitOfWork;

        public EventService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool CheckEventExists(string eventCode)
        {
            return _unitOfWork.EventRepository.Any(u => u.EventCode == eventCode);
        }

        public void CreateEvent(Event model)
        {
           // model.UserId = currentUserId;

            _unitOfWork.EventRepository.Add(model);
            _unitOfWork.SaveChanges();
        }

        public bool DeleteEvent(int id)
        {
            try
            {
                var fromdb = _unitOfWork.EventRepository.Get(p => p.EventId == id);
                if (fromdb is not null)
                {
                    _unitOfWork.EventRepository.Remove(fromdb);
                    _unitOfWork.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public IEnumerable<Event> GetAllEvents()
        {
            return _unitOfWork.EventRepository.GetAll(includeProperties: "User");
        }

        public IEnumerable<EventSubscription> GetAllSubscriptions()
        {
            return _unitOfWork.EventSubscriptionRepository.GetAll(includeProperties: "User");
        }

        public Event GetEventById(int id)
        {
            return _unitOfWork.EventRepository.Get(u => u.EventId == id, includeProperties: "User", tracked:false);
        }


        public Event GetOnlyEventById(int id)
        {
            return _unitOfWork.EventRepository.Get(u => u.EventId == id);
        }

        public void UpdateEvent(Event model)
        {
            _unitOfWork.EventRepository.Update(model);
            _unitOfWork.SaveChanges();
        }


        public async Task<bool> CreateSubscriptionAsync(EventSubscription subscription)
        {
            try
            {
                // Check if the user is already subscribed
                var existingSubscription = _unitOfWork.EventSubscriptionRepository.Get(s => s.UserId == subscription.UserId && s.EventId == subscription.EventId) ;

                if (existingSubscription != null)
                {
                    // Return false if the user is already subscribed
                    return false;
                }
             
                  _unitOfWork.EventSubscriptionRepository.Add(subscription);
                  _unitOfWork.SaveChanges();

                return true;
            }
            catch (Exception ex)
            {
             
                return false;
            }
        }


        public async Task<EventSubscription> GetEventSubscriptionAsync(int eventId, string userId)
        {
            // Assuming you are using a repository pattern or similar for data access
            return  _unitOfWork.EventSubscriptionRepository
                .Get(sub => sub.EventId == eventId && sub.UserId == userId);
        }

        public async Task<bool> DeleteEventSubscriptionAsync(EventSubscription subscription)
        {
            try
            {
                _unitOfWork.EventSubscriptionRepository.Remove(subscription);
                _unitOfWork.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                // Handle error, possibly log it
                return false;
            }
        }

        public  async Task<IEnumerable<EventSubscription>> GetUserSubscriptionsAsync(string userId)
        {
            return  _unitOfWork.EventSubscriptionRepository.GetAll(s => s.UserId == userId);
        }


    }
}
