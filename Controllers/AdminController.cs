using BlackStoneMovies.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace BlackStoneMovies.Controllers
{
    public class AdminController : Controller
    {
        HttpClient client = new HttpClient();

        // GET: Admin
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(string Email,string Password)
        {
            client.BaseAddress = new Uri("http://localhost:64815/api/AdminApi");
            var response = client.GetAsync("AdminApi");
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                var display = test.Content.ReadAsAsync<Role>();
                display.Wait();
                Session["Role"] = display.Result.URole;
                Session["Name"]
            }
            return View();
        }
    }
}