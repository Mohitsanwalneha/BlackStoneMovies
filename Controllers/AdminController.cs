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
                    return RedirectToAction("AllMovies", "Admin");
                }
                else
                {
                    return RedirectToAction("GetMovies", "Customer");
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
                return RedirectToAction("GetMovies", "Customer");
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
        public ActionResult AddMovie()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddMovie(Movy m)
        {
            if (!ModelState.IsValid)
            {
                return View(m);
            }
            client.BaseAddress = new Uri("http://localhost:63540/api/AdminApi");
            var response = client.PostAsJsonAsync<Movy>("AdminApi/AddMovie", m);
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                return RedirectToAction("AllMovie");
            }
            else
                ModelState.AddModelError(string.Empty, "Data already present please modify it");
            return View(m);

        }
        [HttpGet]
        public ActionResult UpdateMovie(int id)
        {
            Movy e = null;
            client.BaseAddress = new Uri("http://localhost:63540/api/AdminApi");
            var response = client.GetAsync("AdminApi/UpdateMovie?id=" + id.ToString());
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                var display = test.Content.ReadAsAsync<Movy>();
                display.Wait();
                e = display.Result;
            }
            return View(e);

        }
        [HttpPost]
        public ActionResult UpdateMovie(Movy mo)
        {
            client.BaseAddress = new Uri("http://localhost:63540/api/AdminApi");
            var response = client.PutAsJsonAsync<Movy>("AdminApi/UpdateMovie", mo);
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                return RedirectToAction("AllMovies");
            }
            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            return View(mo);
            
        }
        [HttpGet]
        public ActionResult DeleteMovie(int id)
        {
            Movy e = null;
            client.BaseAddress = new Uri("http://localhost:63540/api/AdminApi");
            var response = client.GetAsync("AdminApi/UpdateMovie?id=" + id.ToString());
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                var display = test.Content.ReadAsAsync<Movy>();
                display.Wait();
                e = display.Result;
            }
            return View(e);
        }
        [HttpPost,ActionName("DeleteMovie")]
        public ActionResult ConfirmDelete(int id)
        {
            client.BaseAddress = new Uri("http://localhost:63540/api/AdminApi");
            var response = client.DeleteAsync("AdminApi/DeleteMovie/" + id.ToString());
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                return RedirectToAction("AllMovies");
            }
            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            return View("DeleteMovie");
        }
    }
}