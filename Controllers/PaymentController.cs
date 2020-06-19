using Antlr.Runtime;
using AutoMapper;
using EthioProductShoppingCenter.BusinessLogic;
using EthioProductShoppingCenter.DAL;
using EthioProductShoppingCenter.DomainLayer;
using EthioProductShoppingCenter.Models;
using EthioProductShoppingCenter.Models.ShoppingCart;
using EthioProductShoppingCenter.Repository;
using PayPal.Api;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace EthioProductShoppingCenter.Controllers
{
    [Authorize]
    public class PaymentController : Controller
    {
        private IGenericUnitOfWork<EthioProductEntities> uow;
        //private IRepository<tblProduct> repoProduct;
        //private IShoppingCart shoppingCart;


        public PaymentController(IGenericUnitOfWork<EthioProductEntities> _uow)
        {
            uow = _uow;
        }

       
        // GET: Payment
        public ActionResult PaymantWithPayPal()
        {
            APIContext apiContext = PayPalConfiguration.GetAPIContext();

            try
            {
                string payerId = Request.Params["PayerID"];

                if(string.IsNullOrEmpty(payerId))
                {
                    string baseUri = Request.Url.Scheme + "://" + Request.Url.Authority +
                        "/Payment/PaymentAndOrderInfo?";
                    var guid = Convert.ToString((new Random().Next(1000000)));
                    //var GUID = Guid.NewGuid();
                    var createPayment = this.CreatePayment(apiContext, baseUri + "guid=" + guid);
                    var links = createPayment.links.GetEnumerator();

                    string payPalRedirectUrl = null;

                    while(links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            payPalRedirectUrl = lnk.href;

                        }
                    }

                    // saving the paymentID in the key guid  
                    Session.Add(guid, createPayment.id);
                    return Redirect(payPalRedirectUrl);
                }
               
                else
                {
                    var guid = Request.Params["guid"];
                    var excutePayment = ExecutePayment(apiContext, payerId, Session[guid] as string);

                    if (excutePayment.ToString().ToLower() != "approved")
                    {
                        return View("FalureView");
                    }
                }

            }
            catch (Exception)
            {

                return View("FalureView");
            }

            
             return View("SuccessView");
        }

        private object ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var executePayment = new PaymentExecution() { payer_id = payerId };
            this.payment = new Payment() { id = paymentId };
            return this.payment.Execute(apiContext, executePayment);
        }

        private PayPal.Api.Payment payment;

        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            var itemList = new ItemList() { items = new List<Item>() };

            if (Session["cart"] != null)
            {
                List<tblCart> cart = (List<tblCart>)Session["cart"];
                foreach (var item in cart)
                {
                    itemList.items.Add(new Item()
                    {
                        name = item.tblProduct.ProductName,
                        currency = "USD",
                        price = item.tblProduct.Price.ToString(),
                        quantity = item.Count.ToString(),
                        sku = "sku"
                    }); 
                }

            }
            

            var payer = new Payer() { payment_method = "paypal" };

            var redirUrl = new RedirectUrls()
            {
                return_url = redirectUrl,
                cancel_url = redirectUrl + "&cancel=true"
            };

            var detail = new Details()
            {
                tax = "1",
                shipping = "1",
                subtotal = Session["payment_amt"].ToString()
            };

            decimal total = Convert.ToDecimal(detail.subtotal) + Convert.ToDecimal(detail.tax) + Convert.ToDecimal(detail.shipping);

            var amount = new Amount()
            {
                currency = "USD",
                total = total.ToString(),
                details = detail
            };

            var transactionList = new List<Transaction>();
            transactionList.Add(new Transaction()
            {
                description = "Transaction Description",
                item_list = itemList,
                amount = amount,
                invoice_number = "#10000000"
            });

            this.payment = new Payment()
            {
                intent = "Sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrl
            };
            
            return this.payment.Create(apiContext);
           
        }

        public ActionResult PaymentAndOrderInfo()
        {
            ViewBag.paymentId = Request.QueryString["paymentId"];
            Session["paymentId"] = Request.QueryString["paymentId"];

            ViewBag.token = Request.QueryString["token"];

            ViewBag.payerID = Request.QueryString["PayerID"];
            Session["payerID"] = Request.QueryString["PayerID"];

            ViewBag.CartProducts = Session["cart"];

            ViewBag.Total = Session["payment_amt"];

            OrderVM orderVM = new OrderVM();


            return View(orderVM);
        }

        [HttpPost]
        public ActionResult PaymentAndOrderInfo(FormCollection values)
        {
           
            string paymentId = Session["paymentId"].ToString();

            OrderVM orderVM = new OrderVM();
            TryUpdateModel(orderVM);

            tblOrder order = new tblOrder();
            order.Username = User.Identity.Name;
            order.FirstName = orderVM.FirstName;
            order.LastName = orderVM.LastName;
            order.Address = orderVM.Address;
            order.City = orderVM.City;
            order.State = orderVM.State;
            order.postalCode = orderVM.postalCode;
            order.Country = orderVM.Country;
            order.Phone = orderVM.Phone;
            order.Email = User.Identity.Name;
            //order.Total = orderVM.Total;
            order.OrderDate = orderVM.OrderDate;


            if (paymentId != null)
            {
                //string payerID = Session["payerId"].ToString();
                //Mapper.Map<OrderVM, tblOrder>(orderVM);

                order.OrderDate = DateTime.Now;
                order.PaymentTranID = paymentId;
                order.Total = Convert.ToDecimal(Session["payment_amt"]);

                order.HasBeenShipped = false;

                //Save Order
                uow.Context.tblOrders.Add(order);
                //uow.GenericRepository<tblOrder>().Add(order);
                uow.Context.SaveChanges();

                List<tblCart> myOrderList = (List<tblCart>)Session["cart"];

                // Add OrderDetail information to the DB for each product purchased.
                for (int i = 0; i < myOrderList.Count; i++)
                {
                    // Create a new OrderDetail object.
                    var myOrderDetail = new tblOrderDetail();
                    myOrderDetail.OrderId = order.OrderId;
                    myOrderDetail.ProductId = Convert.ToInt32(Session["productID"]);
                    myOrderDetail.Quantity = Convert.ToInt32(Session["quantity"]);
                    myOrderDetail.UnitPrice = Convert.ToDecimal(Session["eachprodprice"]);

                    // Add OrderDetail to DB.
                    uow.Context.tblOrderDetails.Add(myOrderDetail);
                    uow.Context.SaveChanges();
                }

                return RedirectToAction("OrderComplete", new { id = order.OrderId });

            }
            return View("FailureView");

        }

        public ActionResult OrderComplete(int id)
        {
            // && o.Email == User.Identity.Name
            bool isValid = uow.Context.tblOrders.Any(o => o.OrderId == id);
            if (isValid)
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