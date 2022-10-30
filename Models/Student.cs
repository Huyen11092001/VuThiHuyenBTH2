using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
namespace VuThiHuyenBTH2.Models
{
    public class Student
    {
        //Khai báo các thuộc tính
        [Key]
        [Required(ErrorMessage =" Mã StudentID không được để trống ")]
        public string StudentID { get; set; }
        public string StudentName { get; set; }
    }
}