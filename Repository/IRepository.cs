using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace EthioProductShoppingCenter.Repository
{
    public interface IRepository<tblEntity> where tblEntity: class
    {
        int GetAllRecordsCount();
        IEnumerable<tblEntity> GetAllRecords();
        IQueryable<tblEntity> GetAllRecordsIQueryable();
        void Add(tblEntity entity);
        void Update(tblEntity entity);
        void UpdateByWhereClause(Expression<Func<tblEntity, bool>> wherePredict, Action<tblEntity> ForEachPredicate);
        tblEntity GetFirstOrDefault(int recordId);
        void Remove(tblEntity entity);
        void RemoveByWhereClause(Expression<Func<tblEntity, bool>> wherePredict);
        void RemoveRangeByWhereClause(Expression<Func<tblEntity, bool>> wherePredict);
        void InactiveAndDeleteMarkByWhereClause(Expression<Func<tblEntity, bool>> wherePredict, Action<tblEntity> ForEachPredicate);
        tblEntity GetFirstOrDefaultByParameter(Expression<Func<tblEntity, bool>> wherePredict);
        IEnumerable<tblEntity> GetListParameter(Expression<Func<tblEntity, bool>> wherePredict);
        IEnumerable<tblEntity> GetResultBySqlProcedure(string query, params object[] parameters);
        IEnumerable<tblEntity> GetRecordsToShow(int pageNo, int pageSize, int currentPage, Expression<Func<tblEntity, bool>> wherePredict, Expression<Func<tblEntity, int>> orderByPredict);
    }
}