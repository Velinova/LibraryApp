using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Library.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Library.Controllers
{
    public class HomeController : Controller
    {
        private readonly db_201920z_va_prj_my_libContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public HomeController(db_201920z_va_prj_my_libContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public IActionResult Index()
        {
            ViewBag.UserId = _httpContextAccessor.HttpContext.Request.Cookies["id"];
            ViewBag.UserName = _httpContextAccessor.HttpContext.Request.Cookies["username"];
            ViewBag.UserRole = _httpContextAccessor.HttpContext.Request.Cookies["userrole"];
            return View();
        }

        public IActionResult Privacy()
        {
            ViewBag.UserId = _httpContextAccessor.HttpContext.Request.Cookies["id"];
            ViewBag.UserName = _httpContextAccessor.HttpContext.Request.Cookies["username"];
            ViewBag.UserRole = _httpContextAccessor.HttpContext.Request.Cookies["userrole"];
            return View();
        }

        public IActionResult HomePage()
        {
            ViewBag.UserId = _httpContextAccessor.HttpContext.Request.Cookies["id"];
            ViewBag.UserName = _httpContextAccessor.HttpContext.Request.Cookies["username"];
            ViewBag.UserRole = _httpContextAccessor.HttpContext.Request.Cookies["userrole"];
            return View();
        }
        //*******************
        //GET> Home/LogIn
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

                _httpContextAccessor.HttpContext.Response.Cookies.Append("id", id.ToString());
                _httpContextAccessor.HttpContext.Response.Cookies.Append("username", profile.Username);
                _httpContextAccessor.HttpContext.Response.Cookies.Append("userrole", isLibrarian ? "librarian" : "member");

                ViewBag.UserId = _httpContextAccessor.HttpContext.Request.Cookies["id"]; 
                ViewBag.UserName = _httpContextAccessor.HttpContext.Request.Cookies["username"];
                ViewBag.UserRole = _httpContextAccessor.HttpContext.Request.Cookies["userrole"];
                return RedirectToAction("Index", "Books");
            }
            return View("Index");

        }
        //*******************
        //GET> Home/LogOff
        public IActionResult LogOff()
        {
            
            _httpContextAccessor.HttpContext.Response.Cookies.Delete("id");
            _httpContextAccessor.HttpContext.Response.Cookies.Delete("username");
            _httpContextAccessor.HttpContext.Response.Cookies.Delete("userrole");
            ViewBag.UserId = null;
            ViewBag.UserName = null;
            ViewBag.UserRole = null;
            return View("Index");

        }
        // GET: Books/Create
        public IActionResult Register()
        {
            ViewBag.UserId = _httpContextAccessor.HttpContext.Request.Cookies["id"];
            ViewBag.UserName = _httpContextAccessor.HttpContext.Request.Cookies["username"];
            ViewBag.UserRole = _httpContextAccessor.HttpContext.Request.Cookies["userrole"];
            return View();
        }

        //<POST> Home/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Username,Userpassword","Profession", "Surname", "Profilename")] RegisterMember registerMember)
        {

            if(ModelState.IsValid)
            {
                var newProfile = new Profiles();
                var newMember = new Members();
                newProfile.Profilename = registerMember.Profilename;
                newProfile.Surname = registerMember.Surname;
                newProfile.Username = registerMember.Username;
                newProfile.Userpassword = registerMember.Userpassword;
                var maxId = _context.Profiles.Max(x => x.Id);
                newProfile.Id = ++maxId;
                _context.Add(newProfile);
                await _context.SaveChangesAsync();
                var id = _context.Profiles.Where(x => x.Username == registerMember.Username && x.Userpassword == registerMember.Userpassword).FirstOrDefault().Id;
                newMember.Profileid = id;
                newMember.Profession = registerMember.Profession;
                newMember.Registrationdate = DateTime.Now;
                _context.Add(newMember);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
