using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Library;

namespace Library.Controllers
{
    public class BorrowingsController : Controller
    {
        private readonly db_201920z_va_prj_my_libContext _context;

        public BorrowingsController(db_201920z_va_prj_my_libContext context)
        {
            _context = context;
        }

        // GET: Borrowings
        public async Task<IActionResult> Index()
        {
            var db_201920z_va_prj_my_libContext = _context.Borrowings.Include(b => b.Profile).Include(b => b.Sample);
            return View(await db_201920z_va_prj_my_libContext.ToListAsync());
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

            return View(borrowings);
        }

        // GET: Borrowings/Create
        public IActionResult Create()
        {
            List<Books> books = _context.Books.Where(x => x.Numberofsamples > 0).ToList();
            ViewData["Books"] = new SelectList(books, "Id", "Bookname");
            ViewData["Members"] = new SelectList(_context.Members, "Profileid", "Profileid");
            return View();
        }

        // POST: Borrowings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookId,MemberId,BorrowDate,ExpirationDate")] MakeBorrowing makeBorrowing)
        {
            if (ModelState.IsValid)
            {
                Borrowings borrowing = new Borrowings();
                borrowing.Borrowdate = makeBorrowing.BorrowDate;
                borrowing.Expirationdate = makeBorrowing.ExpirationDate;
                borrowing.Profileid = makeBorrowing.MemberId;
                Samples sampleToBorrow = _context.Samples.Where(x => x.Bookid == makeBorrowing.BookId && x.Samplestatus == true).OrderBy(x => x.Id).FirstOrDefault();
                borrowing.Sampleid = sampleToBorrow.Id;
                var maxId = _context.Borrowings.Max(x => x.Id);
                borrowing.Id = ++maxId;
                _context.Add(borrowing);
                sampleToBorrow.Samplestatus = false;
                _context.Update(sampleToBorrow);
                Books book = _context.Books.Where(x => x.Id == sampleToBorrow.Bookid).FirstOrDefault();
                book.Numberofsamples = book.Numberofsamples-1;
                _context.Update(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            List<Books> books = _context.Books.Where(x => x.Numberofsamples > 0).ToList();
            ViewData["Books"] = new SelectList(books, "Id", "Bookname");
            ViewData["Members"] = new SelectList(_context.Members, "Profileid", "Profileid");
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
                    _context.Update(borrowings);
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
            return RedirectToAction(nameof(Index));
        }

        private bool BorrowingsExists(int id)
        {
            return _context.Borrowings.Any(e => e.Id == id);
        }
    }
}
