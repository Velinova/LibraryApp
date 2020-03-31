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
    public class BooksController : Controller
    {
        private readonly db_201920z_va_prj_my_libContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BooksController(db_201920z_va_prj_my_libContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            ViewBag.UserId = _httpContextAccessor.HttpContext.Request.Cookies["id"];
            ViewBag.UserName = _httpContextAccessor.HttpContext.Request.Cookies["username"];
            ViewBag.UserRole = _httpContextAccessor.HttpContext.Request.Cookies["userrole"];
        }

        // GET: Books
        public async Task<IActionResult> Index()
        {
            ViewBag.UserId = _httpContextAccessor.HttpContext.Request.Cookies["id"];
            ViewBag.UserName = _httpContextAccessor.HttpContext.Request.Cookies["username"];
            ViewBag.UserRole = _httpContextAccessor.HttpContext.Request.Cookies["userrole"];
            return View(await _context.Books.ToListAsync());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var books = await _context.Books
                .FirstOrDefaultAsync(m => m.Id == id);
            if (books == null)
            {
                return NotFound();
            }
            ViewBag.UserId = _httpContextAccessor.HttpContext.Request.Cookies["id"];
            ViewBag.UserName = _httpContextAccessor.HttpContext.Request.Cookies["username"];
            ViewBag.UserRole = _httpContextAccessor.HttpContext.Request.Cookies["userrole"];
            return View(books);
        }

        // GET: Books/Create
        public IActionResult Create()
        {
            ViewBag.UserId = _httpContextAccessor.HttpContext.Request.Cookies["id"];
            ViewBag.UserName = _httpContextAccessor.HttpContext.Request.Cookies["username"];
            ViewBag.UserRole = _httpContextAccessor.HttpContext.Request.Cookies["userrole"];
            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Numberofpages,Genre,Booklanguage,Bookname,Plot,AuthorName")] BookCreateModel books)
        {
            if (ModelState.IsValid)
            {
                var maxId = 0;
                if (_context.Books.Count() == 0)
                {
                    maxId = 0;
                }
                else
                {
                    maxId = _context.Books.Max(x => x.Id);
                }
                var book = new Books();
                book.Id = ++maxId;
                book.Numberofpages = books.Numberofpages;
                book.Genre = books.Genre;
                book.Booklanguage = books.Booklanguage;
                book.Bookname = books.Bookname;
                book.Plot = books.Plot;
                book.Numberofsamples = 0;
                _context.Add(book);
                await _context.SaveChangesAsync();
                //dodadi red vo writtenby tabelata
                var author = _context.Authors.Where(x => x.Authorname == books.AuthorName).FirstOrDefault();
                var row = new Writtenby();
                row.Authorid = author.Id;
                row.Bookid = book.Id;
                _context.Add(row);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.UserId = _httpContextAccessor.HttpContext.Request.Cookies["id"];
            ViewBag.UserName = _httpContextAccessor.HttpContext.Request.Cookies["username"];
            ViewBag.UserRole = _httpContextAccessor.HttpContext.Request.Cookies["userrole"];
            return View(books);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var books = await _context.Books.FindAsync(id);
            if (books == null)
            {
                return NotFound();
            }
            ViewBag.UserId = _httpContextAccessor.HttpContext.Request.Cookies["id"];
            ViewBag.UserName = _httpContextAccessor.HttpContext.Request.Cookies["username"];
            ViewBag.UserRole = _httpContextAccessor.HttpContext.Request.Cookies["userrole"];
            return View(books);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Numberofpages,Genre,Booklanguage,Bookname,Plot")] Books books)
        {
            if (id != books.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(books);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BooksExists(books.Id))
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
            return View(books);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var books = await _context.Books
                .FirstOrDefaultAsync(m => m.Id == id);
            if (books == null)
            {
                return NotFound();
            }
            ViewBag.UserId = _httpContextAccessor.HttpContext.Request.Cookies["id"];
            ViewBag.UserName = _httpContextAccessor.HttpContext.Request.Cookies["username"];
            ViewBag.UserRole = _httpContextAccessor.HttpContext.Request.Cookies["userrole"];
            return View(books);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var booksid = await _context.Books.FindAsync(id);
            var broj = _context.Writtenby.Join(_context.Authors,
               written => written.Authorid,
               author => author.Id,
               (written, author) => new
               {
                   aid = written.Authorid,
                   bid =written.Bookid,

               }).Where(x => x.bid == id).FirstOrDefault();
            
            var writtenby = await _context.Writtenby.FindAsync(id,broj.aid);

            _context.Writtenby.Remove(writtenby);
            _context.Books.Remove(booksid);

            await _context.SaveChangesAsync();
            ViewBag.UserId = _httpContextAccessor.HttpContext.Request.Cookies["id"];
            ViewBag.UserName = _httpContextAccessor.HttpContext.Request.Cookies["username"];
            ViewBag.UserRole = _httpContextAccessor.HttpContext.Request.Cookies["userrole"];
            return RedirectToAction(nameof(Index));
        }

        private bool BooksExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }
    }
}
