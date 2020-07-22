using EthioProductShoppingCenter.DAL;
using PagedList.Mvc;
using PagedList;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using EthioProductShoppingCenter.BusinessLogic;
using EthioProductShoppingCenter.DomainLayer;

namespace EthioProductShoppingCenter.Models.Home
{
    public class HomeIndexViewModel
    {
        [Key]
        public int Id { get; set; }
        //Adding list of products and enabling searching based on Products or Catagory name to the Home page. 
        public EthioProductEntities dbContext = new EthioProductEntities();
        public IPagedList<Product> ListOfProducts { get; set; }

        //public HomeIndexViewModel CreateModel(string searchTerm, int pageSize, int? page)
        //{
        //    SqlParameter[] param = new SqlParameter[] { new SqlParameter("@search", searchTerm ?? (object)DBNull.Value) };
        //    //The Stored procedure name is searchResult and the input parameter name is @search on the database
        //    IPagedList<tblProduct> data = dbContext.Database.SqlQuery<tblProduct>("searchResult @search", param).ToList().ToPagedList(page ?? 1, pageSize);
        //    return new HomeIndexViewModel
        //    {
        //        ListOfProducts = data
        //    };
        //}

        public HomeIndexViewModel CreateModel(string searchTerm, int pageSize, int? page)
        {
            ProductLogic logic = new ProductLogic();
            IPagedList<Product> products = logic.CreateModel(searchTerm, pageSize, page).ToPagedList(page ?? 1, pageSize); ;
            return new HomeIndexViewModel
            {
                ListOfProducts = products
            };
        }

    }
}