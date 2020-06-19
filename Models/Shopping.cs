using EthioProductShoppingCenter.DAL;
using EthioProductShoppingCenter.Repository;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EthioProductShoppingCenter.Models
{
    public class ShippingDetail
    {
        public int ShippingDetailId { get; set; }

        [Required]
        public Nullable<int> MemberId { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string Zipcode { get; set; }
        public Nullable<int> OrderId { get; set; }
        public Nullable<decimal> AmountPaid { get; set; }

        [Required]
        public string PaymentType { get; set; }
    }

    public partial class OrderVM 
    {
        [Key]
        [ScaffoldColumn(false)]
        public int OrderId { get; set; } 

        [ScaffoldColumn(false)]
        public string Username { get; set; }
        [Required(ErrorMessage = "First name is required")]
        [DisplayName("First Name")]
        [StringLength(160)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [DisplayName("Last Name")]
        [StringLength(160)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(70)]
        public string Address { get; set; }

        [Required(ErrorMessage = "City is required")]
        [StringLength(40)]
        public string City { get; set; }

        [Required(ErrorMessage = "State is required")]
        [StringLength(40)]
        public string State { get; set; }

        [Required(ErrorMessage = "Postal Code is required")]
        [DisplayName("Postal Code")]
        [StringLength(10)]
        public string postalCode { get; set; }

        [Required(ErrorMessage = "Country is required")]
        [StringLength(40)]
        public string Country { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [StringLength(24)]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [DisplayName("Email Address")]
        [StringLength(70)]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Email is is not valid.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [ScaffoldColumn(false)]
        public Nullable<decimal> Total { get; set; }

        [ScaffoldColumn(false)]
        public Nullable<System.DateTime> OrderDate { get; set; }

        //public virtual ICollection<tblOrderDetail> tblOrderDetails { get; set; }


    }
}