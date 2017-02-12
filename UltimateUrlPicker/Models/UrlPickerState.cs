using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UltimateUrlPicker.Models
{
    public class UrlPickerState
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("mode")]
        public UrlPickerMode Mode { get; set; }

        [JsonProperty("nodeId")]
        public int? NodeId { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("newWindow")]
        public bool NewWindow { get; set; }
    }
}
