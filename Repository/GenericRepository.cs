using EthioProductShoppingCenter.DAL;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI.WebControls;

namespace EthioProductShoppingCenter.Repository
{
    public class GenericRepository<tblEntity> : IRepository<tblEntity>, IDisposable where tblEntity : class
    {
        internal DbSet<tblEntity> table;
        //private string _errorMessage = string.Empty;
        internal bool _isDisposed;
        //private EthioProductEntities Context;



        public GenericRepository(IGenericUnitOfWork<EthioProductEntities> unitOfWork)
        :this(unitOfWork.Context)
        {
        }
        public GenericRepository(EthioProductEntities _context)
        {
            _isDisposed = false;
            Context = _context;
            this.table = Context.Set<tblEntity>();
            
        }
        

        public EthioProductEntities Context { get; set; }

        //public virtual IQueryable<tblEntity> Entity
        //{
        //    get { return Table; }
        //}
        //public virtual DbSet<tblEntity> Table { get; set; } 
       

        public void Add(tblEntity entity)
        {
            table.Add(entity);
            Context.SaveChanges();
        }

        public IEnumerable<tblEntity> GetProduct()
        {
            return table.ToList();
        }

        public IEnumerable<tblEntity> GetAllRecords()
        {
            return table.ToList();
        }

        public int GetAllRecordsCount()
        {
            return table.Count();
        }

        public IQueryable<tblEntity> GetAllRecordsIQueryable()
        {
            return table;
        }

        public tblEntity GetFirstOrDefault(int recordId)
        {
            return table.Find(recordId);
        }

        public tblEntity GetFirstOrDefaultByParameter(System.Linq.Expressions.Expression<Func<tblEntity, bool>> wherePredict)
        {
            return table.Where(wherePredict).FirstOrDefault();
        }

        public IEnumerable<tblEntity> GetListParameter(System.Linq.Expressions.Expression<Func<tblEntity, bool>> wherePredict)
        {
            return table.Where(wherePredict).ToList();
        }

        public IEnumerable<tblEntity> GetRecordsToShow(int pageNo, int pageSize, int currentPage, Expression<Func<tblEntity, bool>> wherePredict, Expression<Func<tblEntity, int>> orderByPredict)
        {
            if(wherePredict != null)
            {
                return table.OrderBy(orderByPredict).Where(wherePredict).ToList();
            }
            else
            {
                return table.OrderBy(orderByPredict).ToList();
            }
        }

        public IEnumerable<tblEntity> GetResultBySqlProcedure(string query, params object[] parameters)
        {
            if(parameters != null)
            {
                return Context.Database.SqlQuery<tblEntity>(query, parameters).ToList();
            }
            else
            {
                return Context.Database.SqlQuery<tblEntity>(query).ToList();
            }
        }

        public void InactiveAndDeleteMarkByWhereClause(System.Linq.Expressions.Expression<Func<tblEntity, bool>> wherePredict, Action<tblEntity> ForEachPredicate)
        {
            table.Where(wherePredict).ToList().ForEach(ForEachPredicate);
           
        }

        public void Remove(tblEntity entity)
        {
            if(Context.Entry(entity).State == EntityState.Detached)
            {
                table.Attach(entity);
                table.Remove(entity);
            }
        }

        public void RemoveByWhereClause(System.Linq.Expressions.Expression<Func<tblEntity, bool>> wherePredict)
        {
            tblEntity entity = table.Where(wherePredict).FirstOrDefault();
            Remove(entity);
        }

        public void RemoveRangeByWhereClause(System.Linq.Expressions.Expression<Func<tblEntity, bool>> wherePredict)
        {
            List<tblEntity> entities = table.Where(wherePredict).ToList();

            foreach (tblEntity entity in entities)
            {
                Remove(entity);
            }
        }

        
        public void Update(tblEntity entity)
        {
            table.Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
            Context.SaveChanges();

        }

        public void UpdateByWhereClause(System.Linq.Expressions.Expression<Func<tblEntity, bool>> wherePredict, Action<tblEntity> ForEachPredicate)
        {
            table.Where(wherePredict).ToList().ForEach(ForEachPredicate);
        }

        public void Dispose()
        {
            if (Context != null)
                Context.Dispose();
            _isDisposed = true;
        }
    }
}