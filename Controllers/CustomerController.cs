using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VuThiHuyenBTH2.Data;
using VuThiHuyenBTH2.Models.Process;
using VuThiHuyenBTH2.Models;
namespace VuThiHuyenBTH2.Controllers
{
public class CustomerController : Controller
    {
        private readonly ApplicationDbContext _context;
        private ExcelsProcess _excelProcess = new ExcelsProcess();
        public CustomerController (ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Customers.ToListAsync());
            
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
                            var std = new Customer();
                            // set values for attrinutes
                            std.CustomerID= dt.Rows[i][0].ToString();
                            std.CustomerName= dt.Rows[i][1].ToString();
                            std.Address= dt.Rows[i][2].ToString();
                             //add object to Context
                             _context.Customers.Add(std);
                        }
                        //save to database 
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
    
            return View();
        }
        private bool CustomerExists(string id)
         {
            return _context.Customers.Any(e =>e.CustomerID ==id);
         }
             public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create (Customer cus)
        {
            if(ModelState.IsValid)
            {
                _context.Add(cus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(cus);
            
        }
    }
}
    
    //     // GET: Student/Edit/5
    //     public async Task<IActionResult> Edit(string id)
    //     {
    //         if (id == null)
    //         {
    //             return View("NotFound");
    //         }
    //         var customer = await _context.Customers.FindAsync(id);
    //         if (customer == null)
    //         {
    //            // return NotFound();
    //            return View("NotFound");
    //         }
    //         return View(customer);
    //     }
    //     // POST: Employee/Edit
    //     [HttpPost]
    //     [ValidateAntiForgeryToken]

    //     public async Task<IActionResult>Edit(string id, [Bind("CustomerID,CustomerName")] Customer cus)
    //     {
    //         if (id != cus.CustomerID)
    //         {
    //            // return NotFound();
    //            return View("NotFound");
    //         }
    //         if (ModelState.IsValid)
    //         {
    //             try
    //             {
    //                 _context.Update(cus);
    //                 await _context.SaveChangesAsync();
    //             }
    //             catch (DbUpdateConcurrencyException)
    //             {
    //                 if( !CustomerExists(cus.CustomerID))
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
    //         return View(cus);
    //     }
    //     public async Task<IActionResult> Delete(string id)
    //     {
    //         if (id == null)
    //         {
    //            // return NotFound();
    //            return View("NotFound");
    //         }
    //         var std = await _context.Customers
    //             .FirstOrDefaultAsync(m =>m.CustomerID ==id);
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
    //         var cus = await _context.Customers.FindAsync(id);
    //         _context.Customers.Remove(cus);
    //         await _context.SaveChangesAsync();
    //         return RedirectToAction(nameof(Index));
    //     }
    //     private bool CustomerExists(string id)
    //     {
    //         return _context.Customers.Any(e =>e.CustomerID ==id);
    //     }
    // }