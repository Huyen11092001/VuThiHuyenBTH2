using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VuThiHuyenBTH2.Data;
using VuThiHuyenBTH2.Models.Process;
using VuThiHuyenBTH2.Models;
namespace VuThiHuyenBTH2.Controllers
{
public class PersonController : Controller
   {
       private readonly ApplicationDbContext _context;
        private ExcelsProcess _excelProcess = new ExcelsProcess();
        public PersonController (ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
           return View (await _context.Persons.ToListAsync());
            
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
                            //create a new Person object
                            var per = new Person();
                            // set values for attrinutes
                            per.PersonID= dt.Rows[i][0].ToString();
                            per.PersonName= dt.Rows[i][1].ToString();
                            per.Address= dt.Rows[i][2].ToString();
                             //add object to Context
                             _context.Persons.Add(per);
                        }
                        //save to database 
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                }
            }
    
            return View();
        }
        private bool PersonExists(string id)
        {
          return _context.Persons.Any(e =>e.PersonID ==id);
        }
          public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create (Person std)
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
}
     
//          // GET: Student/Edit/5
//         public async Task<IActionResult> Edit(string id)
//         {
//             if (id == null)
//             {
//                 return View("NotFound");
//             }
//             var person = await _context.Persons.FindAsync(id);
//             if (person == null)
//             {
//                // return NotFound();
//                return View("NotFound");
//             }
//             return View(person);
//         }
//         // POST: Student/Edit
//         [HttpPost]
//         [ValidateAntiForgeryToken]

//         public async Task<IActionResult>Edit(string id, [Bind("PersonID,PersonName")] Person std)
//         {
//             if (id != std.PersonID)
//             {
//                // return NotFound();
//                return View("NotFound");
//             }
//             if (ModelState.IsValid)
//             {
//                 try
//                 {
//                     _context.Update(std);
//                     await _context.SaveChangesAsync();
//                 }
//                 catch (DbUpdateConcurrencyException)
//                 {
//                     if( !PersonExists(std.PersonID))
//                     {
//                        // return NotFound();
//                        return View("NotFound");
//                     }
//                     else
//                     {
//                         throw;
//                     }
//                 }
//                 return RedirectToAction(nameof(Index));
//             }
//             return View(std);
//         }
//         public async Task<IActionResult> Delete(string id)
//         {
//             if (id == null)
//             {
//                // return NotFound();
//                return View("NotFound");
//             }
//             var std = await _context.Persons
//                 .FirstOrDefaultAsync(m =>m.PersonID ==id);
//             if (std == null)
//             {
//                // return NotFound();
//                return View("NotFound");
//             }
//             return View(std);
//         }
//         // POST: Product/Delete/
//         [HttpPost, ActionName("Delete")]
//         [ValidateAntiForgeryToken]
//         public async Task<IActionResult> DeleteConfirmed(string id)
//         {
//             var std = await _context.Persons.FindAsync(id);
//             _context.Persons.Remove(std);
//             await _context.SaveChangesAsync();
//             return RedirectToAction(nameof(Index));
//         }
//         private bool PersonExists(string id)
//         {
//             return _context.Persons.Any(e =>e.PersonID ==id);
//         }
//    }       