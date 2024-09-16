using AutoMapper;
using BrightStar.Services.Application.Common.DTO;
using BrightStar.Services.Domain.Entities;

namespace BrightStar.Services.EventAPI.Mapping
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<Event, EventDto>().ReverseMap();
            });
            return mappingConfig;
        }

    }
}
