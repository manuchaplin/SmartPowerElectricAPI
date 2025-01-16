using SmartPowerElectricAPI.Utilities;
using System.Linq.Expressions;

namespace SmartPowerElectricAPI.Repository
{
    public interface IGenericRepository<TEntity>
    {
        TEntity GetByID(object id);
        TEntity GetByID(object id, string includeProperties = null);
        void Insert(TEntity entity);
        void InsertMassive(List<TEntity> entities);
        void Save();
        void Delete(object id);
        void Delete(TEntity userToDelete);
        void Update(TEntity entityToUpdate);

        void DeleteRange(IEnumerable<TEntity> entityToDelete, TEntity entityToDeleteAux);

        int Count(String groupBy, List<Expression<Func<TEntity, bool>>> filters);

        IEnumerable<TEntity> Take(int numberTake, List<Expression<Func<TEntity, bool>>> filters = null);
        IEnumerable<TEntity> Take(int numberTake, List<Expression<Func<TEntity, bool>>> filters, List<OrderByHelper<TEntity>> orderByProperties);
        IEnumerable<TEntity> Take(int numberTake, List<Expression<Func<TEntity, bool>>> filters, List<OrderByHelper<TEntity>> orderByProperties, string includeProperties);

        IQueryable<TEntity> Get();
        IEnumerable<TEntity> Get(List<Expression<Func<TEntity, bool>>> filters);
        IEnumerable<TEntity> Get(List<Expression<Func<TEntity, bool>>> filters, string includeProperties);
        IEnumerable<TEntity> Get(List<Expression<Func<TEntity, bool>>> filters, List<OrderByHelper<TEntity>> orderByProperties);
        IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);

        void RefreshDataContext(TEntity entity);
    }
}

