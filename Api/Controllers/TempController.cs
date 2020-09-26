using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class TempController : BaseApiController
    {
        [HttpGet("badrequest")]
        public IActionResult Bad()
        {
            var i = 1;
            var r = i / 0;
            return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest));
        }
        [HttpGet("unauthorized")]
        public IActionResult Bad2()
        {
            return Unauthorized(new ApiResponse(StatusCodes.Status401Unauthorized));
        }
        [HttpGet("num/{id}")]
        public IActionResult Bad3(int id)
        {
            return Unauthorized(new ApiResponse(StatusCodes.Status401Unauthorized));
        }
    }
}
