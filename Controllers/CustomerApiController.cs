using BlackStoneMovies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BlackStoneMovies.Controllers
{
    public class CustomerApiController : ApiController
    {
        MoviesBookingEntities db = new MoviesBookingEntities();
        public IHttpActionResult GetMovies()
        {

            List<Movy> m = db.Movies.ToList();
            List<Movy> q = new List<Movy>();
            foreach (var e in m)
            {
                q.Add(new Movy
                {
                    MDirector = e.MDirector,
                    MName = e.MName,
                    MProducer = e.MProducer,
                    MId = e.MId
                });
            }
            return Ok(q);
        }
    }
}
