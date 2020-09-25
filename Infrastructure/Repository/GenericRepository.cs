using Core.Models;
using Core.Specifications;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        private readonly StoreContext _storeContext;

        public GenericRepository(StoreContext storeContext)
        {
            _storeContext = storeContext;
        }
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _storeContext.Set<TEntity>().ToListAsync();
        }
        public async Task<TEntity> GetByIdAsync(int id)
        {
            return await _storeContext.Set<TEntity>().FirstOrDefaultAsync(o => o.Id == id);
        }
        public async Task<IEnumerable<TEntity>> GetAllWithSpecificationAsync(ISpecification<TEntity> specification)
        {
            var query = EvaluateQuery(specification);

            return await query.ToListAsync();
        }
        public async Task<TEntity> GetWithSpecificationAsync(ISpecification<TEntity> specification)
        {
            var query = EvaluateQuery(specification);

            return await query.FirstOrDefaultAsync();
        }
        private IQueryable<TEntity> EvaluateQuery(ISpecification<TEntity> specification)
        {
            var query = _storeContext.Set<TEntity>().AsQueryable();
            query = SpecificationEvaluator<TEntity>.GetQuery(query, specification);

            return query;
        }
    }
}
