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
       
        public IHttpActionResult GetShow( int id)
        {
            var query = from t1 in db.Shows
                        where t1.MId == id
                        select new
                        {
                            price=t1.Price,
                            sid=t1.SId,
                            mid=t1.MId,
                            mshow=t1.MShow,
                            ashow=t1.AShow,
                            eshow=t1.EShow,
                            Movie=t1.Movy.MName,
                            day=t1.Mday
                        };
            List<GetShow> g = new List<GetShow>();
            foreach(var e in query)
            {
                g.Add(new GetShow
                {

                    Movie_Name = e.Movie,
                    Morning_ticket=e.mshow.Value,
                    Afternoon_ticket=e.ashow.Value,
                    Evening_ticket=e.eshow.Value,
                    Day=e.day.Value,
                    SId=e.sid,
                    MId=e.mid.Value,
                    Price=e.price.Value
                });
            }
            return Ok(g);
        }
        [HttpGet]
        public IHttpActionResult UserBooking(int id)
        {
            var query = from t1 in db.Bookings
                        from t2 in db.Shows
                        where t1.RId==id && t2.SId==t1.SId
                        select new
                        {
                            Shows = t1.Rshow,
                            Day = t2.Mday,
                            People = t1.RPeople,
                            Name=t2.Movy.MName
                        };

            List<UserBooking> b = new List<UserBooking>();
            foreach(var e in query)
            {
                b.Add(new UserBooking
                {
                    date=e.Day.Value,
                    people=e.People,
                    Name=e.Name,
                    Showno=e.Shows
                });
            }
            
            return Ok(b);
        }
        [HttpPost]
        public IHttpActionResult BookingTicket(Booking b)
        {
            Show q = db.Shows.Where(m => m.SId == b.SId).FirstOrDefault();
            if (b.Rshow == "Afternoon") {
                q.AShow = q.AShow - b.RPeople;
                if (q.AShow >= 0)
                {
                    db.Entry(q).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    return NotFound();
                }
            }
               
            else if (b.Rshow == "Morning")
            {
                q.MShow = q.MShow - b.RPeople;
                if (q.MShow >=0)
                {
                    db.Entry(q).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    return NotFound();
                }
            }
            else if (b.Rshow == "Evening")
            {
                q.EShow = q.EShow - b.RPeople;
                if (q.EShow >=0)
                {
                    db.Entry(q).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                else
                {
                    return NotFound();
                }
            }
            
            db.Bookings.Add(b);
            db.SaveChanges();
            return Ok();
        }
    }
}
