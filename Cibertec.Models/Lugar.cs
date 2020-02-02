using System.ComponentModel.DataAnnotations;

namespace Cibertec.Models
{
    public class Lugar
    {
        public int Id { get; set; }
        [Required]
        [Key]
        public int Codigo { get; set; }
        public string Ciudad { get; set; }
    }
}