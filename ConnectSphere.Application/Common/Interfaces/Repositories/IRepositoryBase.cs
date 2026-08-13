using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ConnectSphere.Application.Common.Interfaces.Repositories
{
    public interface IRepositoryBase<TEntity>
    {
        //We make all Functionalities non async to easy to switch to synchronous 

        //Create
        protected void Create(TEntity entity);
        protected void CreateCollection(IEnumerable<TEntity> entities);

        //Read
        protected IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression
           , bool trackChanges
           , Func<IQueryable<TEntity>
           , IIncludableQueryable<TEntity, object>>? include = null);


        protected IQueryable<TEntity> FindAll(bool trackChanges, Func<IQueryable<TEntity>
            , IIncludableQueryable<TEntity, object?>>? include = null);


        //Update for disconnected updates
        protected void Update(TEntity entity);


        //Delete
        protected void Delete(TEntity entity);


        protected Task<bool> IsExist(Expression<Func<TEntity, bool>> expression);

    }
}
