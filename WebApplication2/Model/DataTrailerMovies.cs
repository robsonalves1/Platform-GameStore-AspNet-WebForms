using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model
{
    public class DataTrailerMovies
    {
        [JsonProperty("480")]
        public string _480 { get; set; }
        public string Max { get; set; }
    }
}