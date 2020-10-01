using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Serialization;
using System.Runtime.Serialization;
using System.Text;

namespace serverinvmgt.Models
{
    [DataContract]
    public class Response
    {
        
        [DataMember(Name = "data")]
        public dynamic Data { get; set; }

        [DataMember(Name = "error")]
        public ErrorResponse Error { get; set; }
    }
}