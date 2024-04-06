using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Doan_TranTheAnh_2020607026.Models
{
    public class Product
    {
        [Key]
        public string ProductID { get; set; }
        [Required]
        public string? ProductName { get; set; }
        [Required]
        public string ProductImage { get; set; }
        [Required]
        public decimal? Price { get; set; }
            [Required]
        public string? Color { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public string Size { get; set; }

    }
}
