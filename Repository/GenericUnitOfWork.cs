using EthioProductShoppingCenter.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EthioProductShoppingCenter.Repository
{
    public class GenericUnitOfWork : IDisposable
    {
        private EthioProductEntities dbEntity = new EthioProductEntities();

        public IRepository<tblEntity> GetRepositoryInstance<tblEntity>() where tblEntity : class
        {
            return new GenericRepository<tblEntity>(dbEntity);
        }

        public void SaveChanges()
        {
            dbEntity.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if(!this.disposed)
            {
                if(disposing)
                {
                    dbEntity.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool disposed = false;
    }
}