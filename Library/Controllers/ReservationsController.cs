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
    public class ReservationsController : Controller
    {
        private readonly db_201920z_va_prj_my_libContext _context;

        public ReservationsController(db_201920z_va_prj_my_libContext context)
        {
            _context = context;
        }

        // GET: Reservations
        public async Task<IActionResult> Index()
        {
            var db_201920z_va_prj_my_libContext = _context.Reservations.Include(r => r.Profile);
            return View(await db_201920z_va_prj_my_libContext.ToListAsync());
        }

        // GET: Reservations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservations = await _context.Reservations
                .Include(r => r.Profile)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservations == null)
            {
                return NotFound();
            }

            return View(reservations);
        }

        // GET: Reservations/Create
        public IActionResult Create()
        {
            List<Books> books = _context.Books.Where(x => x.Numberofsamples > 0).ToList();
            ViewData["Books"] = new SelectList(books, "Id", "Bookname");
            return View();
        }

        // POST: Reservations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookId,ReservationDate")] MakeReservation makeReservation)
        {
            if (ModelState.IsValid)
            {
                Reservations reservation = new Reservations();
                reservation.Reservationstatus = false;
                reservation.Reservationdate = makeReservation.ReservationDate;
                reservation.Profileid = 1;
                var maxId = _context.Reservations.Max(x => x.Id);
                reservation.Id = ++maxId;
                _context.Add(reservation);
                await _context.SaveChangesAsync();
                int newId = reservation.Id;
                Reservedby reservedBy = new Reservedby();
                reservedBy.Bookid = makeReservation.BookId;
                reservedBy.Reservationid = newId;
                _context.Add(reservedBy);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Books"] = new SelectList(_context.Books, "Id", "Bookname", makeReservation.BookId);
            return View(makeReservation);
        }

        // POST: Reservations/Update/5
        //****************************
        [HttpGet]
        public async Task<IActionResult> ConfirmReservation(int id)
        {
            Reservations reservations = _context.Reservations.Where(x => x.Id == id).SingleOrDefault();
            if (reservations == null)
            {
                return NotFound();
            }
            
                try
                {
                    reservations.Reservationstatus = true;
                    _context.Update(reservations);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationsExists(reservations.Id))
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
        //******************
        // GET: Reservations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservations = await _context.Reservations.FindAsync(id);
            if (reservations == null)
            {
                return NotFound();
            }
            ViewData["Profileid"] = new SelectList(_context.Members, "Profileid", "Profileid", reservations.Profileid);
            return View(reservations);
        }

        // POST: Reservations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Reservationstatus,Reservationdate,Profileid")] Reservations reservations)
        {
            if (id != reservations.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(reservations);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ReservationsExists(reservations.Id))
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
            ViewData["Profileid"] = new SelectList(_context.Members, "Profileid", "Profileid", reservations.Profileid);
            return View(reservations);
        }

        // GET: Reservations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var reservations = await _context.Reservations
                .Include(r => r.Profile)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (reservations == null)
            {
                return NotFound();
            }

            return View(reservations);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservations = await _context.Reservations.FindAsync(id);
            _context.Reservations.Remove(reservations);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationsExists(int id)
        {
            return _context.Reservations.Any(e => e.Id == id);
        }
    }
}
