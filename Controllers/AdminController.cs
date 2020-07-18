
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
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        public GenericUnitOfWork<EthioProductEntities> unitOfWork = new GenericUnitOfWork<EthioProductEntities>();

        public List<SelectListItem>  GetCatagory()
        {
            List<SelectListItem> lists = new List<SelectListItem>();
            var catagories = unitOfWork.GenericRepository<tblCatagory>().GetAllRecords();
            foreach (var item in catagories)
            {
                lists.Add(new SelectListItem { Value = item.ID.ToString(), Text = item.CatagoryName });
            }

            
            return lists;
        }
        // GET: Admin
        public ActionResult DashBoard()
        {
            return View();
        }

        public ActionResult Catagories()
        {

            List<tblCatagory> allCatagories = unitOfWork.GenericRepository<tblCatagory>().GetAllRecordsIQueryable().Where(x => x.IsDelete == false).ToList();
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
                cd = JsonConvert.DeserializeObject<CatagoryDetail>(JsonConvert.SerializeObject(unitOfWork.GenericRepository<tblCatagory>().GetFirstOrDefault(catagoryId)));
            }
            else
            {
                cd = new CatagoryDetail();
            }
            return View("UpdateCatagory", cd);
        }

        //GET: Admin/EditCatagory/catagoryId
        public ActionResult EditCatagory(int catagoryId)
        {
            return View(unitOfWork.GenericRepository<tblCatagory>().GetFirstOrDefault(catagoryId));
        }

        //POST: Admin/EditCatagory/catagoryId
        [HttpPost]
        public ActionResult EditCatagory(tblCatagory catagory)
        {
            unitOfWork.GenericRepository<tblCatagory>().Update(catagory);
            return RedirectToAction("Catagories");
        }

        //GET: Admin/Product
        public ActionResult Product()
        {
            return View(unitOfWork.GenericRepository<tblProduct>().GetProduct());
        }

        //GET: Admin/EditProduct/productId
        public ActionResult EditProduct(int productId)
        {
            ViewBag.CatagoryList = GetCatagory();
            return View(unitOfWork.GenericRepository<tblProduct>().GetFirstOrDefault(productId));
        }

        //POST: Admin/EditProduct/productId
        [HttpPost]
        public ActionResult EditProduct(tblProduct product, HttpPostedFileBase file)
        {
            string pic = null;
            if (file != null)
            {
                pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/ProductImg/"), pic);

                file.SaveAs(path);
            }
            product.ProductImage = file != null ? pic : product.ProductImage; 
            product.ModifiedDate = DateTime.Now;
            unitOfWork.GenericRepository<tblProduct>().Update(product);
            return RedirectToAction("Product");
        }

        //GET: Admin/AddProduct
        public ActionResult AddProduct()
        {
            ViewBag.CatagoryList = GetCatagory();
            return View();

        }

        //POST: Admin/AddProduct
        [HttpPost]
        public ActionResult AddProduct(tblProduct product, HttpPostedFileBase file)
        {
            string pic = null;
            if(file != null)
            {
                pic = System.IO.Path.GetFileName(file.FileName);
                string path = System.IO.Path.Combine(Server.MapPath("~/ProductImg/"), pic);

                file.SaveAs(path);
            }

            product.ProductImage = pic;

            product.CreatedDate = DateTime.Now;
            unitOfWork.GenericRepository<tblProduct>().Add(product);
            return RedirectToAction("Product");
        }
    }
}