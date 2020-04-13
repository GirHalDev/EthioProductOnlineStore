using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EthioProductShoppingCenter.Models
{
    public class CatagoryDetail
    {
        public int CatagoryId { get; set; }

        [Required(ErrorMessage = "Catagory name required")]
        [StringLength(100, ErrorMessage = "Minimium 3 and minimum 5 and maximum 100 characters allowed", MinimumLength = 3)]
        public string CatagoryName { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsDelete { get; set; }

    }

    public class ProductDetail
    {
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Product name required")]
        [StringLength(100, ErrorMessage = "Minimium 3 and minimum 5 and maximum 100 characters allowed", MinimumLength = 3)]
        public string ProductName { get; set; }
        [Required]
        [Range(1, 50)]
        public Nullable<int> CatagoryId { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> IsDelete { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
        public string ProductImage { get; set; }
        public Nullable<bool> IsFeatured { get; set; }

        [Required]
        [Range(typeof(int), "1", "500", ErrorMessage = "Invalid quantity")]
        public Nullable<int> Quantity { get; set; }

        [Required]
        [Range(typeof(decimal), "1", "200000", ErrorMessage = "Invalid price")]
        public Nullable<decimal> Price { get; set; }

        public SelectList Catagories { get; set; }
    }
}