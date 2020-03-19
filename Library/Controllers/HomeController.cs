using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Library.Models;
using Microsoft.AspNetCore.Http;

namespace Library.Controllers
{
    public class HomeController : Controller
    {
        private readonly db_201920z_va_prj_my_libContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(db_201920z_va_prj_my_libContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
        }
        public IActionResult Index()
        {
            ViewBag.UserRole = "librarian";

            //ViewBag.UserId = _httpContextAccessor.HttpContext.Session.GetInt32("id");
            //ViewBag.UserName = _httpContextAccessor.HttpContext.Session.GetString("username");
            //ViewBag.UserRole = _httpContextAccessor.HttpContext.Session.GetString("userrole");

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult HomePage()
        {
            return View();
        }
        //*******************
        //GET> Profiles/LogIn
        public IActionResult LogIn([Bind("Username,Userpassword")] LogInProfile profile)
        {
            var UserName = _context.Profiles.Where(x => x.Username == profile.Username).FirstOrDefault();
            var UserPassword = _context.Profiles.Where(x => x.Userpassword == profile.Userpassword).FirstOrDefault();
            if (ModelState.IsValid)
            {
                if (UserName == null || UserPassword == null)
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                    return View();
                }
                var id = _context.Profiles.Where(x => x.Username == profile.Username && x.Userpassword == profile.Userpassword).FirstOrDefault().Id;
                var librarian = _context.Librarians.Where(x => x.Profileid == id).FirstOrDefault();
                bool isLibrarian;
                if(librarian == null)
                {
                    isLibrarian = false;
                }
                else
                {
                    isLibrarian = true;
                }
                _httpContextAccessor.HttpContext.Session.SetInt32("id", id);
                _httpContextAccessor.HttpContext.Session.SetString("username", profile.Username);
                _httpContextAccessor.HttpContext.Session.SetString("userrole", isLibrarian? "librarian" : "member");
                return View("HomePage");
            }
            return View();

        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
