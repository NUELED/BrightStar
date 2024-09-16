using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrightStar.Services.Application.Common.DTO
{
    public class CustomResult<T>
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public static CustomResult<T> Success(T data, string message, int statusCode = 200)
        {
            return new CustomResult<T>
            {
                StatusCode = statusCode,
                Message = message,
                Data = data
            };
        }

        public static CustomResult<T> Fail(string message, int statusCode = 400)
        {
            return new CustomResult<T>
            {
                StatusCode = statusCode,
                Message = message
            };
        }
    }
}
