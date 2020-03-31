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
    public class BorrowingsController : Controller
    {
        private readonly db_201920z_va_prj_my_libContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public BorrowingsController(db_201920z_va_prj_my_libContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            ViewBag.UserId = _httpContextAccessor.HttpContext.Request.Cookies["id"];
            ViewBag.UserName = _httpContextAccessor.HttpContext.Request.Cookies["username"];
            ViewBag.UserRole = _httpContextAccessor.HttpContext.Request.Cookies["userrole"];
        }

        // GET: Borrowings
        public async Task<IActionResult> Index()
        {
            var list = _context.Borrowings.Join(_context.Samples,
               borrowing => borrowing.Sampleid,
               sample => sample.Id,
               (borrowing, sample) => new
               {
                   Id = borrowing.Id,
                   Borrowdate = borrowing.Borrowdate,
                   Expirationdate = borrowing.Expirationdate,
                   Returndate = borrowing.Returndate,
                   Bookid = sample.Bookid,
                   Profileid = borrowing.Profileid,
                   Sampleid = borrowing.Sampleid
               }).Join(_context.Books,
               bs => bs.Bookid,
               book => book.Id,
               (bs, book) => new BorrowingGridModel
               {
                   Id = bs.Id,
                   Bookname = book.Bookname,
                   Borrowdate = bs.Borrowdate,
                   Expirationdate = bs.Expirationdate,
                   Returndate = bs.Returndate,
                   Bookid = bs.Bookid,
                   Profileid = bs.Profileid,
                   Sampleid = bs.Sampleid
               });
            ViewBag.UserId = _httpContextAccessor.HttpContext.Request.Cookies["id"];
            ViewBag.UserName = _httpContextAccessor.HttpContext.Request.Cookies["username"];
            ViewBag.UserRole = _httpContextAccessor.HttpContext.Request.Cookies["userrole"];
            return View(await list.ToListAsync());
        }
        
        // GET: Borrowings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrowings = await _context.Borrowings
                .Include(b => b.Profile)
                .Include(b => b.Sample)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (borrowings == null)
            {
                return NotFound();
            }
            ViewBag.UserId = _httpContextAccessor.HttpContext.Request.Cookies["id"];
            ViewBag.UserName = _httpContextAccessor.HttpContext.Request.Cookies["username"];
            ViewBag.UserRole = _httpContextAccessor.HttpContext.Request.Cookies["userrole"];
            return View(borrowings);
        }

        // GET: Borrowings/Create
        public IActionResult Create()
        {
            List<Books> books = _context.Books.Where(x => x.Numberofsamples > 0).ToList();
            ViewData["Books"] = new SelectList(books, "Id", "Bookname");
            var members = _context.Members.Join(_context.Profiles,
               member => member.Profileid,
               profile => profile.Id,
               (member, profile) => new
               {
                   UserName = profile.Username
               }).ToArray().ToList();

            ViewData["Members"] = new SelectList(members, "UserName", "UserName");
            ViewBag.UserId = _httpContextAccessor.HttpContext.Request.Cookies["id"];
            ViewBag.UserName = _httpContextAccessor.HttpContext.Request.Cookies["username"];
            ViewBag.UserRole = _httpContextAccessor.HttpContext.Request.Cookies["userrole"];
            return View();
        }

        // POST: Borrowings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookId,UserName,BorrowDate,ExpirationDate")] MakeBorrowing makeBorrowing)
        {
            if (ModelState.IsValid)
            {
                var profile = _context.Profiles.Where(x => x.Username == makeBorrowing.UserName).FirstOrDefault();
                if( _context.Borrowings.Where(x=> x.Profileid == profile.Id && x.Returndate == null).ToList().Count == 0)
                {
                    //add new borrowing
                    Borrowings borrowing = new Borrowings();
                    borrowing.Borrowdate = makeBorrowing.BorrowDate;
                    borrowing.Expirationdate = makeBorrowing.ExpirationDate;
                    
                    borrowing.Profileid = profile.Id;
                    Samples sampleToBorrow = _context.Samples.Where(x => x.Bookid == makeBorrowing.BookId && x.Samplestatus == true).OrderBy(x => x.Id).First();
                    borrowing.Sampleid = sampleToBorrow.Id;
                    var maxId = 0;
                    if (_context.Borrowings.Count() == 0)
                    {
                        maxId = 0;
                    }
                    else
                    {
                        maxId = _context.Borrowings.Max(x => x.Id);
                    }
                    borrowing.Id = ++maxId;
                    _context.Add(borrowing);
                    //sample status false
                    sampleToBorrow.Samplestatus = false;
                    _context.Update(sampleToBorrow);
                    //decrement number of samples for borrowed book
                    Books book = _context.Books.Where(x => x.Id == sampleToBorrow.Bookid).FirstOrDefault();
                    book.Numberofsamples = book.Numberofsamples - 1;
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                    ViewBag.UserId = _httpContextAccessor.HttpContext.Request.Cookies["id"];
                    ViewBag.UserName = _httpContextAccessor.HttpContext.Request.Cookies["username"];
                    ViewBag.UserRole = _httpContextAccessor.HttpContext.Request.Cookies["userrole"];
                    ViewBag.Error = "true";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ViewBag.UserId = _httpContextAccessor.HttpContext.Request.Cookies["id"];
                    ViewBag.UserName = _httpContextAccessor.HttpContext.Request.Cookies["username"];
                    ViewBag.UserRole = _httpContextAccessor.HttpContext.Request.Cookies["userrole"];
                    ViewBag.Error = "true";
                    var list = _context.Borrowings.Join(_context.Samples,
              borrowing => borrowing.Sampleid,
              sample => sample.Id,
              (borrowing, sample) => new
              {
                  Id = borrowing.Id,
                  Borrowdate = borrowing.Borrowdate,
                  Expirationdate = borrowing.Expirationdate,
                  Returndate = borrowing.Returndate,
                  Bookid = sample.Bookid,
                  Profileid = borrowing.Profileid,
                  Sampleid = borrowing.Sampleid
              }).Join(_context.Books,
              bs => bs.Bookid,
              book => book.Id,
              (bs, book) => new BorrowingGridModel
              {
                  Id = bs.Id,
                  Bookname = book.Bookname,
                  Borrowdate = bs.Borrowdate,
                  Expirationdate = bs.Expirationdate,
                  Returndate = bs.Returndate,
                  Bookid = bs.Bookid,
                  Profileid = bs.Profileid,
                  Sampleid = bs.Sampleid
              });
                    ViewBag.UserId = _httpContextAccessor.HttpContext.Request.Cookies["id"];
                    ViewBag.UserName = _httpContextAccessor.HttpContext.Request.Cookies["username"];
                    ViewBag.UserRole = _httpContextAccessor.HttpContext.Request.Cookies["userrole"];
                    return View("Index",await list.ToListAsync());
                   
                }
               
            }
            //zemi gi usernames samo na members od profiles
            var members = _context.Members.Join(_context.Profiles,
                member => member.Profileid,
                profile => profile.Id,
                (member, profile) => new
                {
                    UserName = profile.Username
                }).ToArray().ToList();

            List<Books> books = _context.Books.Where(x => x.Numberofsamples > 0).ToList();
            ViewData["Books"] = new SelectList(books, "Id", "Bookname");
            ViewData["Members"] = new SelectList(members, "UserName", "UserName");
            ViewBag.UserId = _httpContextAccessor.HttpContext.Request.Cookies["id"];
            ViewBag.UserName = _httpContextAccessor.HttpContext.Request.Cookies["username"];
            ViewBag.UserRole = _httpContextAccessor.HttpContext.Request.Cookies["userrole"];
            return View();
        }

        // GET: Borrowings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrowings = await _context.Borrowings.FindAsync(id);
            if (borrowings == null)
            {
                return NotFound();
            }
            ViewData["Profileid"] = new SelectList(_context.Members, "Profileid", "Profileid", borrowings.Profileid);
            ViewData["Sampleid"] = new SelectList(_context.Samples, "Id", "Id", borrowings.Sampleid);
            ViewBag.UserId = _httpContextAccessor.HttpContext.Request.Cookies["id"];
            ViewBag.UserName = _httpContextAccessor.HttpContext.Request.Cookies["username"];
            ViewBag.UserRole = _httpContextAccessor.HttpContext.Request.Cookies["userrole"];
            return View(borrowings);
        }

        // POST: Borrowings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Borrowdate,Expirationdate,Returndate,Profileid,Sampleid")] Borrowings borrowings)
        {
            if (id != borrowings.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //update borrowing so return date
                    _context.Update(borrowings);
                    await _context.SaveChangesAsync();
                    //add fee for borrowing
                    TimeSpan span = (TimeSpan)(borrowings.Returndate - borrowings.Expirationdate);
                    double days = span.TotalDays;
                    if (days > 0)
                    {
                        var fee = new Fees();
                        fee.Borrowid = borrowings.Id;
                        fee.Feetotal = (int)days * 5;
                        var maxId = 0;
                        if (_context.Fees.Count() == 0)
                        {
                            maxId = 0;
                        }
                        else
                        {
                            maxId = _context.Fees.Max(x => x.Id);
                        }
                        fee.Id = ++maxId;
                        _context.Add(fee);
                        await _context.SaveChangesAsync();
                    }
                    //zgolemi go numberofsamples na book
                    var borrowing = _context.Borrowings.Where(x => x.Id == borrowings.Id).FirstOrDefault();
                    var samplee = _context.Samples.Where(x => x.Id == borrowings.Sampleid).FirstOrDefault();
                    var book = _context.Books.Where(x => x.Id == samplee.Bookid).FirstOrDefault();
                    book.Numberofsamples++;
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                    //smeni go statuso na sample vo true
                    var sample = _context.Samples.Where(x => x.Id == borrowings.Sampleid).FirstOrDefault();
                    sample.Samplestatus = true;
                    _context.Update(sample);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BorrowingsExists(borrowings.Id))
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
            ViewData["Profileid"] = new SelectList(_context.Members, "Profileid", "Profileid", borrowings.Profileid);
            ViewData["Sampleid"] = new SelectList(_context.Samples, "Id", "Id", borrowings.Sampleid);
            ViewBag.UserId = _httpContextAccessor.HttpContext.Request.Cookies["id"];
            ViewBag.UserName = _httpContextAccessor.HttpContext.Request.Cookies["username"];
            ViewBag.UserRole = _httpContextAccessor.HttpContext.Request.Cookies["userrole"];
            var list = _context.Borrowings.Join(_context.Samples,
               borrowing => borrowing.Sampleid,
               sample => sample.Id,
               (borrowing, sample) => new
               {
                   Id = borrowing.Id,
                   Borrowdate = borrowing.Borrowdate,
                   Expirationdate = borrowing.Expirationdate,
                   Returndate = borrowing.Returndate,
                   Bookid = sample.Bookid,
                   Profileid = borrowing.Profileid,
                   Sampleid = borrowing.Sampleid
               }).Join(_context.Books,
               bs => bs.Bookid,
               book => book.Id,
               (bs, book) => new BorrowingGridModel
               {
                   Id = bs.Id,
                   Bookname = book.Bookname,
                   Borrowdate = bs.Borrowdate,
                   Expirationdate = bs.Expirationdate,
                   Returndate = bs.Returndate,
                   Bookid = bs.Bookid,
                   Profileid = bs.Profileid,
                   Sampleid = bs.Sampleid
               });
            return View(borrowings);


        }

        // GET: Borrowings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var borrowings = await _context.Borrowings
                .Include(b => b.Profile)
                .Include(b => b.Sample)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (borrowings == null)
            {
                return NotFound();
            }
            ViewBag.UserId = _httpContextAccessor.HttpContext.Request.Cookies["id"];
            ViewBag.UserName = _httpContextAccessor.HttpContext.Request.Cookies["username"];
            ViewBag.UserRole = _httpContextAccessor.HttpContext.Request.Cookies["userrole"];
            return View(borrowings);
        }

        // POST: Borrowings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var borrowings = await _context.Borrowings.FindAsync(id);
            _context.Borrowings.Remove(borrowings);
            await _context.SaveChangesAsync();
            var borrowing = _context.Borrowings.Where(x => x.Id == id).FirstOrDefault();

            var book = _context.Books.Where(x => x.Id == borrowing.Sample.Bookid).FirstOrDefault();
            book.Numberofsamples++;
            _context.Update(book);
            await _context.SaveChangesAsync();


            ViewBag.UserId = _httpContextAccessor.HttpContext.Request.Cookies["id"];
            ViewBag.UserName = _httpContextAccessor.HttpContext.Request.Cookies["username"];
            ViewBag.UserRole = _httpContextAccessor.HttpContext.Request.Cookies["userrole"];
            return RedirectToAction(nameof(Index));
        }

        private bool BorrowingsExists(int id)
        {
            ViewBag.UserId = _httpContextAccessor.HttpContext.Request.Cookies["id"];
            ViewBag.UserName = _httpContextAccessor.HttpContext.Request.Cookies["username"];
            ViewBag.UserRole = _httpContextAccessor.HttpContext.Request.Cookies["userrole"];
            return _context.Borrowings.Any(e => e.Id == id);
        }
    }
}
