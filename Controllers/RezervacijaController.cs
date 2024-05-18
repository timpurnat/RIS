using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using web.Data;
using web.Models;
using Microsoft.AspNetCore.Authorization;
using System.Data.SqlClient;
namespace web.Controllers


{
    [Authorize]
    public class RezervacijaController : Controller
    {
        private readonly SchoolContext _context;
private readonly UserManager<ApplicationUser> _usermanager;
       public RezervacijaController(SchoolContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _usermanager = userManager;
        }

    
public async Task<IActionResult> Index()
{
   
    var schoolContext = _context.Rezervacije
        .Include(r => r.Izdelek)
        .Include(r => r.Owner);

    return View(await schoolContext.ToListAsync());
}


      
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervacija = await _context.Rezervacije
                .Include(r => r.Izdelek)
                .FirstOrDefaultAsync(m => m.RezervacijaId == id);
            if (rezervacija == null)
            {
                return NotFound();
            }

            return View(rezervacija);
        }

    
public IActionResult Create()
{
    ViewBag.Naslov = new SelectList(_context.Izdelki, "IzdelekId", "Naslov");
    return View();
}


      
        [ValidateAntiForgeryToken]
[HttpPost]

public async Task<IActionResult> Create([Bind("RezervacijaId,datumPrevzema,datumZapadlosti,IzdelekId,DateEdited,DateCreated")] Rezervacija rezervacija)
        {
           var currentUser = await _usermanager.GetUserAsync(User);
            if (ModelState.IsValid)
            {
                rezervacija.DateCreated = DateTime.Now;
                rezervacija.DateEdited = DateTime.Now;
                rezervacija.Owner = currentUser;
                _context.Add(rezervacija);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IzdelekId"] = new SelectList(_context.Izdelki, "IzdelekId", "Naslov", rezervacija.IzdelekId);
            return View(rezervacija);
        }
    




     
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervacija = await _context.Rezervacije.FindAsync(id);
            if (rezervacija == null)
            {
                return NotFound();
            }
            ViewData["KnjigaId"] = new SelectList(_context.Izdelki, "KnjigaId", "KnjigaId", rezervacija.IzdelekId);
            return View(rezervacija);
        }

   
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RezervacijaId,datumPrevzema,datumZapadlosti,KnjigaId,DateEdited,DateCreated")] Rezervacija rezervacija)
        {
            if (id != rezervacija.RezervacijaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rezervacija);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RezervacijaExists(rezervacija.RezervacijaId))
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
            ViewData["KnjigaId"] = new SelectList(_context.Izdelki, "KnjigaId", "KnjigaId", rezervacija.IzdelekId);
            return View(rezervacija);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var rezervacija = await _context.Rezervacije
                .Include(r => r.Izdelek)
                .FirstOrDefaultAsync(m => m.RezervacijaId == id);
            if (rezervacija == null)
            {
                return NotFound();
            }

            return View(rezervacija);
        }

        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var rezervacija = await _context.Rezervacije.FindAsync(id);
            if (rezervacija != null)
            {
                _context.Rezervacije.Remove(rezervacija);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RezervacijaExists(int id)
        {
            return _context.Rezervacije.Any(e => e.RezervacijaId == id);
        }
    }
}
