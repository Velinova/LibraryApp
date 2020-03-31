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
    public class SamplesController : Controller
    {
        private readonly db_201920z_va_prj_my_libContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public SamplesController(db_201920z_va_prj_my_libContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            ViewBag.UserId = _httpContextAccessor.HttpContext.Request.Cookies["id"];
            ViewBag.UserName = _httpContextAccessor.HttpContext.Request.Cookies["username"];
            ViewBag.UserRole = _httpContextAccessor.HttpContext.Request.Cookies["userrole"];
        }

        // GET: Samples
        public async Task<IActionResult> Index()
        {
            var db_201920z_va_prj_my_libContext = _context.Samples.Include(s => s.Book).Include(s => s.Library);
            ViewBag.UserId = _httpContextAccessor.HttpContext.Request.Cookies["id"];
            ViewBag.UserName = _httpContextAccessor.HttpContext.Request.Cookies["username"];
            ViewBag.UserRole = _httpContextAccessor.HttpContext.Request.Cookies["userrole"];
            return View(await db_201920z_va_prj_my_libContext.ToListAsync());
        }

        // GET: Samples/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var samples = await _context.Samples
                .Include(s => s.Book)
                .Include(s => s.Library)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (samples == null)
            {
                return NotFound();
            }
            ViewBag.UserId = _httpContextAccessor.HttpContext.Request.Cookies["id"];
            ViewBag.UserName = _httpContextAccessor.HttpContext.Request.Cookies["username"];
            ViewBag.UserRole = _httpContextAccessor.HttpContext.Request.Cookies["userrole"];
            return View(samples);
        }

        // GET: Samples/Create
        public IActionResult Create()
        {
            ViewData["Bookid"] = new SelectList(_context.Books, "Id", "Bookname");
            ViewData["Libraryid"] = new SelectList(_context.Libraries, "Id", "Id");
            ViewBag.UserId = _httpContextAccessor.HttpContext.Request.Cookies["id"];
            ViewBag.UserName = _httpContextAccessor.HttpContext.Request.Cookies["username"];
            ViewBag.UserRole = _httpContextAccessor.HttpContext.Request.Cookies["userrole"];
            return View();
        }

        // POST: Samples/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LibraryName,Price,BookName")] SampleCreateModel samples)
        {
            if (ModelState.IsValid)
            {
                Samples sample = new Samples();
                var libraryId = _context.Libraries.Where(x => x.Libraryname == samples.LibraryName).FirstOrDefault();
                var bookId = _context.Books.Where(x => x.Bookname == samples.BookName).FirstOrDefault();
                sample.Price = samples.Price;
                sample.Bookid = bookId.Id;
                sample.Libraryid = libraryId.Id;
                sample.Samplestatus = true;
                var maxId = 0;
                if (_context.Samples.Count() == 0)
                {
                    maxId = 0;
                }
                else
                {
                    maxId = _context.Samples.Max(x => x.Id);
                }
                sample.Id = ++maxId;
                _context.Add(sample);
                await _context.SaveChangesAsync();
                var book = _context.Books.Where(x => x.Id == bookId.Id).FirstOrDefault();
                book.Numberofsamples++;
                _context.Update(book);
                await _context.SaveChangesAsync();
                var library = _context.Libraries.Where(x => x.Id == libraryId.Id).FirstOrDefault();
                library.Numberofbooks++;
                _context.Update(library);
                Addedby addedby = new Addedby();
                addedby.Profileid = Int32.Parse(_httpContextAccessor.HttpContext.Request.Cookies["id"]);
                addedby.Sampleid = maxId;
                _context.Add(addedby);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            ViewBag.UserId = _httpContextAccessor.HttpContext.Request.Cookies["id"];
            ViewBag.UserName = _httpContextAccessor.HttpContext.Request.Cookies["username"];
            ViewBag.UserRole = _httpContextAccessor.HttpContext.Request.Cookies["userrole"];
            return View(samples);
        }

        // GET: Samples/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var samples = await _context.Samples.FindAsync(id);
            if (samples == null)
            {
                return NotFound();
            }
            ViewData["Bookid"] = new SelectList(_context.Books, "Id", "Bookname", samples.Bookid);
            ViewData["Libraryid"] = new SelectList(_context.Libraries, "Id", "Id", samples.Libraryid);
            ViewBag.UserId = _httpContextAccessor.HttpContext.Request.Cookies["id"];
            ViewBag.UserName = _httpContextAccessor.HttpContext.Request.Cookies["username"];
            ViewBag.UserRole = _httpContextAccessor.HttpContext.Request.Cookies["userrole"];
            return View(samples);
        }

        // POST: Samples/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Price,Samplestatus,Libraryid,Bookid")] Samples samples)
        {
            if (id != samples.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(samples);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SamplesExists(samples.Id))
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
            ViewData["Bookid"] = new SelectList(_context.Books, "Id", "Bookname", samples.Bookid);
            ViewData["Libraryid"] = new SelectList(_context.Libraries, "Id", "Id", samples.Libraryid);
            ViewBag.UserId = _httpContextAccessor.HttpContext.Request.Cookies["id"];
            ViewBag.UserName = _httpContextAccessor.HttpContext.Request.Cookies["username"];
            ViewBag.UserRole = _httpContextAccessor.HttpContext.Request.Cookies["userrole"];
            return View(samples);
        }

        // GET: Samples/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var samples = await _context.Samples
                .Include(s => s.Book)
                .Include(s => s.Library)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (samples == null)
            {
                return NotFound();
            }
            ViewBag.UserId = _httpContextAccessor.HttpContext.Request.Cookies["id"];
            ViewBag.UserName = _httpContextAccessor.HttpContext.Request.Cookies["username"];
            ViewBag.UserRole = _httpContextAccessor.HttpContext.Request.Cookies["userrole"];
            return View(samples);
        }

        // POST: Samples/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var samples = await _context.Samples.FindAsync(id);
            _context.Samples.Remove(samples);

            await _context.SaveChangesAsync();
            ViewBag.UserId = _httpContextAccessor.HttpContext.Request.Cookies["id"];
            ViewBag.UserName = _httpContextAccessor.HttpContext.Request.Cookies["username"];
            ViewBag.UserRole = _httpContextAccessor.HttpContext.Request.Cookies["userrole"];
            return RedirectToAction(nameof(Index));
        }

        private bool SamplesExists(int id)
        {
            ViewBag.UserId = _httpContextAccessor.HttpContext.Request.Cookies["id"];
            ViewBag.UserName = _httpContextAccessor.HttpContext.Request.Cookies["username"];
            ViewBag.UserRole = _httpContextAccessor.HttpContext.Request.Cookies["userrole"];
            return _context.Samples.Any(e => e.Id == id);
        }
    }
}
