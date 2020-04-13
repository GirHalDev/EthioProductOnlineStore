using EthioProductShoppingCenter.DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace EthioProductShoppingCenter.Repository
{
    public class GenericRepository<tblEntity> : IRepository<tblEntity> where tblEntity : class
    {
        DbSet<tblEntity> table;

        private EthioProductEntities dbEntities;

        public GenericRepository(EthioProductEntities _dbEntities)
        {
            dbEntities = _dbEntities;
            table = dbEntities.Set<tblEntity>();
        }

        public void Add(tblEntity entity)
        {
            table.Add(entity);
            dbEntities.SaveChanges();
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
                return dbEntities.Database.SqlQuery<tblEntity>(query, parameters).ToList();
            }
            else
            {
                return dbEntities.Database.SqlQuery<tblEntity>(query).ToList();
            }
        }

        public void InactiveAndDeleteMarkByWhereClause(System.Linq.Expressions.Expression<Func<tblEntity, bool>> wherePredict, Action<tblEntity> ForEachPredicate)
        {
            table.Where(wherePredict).ToList().ForEach(ForEachPredicate);
           
        }

        public void Remove(tblEntity entity)
        {
            if(dbEntities.Entry(entity).State == EntityState.Detached)
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
            dbEntities.Entry(entity).State = EntityState.Modified;
            dbEntities.SaveChanges();
        }

        public void UpdateByWhereClause(System.Linq.Expressions.Expression<Func<tblEntity, bool>> wherePredict, Action<tblEntity> ForEachPredicate)
        {
            table.Where(wherePredict).ToList().ForEach(ForEachPredicate);
        }
    }
}