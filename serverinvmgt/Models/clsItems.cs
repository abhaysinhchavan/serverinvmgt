using System;
using System.Text;
using System.Runtime.Serialization;
using System.IO;

namespace serverinvmgt.Models
{
    public class clsItems
    {
        [DataMember(Name = "id")]
        public long Id { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "price")]
        public double Price { get; set; }

        [DataMember(Name = "desc")]
        public string Desc { get; set; }

        [DataMember(Name = "file")]
        public dynamic File { get; set; }
    }
}