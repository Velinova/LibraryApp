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
    public class ReservationsController : Controller
    {
        private readonly db_201920z_va_prj_my_libContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReservationsController(db_201920z_va_prj_my_libContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            ViewBag.UserId = _httpContextAccessor.HttpContext.Request.Cookies["id"];
            ViewBag.UserName = _httpContextAccessor.HttpContext.Request.Cookies["username"];
            ViewBag.UserRole = _httpContextAccessor.HttpContext.Request.Cookies["userrole"];

        }

        // GET: Reservations
        public async Task<IActionResult> Index()
        {
            //var db_201920z_va_prj_my_libContext = _context.Reservations;
            var list = _context.Reservations.Join(_context.Reservedby,
               reservation => reservation.Id,
               reserved => reserved.Reservationid,
               (reservation, reserved) => new
               {
                   Id = reservation.Id,
                   ProfileId = reservation.Profileid,
                   RId = reserved.Bookid,
                   Reservationstatus = reservation.Reservationstatus,
                   Reservationdate = reservation.Reservationdate,

               }).Join(_context.Books,
               rr => rr.RId,
               book => book.Id,
               (rr, book) => new ReservationGridModel
               {
                   Id = rr.Id,
                   RId = rr.RId,
                   Profileid = rr.ProfileId,
                   Reservationstatus = rr.Reservationstatus,
                   Reservationdate = rr.Reservationdate,
                   Bookname = book.Bookname,
               });
            ViewBag.UserId = _httpContextAccessor.HttpContext.Request.Cookies["id"];
            ViewBag.UserName = _httpContextAccessor.HttpContext.Request.Cookies["username"];
            ViewBag.UserRole = _httpContextAccessor.HttpContext.Request.Cookies["userrole"];
            var id = Int32.Parse(_httpContextAccessor.HttpContext.Request.Cookies["id"]);
            var numberOfReservations = _context.Reservations.Where(x => x.Profileid == id && x.Reservationstatus == false).ToList().Count;
            ViewBag.NumberOfReservations = numberOfReservations;
            return View(await list.ToListAsync());

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
            ViewBag.UserId = _httpContextAccessor.HttpContext.Request.Cookies["id"];
            ViewBag.UserName = _httpContextAccessor.HttpContext.Request.Cookies["username"];
            ViewBag.UserRole = _httpContextAccessor.HttpContext.Request.Cookies["userrole"];
            return View(reservations);
        }

        // GET: Reservations/Create
        public IActionResult Create()
        {
            List<Books> books = _context.Books.Where(x => x.Numberofsamples > 0).ToList();
            ViewData["Books"] = new SelectList(books, "Id", "Bookname");
            ViewBag.UserId = _httpContextAccessor.HttpContext.Request.Cookies["id"];
            ViewBag.UserName = _httpContextAccessor.HttpContext.Request.Cookies["username"];
            ViewBag.UserRole = _httpContextAccessor.HttpContext.Request.Cookies["userrole"];
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
                TimeSpan razlika = makeReservation.ReservationDate - System.DateTime.Now;                
                if (razlika.Days <= 3)
                {
                    Reservations reservation = new Reservations();
                    reservation.Reservationstatus = false;
                    reservation.Reservationdate = makeReservation.ReservationDate;
                    reservation.Profileid = Int32.Parse(_httpContextAccessor.HttpContext.Request.Cookies["id"]);
                    var maxId = 0;
                    if (_context.Reservations.Count() == 0)
                    {
                        maxId = 0;
                    }
                    else
                    {
                        maxId = _context.Reservations.Max(x => x.Id);
                    }
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
                else
                {
                    ViewBag.Error = "true";
                    return RedirectToAction(nameof(Index));
                }
            }
            //ViewData["Books"] = new SelectList(_context.Books, "Id", "Bookname", makeReservation.BookId);
            ViewBag.UserId = _httpContextAccessor.HttpContext.Request.Cookies["id"];
            ViewBag.UserName = _httpContextAccessor.HttpContext.Request.Cookies["username"];
            ViewBag.UserRole = _httpContextAccessor.HttpContext.Request.Cookies["userrole"];
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
            ViewBag.UserId = _httpContextAccessor.HttpContext.Request.Cookies["id"];
            ViewBag.UserName = _httpContextAccessor.HttpContext.Request.Cookies["username"];
            ViewBag.UserRole = _httpContextAccessor.HttpContext.Request.Cookies["userrole"];
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
            ViewBag.UserId = _httpContextAccessor.HttpContext.Request.Cookies["id"];
            ViewBag.UserName = _httpContextAccessor.HttpContext.Request.Cookies["username"];
            ViewBag.UserRole = _httpContextAccessor.HttpContext.Request.Cookies["userrole"];
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
            ViewBag.UserId = _httpContextAccessor.HttpContext.Request.Cookies["id"];
            ViewBag.UserName = _httpContextAccessor.HttpContext.Request.Cookies["username"];
            ViewBag.UserRole = _httpContextAccessor.HttpContext.Request.Cookies["userrole"];
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
            ViewBag.UserId = _httpContextAccessor.HttpContext.Request.Cookies["id"];
            ViewBag.UserName = _httpContextAccessor.HttpContext.Request.Cookies["username"];
            ViewBag.UserRole = _httpContextAccessor.HttpContext.Request.Cookies["userrole"];
            return View(reservations);
        }

        // POST: Reservations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var reservationid = await _context.Reservations.FindAsync(id);
            var broj = _context.Reservedby.Join(_context.Books,
               reserved => reserved.Bookid,
               book => book.Id,
               (reserved, book) => new
               {
                   rid = reserved.Reservationid,
                   bid = reserved.Bookid,

               }).Where(x => x.rid == id).FirstOrDefault();

            var reservedby = await _context.Reservedby.FindAsync(id, broj.bid);

            _context.Reservedby.Remove(reservedby);
            _context.Reservations.Remove(reservationid);
            await _context.SaveChangesAsync();
            ViewBag.UserId = _httpContextAccessor.HttpContext.Request.Cookies["id"];
            ViewBag.UserName = _httpContextAccessor.HttpContext.Request.Cookies["username"];
            ViewBag.UserRole = _httpContextAccessor.HttpContext.Request.Cookies["userrole"];
            return RedirectToAction(nameof(Index));
        }

        private bool ReservationsExists(int id)
        {
            ViewBag.UserId = _httpContextAccessor.HttpContext.Request.Cookies["id"];
            ViewBag.UserName = _httpContextAccessor.HttpContext.Request.Cookies["username"];
            ViewBag.UserRole = _httpContextAccessor.HttpContext.Request.Cookies["userrole"];
            return _context.Reservations.Any(e => e.Id == id);
        }
    }
}
