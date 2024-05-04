using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SWDteam.Data;
using SWDteam.Models;

namespace SWDteam.Controllers
{
    public class ADMINController : Controller
    {
        
        private readonly AppDbContext _dbcontext;
        public ADMINController(AppDbContext context)
        {
            _dbcontext = context;
        }
        public IActionResult Index2()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Register2()
        {
            return View(new Admin());
        }
        [HttpPost]
        public IActionResult Register2(Admin user)
        {
            if (ModelState.IsValid)
            {
                _dbcontext.admins.Add(user);
                _dbcontext.SaveChanges();
                return RedirectToAction("Login2","ADMIN");
            }
            return View(user);
        }
        [HttpGet]
        public IActionResult Login2()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login2(Admin_login model)
        {
            if (ModelState.IsValid)
            {
               var user= _dbcontext.admins.FirstOrDefault(u => u.Email == model.Email);
                if (user!=null && user.password==model.password)
                {

                    return RedirectToAction("Index2", "Admin");
                }
                else
                {
                    ModelState.AddModelError("","Invalid Email or Password");
                }
            }
            return View(model);
        }
    }
}
