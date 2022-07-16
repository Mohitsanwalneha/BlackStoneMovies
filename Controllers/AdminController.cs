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
        public ActionResult Login(Role r)
        {
            Role e = null;
            client.BaseAddress = new Uri("http://localhost:63540/api/AdminApi");
            var response = client.PostAsJsonAsync<Role>("AdminApi/Check", r);
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                var display = test.Content.ReadAsAsync<Role>();
                display.Wait();
                e = display.Result;
                Session.Clear();
                Session["Role"] = e.URole;
                Session["Name"] = e.Name;
                Session["Rid"] = e.RID;
                if (e.URole == "Admin")
                {
                    return RedirectToAction("AllMovies","Admin");
                }
                else
                {
                    return RedirectToAction("GetMovies","Customer");
                }
            }
            ModelState.AddModelError(string.Empty, "No such data found");
                return View();
            
        }
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(Role r)
        {
            r.URole = "Customer";
            Role e = null;
            client.BaseAddress = new Uri("http://localhost:63540/api/AdminApi");
            var response = client.PostAsJsonAsync<Role>("AdminApi/Register", r);
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                var display = test.Content.ReadAsAsync<Role>();
                display.Wait();
                e = display.Result;
                Session.Clear();
                Session["Role"] = "Customer";
                Session["Name"] = e.Name;
                Session["Rid"] = e.RID;
                return RedirectToAction("GetMovies","Customer");
            }
            ModelState.AddModelError(string.Empty, "Some error occured please check !");
        return View(r);
        }
        public ActionResult AllMovies()
        {
            if (Convert.ToString(Session["Role"]) != "Admin")
            {
                return RedirectToAction("Login", "Admin");
            }
            List<Movy> m = new List<Movy>();
            client.BaseAddress = new Uri("http://localhost:63540/api/AdminApi");
            var response = client.GetAsync("AdminApi/AllMovies");
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