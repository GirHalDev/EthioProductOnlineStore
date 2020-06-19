using EthioProductShoppingCenter.DAL;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EthioProductShoppingCenter.DomainLayer
{
    public class Cart : Item
    {
        public int ID { get; set; }
        public Nullable<int> ProductId { get; set; }
        public string CartId { get; set; }
        public Nullable<int> CatagoryId { get; set; }
        public Nullable<int> Count { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }

        public virtual tblCatagory tblCatagory { get; set; }
        public virtual tblProduct tblProduct { get; set; }
    }
}