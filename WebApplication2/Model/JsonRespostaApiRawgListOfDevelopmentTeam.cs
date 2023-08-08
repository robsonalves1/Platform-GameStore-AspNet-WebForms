using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model
{
    public class JsonRespostaApiRawgListOfDevelopmentTeam
    {
        public int Count { get; set; }
        public string Next { get; set; }
        public string Previous { get; set; }
        public List<ResultsDevelopmentTeam> Results { get; set; }
    }
}