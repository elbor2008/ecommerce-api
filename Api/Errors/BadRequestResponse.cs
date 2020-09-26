using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Errors
{
    public class BadRequestResponse : ApiResponse
    {
        public string[] Errors { get; }
        public BadRequestResponse(int statusCode, string message = null, string[] errors = null) : base(statusCode, message)
        {
            if (errors != null)
            {
                Errors = errors;
            }
        }
    }
}
