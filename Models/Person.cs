using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
namespace VuThiHuyenBTH2.Models
{
    public class Person
    {
         
        public string PersonID { get; set; }
        public string PersonName { get; set; }
        public string Address { get; set; }
     
      
    }
}