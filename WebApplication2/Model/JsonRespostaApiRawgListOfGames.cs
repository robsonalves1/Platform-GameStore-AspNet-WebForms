using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model
{
    public class JsonRespostaApiRawgListOfGames
    {
        public string Count { get; set; }
        public string Next { get; set; }
        public string Previous { get; set; }
        public List<ResultsJogo> Results { get; set; }
        public string Seo_Title { get; set; }
        public string Seo_Description { get; set; }
        public string Seo_Keyword { get; set; }
        public string Seo_H1 { get; set; }
        public string Nofollow { get; set; }
        public string Description { get; set; }

    }
}