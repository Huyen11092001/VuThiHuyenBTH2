using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VuThiHuyenBTH2.Data;
using VuThiHuyenBTH2.Models;


    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;
        public StudentController (ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _context.Students.ToListAsync();
            return View(model);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create (Student std)
        {
            if(ModelState.IsValid)
            {
                _context.Add(std);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(std);
            
        }
    }
