using AutoMapper;
using BrightStar.Services.Application.Common.DTO;
using BrightStar.Services.Domain.Entities;
using BrightStar.Services.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BrightStar.Services.EventAPI.Controllers
{
    [Authorize]
    [Route("api/event")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly AppDbContext _db;
        private ResponseDto _response;
        private readonly IMapper _mapper;

        public EventController(AppDbContext db, IMapper mapper)
        {
            _db = db;
            _response = new ResponseDto();
            _mapper = mapper;
        }


        [HttpGet]
        public  ResponseDto Get()
        {
            try
            {
                IEnumerable<Event> objList =   _db.Events.ToList();
                _response.Result = _mapper.Map<IEnumerable<EventDto>>(objList);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }



        [HttpGet]
        [Route("{id:int}")]
        public ResponseDto Get(int id)
        {
            try
            {
                Event obj = _db.Events.FirstOrDefault(u => u.EventId == id);
                if (obj == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Record does not exist in the database.";
                }
                _response.Result = _mapper.Map<EventDto>(obj);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }



        [HttpGet]
        [Route("GetByCode/{code}")]
        public ResponseDto GetByCode(string code)
        {
            try
            {
                Event obj = _db.Events.FirstOrDefault(u => u.EventCode.ToLower() == code.ToLower());
                if (obj == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Record does not exist in the database.";
                }
                _response.Result = _mapper.Map<EventDto>(obj);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }



        [HttpPost]
        //[Authorize(Roles = "ADMIN")]
        public ResponseDto Post([FromBody] EventDto eventDto)
        {
            try
            {
                // Extract the user ID from the JWT claims
                var userId = User.FindFirstValue(ClaimTypes.GivenName); 

                // Map DTO to Event entity
                Event obj = _mapper.Map<Event>(eventDto);

                // Set the UserId to the authenticated user's ID
                 obj.UserId = userId;

                // Save the event
                _db.Events.Add(obj);
                _db.SaveChanges();

                // Return the created event
                _response.Result = _mapper.Map<EventDto>(obj);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }






        [HttpPut]
      //  [Authorize(Roles = "ADMIN")]
        public ResponseDto Update([FromBody] EventDto eventDto)
        {
            try
            {
                Event obj = _mapper.Map<Event>(eventDto);
                _db.Events.Update(obj);
                _db.SaveChanges();

                _response.Result = _mapper.Map<EventDto>(obj);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }





        [HttpDelete]
        [Route("{id:int}")]
        //[Authorize(Roles = "ADMIN")]
        public ResponseDto Delete(int id)
        {
            try
            {
                Event obj = _db.Events.FirstOrDefault(u => u.EventId == id);
                _db.Events.Remove(obj);
                _db.SaveChanges();

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

    }
}
