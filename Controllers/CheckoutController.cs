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
            try
            {
                OrderVM orderVM = new OrderVM();
                TryUpdateModel(orderVM);

                tblOrder order = new tblOrder();

                order.Username = orderVM.Username;
                order.FirstName = orderVM.FirstName;
                order.LastName = orderVM.LastName;
                order.Address = orderVM.Address;
                order.City = orderVM.City;
                order.State = orderVM.State;
                order.postalCode = orderVM.postalCode;
                order.Country = orderVM.Country;
                order.Phone = orderVM.Phone;
                order.Email = orderVM.Email;
                order.Total = orderVM.Total;
                order.OrderDate = orderVM.OrderDate;


                Mapper.Map<tblOrder, OrderVM>(order);

                //var order = new OrderVM();

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
                return View("Error");
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