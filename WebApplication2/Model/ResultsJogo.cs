using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model
{
    public class ResultsJogo
    {
        public int Id { get; set; }
        public string Slug { get; set; }
        public string Name { get; set; }
        public string Released { get; set; }        
        public bool Tba { get; set; }
        public string Background_Image { get; set; }
        public double Rating { get; set; }
        public int? Metacritic { get; set; }
        public int Playtime { get; set; }
        public string DominantColor { get; set; }
    }
}