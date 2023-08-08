using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model
{
    public class JsonRespostaApiRawgListOfScreenshots
    {
        public int Count { get; set; }
        public string Next { get; set; }
        public string Previous { get; set; }
        public List<ResultsScreenshots> Results { get; set; }
    }
}