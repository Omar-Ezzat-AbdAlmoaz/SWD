using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using SWDteam.Data;
using SWDteam.Models;

namespace SWDteam.Controllers
{
    public class CoursesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public CoursesController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Courses
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.courses.Include(c => c.Department).Include(c => c.Instructor);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.courses == null)
            {
                return NotFound();
            }

            var course = await _context.courses
                .Include(c => c.Department)
                .Include(c => c.Instructor)
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            ViewData["DepartmentID"] = new SelectList(_context.departments, "DepartmentID", "DepartmentName");
            ViewData["InstructorID"] = new SelectList(_context.instructors, "InstructorId", "InstructorName");
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CourseId,CourseName,CourseDescription,CourseDuration,CoursePrice,Coursedate,DepartmentID,InstructorID")] Course course, IFormFile img_file, IFormFile vedio_file)
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
                    //ViewBag.Message = string.Format("<b>{0}<b> uploaded .</br>",img_file.FileName.ToString());
                    course.CourseImage = img_file.FileName;
                }

            }
            else
            {
                course.CourseImage = "DefaultCourse.png";
            }

            string pathVedio = Path.Combine(_environment.WebRootPath, "Vedio");
            if (!Directory.Exists(pathVedio))
            {
                Directory.CreateDirectory(pathVedio);
            }

            if (vedio_file != null)
            {
                pathVedio = Path.Combine(pathVedio, vedio_file.FileName);
                using (var stream = new FileStream(pathVedio, FileMode.Create))
                {
                    await vedio_file.CopyToAsync(stream);
                    //ViewBag.Message = string.Format("<b>{0}<b> uploaded .</br>",img_file.FileName.ToString());
                    course.CourseVedio = vedio_file.FileName;
                }

            }

            try
            {
                _context.Add(course);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ViewBag.exc = ex.Message;
            }
            ViewData["DepartmentID"] = new SelectList(_context.departments, "DepartmentID", "DepartmentName", course.DepartmentID);
            ViewData["InstructorID"] = new SelectList(_context.instructors, "InstructorId", "InstructorName", course.InstructorID);
            return View(course);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.courses == null)
            {
                return NotFound();
            }

            var course = await _context.courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            ViewData["DepartmentID"] = new SelectList(_context.departments, "DepartmentID", "DepartmentName", course.DepartmentID);
            ViewData["InstructorID"] = new SelectList(_context.instructors, "InstructorId", "InstructorName", course.InstructorID);
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CourseId,CourseName,CourseDescription,CourseImage,CourseVedio,CourseDuration,CoursePrice,Coursedate,DepartmentID,InstructorID")] Course course)
        {
            if (id != course.CourseId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.CourseId))
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
            ViewData["DepartmentID"] = new SelectList(_context.departments, "DepartmentID", "DepartmentName", course.DepartmentID);
            ViewData["InstructorID"] = new SelectList(_context.instructors, "InstructorId", "InstructorName", course.InstructorID);
            return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.courses == null)
            {
                return NotFound();
            }

            var course = await _context.courses
                .Include(c => c.Department)
                .Include(c => c.Instructor)
                .FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.courses == null)
            {
                return Problem("Entity set 'AppDbContext.courses'  is null.");
            }
            var course = await _context.courses.FindAsync(id);
            if (course != null)
            {
                _context.courses.Remove(course);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
            return (_context.courses?.Any(e => e.CourseId == id)).GetValueOrDefault();
        }
    }
}
