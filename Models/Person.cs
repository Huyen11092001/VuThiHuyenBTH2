using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
namespace VuThiHuyenBTH2.Models
{
    public class Person
    {
          [Key]
        [Required(ErrorMessage =" Mã PersonID không được để trống ")]
        public string PersonID { get; set; }
        public string PersonName { get; set; }
     
      
    }
}