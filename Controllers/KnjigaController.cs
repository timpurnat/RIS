using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using web.Data;
using web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using web.Models;
using help;

namespace web.Controllers
{
    public class KnjigaController : Controller
    {
        private readonly SchoolContext _context;

        public KnjigaController(SchoolContext context)
        {
            _context = context;
        }

      
        public async Task<IActionResult> Index(string searchString, string sortOrder, int? page, string sortBy)
        {
            
            int pageSize = 6;

            
            var izdelki = _context.Izdelki.AsQueryable();

            
            if (!String.IsNullOrEmpty(searchString))
            {
                izdelki = izdelki.Where(k => k.Naslov.Contains(searchString));
            }

           
            ViewData["NaslovSortParm"] = sortOrder == "naslov" ? "naslov_desc" : "naslov";
            ViewData["AvtorSortParm"] = sortOrder == "avtor" ? "avtor_desc" : "avtor";
            ViewData["ZvrstSortParm"] = sortOrder == "zvrst" ? "zvrst_desc" : "zvrst";
            ViewData["KategorijaSortParm"] = sortOrder == "kategorija" ? "kategorija_desc" : "kategorija";
            ViewData["OcenaSortParm"] = sortOrder == "ocena" ? "ocena_desc" : "ocena";



            
            izdelki = izdelki.Include(k => k.znamka).Include(k => k.Kategorija).Include(k => k.Zvrst);

            
            var pageNumber = page ?? 1;
            var pagedIzdelki = await PaginatedList<Izdelek>.CreateAsync(izdelki.AsNoTracking(), pageNumber, pageSize);

           
            return View(pagedIzdelki);
        }

      
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var knjiga = await _context.Izdelki
                .Include(k => k.znamka)
                .Include(k => k.Kategorija)
                .Include(k => k.Zvrst)
                .FirstOrDefaultAsync(m => m.IzdelekId == id);
            if (knjiga == null)
            {
                return NotFound();
            }

            return View(knjiga);
        }
public IActionResult Rezervacija(int? id)
{
    if (id == null)
    {
        return NotFound();
    }

    return RedirectToAction("Create", "Rezervacija", new { id = id });
}
        // GET: Knjiga/Create
        [Authorize(Roles = "Administrator, Manager")]
        public IActionResult Create()
        {
            ViewData["AvtorID"] = new SelectList(_context.Znamke, "AvtorID", "PriimekIme");
            ViewData["KategorijaID"] = new SelectList(_context.Kategorija, "KategorijaID", "imeKategorije");
            ViewData["ZvrstID"] = new SelectList(_context.Zvrsti, "ZvrstID", "ImeZvrsti");
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Manager")]
        public async Task<IActionResult> Create([Bind("KnjigaId,Naslov,Ocena,AvtorID,ZvrstID,KategorijaID,ImageLink")] Izdelek izdelek)
        {
            if (ModelState.IsValid)
            {
                _context.Add(izdelek);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AvtorID"] = new SelectList(_context.Znamke, "AvtorID", "AvtorID", izdelek.znamkaID);
            ViewData["KategorijaID"] = new SelectList(_context.Kategorija, "KategorijaID", "KategorijaID", izdelek.KategorijaID);
            ViewData["ZvrstID"] = new SelectList(_context.Zvrsti, "ZvrstID", "ZvrstID", izdelek.ZvrstID);
            return View(izdelek);
        }

   
        [Authorize(Roles = "Administrator, Manager")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var izdelek = await _context.Izdelki.FindAsync(id);
            if (izdelek == null)
            {
                return NotFound();
            }

          
            if (User.IsInRole("Administrator") || User.IsInRole("Manager"))
            {
                ViewData["AvtorID"] = new SelectList(_context.Znamke, "AvtorID", "AvtorID", izdelek.znamkaID);
                ViewData["KategorijaID"] = new SelectList(_context.Kategorija, "KategorijaID", "KategorijaID", izdelek.KategorijaID);
                ViewData["ZvrstID"] = new SelectList(_context.Zvrsti, "ZvrstID", "ZvrstID", izdelek.ZvrstID);
                return View(izdelek);
            }
            else
            {
               
                return RedirectToAction("UnauthorizedAction", "Account");
            }
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Manager")]
        public async Task<IActionResult> Edit(int id, [Bind("KnjigaId,Naslov,Ocena,AvtorID,ZvrstID,KategorijaID")] Izdelek izdelek)
        {
            if (id != izdelek.IzdelekId)
            {
                return NotFound();
            }

        
            if (User.IsInRole("Administrator") || User.IsInRole("Manager"))
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(izdelek);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!KnjigaExists(izdelek.IzdelekId))
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
                ViewData["AvtorID"] = new SelectList(_context.Znamke, "AvtorID", "AvtorID", izdelek.znamkaID);
                ViewData["KategorijaID"] = new SelectList(_context.Kategorija, "KategorijaID", "KategorijaID", izdelek.KategorijaID);
                ViewData["ZvrstID"] = new SelectList(_context.Zvrsti, "ZvrstID", "ZvrstID", izdelek.ZvrstID);
                return View(izdelek);
            }
            else
            {
             
                return RedirectToAction("UnauthorizedAction", "Account");
            }
        }

      
        [Authorize(Roles = "Administrator, Manager")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var knjiga = await _context.Izdelki
                .Include(k => k.znamka)
                .Include(k => k.Kategorija)
                .Include(k => k.Zvrst)
                .FirstOrDefaultAsync(m => m.IzdelekId == id);
            if (knjiga == null)
            {
                return NotFound();
            }

            
            if (User.IsInRole("Administrator") || User.IsInRole("Manager"))
            {
                return View(knjiga);
            }
            else
            {
                
                return RedirectToAction("UnauthorizedAction", "Account");
            }
        }

      
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator, Manager")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            
            if (User.IsInRole("Administrator") || User.IsInRole("Manager"))
            {
                var knjiga = await _context.Izdelki.FindAsync(id);
                if (knjiga != null)
                {
                    _context.Izdelki.Remove(knjiga);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
              
                return RedirectToAction("UnauthorizedAction", "Account");
            }
        }

        private bool KnjigaExists(int id)
        {
            return _context.Izdelki.Any(e => e.IzdelekId == id);
        }
    }
}
