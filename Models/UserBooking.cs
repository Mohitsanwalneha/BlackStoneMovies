using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BlackStoneMovies.Models
{
    public class UserBooking
    {
        public string Name { get; set; }
        public int people { get; set; }
        public string Showno { get; set; }
        public DateTime date { get; set; }
    }
}