using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace PaySlipManagement.UI.Models
{
    public class GoogleSheetResponse
    {
        [JsonProperty("range")]
        public string Range { get; set; }

        [JsonProperty("majorDimension")]
        public string MajorDimension { get; set; }

        [JsonProperty("values")]
        public List<List<string>> Values { get; set; }
    }
}
