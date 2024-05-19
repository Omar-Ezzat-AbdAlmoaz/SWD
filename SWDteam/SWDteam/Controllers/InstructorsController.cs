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
    public class InstructorsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;
        public InstructorsController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
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
            List<Course> courses = _context.courses.Where(m => m.InstructorID == id).ToList();
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        // GET: Instructors/Create
        public IActionResult Create()
        {
            ViewData["DepartmentID"] = new SelectList(_context.departments, "DepartmentID", "DepartmentName");
            return View();
        }

        // POST: Instructors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("InstructorName,InstructorEmail,Instructorbiography,Instructorexperience,DepartmentID")] Instructor instructor, IFormFile img_file)
        {
            List<Instructor> instructors = _context.instructors.ToList();
            string s = instructor.InstructorName;
            int z = instructor.DepartmentID;
            int x = 0, m = 0;
            foreach (var i in instructors)
            {
                if (i.InstructorName == s)
                {
                    x++;
                }
            }
            if (x != 0)
            {
                foreach (var ii in instructors)
                {
                    if (ii.DepartmentID == z)
                    {
                        m++;
                    }
                }
            }
            if (m == 0)
            {
                string path = Path.Combine(_environment.WebRootPath, "Img");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                if (img_file != null)
                {
                    path = Path.Combine(path, img_file.FileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await img_file.CopyToAsync(stream);
                        ViewBag.Message = string.Format("<b>{0}<b> uploaded .</br>", img_file.FileName.ToString());
                        instructor.InstrucrorImage = img_file.FileName;

                    }

                }
                else
                {
                    instructor.InstrucrorImage = "DefaultInstructor.png";
                }

                try
                {
                    _context.Add(instructor);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ViewBag.exc = ex.Message;
                }
                ViewData["DepartmentID"] = new SelectList(_context.departments, "DepartmentID", "DepartmentName", instructor.DepartmentID);
                return View(instructor);
            }
            else
            {
                ModelState.AddModelError("", "Instructor with the same name already exists.");

                ViewData["DepartmentID"] = new SelectList(_context.departments, "DepartmentID", "DepartmentName", instructor.DepartmentID);
                return View(instructor);
            }
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
            ViewData["DepartmentID"] = new SelectList(_context.departments, "DepartmentID", "DepartmentName", instructor.DepartmentID);
            return View(instructor);
        }

        // POST: Instructors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InstructorId,InstructorName,InstructorEmail,Instructorbiography,Instructorexperience,DepartmentID")] Instructor instructor, IFormFile img_file)
        {
            if (id != instructor.InstructorId)
            {
                return NotFound();
            }
            string path = Path.Combine(_environment.WebRootPath, "Img");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            if (img_file != null)
            {
                path = Path.Combine(path, img_file.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await img_file.CopyToAsync(stream);
                    ViewBag.Message = string.Format("<b>{0}<b> uploaded .</br>", img_file.FileName.ToString());
                    instructor.InstrucrorImage = img_file.FileName;

                }

            }
            else
            {
                instructor.InstrucrorImage = "DefaultInstructor.png";
            }

            if (instructor.InstructorName != null && instructor.InstrucrorImage != null && instructor.InstructorEmail != null && instructor.Instructorbiography != null && instructor.Instructorexperience != null)
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
                return RedirectToAction("Index");
            }
            ViewData["DepartmentID"] = new SelectList(_context.departments, "DepartmentID", "DepartmentName", instructor.DepartmentID);
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
                List<Course> courses = _context.courses.Where(m => m.InstructorID == id).ToList();
                if (courses != null)
                {
                    foreach (var course in courses)
                    {
                        _context.courses.Remove(course);
                    }
                }
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
