using EthioProductShoppingCenter.DAL;
using EthioProductShoppingCenter.Models;
using EthioProductShoppingCenter.Repository;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EthioProductShoppingCenter.Controllers
{
    public class AdminController : Controller
    {
        public GenericUnitOfWork unitOfWork = new GenericUnitOfWork();
        // GET: Admin
        public ActionResult DashBoard()
        {
            return View();
        }

        public ActionResult Catagories()
        {
            List<tblCatagory> allCatagories = unitOfWork.GetRepositoryInstance<tblCatagory>().GetAllRecordsIQueryable().Where(x => x.IsDelete == false).ToList();
            return View(allCatagories);
        }

        public ActionResult AddCatagory()
        {
            return UpdateCatagory(0);
        }

        public ActionResult UpdateCatagory(int catagoryId)
        {
            CatagoryDetail cd;

            if (catagoryId != null)
            {
                cd = JsonConvert.DeserializeObject<CatagoryDetail>(JsonConvert.SerializeObject(unitOfWork.GetRepositoryInstance<tblCatagory>().GetFirstOrDefault(catagoryId)));
            }
            else
            {
                cd = new CatagoryDetail();
            }
            return View("UpdateCatagory", cd);
        }

        //GET: Admin/Product
        public ActionResult Product()
        {
            return View(unitOfWork.GetRepositoryInstance<tblProduct>().GetProduct());
        }

        //GET: Admin/EditProduct/productId
        public ActionResult EditProduct(int productId)
        {
            return View(unitOfWork.GetRepositoryInstance<tblProduct>().GetFirstOrDefault(productId));
        }

        //POST: Admin/EditProduct/productId
        [HttpPost]
        public ActionResult EditProduct(tblProduct product)
        {
            unitOfWork.GetRepositoryInstance<tblProduct>().Update(product);
            return RedirectToAction("Product");
        }

        //GET: Admin/AddProduct
        public ActionResult AddProduct()
        {
            return View();
        }

        //POST: Admin/AddProduct
        [HttpPost]
        public ActionResult AddProduct(tblProduct product)
        {
            unitOfWork.GetRepositoryInstance<tblProduct>().Add(product);
            return RedirectToAction("Product");
        }
    }
}