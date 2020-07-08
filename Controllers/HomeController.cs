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

        //public decimal CartItemCount()
        //{
        //    IShoppingCart vm = null;
        //    return vm.GetCount();
        //}


        //Autocomplete jquery widget for search
        //public JsonResult GetProduct(string term)
        //{
        //    EthioProductEntities dbEntity = new EthioProductEntities();

        //    List<string> pro = dbEntity.tblProducts.Where(x => x.ProductName.StartsWith(term)).Select(y => y.ProductName).ToList();

        //    return Json(pro, JsonRequestBehavior.AllowGet.ToString());
        //}

        [Authorize(Roles = "Admin")]
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