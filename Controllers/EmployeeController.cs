using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VuThiHuyenBTH2.Data;
using VuThiHuyenBTH2.Models.Process;
using VuThiHuyenBTH2.Models;

public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private ExcelsProcess _excelProcess = new ExcelsProcess();
        public EmployeeController (ApplicationDbContext context)
        {
            _context = context;
        }
        // GET: Employee
        public async Task<IActionResult> Index()
        {
            return View (await _context.Employees.ToListAsync());
           
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
                        var dt =_excelProcess.ExcelToDataTable(fileExtension);
                        //using for loop to read data from dt
                        for (int i=0; i< dt.Rows.Count;i++)
                        {
                            //create a new Employee object
                            var emp = new Employee();
                            // set values for attrinutes
                            emp.EmployeeID= dt.Rows[i][0].ToString();
                             emp.EmployeeName= dt.Rows[i][1].ToString();
                             //add object to Context
                             _context.Employees.Add(emp);
                        }
                        //save to database 
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
            return View();
        }
        private bool EmployeeExists(string id)
        {
            return _context.Employees.Any(e =>e.EmployeeID ==id);
        }
    }
    //     public async Task<IActionResult> Create (Employee emp)
    //     {
    //         if(ModelState.IsValid)
    //         {
    //             _context.Add(emp);
    //             await _context.SaveChangesAsync();
    //             return RedirectToAction(nameof(Index));
    //         }
    //         return View(emp);
            
    //     }
    //     // GET: Student/Edit/5
    //     public async Task<IActionResult> Edit(string id)
    //     {
    //         if (id == null)
    //         {
    //             return View("NotFound");
    //         }
    //         var employee = await _context.Employees.FindAsync(id);
    //         if (employee == null)
    //         {
    //            // return NotFound();
    //            return View("NotFound");
    //         }
    //         return View(employee);
    //     }
    //     // POST: Employee/Edit
    //     [HttpPost]
    //     [ValidateAntiForgeryToken]

    //     public async Task<IActionResult>Edit(string id, [Bind("EmployeeID,EmployeeName")] Employee emp)
    //     {
    //         if (id != emp.EmployeeID)
    //         {
    //            // return NotFound();
    //            return View("NotFound");
    //         }
    //         if (ModelState.IsValid)
    //         {
    //             try
    //             {
    //                 _context.Update(emp);
    //                 await _context.SaveChangesAsync();
    //             }
    //             catch (DbUpdateConcurrencyException)
    //             {
    //                 if( !EmployeeExists(emp.EmployeeID))
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
    //         return View(emp);
    //     }
    //     public async Task<IActionResult> Delete(string id)
    //     {
    //         if (id == null)
    //         {
    //            // return NotFound();
    //            return View("NotFound");
    //         }
    //         var std = await _context.Employees
    //             .FirstOrDefaultAsync(m =>m.EmployeeID ==id);
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
    //         var emp = await _context.Employees.FindAsync(id);
    //         _context.Employees.Remove(emp);
    //         await _context.SaveChangesAsync();
    //         return RedirectToAction(nameof(Index));
    //     }
    //     private bool EmployeeExists(string id)
    //     {
    //         return _context.Employees.Any(e =>e.EmployeeID ==id);
    //     }
    // }
    