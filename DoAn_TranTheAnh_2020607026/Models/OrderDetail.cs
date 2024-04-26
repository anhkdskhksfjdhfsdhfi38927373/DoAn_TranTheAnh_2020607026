namespace DoAn_TranTheAnh_2020607026.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class OrderDetail
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderDetailId { get; set; }

        public int OrderID { get; set; }

        public int OrderQuantity { get; set; }

        public double TotalPrice { get; set; }

        public int ProductID { get; set; }

        public virtual Order Order { get; set; }

        public virtual Product Product { get; set; }
    }
}
