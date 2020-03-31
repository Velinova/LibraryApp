using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Library;
using Microsoft.AspNetCore.Http;

namespace Library.Controllers
{
    public class AuthorsController : Controller
    {
        private readonly db_201920z_va_prj_my_libContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthorsController(db_201920z_va_prj_my_libContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            ViewBag.UserId = _httpContextAccessor.HttpContext.Request.Cookies["id"];
            ViewBag.UserName = _httpContextAccessor.HttpContext.Request.Cookies["username"];
            ViewBag.UserRole = _httpContextAccessor.HttpContext.Request.Cookies["userrole"];
        }

        // GET: Authors
        public async Task<IActionResult> Index()
        {
            ViewBag.UserId = _httpContextAccessor.HttpContext.Request.Cookies["id"];
            ViewBag.UserName = _httpContextAccessor.HttpContext.Request.Cookies["username"];
            ViewBag.UserRole = _httpContextAccessor.HttpContext.Request.Cookies["userrole"];
            return View(await _context.Authors.ToListAsync());
        }

        // GET: Authors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var authors = await _context.Authors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (authors == null)
            {
                return NotFound();
            }
            ViewBag.UserId = _httpContextAccessor.HttpContext.Request.Cookies["id"];
            ViewBag.UserName = _httpContextAccessor.HttpContext.Request.Cookies["username"];
            ViewBag.UserRole = _httpContextAccessor.HttpContext.Request.Cookies["userrole"];
            return View(authors);
        }

        // GET: Authors/Create
        public IActionResult Create()
        {
            ViewBag.UserId = _httpContextAccessor.HttpContext.Request.Cookies["id"];
            ViewBag.UserName = _httpContextAccessor.HttpContext.Request.Cookies["username"];
            ViewBag.UserRole = _httpContextAccessor.HttpContext.Request.Cookies["userrole"];
            return View();
        }

        // POST: Authors/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Authorname")] Authors authors)
        {
            if (ModelState.IsValid)
            {
                Authors author = new Authors();
                var maxId = 0;
                if (_context.Books.Count() == 0)
                {
                    maxId = 0;
                }
                else
                {
                    maxId = _context.Books.Max(x => x.Id);
                }
                author.Id = ++maxId;
                author.Authorname = authors.Authorname;
                _context.Add(author);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            ViewBag.UserId = _httpContextAccessor.HttpContext.Request.Cookies["id"];
            ViewBag.UserName = _httpContextAccessor.HttpContext.Request.Cookies["username"];
            ViewBag.UserRole = _httpContextAccessor.HttpContext.Request.Cookies["userrole"];
            return View(authors);
        }

        // GET: Authors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var authors = await _context.Authors.FindAsync(id);
            if (authors == null)
            {
                return NotFound();
            }
            ViewBag.UserId = _httpContextAccessor.HttpContext.Request.Cookies["id"];
            ViewBag.UserName = _httpContextAccessor.HttpContext.Request.Cookies["username"];
            ViewBag.UserRole = _httpContextAccessor.HttpContext.Request.Cookies["userrole"];
            return View(authors);
        }

        // POST: Authors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Authorname")] Authors authors)
        {
            if (id != authors.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(authors);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuthorsExists(authors.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewBag.UserId = _httpContextAccessor.HttpContext.Request.Cookies["id"];
            ViewBag.UserName = _httpContextAccessor.HttpContext.Request.Cookies["username"];
            ViewBag.UserRole = _httpContextAccessor.HttpContext.Request.Cookies["userrole"];
            return View(authors);
        }

        // GET: Authors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var authors = await _context.Authors
                .FirstOrDefaultAsync(m => m.Id == id);
            if (authors == null)
            {
                return NotFound();
            }
            ViewBag.UserId = _httpContextAccessor.HttpContext.Request.Cookies["id"];
            ViewBag.UserName = _httpContextAccessor.HttpContext.Request.Cookies["username"];
            ViewBag.UserRole = _httpContextAccessor.HttpContext.Request.Cookies["userrole"];
            return View(authors);
        }

        // POST: Authors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var authors = await _context.Authors.FindAsync(id);
            _context.Authors.Remove(authors);
            await _context.SaveChangesAsync();
            ViewBag.UserId = _httpContextAccessor.HttpContext.Request.Cookies["id"];
            ViewBag.UserName = _httpContextAccessor.HttpContext.Request.Cookies["username"];
            ViewBag.UserRole = _httpContextAccessor.HttpContext.Request.Cookies["userrole"];
            return RedirectToAction(nameof(Index));
        }

        private bool AuthorsExists(int id)
        {
            ViewBag.UserId = _httpContextAccessor.HttpContext.Request.Cookies["id"];
            ViewBag.UserName = _httpContextAccessor.HttpContext.Request.Cookies["username"];
            ViewBag.UserRole = _httpContextAccessor.HttpContext.Request.Cookies["userrole"];
            return _context.Authors.Any(e => e.Id == id);
        }
    }
}
