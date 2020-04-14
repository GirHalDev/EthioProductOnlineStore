using EthioProductShoppingCenter.DAL;
using EthioProductShoppingCenter.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace EthioProductShoppingCenter.Models.Home
{
    public class HomeIndexViewModel
    {
        [Key]
        public int Id { get; set; }
        //Adding list of products to the Home page
        public GenericUnitOfWork unitOfWork = new GenericUnitOfWork();
        
       public IEnumerable<tblProduct> ListOfProducts { get; set; }
        public HomeIndexViewModel CreateModel()
        {
            return new HomeIndexViewModel
            {
                ListOfProducts = unitOfWork.GetRepositoryInstance<tblProduct>().GetAllRecords()
            };
        }
    }
}