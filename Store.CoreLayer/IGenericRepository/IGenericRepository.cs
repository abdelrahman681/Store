using Store.CoreLayer.ISepecfication;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.CoreLayer.IGenericRepository
{
    public interface IGenericRepository<T> where T: class
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int Id);
        void Delete(T Entiety);
        void Update(T Entiety);
        Task AddAsync(T Entiety);
        Task<IReadOnlyList<T?>> GetAllAsyncWithSpecification(ISpecification<T> specification);
        Task<int> GetCountWithSpecification(ISpecification<T> specification);
        Task<T?> GetByIdAsyncWithSpecification(ISpecification<T> specification);
    }
}
