using BrightStar.Services.Application.Common.DTO;
using BrightStar.Services.Application.Common.Interfaces;
using BrightStar.Services.Application.Common.Utility;
using Event.Web.Service.IService;

namespace Event.Web.Service
{
    public class EventService : IEventService
    {
        private readonly IBaseService _baseService;

        public EventService(IBaseService baseService)
        {
            _baseService = baseService;
        }

        public async Task<ResponseDto?> CreateEventAsync(EventDto eventDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.POST,
                Data = eventDto,
                Url = SD.EventAPIBase + "/api/event"
            });
        }

        public async Task<ResponseDto?> DeleteEventAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.DELETE,
                Url = SD.EventAPIBase + "/api/event/" + id
            });
        }

        public async Task<ResponseDto?> GetAllEventAsync()
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.EventAPIBase + "/api/event"
            });
        }

        public async Task<ResponseDto?> GetEventAsync(string SubscriptionId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.EventAPIBase + "/api/event/GetByCode/" + SubscriptionId
            });
        }

        public async Task<ResponseDto?> GetEventByIdAsync(int id)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.GET,
                Url = SD.EventAPIBase + "/api/event/" + id
            });
        }

        public async Task<ResponseDto?> UpdateEventAsync(EventDto eventDto)
        {

            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = SD.ApiType.PUT,
                Data = eventDto,
                Url = SD.EventAPIBase + "/api/event"
            });
        }

    }
}
