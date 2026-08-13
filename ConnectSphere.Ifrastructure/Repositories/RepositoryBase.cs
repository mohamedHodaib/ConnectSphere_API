using ConnectSphere.Application.Common.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ConnectSphere.Infrastructure.Persistence;

namespace ConnectSphere.Infrastructure.Repositories
{
    public abstract class RepositoryBase<TEntity> : IRepositoryBase<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext _appDbContext;

        public RepositoryBase(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }


        //Create
        public void Create(TEntity entity) => GetDBSet().Add(entity);
        public void CreateCollection(IEnumerable<TEntity> entities) =>
           GetDBSet().AddRange(entities);

        //Read
        public IQueryable<TEntity> FindByCondition(Expression<Func<TEntity, bool>> expression
            , bool trackChanges, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object?>>? include = null)
        {
            IQueryable<TEntity> query = GetDBSet();

            var queryAfterApplyingInclude = Getqueryafterusinginclude(query, include);

            return trackChanges
                   ? queryAfterApplyingInclude.Where(expression)
                   : queryAfterApplyingInclude.Where(expression).AsNoTracking();
        }


        public IQueryable<TEntity> FindAll(bool trackChanges, Func<IQueryable<TEntity>
            , IIncludableQueryable<TEntity, object?>>? include = null) =>
                    trackChanges
                   ? Getqueryafterusinginclude(GetDBSet(), include)
                   : Getqueryafterusinginclude(GetDBSet(), include).AsNoTracking();


        //Update for disconnected updates
        public void Update(TEntity entity) =>
            GetDBSet().Update(entity);


        //Delete
        public void Delete(TEntity entity) =>
            GetDBSet().Remove(entity);


        public async Task<bool> IsExist(Expression<Func<TEntity, bool>> expression) =>
            await GetDBSet().AnyAsync(expression);




        #region helper
        private IQueryable<TEntity> Getqueryafterusinginclude(IQueryable<TEntity> query, Func<IQueryable<TEntity>
            , IIncludableQueryable<TEntity, object?>>? include)
        {
            if (include != null)
            {
                query = include(query);
            }

            return query;
        }


        private DbSet<TEntity> GetDBSet() => _appDbContext.Set<TEntity>();
        #endregion

    }
}
