using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace EthioProductShoppingCenter.Repository
{
    public interface IGenericUnitOfWork<out EthioProductEntities> where EthioProductEntities : DbContext, new()
    {
        EthioProductEntities Context { get; }
        IGenericUnitOfWork<DAL.EthioProductEntities> context { get; }

        void CreateTransaction();
        void Commit();
        void Rollback();
        void Save();
       
    }
}