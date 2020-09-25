using Core.Models;
using Core.Specifications;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public interface IGenericRepository<TEntity> where TEntity : BaseEntity
    {
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task<TEntity> GetByIdAsync(int id);
        Task<IEnumerable<TEntity>> GetAllWithSpecificationAsync(ISpecification<TEntity> specification);
        Task<TEntity> GetWithSpecificationAsync(ISpecification<TEntity> specification);
    }
}
