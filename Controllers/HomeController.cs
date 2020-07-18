using EthioProductShoppingCenter.DAL;
using EthioProductShoppingCenter.Models.Home;
using EthioProductShoppingCenter.Repository;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList.Mvc;
using PagedList;
using EthioProductShoppingCenter.Models.ShoppingCart;
using EthioProductShoppingCenter.DomainLayer;
using EthioProductShoppingCenter.BusinessLogic;

namespace EthioProductShoppingCenter.Controllers
{

    //Rerouting Http request to Https
    [RequireHttps]
    public class HomeController : Controller
    {
        
       
                
        public ActionResult Index(string searchTerm, int?page)
        {
            HomeIndexViewModel model = new HomeIndexViewModel();
            //ViewBag.Value = CartItemCount();

            return View(model.CreateModel(searchTerm, 4, page));        
        }

        public ActionResult GetProduct(int id)
        {

            ProductLogic proLogic = new ProductLogic();
            Product pro = proLogic.GetProduct(id);

            return View(pro);
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact Us For Further Information";

            return View();
        }
    }
}