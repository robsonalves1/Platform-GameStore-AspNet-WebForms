using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model
{
    public class ResultsPublishers
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public int Games_Count { get; set; }
        public string Image_Background { get; set; }
        public List<GamesInResultsPublishers> Games { get; set; }
    }
}