using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace ClassicDataTypeGrid.Models
{
    [DataContract()]
    public class ClassicGridDataTypeModel
    {
        [DataMember(Name = "alias")]
        public string Alias { get; set; }

        [DataMember(Name = "view")]
        public string View { get; set; }

        [DataMember(Name = "config")]
        public IDictionary<string, object> Config { get; set; }

        [DataMember(Name = "id")]
        public int Id { get;  set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}
