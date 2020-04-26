using EthioProductShoppingCenter.DAL;
using EthioProductShoppingCenter.Repository;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EthioProductShoppingCenter.Models.ShoppingCart
{
    public class ShoppingCartViewModel
    {

        //public EthioProductEntities entities = new EthioProductEntities();

        //IShoppingCart cart;
        [Key]
        public int Id { get; set; }
        public List<tblCart> CartItems;
        public decimal CartTotal;

        //public List<tblCart> cartItems;
        //public decimal cartTotal;
        //public ShoppingCartViewModel()
        //{
        //    cart = new ShoppingCartRepository();

        //}
        //public ShoppingCartViewModel(ShoppingCartRepository _cart)
        //{
        //    cart = _cart;
        //}

        //public List<tblCart> CartItems
        //{
        //    get
        //    {

        //        return cart.GetCartItems();
        //    }
        //    set
        //    {
        //        cartItems = value;
        //    }
        //}
        //public decimal CartTotal
        //{
        //    get
        //    {
        //        return cart.GetCount();
        //    }
        //    set
        //    {
        //        cartTotal = value;
        //    }
        //}
    }
}