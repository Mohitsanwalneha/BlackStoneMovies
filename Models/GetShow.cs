using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlackStoneMovies.Models
{
    public class GetShow
    {
        public int SId { get; set; }
        public int MId { get; set; }
        public string Movie_Name { get; set; }
        public int Morning_ticket { get; set; }
        public int Evening_ticket { get; set; }
        public int Afternoon_ticket { get; set; }
        public int Price { get; set; }
        public DateTime Day { get; set; }
    }
}