using AutoMapper;
using EthioProductShoppingCenter.DAL;
using EthioProductShoppingCenter.Models;
using EthioProductShoppingCenter.Repository;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Web;
using System.Web.Mvc;

namespace EthioProductShoppingCenter.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        GenericUnitOfWork<EthioProductEntities> uow = new GenericUnitOfWork<EthioProductEntities>();
        private IGenericRepository<tblOrder> repositoryOrder;
        
        const string PromoCode = "FREE";

        public CheckoutController()
        {
            //If you want to use Generic Repository with Unit of work
            repositoryOrder = new IGenericRepository<tblOrder>(uow);
            
        }
        // GET: Checkout/AddressPayment. Requesting to login before checkout
        public ActionResult AddressAndPayment()
        {
           
            return View();
        }

        // POST: Checkout/AddressPayment
        [HttpPost]
        public ActionResult AddressAndPayment(FormCollection values)
        {
            //List<OrderVM> orderVM = new List<OrderVM>();
            //List<tblOrder> order = uow.Context.tblOrders.ToList();

            //Mapper.Map<List<OrderVM>, List<tblOrder>>(orderVM);
            OrderVM orderVM = new OrderVM();
            tblOrder order = new tblOrder();
            
            Mapper.Map<tblOrder, OrderVM>(order);
            //var order = new OrderVM();

            TryUpdateModel(order);

            try
            {
                if (string.Equals(values["PromoCode"], PromoCode, StringComparison.OrdinalIgnoreCase) == false)
                {
                    return View(orderVM);
                }

                else
                {

                    order.Email = User.Identity.Name;
                    order.OrderDate = DateTime.Now;

                    //Save Order
                    uow.Context.tblOrders.Add(order);
                    //uow.GenericRepository<tblOrder>().Add(order);
                    uow.Context.SaveChanges();
                    //Process the Order
                    var cart = ShoppingCartRepository.GetCart(this.HttpContext);
                    cart.CreateOrder(order);
                    return RedirectToAction("Complete", new { id = order.OrderId });
                }
            }
            catch
            {
                //Invalid - redisplay with errors
                return View(orderVM);
            }

        }

        //Get: /Checkout/Complete
        public ActionResult Complete(int id)
        {
            bool isValid = uow.Context.tblOrders.Any(o => o.OrderId == id && o.Email == User.Identity.Name);
            if(isValid)
            {
                return View(id);
            }
            else
            {
                return View("Error");
            }

        }
    }
}