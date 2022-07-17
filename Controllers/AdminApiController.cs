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
            if (ex!=null){
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
            foreach(var e in m)
            {
                q.Add(new Movy
                {
                   MDirector=e.MDirector,
                   MName=e.MName,
                   MProducer=e.MProducer,
                   MId=e.MId
                });
            }
            return Ok(q);
        }
        [HttpPost]
        public IHttpActionResult AddMovie(Movy mo)
        {
            var ec = db.Movies.Where(m => m.MDirector == mo.MDirector && m.MName ==mo.MName );
            if (ec.Count() > 0)
                return NotFound();
            db.Movies.Add(mo);
            db.SaveChanges();
            return Ok();
        }
        [HttpGet]
        public IHttpActionResult UpdateMovie(int id)
        {
            var ex=db.Movies.Where(m=>m.MId==id).FirstOrDefault();
            return Ok(ex);
        }
        [HttpPut]
        public IHttpActionResult UpdateMovie( Movy mo)
        {
            db.Entry(mo).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return Ok();
           
        }
        [HttpDelete]
        public IHttpActionResult DeleteMovie(int id)
        {
            var Ex = db.Movies.Where(m => m.MId == id).FirstOrDefault();
            db.Entry(Ex).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();
            return Ok();
        }
    }
}
