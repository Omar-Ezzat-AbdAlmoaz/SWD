using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SWDteam.Data;
using SWDteam.Models;

namespace SWDteam.Controllers
{
    [Authorize]
    public class DepartmentsController : Controller
    {
        private readonly AppDbContext _context;

        public DepartmentsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Departments
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.departments.Include(d => d.Category);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Departments/Details/5

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.departments == null)
            {
                return NotFound();
            }

            var department = await _context.departments
                .Include(d => d.Category)
                .FirstOrDefaultAsync(m => m.DepartmentID == id);
            List<Course> Courses = _context.courses.Where(m => m.DepartmentID == id).ToList();
            List<Instructor> instructors = _context.instructors.Where(m => m.DepartmentID == id).ToList();

            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }


        // GET: Departments/Create
        [Authorize(Roles = "Admin")]

        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.categories, "CategoryId", "CategoryName");
            return View();
        }

        // POST: Departments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Create([Bind("DepartmentID,DepartmentName,DepartmentDescription,CategoryId")] Department department)
        {

            _context.Add(department);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

            ViewData["CategoryId"] = new SelectList(_context.categories, "CategoryId", "CategoryName", department.CategoryId);
            return View(department);
        }

        // GET: Departments/Edit/5
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.departments == null)
            {
                return NotFound();
            }
            var department = await _context.departments.FindAsync(id);
            if (department == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.categories, "CategoryId", "CategoryName", department.CategoryId);
            return View(department);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Edit(int id, [Bind("DepartmentId,DepartmentName,DepartmentDescription,CategoryId")] Department department)
        {
            if (id != department.DepartmentID)
            {
                return NotFound();
            }

            if (department.DepartmentName != null && department.DepartmentDescription!= null)
            {
                try
                {
                    _context.Update(department);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentExists(department.DepartmentID))
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

            ViewData["CategoryId"] = new SelectList(_context.categories, "CategoryId", "CategoryName", department.CategoryId);
            return View(department);
        }

        // GET: Departments/Delete/5
        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.departments == null)
            {
                return NotFound();
            }

            var department = await _context.departments
                .Include(d => d.Category)
                .FirstOrDefaultAsync(m => m.DepartmentID == id);
            if (department == null)
            {
                return NotFound();
            }

            return View(department);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.departments == null)
            {
                return Problem("Entity set 'AppDbContext.departments'  is null.");
            }
            var department = await _context.departments.FindAsync(id);
            if (department != null)
            {
                List<Course> courses = _context.courses.Where(m => m.DepartmentID == id).ToList();
                List<Instructor> instructors = _context.instructors.Where(m => m.DepartmentID == id).ToList();
                if (courses != null)
                {
                    foreach (var course in courses)
                    {
                        _context.courses.Remove(course);
                    }
                }
                if (instructors != null) { 
                    foreach (var instructor in instructors)
                    {
                        _context.instructors.Remove(instructor);
                    }
                }
                _context.departments.Remove(department);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DepartmentExists(int id)
        {
            return (_context.departments?.Any(e => e.DepartmentID == id)).GetValueOrDefault();
        }

    }
}
