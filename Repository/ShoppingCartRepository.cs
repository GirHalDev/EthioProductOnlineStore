using EthioProductShoppingCenter.DAL;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EthioProductShoppingCenter.Repository
{
    public class ShoppingCartRepository : IGenericRepository<tblCart>, IShoppingCart
    {
        string ShoppingCartId { get; set; }

        public const string CartSessionKey = "CartId";

        //Initializing the base class (new GenericRepository(IGenericUnitOfWork<EthioProductEntities> _context))
        public ShoppingCartRepository(IGenericUnitOfWork<EthioProductEntities> unitOfWork)
        : base(unitOfWork)
        {
        }

        public ShoppingCartRepository(EthioProductEntities context)
        : base(context)
        {
        }

       


        public static ShoppingCartRepository GetCart(HttpContextBase value)
        {
            GenericUnitOfWork<EthioProductEntities> unitOfWork = new GenericUnitOfWork<EthioProductEntities>();
             ShoppingCartRepository cart = new ShoppingCartRepository(unitOfWork);
             cart.ShoppingCartId = cart.GetCartId();
            return cart;
        }
        // Helper method to simplify shopping cart calls
        public static ShoppingCartRepository GetCart(Controller controller)
        {
            return GetCart(controller.HttpContext);
        }

        public string GetCartId()
        {
            if (HttpContext.Current.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrWhiteSpace(HttpContext.Current.User.Identity.Name))
                {
                    HttpContext.Current.Session[CartSessionKey] = HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    // Generate a new random GUID using System.Guid class.     
                    Guid tempCartId = Guid.NewGuid();
                    HttpContext.Current.Session[CartSessionKey] = tempCartId.ToString();
                }
            }
            return HttpContext.Current.Session[CartSessionKey].ToString();
        }
        public void AddToCart(tblProduct product)
        {
            // Get the matching cart and product instances
            var cartItem = Context.tblCarts.SingleOrDefault(c => c .CartId == ShoppingCartId && c.tblProduct.ID == product.ID);

            if (cartItem == null)
            {
                // Create a new cart item if no cart item exists
                cartItem = new tblCart
                {
                    ProductId = product.ID,
                    CartId = ShoppingCartId,
                    Count = 1,
                    DateCreated = DateTime.Now
                };
                //entity.tblCarts.Add(cartItem);
                Context.tblCarts.Add(cartItem);
                
            }
            else
            {
                // If the item does exist in the cart, 
                // then add one to the quantity
                cartItem.Count++;
            }
            // Save changes
            //entity.SaveChanges();
            Context.SaveChanges();
        }
        public int RemoveFromCart(string cartIds, int id)
        {
            //ShoppingCartId = GetCartId();
            //// Get the cart
            var cartItem = Context.tblCarts.FirstOrDefault(cart => cart.CartId == cartIds
                && cart.ProductId == id);
            //var cartItem = uow.GetRepositoryInstance<tblCart>().GetFirstOrDefault(id);

            int itemCount = 0;

            if (cartItem != null)
            {
                if (cartItem.Count > 1)
                {
                    cartItem.Count--;
                    itemCount = (int)cartItem.Count;
                }
                else
                {
                    Context.tblCarts.Remove(cartItem);
                }
                // Save changes
                Context.SaveChanges();
            }
            return itemCount;
        }

       
        public void EmptyCart()
        {
            var cartItems = Context.tblCarts.Where(cart => cart.CartId == ShoppingCartId).ToList();

            foreach (var cartItem in cartItems)
            {
                Context.tblCarts.Remove(cartItem);
            }
            // Save changes
            Context.SaveChanges();
        }

        public List<tblCart> GetCartItem()
        {
            ShoppingCartId = GetCartId();

            return Context.tblCarts.Where(cart => cart.CartId == ShoppingCartId).ToList();
        }
        public int GetCount()
        {
            // Get the count of each item in the cart and sum them up
            int? count = (from cartItems in Context.tblCarts
                          where cartItems.CartId == ShoppingCartId
                          select cartItems.Count).Sum();
            // Return 0 if all entries are null
            return count ?? 0;
        }
        public decimal GetTotal()
        {
            // Multiply album price by count of that album to get 
            // the current price for each of those albums in the cart
            // sum all album price totals to get the cart total
            decimal? total = (from cartItems in Context.tblCarts
                              where cartItems.CartId == ShoppingCartId
                              select cartItems.Count *
                              cartItems.tblProduct.Price).Sum();

            return total ?? decimal.Zero;
        }
        public int CreateOrder(tblOrder order)
        {
            decimal orderTotal = 0;
            
            var cartItems = GetCartItem();
            // Iterate over the items in the cart, 
            // adding the order details for each
            foreach (var item in cartItems)
            {
                var orderDetail = new tblOrderDetail
                {
                    ProductId = item.ProductId,
                    OrderId = order.OrderId,
                    UnitPrice = item.tblProduct.Price,
                    Quantity = item.Count
                };
                // Set the order total of the shopping cart
                orderTotal += (decimal)(item.Count * item.tblProduct.Price);
                
                Context.tblOrderDetails.Add(orderDetail);

                //int? Quantity = orderDetail.Quantity;
                //Quantity = Quantity - 1;
                //if(Quantity == 0)
                //{
                //    break;
                //}
               
            }
            // Set the order's total to the orderTotal count
            order.Total = orderTotal;

            // Save the order
            Context.SaveChanges();
            // Empty the shopping cart
            EmptyCart();
            // Return the OrderId as the confirmation number
            return order.OrderId;
        }
        // We're using HttpContextBase to allow access to cookies.
        // When a user has logged in, migrate their shopping cart to
        // be associated with their username
        public void MigrateCart(string userName)
        {
            var shoppingCart = Context.tblCarts.Where(c => c.CartId == ShoppingCartId);

            foreach (tblCart item in shoppingCart)
            {
                item.CartId = userName;
            }
            Context.SaveChanges();
        }

        public int RemoveFromCart(int id)
        {
            throw new NotImplementedException();
        }
    }
}