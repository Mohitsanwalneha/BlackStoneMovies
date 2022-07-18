using BlackStoneMovies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace BlackStoneMovies.Controllers
{
    public class CustomerController : Controller
    {
        
        HttpClient client = new HttpClient();
        // GET: Customer
        public ActionResult GetMovies()
        {
            if (Convert.ToString(Session["Role"] )!= "Customer")
            {
                return RedirectToAction("Login", "Admin");
            }
            List<Movy> m = new List<Movy>();
            client.BaseAddress = new Uri("http://localhost:63540/api/CustomerApi");
            var response = client.GetAsync("CustomerApi/GetMovies");
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                var display = test.Content.ReadAsAsync<List<Movy>>();
                display.Wait();
                m = display.Result;
            }

            return View(m);
        }
        [HttpGet]
        public ActionResult GetAvailableShows(int id,string Error)
        {
            if (Convert.ToString(Session["Role"]) != "Customer")
            {
                return RedirectToAction("Login", "Admin");
            }
            List<GetShow> s=new List<GetShow>();
            client.BaseAddress = new Uri("http://localhost:63540/api/CustomerApi");
            var response = client.GetAsync("CustomerApi/GetShow?id=" + id.ToString());
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                var display = test.Content.ReadAsAsync<List<GetShow>>();
                display.Wait();
                s = display.Result;
            }
            ViewBag.Error = Error;
            return View(s);
            
        }
        [HttpPost]
        public ActionResult BookTicket(UserBooking so,int MId)
        {
            if (Convert.ToString(Session["Role"]) != "Customer")
            {
                return RedirectToAction("Login", "Admin");
            }
            Booking b = new Booking();
            b.RId = Convert.ToInt32(Session["Rid"]);
            b.RPeople = so.people;
            b.Rshow = so.Showno;
            
            b.SId =Convert.ToInt32(so.Name);
            client.BaseAddress = new Uri("http://localhost:63540/api/CustomerApi");
            var response = client.PostAsJsonAsync<Booking>("CustomerApi/BookingTicket",b);
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                return RedirectToAction("BookTicket");
            }
            return RedirectToAction("GetAvailableShows","Customer",new { id = MId,Error="Please Check the data"});
            
        }
        [HttpGet]
        public ActionResult BookTicket()
        {
            if (Convert.ToString(Session["Role"]) != "Customer")
            {
                return RedirectToAction("Login", "Admin");
            }
            List<UserBooking> s = new List<UserBooking>();
            client.BaseAddress = new Uri("http://localhost:63540/api/CustomerApi");
            var response = client.GetAsync("CustomerApi/UserBooking?id=" + Session["Rid"].ToString());
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                var display = test.Content.ReadAsAsync<List<UserBooking>>();
                display.Wait();
                s = display.Result;
            }
            
            return View(s);   
        }
        public ActionResult LogOut()
        {
            Session.Clear();
            Session.Abandon();
            Session.RemoveAll();
            return RedirectToAction("Login", "Admin");
        }

    }
}