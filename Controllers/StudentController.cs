using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VuThiHuyenBTH2.Data;
using VuThiHuyenBTH2.Models.Process;
using VuThiHuyenBTH2.Models;


namespace VuThiHuyenBTH2.Controllers
{
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private ExcelsProcess _excelProcess = new ExcelsProcess();
        public StudentController (ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View ( await _context.Students.ToListAsync());
          
        }
         public async Task<IActionResult>Upload()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult>Upload(IFormFile file)
        {
            if (file!=null)
            {
                string fileExtension = Path.GetExtension(file.FileName);
                if (fileExtension != ".xls" && fileExtension !=".xlsx")
                {
                    ModelState.AddModelError("", "Please choose excel file to upload!");
                }
                else
                {
                    //rename
                    var fileName = DateTime.Now.ToShortTimeString() +fileExtension;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory() + "/Uploads/Excels", fileName);
                    var fileLocation = new FileInfo(filePath).ToString();
                    using (var stream = new FileStream(filePath,FileMode.Create))
                    {
                        //save file to server
                        await file.CopyToAsync(stream);
                    
                        // read data from
                        var dt = _excelProcess.ExcelToDataTable(fileLocation);
                        //using for loop to read data from dt
                        for (int i=0; i< dt.Rows.Count;i++)
                        {
                            //create a new Employee object
                            var std = new Student();
                            // set values for attrinutes
                            std.StudentID= dt.Rows[i][0].ToString();
                            std.StudentName= dt.Rows[i][1].ToString();
                            std.Address= dt.Rows[i][2].ToString();
                             //add object to Context
                             _context.Students.Add(std);
                        }
                        //save to database 
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
    
            return View();
        }
        private bool StudentExists(string id)
         {
           return _context.Students.Any(e =>e.StudentID ==id);
        }
    }
}
    //     public IActionResult Create()
    //     {
    //         return View();
    //     }
    //     [HttpPost]
    //     public async Task<IActionResult> Create (Student std)
    //     {
    //         if(ModelState.IsValid)
    //         {
    //             _context.Add(std);
    //             await _context.SaveChangesAsync();
    //             return RedirectToAction(nameof(Index));
    //         }
    //         return View(std);
            
    //     }
    //     // GET: Student/Edit/5
    //     public async Task<IActionResult> Edit(string id)
    //     {
    //         if (id == null)
    //         {
    //             return View("NotFound");
    //         }
    //         var student = await _context.Students.FindAsync(id);
    //         if (student == null)
    //         {
    //            // return NotFound();
    //            return View("NotFound");
    //         }
    //         return View(student);
    //     }
    //     // POST: Student/Edit
    //     [HttpPost]
    //     [ValidateAntiForgeryToken]

    //     public async Task<IActionResult>Edit(string id, [Bind("StudentID,StudentName")] Student std)
    //     {
    //         if (id != std.StudentID)
    //         {
    //            // return NotFound();
    //            return View("NotFound");
    //         }
    //         if (ModelState.IsValid)
    //         {
    //             try
    //             {
    //                 _context.Update(std);
    //                 await _context.SaveChangesAsync();
    //             }
    //             catch (DbUpdateConcurrencyException)
    //             {
    //                 if( !StudentExists(std.StudentID))
    //                 {
    //                    // return NotFound();
    //                    return View("NotFound");
    //                 }
    //                 else
    //                 {
    //                     throw;
    //                 }
    //             }
    //             return RedirectToAction(nameof(Index));
    //         }
    //         return View(std);
    //     }
    //     public async Task<IActionResult> Delete(string id)
    //     {
    //         if (id == null)
    //         {
    //            // return NotFound();
    //            return View("NotFound");
    //         }
    //         var std = await _context.Students
    //             .FirstOrDefaultAsync(m =>m.StudentID ==id);
    //         if (std == null)
    //         {
    //            // return NotFound();
    //            return View("NotFound");
    //         }
    //         return View(std);
    //     }
    //     // POST: Product/Delete/
    //     [HttpPost, ActionName("Delete")]
    //     [ValidateAntiForgeryToken]
    //     public async Task<IActionResult> DeleteConfirmed(string id)
    //     {
    //         var std = await _context.Students.FindAsync(id);
    //         _context.Students.Remove(std);
    //         await _context.SaveChangesAsync();
    //         return RedirectToAction(nameof(Index));
    //     }
    //     private bool StudentExists(string id)
    //     {
    //         return _context.Students.Any(e =>e.StudentID ==id);
    //     }
    // }
