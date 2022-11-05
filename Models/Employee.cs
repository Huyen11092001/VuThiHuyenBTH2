using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
namespace VuThiHuyenBTH2.Models
{
    public class Employee
    {
        [Key]
        public string EmployeeID { get; set; }
        public string EmployeeName { get; set; }
    }
}