using Store.CoreLayer.IGenericRepository;
using Store.CoreLayer.IUnitOfWork;
using Store.Repository.GenericRepository;
using Store.Repository.StoreContext;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Store.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Field
        private readonly StoreDbContext _context;
        private  Hashtable _hashtable;
        #endregion

        #region Ctor
        public UnitOfWork(StoreDbContext context)
        {
            _context = context;
            _hashtable= new Hashtable();
        }
        #endregion
        public async Task<int> CompleteAsync()
        =>await _context.SaveChangesAsync();

        public async ValueTask DisposeAsync()
        => await _context.DisposeAsync();

        public IGenericRepository<T> Repository<T>() where T : class
        {
            var type = typeof(T).Name;
            if (!_hashtable.ContainsKey(type))
            {
                var repo = new GenericRepository<T>(_context);
                _hashtable.Add(type, repo);
            }

            return _hashtable[type] as IGenericRepository<T>;
        }
    }
}
