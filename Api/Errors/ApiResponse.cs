using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Api.Errors
{
    public class ApiResponse
    {
        public string Message { get; set; }
        public int StatusCode { get; set; }

        public ApiResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessage();
        }

        private string GetDefaultMessage()
        {
            return StatusCode switch
            {
                StatusCodes.Status400BadRequest => "Bad Request!",
                StatusCodes.Status401Unauthorized => "Unauthorized!",
                StatusCodes.Status404NotFound => "Not Found!",
                StatusCodes.Status500InternalServerError => "Internal Server Error!",
                _ => null
            };
        }
    }
}
