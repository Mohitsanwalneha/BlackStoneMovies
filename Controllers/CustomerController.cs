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
    }
}