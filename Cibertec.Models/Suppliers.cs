using System.ComponentModel.DataAnnotations;

namespace Cibertec.Models
{
    public class Suppliers
    {
        public int Id { get; set; }
        [Key]
        public int SupplierID { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
    }
}
