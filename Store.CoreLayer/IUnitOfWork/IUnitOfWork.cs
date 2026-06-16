using Store.CoreLayer.IGenericRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace Store.CoreLayer.IUnitOfWork
{
    public interface IUnitOfWork :IAsyncDisposable
    {
        Task<int> CompleteAsync();
        IGenericRepository<T> Repository<T>() where T: class;
    }
}
