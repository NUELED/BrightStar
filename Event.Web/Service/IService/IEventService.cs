using BrightStar.Services.Application.Common.DTO;

namespace Event.Web.Service.IService
{
    public interface IEventService
    {

        Task<ResponseDto?> GetEventAsync(string SubscriptionId);
        Task<ResponseDto?> GetAllEventAsync();
        Task<ResponseDto?> GetEventByIdAsync(int id);
        Task<ResponseDto?> CreateEventAsync(EventDto eventDto);
        Task<ResponseDto?> UpdateEventAsync(EventDto eventDto);
        Task<ResponseDto?> DeleteEventAsync(int id);
    }
}
