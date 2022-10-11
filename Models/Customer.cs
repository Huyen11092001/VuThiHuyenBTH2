using Microsoft.AspNetCore.Mvc;

namespace VuThiHuyenBTH2.Models
{
    public class Customer
    {
        public string CustomerID { get; set; }
        public string CustomerName { get; set; }
         public int CustomerAge { get; set; }
          public string CustomerAddress { get; set; }
    }
}