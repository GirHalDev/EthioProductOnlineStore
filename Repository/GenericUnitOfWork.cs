using EthioProductShoppingCenter.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;

namespace EthioProductShoppingCenter.Repository
{
    //public class GenericUnitOfWork : IDisposable
    //{
    //    private EthioProductEntities dbEntity = new EthioProductEntities();

    //    public IRepository<tblEntity> GetRepositoryInstance<tblEntity>() where tblEntity : class
    //    {
    //        return new GenericRepository<tblEntity>(dbEntity);
    //    }

    //    public void SaveChanges()
    //    {
    //        dbEntity.SaveChanges();
    //    }

    //    protected virtual void Dispose(bool disposing)
    //    {
    //        if(!this.disposed)
    //        {
    //            if(disposing)
    //            {
    //                dbEntity.Dispose();
    //            }
    //        }
    //        this.disposed = true;
    //    }

    //    public void Dispose()
    //    {
    //        Dispose(true);
    //        GC.SuppressFinalize(this);
    //    }

    //    private bool disposed = false;
    //}

    public class GenericUnitOfWork<EthioProductEntities> : IGenericUnitOfWork<EthioProductEntities>, IDisposable where EthioProductEntities : DbContext, new()
    {
        private bool _disposed = false;
        private string _errorMessage = string.Empty;
        private DbContextTransaction _objTran;
        private Dictionary<string, object> _repositories;
        //Using the Constructor we are initializing the _context variable is nothing but
        //we are storing the DBContext (SampleSecondDBTrialEntities) object in _context variable
        public GenericUnitOfWork()
        {
            Context = new EthioProductEntities();
        }
        //The Dispose() method is used to free unmanaged resources like files, 
        //database connections etc. at any time.
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        //This Context property will return the DBContext object i.e. (SampleSecondDBTrialEntities) object
        public EthioProductEntities Context { get; }

        public IGenericUnitOfWork<DAL.EthioProductEntities> context => throw new NotImplementedException();

        //EthioProductEntities IGenericUnitOfWork<EthioProductEntities>.Context => throw new NotImplementedException();

        //This CreateTransaction() method will create a database Trnasaction so that we can do database operations by
        //applying do evrything and do nothing principle
        public void CreateTransaction()
        {
            _objTran = Context.Database.BeginTransaction();
        }
        //If all the Transactions are completed successfuly then we need to call this Commit() 
        //method to Save the changes permanently in the database
        public void Commit()
        {
            _objTran.Commit();
        }
        //If atleast one of the Transaction is Failed then we need to call this Rollback() 
        //method to Rollback the database changes to its previous state
        public void Rollback()
        {
            _objTran.Rollback();
            _objTran.Dispose();
        }
        //This Save() Method Implement DbContext Class SaveChanges method so whenever we do a transaction we need to
        //call this Save() method so that it will make the changes in the database
        public void Save()
        {
            try
            {
                Context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        _errorMessage += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;
                throw new Exception(_errorMessage, dbEx);
            }
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
                if (disposing)
                    Context.Dispose();
            _disposed = true;
        }
        public GenericRepository<T> GenericRepository<T>() where T : class
        {
            if (_repositories == null)
                _repositories = new Dictionary<string, object>();
            var type = typeof(T).Name;
            if (!_repositories.ContainsKey(type))
            {
                var repositoryType = typeof(GenericRepository<T>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), Context);
                _repositories.Add(type, repositoryInstance);
            }
            return (GenericRepository<T>)_repositories[type];
        }

    }
}