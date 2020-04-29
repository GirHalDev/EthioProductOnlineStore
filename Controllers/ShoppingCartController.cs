using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EthioProductShoppingCenter.DAL;
using EthioProductShoppingCenter.Models.ShoppingCart;
using EthioProductShoppingCenter.Repository;
using Microsoft.Ajax.Utilities;

namespace EthioProductShoppingCenter.Controllers
{
    public class ShoppingCartController : Controller
    {
        private GenericUnitOfWork<EthioProductEntities> uow = new GenericUnitOfWork<EthioProductEntities>();
        private IGenericRepository<tblCart> repositoryCart;
        private IGenericRepository<tblProduct> repositoryProduct;
        private IShoppingCart shoppingCartRepository;
        public ShoppingCartController()
        {
            //If you want to use Generic Repository with Unit of work
            repositoryCart = new IGenericRepository<tblCart>(uow);
            repositoryProduct = new IGenericRepository<tblProduct>(uow);
            //If you want to use Specific Repository with Unit of work
            shoppingCartRepository = new ShoppingCartRepository(uow);
        }
        //
        // GET: /ShoppingCart/
        public ActionResult Index()
        {
            var cart = ShoppingCartRepository.GetCart(this.HttpContext);
            var viewmodel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItem(),
                CartTotal = cart.GetCount()
            };

            ViewBag.CartItemsSum = viewmodel.CartTotal;

            return View(viewmodel);

        }
        //
        // GET: /ShoppingCart/AddToCart/5
        public ActionResult AddToCart(int id)
        {
            // Retrieve the product from the database
            //var addedProduct = repositoryProduct.GetProduct().Single(product => product.ID == id);
            var addedProduct = repositoryProduct.GetFirstOrDefault(id);
           
            // Add it to the shopping cart
            var cart = ShoppingCartRepository.GetCart(this.HttpContext);

            cart.AddToCart(addedProduct);

            // Go back to the main product page for more shopping
            return RedirectToAction("Index", "Home");
        }
        //
        // AJAX: /ShoppingCart/RemoveFromCart/5
        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            // Remove the item from the cart
            var cart = ShoppingCartRepository.GetCart(this.HttpContext);
            string cartIds = cart.GetCartId(); 

            string productName = repositoryCart.GetFirstOrDefault(id).tblProduct.ProductName;

            // Get the name of the product to display confirmation
            //string productName = uow.GetRepositoryInstance<tblCart>().GetProduct().DistinctBy(id).tblProduct.ProductName;


            // Remove from cart
            int itemCount = cart.RemoveFromCart(cartIds, id);

            // Display the confirmation message
            var results = new ShoppingCartRemoveViewModel
            {
                Message = Server.HtmlEncode(productName) +
                    " has been removed from your shopping cart.",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };
            return Json(results);
        }
        //
        // GET: /ShoppingCart/CartSummary
        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCartRepository.GetCart(this.HttpContext);

            ViewData["CartCount"] = cart.GetCount();
            return PartialView("CartSummary");
        }
    }
}