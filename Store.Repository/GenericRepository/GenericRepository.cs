using Microsoft.EntityFrameworkCore;
using Store.CoreLayer.IGenericRepository;
using Store.CoreLayer.ISepecfication;
using Store.Repository.StoreContext;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.Repository.GenericRepository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        #region Field
        private readonly StoreDbContext _context;

        #endregion

        #region Ctor
        public GenericRepository(StoreDbContext context)
        {
           _context = context;
        } 
        #endregion
        public async Task AddAsync(T Entiety)
        =>  await _context.Set<T>().AddAsync(Entiety);


        public void Delete(T Entiety)
        => _context.Set<T>().Remove(Entiety);

        public async Task<IReadOnlyList<T>> GetAllAsync()
          => await _context.Set<T>().ToListAsync();

        public async Task<T?> GetByIdAsync(int Id)
         => await _context.Set<T>().FindAsync(Id);
        public void Update(T Entiety)
        => _context.Set<T>().Update(Entiety);
        public async Task<IReadOnlyList<T?>> GetAllAsyncWithSpecification(ISpecification<T> specification)
         => await ApplySpecification(specification).ToListAsync();


        public async Task<T?> GetByIdAsyncWithSpecification(ISpecification<T> specification)
          =>  ApplySpecification(specification).FirstOrDefault();

        private IQueryable<T> ApplySpecification(ISpecification<T> specification)
        {
            return SpecificationEvaluator<T>.GetQuery(_context.Set<T>(), specification);
        }

        public async Task<int> GetCountWithSpecification(ISpecification<T> specification)
          => ApplySpecification(specification).Count();
    }
}
