using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Cibertec.Models
{
    
    public class Products
    {
        public int Id { get; set; }
        [Key]
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal UnitsInStock { get; set; }
        public bool Discontinued { get; set; }
    }
}
