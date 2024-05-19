using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SWDteam.Data;
using SWDteam.Models;
using Microsoft.Extensions.Hosting;

namespace SWDteam.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public CategoriesController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            return _context.categories != null ?
                        View(await _context.categories.ToListAsync()) :
                        Problem("Entity set 'AppDbContext.categories'  is null.");
        }

        // GET: Categories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.categories == null)
            {
                return NotFound();
            }

            var category = await _context.categories
                .FirstOrDefaultAsync(m => m.CategoryId == id);

            List<Department> departments = _context.departments.Where(m => m.CategoryId == id).ToList();
            //List<Course> courses = _context.courses.Where(m => m.DepartmentID == id).ToList();
            if (category == null)
            {
                return NotFound();
            }
            ViewBag.departments = departments;
           // ViewBag.courses = courses;
            return View(category);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            var Name = HttpContext.Session.GetString("Name");

            if (string.IsNullOrEmpty(Name))
            {
                return RedirectToAction("Login2", "Admin");
            }
            else
                return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CategoryName,CategoryDescription")] Category category, IFormFile img_file)
        {
            var Name = HttpContext.Session.GetString("Name");
            if (string.IsNullOrEmpty(Name))
            {
                return RedirectToAction("Index", "Acount");
            }
            else
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
                        category.CategoryImage = img_file.FileName;

                    }

                }
                else
                {
                    category.CategoryImage = "DefaultCategory.png";
                }

                try
                {
                    _context.Add(category);
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ViewBag.exc = ex.Message;
                }
                return View();
            }
        }
        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.categories == null)
            {
                return NotFound();
            }

            var category = await _context.categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CategoryId,CategoryName,CategoryDescription")] Category category, IFormFile img_file)
        {
            if (id != category.CategoryId)
            {
                return NotFound();
            }

            string path = Path.Combine(_environment.WebRootPath, "Img");
            if (img_file != null)
            {
                path = Path.Combine(path, img_file.FileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await img_file.CopyToAsync(stream);
                    //ViewBag.Message = string.Format("<b>{0}<b> uploaded .</br>",img_file.FileName.ToString());
                    category.CategoryImage = img_file.FileName;
                }
            }
            else
            {
                category.CategoryImage = "DefaultCategory.png";
            }

            if (category.CategoryName != null && category.CategoryDescription != null && category.CategoryImage != null)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.CategoryId))
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

            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.categories == null)
            {
                return NotFound();
            }

            var category = await _context.categories
                .FirstOrDefaultAsync(m => m.CategoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.categories == null)
            {
                return Problem("Entity set 'AppDbContext.categories'  is null.");
            }
            var category = await _context.categories.FindAsync(id);
            if (category != null)
            {
                List<Department> departments = _context.departments.Where(m => m.CategoryId == id).ToList();
                foreach (var department in departments)
                {
                    List<Course> courses = _context.courses.Where(m => m.DepartmentID == department.DepartmentID).ToList();
                    List<Instructor> instructors = _context.instructors.Where(m => m.DepartmentID == department.DepartmentID).ToList();
                    if (courses != null)
                    {
                        foreach (var course in courses)
                        {
                            _context.courses.Remove(course);
                        }
                    }
                    if (instructors != null)
                    {
                        foreach (var instructor in instructors)
                        {
                            _context.instructors.Remove(instructor);
                        }
                    }
                    _context.departments.Remove(department);
                }
                _context.categories.Remove(category);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return (_context.categories?.Any(e => e.CategoryId == id)).GetValueOrDefault();
        }


    }
}
