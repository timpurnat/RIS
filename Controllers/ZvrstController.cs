using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using web.Data;
using web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
namespace web.Controllers
{
    //
    [Authorize(Roles = "Administrator, Manager")]
    public class ZvrstController : Controller
    {
        private readonly SchoolContext _context;
       private readonly UserManager<ApplicationUser> _usermanager;
        public ZvrstController(SchoolContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _usermanager = userManager;
        }

       
        public async Task<IActionResult> Index()
        {
            return View(await _context.Zvrsti.ToListAsync());
        }

     
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zvrst = await _context.Zvrsti
                .FirstOrDefaultAsync(m => m.ZvrstID == id);
            if (zvrst == null)
            {
                return NotFound();
            }

            return View(zvrst);
        }

       
        public IActionResult Create()
        {
            return View();
        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ZvrstID,ImeZvrsti")] Zvrst zvrst)
        {
            var currentUser = await _usermanager.GetUserAsync(User);
            if (ModelState.IsValid)
            {
                _context.Add(zvrst);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(zvrst);
        }

      
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zvrst = await _context.Zvrsti.FindAsync(id);
            if (zvrst == null)
            {
                return NotFound();
            }
            return View(zvrst);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ZvrstID,ImeZvrsti")] Zvrst zvrst)
        {
            if (id != zvrst.ZvrstID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(zvrst);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ZvrstExists(zvrst.ZvrstID))
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
            return View(zvrst);
        }

      
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zvrst = await _context.Zvrsti
                .FirstOrDefaultAsync(m => m.ZvrstID == id);
            if (zvrst == null)
            {
                return NotFound();
            }

            return View(zvrst);
        }

       
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var zvrst = await _context.Zvrsti.FindAsync(id);
            if (zvrst != null)
            {
                _context.Zvrsti.Remove(zvrst);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ZvrstExists(int id)
        {
            return _context.Zvrsti.Any(e => e.ZvrstID == id);
        }
    }
}
