using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace serverinvmgt.Models
{
    public class ErrorResponse
    {
        [DataMember(Name = "code")]
        public string Code { get; set; }

        [DataMember(Name = "message")]
        public string Message { get; set; }

        public ErrorResponse(string errorCode, string errorMessage)
        {
            Code = errorCode;
            Message = errorMessage;
        }
    }
}