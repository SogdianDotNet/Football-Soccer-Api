using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace FL.Common.Base.Data
{
    /// <summary>
    /// Represents an entity repository
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public partial interface IRepository<TEntity> where TEntity : class
    {
        #region Methods

        /// <summary>
        /// Get entity by identifier
        /// </summary>
        /// <param name="id">Identifier</param>
        /// <returns>Entity</returns>
        TEntity GetById(object id);

        TEntity Get(object id, string includeProperties);
        TEntity Get(Expression<Func<TEntity, bool>> filter, string includeProperties);
        TEntity Get(Expression<Func<TEntity, bool>> filter);
        TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> filter);
        TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> filter, string includeProperties);
        TEntity FindBy(Expression<Func<TEntity, bool>> whereCondition, params Expression<Func<TEntity, object>>[] includes);

        /// <summary>
        /// Insert entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Insert(TEntity entity);

        /// <summary>
        /// Insert entities
        /// </summary>
        /// <param name="entities">Entities</param>
        void Insert(IEnumerable<TEntity> entities);

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Update(TEntity entity);

        /// <summary>
        /// Update entities
        /// </summary>
        /// <param name="entities">Entities</param>
        void Update(IEnumerable<TEntity> entities);

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        void Delete(TEntity entity);

        /// <summary>
        /// Delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        void DeleteAll(IEnumerable<TEntity> entities);
        void DeleteAll(List<Guid> list);
        void DeleteAllByIds<TId>(List<TId> Idslist);
        IEnumerable<TEntity> GetList(Expression<Func<TEntity, bool>> whereCondition, params Expression<Func<TEntity, object>>[] includes);
        IEnumerable<TEntity> GetList(params Expression<Func<TEntity, object>>[] includes);
        #endregion

        #region Properties

        /// <summary>
        /// Gets a table
        /// </summary>
        IQueryable<TEntity> Table { get; }

        /// <summary>
        /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
        /// </summary>
        IQueryable<TEntity> TableNoTracking { get; }

        #endregion
    }
}
