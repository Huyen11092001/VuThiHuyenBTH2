using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace VuThiHuyenBTH2.Models
{
    public class Student
    {
        //Khai báo các thuộc tính
     
        public string StudentID { get; set; }
        public string StudentName { get; set; }
        public string Address { get; set; }
        public string FacultyID {get;set;}
        [ForeignKey("FacultyID")]
        public Faculty? Faculty {get;set;}
    }

}