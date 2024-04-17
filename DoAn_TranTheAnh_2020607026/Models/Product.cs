namespace DoAn_TranTheAnh_2020607026.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Product
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Product()
        {
            CartItems = new HashSet<CartItem>();
            OrderDetails = new HashSet<OrderDetail>();
            Rates = new HashSet<Rate>();
        }

        [Required]
        [StringLength(255)]
        public string ProductName { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public int QuantityInStock { get; set; }

        public int CategoryID { get; set; }

        public int? BrandID { get; set; }

        [StringLength(255)]
        public string Images { get; set; }

        public double SaleOff { get; set; }

        public double Price { get; set; }

        public int? SizeID { get; set; }

        public int ProductID { get; set; }

        public virtual Brand Brand { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CartItem> CartItems { get; set; }

        public virtual Category Category { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        public virtual Size Size { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Rate> Rates { get; set; }
        public double newprice(double price,double saleoff)
        {
            this.Price = price;
            this.SaleOff = saleoff;
            return price * (saleoff / 100);
        }
    }
}
