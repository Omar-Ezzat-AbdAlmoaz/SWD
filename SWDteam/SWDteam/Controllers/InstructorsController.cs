using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SWDteam.Data;
using SWDteam.Models;

namespace SWDteam.Controllers
{
    public class InstructorsController : Controller
    {
        private readonly AppDbContext _context;

        public InstructorsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Instructors
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.instructors.Include(i => i.Department);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Instructors/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.instructors == null)
            {
                return NotFound();
            }

            var instructor = await _context.instructors
                .Include(i => i.Department)
                .FirstOrDefaultAsync(m => m.InstructorId == id);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // GET: Instructors/Create
        public IActionResult Create()
        {
            ViewData["DepartmentID"] = new SelectList(_context.departments, "DepartmentId", "DepartmentName");
            return View();
        }

        // POST: Instructors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InstructorId,InstructorName,InstrucrorImage,InstructorEmail,Instructorbiography,Instructorexperience,InstructorRate,DepartmentID")] Instructor instructor)
        {
            if (ModelState.IsValid)
            {
                _context.Add(instructor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentID"] = new SelectList(_context.departments, "DepartmentId", "DepartmentName", instructor.DepartmentID);
            return View(instructor);
        }

        // GET: Instructors/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.instructors == null)
            {
                return NotFound();
            }

            var instructor = await _context.instructors.FindAsync(id);
            if (instructor == null)
            {
                return NotFound();
            }
            ViewData["DepartmentID"] = new SelectList(_context.departments, "DepartmentId", "DepartmentName", instructor.DepartmentID);
            return View(instructor);
        }

        // POST: Instructors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InstructorId,InstructorName,InstrucrorImage,InstructorEmail,Instructorbiography,Instructorexperience,InstructorRate,DepartmentID")] Instructor instructor)
        {
            if (id != instructor.InstructorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(instructor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InstructorExists(instructor.InstructorId))
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
            ViewData["DepartmentID"] = new SelectList(_context.departments, "DepartmentId", "DepartmentName", instructor.DepartmentID);
            return View(instructor);
        }

        // GET: Instructors/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.instructors == null)
            {
                return NotFound();
            }

            var instructor = await _context.instructors
                .Include(i => i.Department)
                .FirstOrDefaultAsync(m => m.InstructorId == id);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // POST: Instructors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.instructors == null)
            {
                return Problem("Entity set 'AppDbContext.instructors'  is null.");
            }
            var instructor = await _context.instructors.FindAsync(id);
            if (instructor != null)
            {
                _context.instructors.Remove(instructor);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InstructorExists(int id)
        {
          return (_context.instructors?.Any(e => e.InstructorId == id)).GetValueOrDefault();
        }
    }
}
