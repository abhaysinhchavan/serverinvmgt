using System;
using System.Text;
using System.Runtime.Serialization;

namespace serverinvmgt.Models
{
    [DataContract]
    public class clsItemRequest
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "price")]
        public double Price { get; set; }

        [DataMember(Name = "desc")]
        public string Desc { get; set; }



    }
}