using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model
{
    public class ResultsScreenshots
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool IsDeleted { get; set; }
    }
}