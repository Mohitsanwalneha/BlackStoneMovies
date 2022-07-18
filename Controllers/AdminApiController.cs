using BlackStoneMovies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BlackStoneMovies.Controllers
{
    public class AdminApiController : ApiController
    {
        MoviesBookingEntities db = new MoviesBookingEntities();
        [HttpPost]
        public IHttpActionResult Check(Role r)
        {
            var ex = db.Roles.Where(m => m.UEmail == r.UEmail && m.UPassword == r.UPassword).FirstOrDefault();
            if (ex != null)
            {
                Role q = new Role();
                q.Name = ex.Name;
                q.URole = ex.URole;
                q.RID = ex.RID;
                return Ok(q);
            }

            return NotFound();
        }
        [HttpPost]
        public IHttpActionResult Register(Role r)
        {
            if (!ModelState.IsValid)
            {
                return NotFound();
            }
            var e = db.Roles.Where(m => m.UEmail == r.UEmail);
            if (e.Count() > 0)
                return NotFound();
            db.Roles.Add(r);
            db.SaveChanges();
            var ex = db.Roles.Where(m => m.UEmail == r.UEmail && m.UPassword == r.UPassword).FirstOrDefault();

            Role q = new Role();
            q.Name = ex.Name;
            q.URole = ex.URole;
            q.RID = ex.RID;
            return Ok(q);

        }
        [HttpGet]
        public IHttpActionResult AllMovies()
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
        [HttpPost]
        public IHttpActionResult AddMovie(Movy mo)
        {
            var ec = db.Movies.Where(m => m.MDirector == mo.MDirector && m.MName == mo.MName);
            if (ec.Count() > 0)
                return NotFound();
            db.Movies.Add(mo);
            db.SaveChanges();
            return Ok();
        }
        [HttpGet]
        public IHttpActionResult UpdateMovie(int id)
        {
            var ex = db.Movies.Where(m => m.MId == id).FirstOrDefault();
            Movy mo = new Movy();
            mo.MId = ex.MId;
            mo.MName = ex.MName;
            mo.MProducer = ex.MProducer;
            mo.MDirector = ex.MDirector;
            return Ok(mo);
        }
        [HttpPut]
        public IHttpActionResult UpdateMovie(Movy mo)
        {
            db.Entry(mo).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return Ok();

        }
        [HttpDelete]
        public IHttpActionResult DeleteMovie(int id)
        {

            var Ex = db.Shows.Where(m => m.MId == id).ToList();
            foreach(var e in Ex)
            {
                var ls = db.Bookings.Where(m => m.SId == e.SId);
                db.Bookings.RemoveRange(ls);
                db.SaveChanges();
            }
            db.Shows.RemoveRange(Ex);
            var Ey = db.Movies.Where(m => m.MId == id).FirstOrDefault();
            db.Entry(Ey).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            return Ok();
        }
        [HttpPost]
        public IHttpActionResult AddShow(Show s)
        {
            db.Shows.Add(s);
            db.SaveChanges();
            return Ok();
        }
        [HttpGet]
        public IHttpActionResult UpdateShow(int id)
        {
            var ex = db.Shows.Where(m => m.SId == id).FirstOrDefault();
            Show s = new Show();
            s.AShow = ex.AShow;
            s.EShow = ex.EShow;
            s.Mday = ex.Mday;
            s.MShow = ex.MShow;
            s.Price = ex.Price;
            s.MId = ex.MId;
            s.SId = ex.SId;
    
            return Ok(s);
        }
        [HttpPut]
        public IHttpActionResult UpdateShow(Show so)
        {
            db.Entry(so).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return Ok();
        }
        [HttpDelete]
        public IHttpActionResult DeleteShow(int id)
        {
            var l = db.Bookings.Where(m => m.SId == id);
            db.Bookings.RemoveRange(l);
            var Ex = db.Shows.Where(m => m.SId == id).FirstOrDefault();
            db.Entry(Ex).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            return Ok();
        }
    }
}
