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

        public ActionResult Product()
        {
            return View();
        }
    }
}