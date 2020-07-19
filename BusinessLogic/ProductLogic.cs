using AutoMapper;
using EthioProductShoppingCenter.DAL;
using EthioProductShoppingCenter.DomainLayer;
using EthioProductShoppingCenter.Infrastructure;
using EthioProductShoppingCenter.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EthioProductShoppingCenter.BusinessLogic
{
    public class ProductLogic
    {
        readonly IRepository<tblProduct> repository;
        private GenericUnitOfWork<EthioProductEntities> uow = new GenericUnitOfWork<EthioProductEntities>();
        public ProductLogic()
        {
            uow = new GenericUnitOfWork<EthioProductEntities>();
            
            repository = new GenericRepository<tblProduct>(uow);
            
        }
       
        public Product GetProduct(int id)
        {
            
            tblProduct product = repository.GetAllRecords().FirstOrDefault(x => x.ID == id);
            var mapper = MappingConfig.MappingObjects();
            

            return mapper.Map<Product>(product);
        }

        //public List<Product> GetProducts()
        //{

        //    List<tblProduct> product = repository.GetAllRecords().;
        //    var mapper = MappingConfig.MappingObjects();


        //    return mapper.Map<List<Product>>(product);
        //}
    }
}