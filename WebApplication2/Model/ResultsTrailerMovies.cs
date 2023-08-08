using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model
{
    public class ResultsTrailerMovies
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Preview { get; set; }
        public DataTrailerMovies Data { get; set; }
    }
}