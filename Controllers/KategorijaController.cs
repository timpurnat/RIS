using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using web.Data;
using web.Models;
using Microsoft.AspNetCore.Authorization;
namespace web.Controllers
{
    [Authorize(Roles = "Administrator, Manager")]
    public class KategorijaController : Controller
    {
        private readonly SchoolContext _context;
private readonly UserManager<ApplicationUser> _usermanager;
        public KategorijaController(SchoolContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _usermanager = userManager;
        }

    
        public async Task<IActionResult> Index()
        {
            return View(await _context.Kategorija.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kategorija = await _context.Kategorija
                .FirstOrDefaultAsync(m => m.KategorijaID == id);
            if (kategorija == null)
            {
                return NotFound();
            }

            return View(kategorija);
        }

     
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("KategorijaID,imeKategorije")] Kategorija kategorija)
        {
            var currentUser = await _usermanager.GetUserAsync(User);
            if (ModelState.IsValid)
            {
                _context.Add(kategorija);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(kategorija);
        }

    
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kategorija = await _context.Kategorija.FindAsync(id);
            if (kategorija == null)
            {
                return NotFound();
            }
            return View(kategorija);
        }

  
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("KategorijaID,imeKategorije")] Kategorija kategorija)
        {
            if (id != kategorija.KategorijaID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kategorija);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KategorijaExists(kategorija.KategorijaID))
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
            return View(kategorija);
        }

      
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kategorija = await _context.Kategorija
                .FirstOrDefaultAsync(m => m.KategorijaID == id);
            if (kategorija == null)
            {
                return NotFound();
            }

            return View(kategorija);
        }

       
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var kategorija = await _context.Kategorija.FindAsync(id);
            if (kategorija != null)
            {
                _context.Kategorija.Remove(kategorija);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KategorijaExists(int id)
        {
            return _context.Kategorija.Any(e => e.KategorijaID == id);
        }
    }
}
