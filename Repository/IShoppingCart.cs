using EthioProductShoppingCenter.DAL;
using System.Collections.Generic;
using System.Web;

namespace EthioProductShoppingCenter.Repository
{
    public interface IShoppingCart : IRepository<tblCart>
    {
        void AddToCart(tblProduct product);
        int CreateOrder(tblOrder order);
        void EmptyCart();
        string GetCartId();
        List<tblCart> GetCartItem();
        int GetCount();
        decimal GetTotal();
        void MigrateCart(string userName);
        int RemoveFromCart(int id);
    }
}