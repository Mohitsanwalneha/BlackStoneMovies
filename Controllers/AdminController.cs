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
            if (Convert.ToString(Session["Role"]) != "Admin")
            {
                return RedirectToAction("Login", "Admin");
            }
            return View();
        }
        [HttpPost]
        public ActionResult AddMovie(Movy m)
        {
            if (Convert.ToString(Session["Role"]) != "Admin")
            {
                return RedirectToAction("Login", "Admin");
            }
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
                return RedirectToAction("AllMovies");
            }
            else
                ModelState.AddModelError(string.Empty, "Data already present please modify it");
            return View(m);

        }
        [HttpGet]
        public ActionResult UpdateMovie(int id)
        {
            if (Convert.ToString(Session["Role"]) != "Admin")
            {
                return RedirectToAction("Login", "Admin");
            }
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
            if (Convert.ToString(Session["Role"]) != "Admin")
            {
                return RedirectToAction("Login", "Admin");
            }
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
            if (Convert.ToString(Session["Role"]) != "Admin")
            {
                return RedirectToAction("Login", "Admin");
            }
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
            if (Convert.ToString(Session["Role"]) != "Admin")
            {
                return RedirectToAction("Login", "Admin");
            }
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
        public ActionResult AllShows(int id,string Error)
        {
            if (Convert.ToString(Session["Role"]) != "Admin")
            {
                return RedirectToAction("Login", "Admin");
            }
            List<GetShow> s = new List<GetShow>();
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
            ViewBag.MId = id;
            Session["MId"] = id;
            return View(s);         
        }
        [HttpGet]
        public ActionResult AddShow(int MId)
        {
            if (Convert.ToString(Session["Role"]) != "Admin")
            {
                return RedirectToAction("Login", "Admin");
            }
            return View();
        }
        [HttpPost]
        public ActionResult AddShow(Show so)
        {
            so.MId = Convert.ToInt32(Session["MId"]);
            
            if (Convert.ToString(Session["Role"]) != "Admin")
            {
                return RedirectToAction("Login", "Admin");
            }
            if (!ModelState.IsValid)
            {
                return View(so);
            }
            client.BaseAddress = new Uri("http://localhost:63540/api/AdminApi");
            var response = client.PostAsJsonAsync<Show>("AdminApi/AddShow", so);
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                return RedirectToAction("AllShows",new { id=Convert.ToInt32(Session["MId"])});
            }
            else
                ModelState.AddModelError(string.Empty, "Cannot Add the show pleace check!");
            return View(so);
        }
        [HttpGet]
        public ActionResult UpdateShow(int id)
        {
            if (Convert.ToString(Session["Role"]) != "Admin")
            {
                return RedirectToAction("Login", "Admin");
            }
            Show e = null;
            client.BaseAddress = new Uri("http://localhost:63540/api/AdminApi");
            var response = client.GetAsync("AdminApi/UpdateShow?id=" + id.ToString());
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                var display = test.Content.ReadAsAsync<Show>();
                display.Wait();
                e = display.Result;
            }
            return View(e);
            
        }
        [HttpPost]
        public ActionResult UpdateShow(Show so)
        {
            if (Convert.ToString(Session["Role"]) != "Admin")
            {
                return RedirectToAction("Login", "Admin");
            }
            client.BaseAddress = new Uri("http://localhost:63540/api/AdminApi");
            var response = client.PutAsJsonAsync<Show>("AdminApi/UpdateShow", so);
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                return RedirectToAction("AllShows", new {id=so.MId });
            }
            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            return View(so);
           
        }
        [HttpGet]
        public ActionResult DeleteShow(int id)
        {

            if (Convert.ToString(Session["Role"]) != "Admin")
            {
                return RedirectToAction("Login", "Admin");
            }
            Show e = null;
            client.BaseAddress = new Uri("http://localhost:63540/api/AdminApi");
            var response = client.GetAsync("AdminApi/UpdateShow?id=" + id.ToString());
            response.Wait();
            var test = response.Result;
            if (test.IsSuccessStatusCode)
            {
                var display = test.Content.ReadAsAsync<Show>();
                display.Wait();
                e = display.Result;
            }
            return View(e);
        }
        [HttpPost, ActionName("DeleteShow")]
        public ActionResult ConfirmDeleteShow(int id)
        {
            if (Convert.ToString(Session["Role"]) != "Admin")
            {
                return RedirectToAction("Login", "Admin");
            }
            client.BaseAddress = new Uri("http://localhost:63540/api/AdminApi");
            var response = client.DeleteAsync("AdminApi/DeleteShow/" + id.ToString());
            response.Wait();
            var test = response.Result;
            
            if (test.IsSuccessStatusCode)
            {
                return RedirectToAction("AllMovies");
            }
            ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            return View("DeleteShow");
        }

    }
}