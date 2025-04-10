using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MODEL;
using MODEL.DTOs;
using MODEL.Entities;
using REPOSITORY.Repositories.IRepositories;
using REPOSITORY.Repositories.Repositories;

namespace REPOSITORY.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{

    private readonly DataContext _dataContext;

  

    public UnitOfWork(DataContext dataContext, IOptions<AppSettings> appsettings )
    {
        _dataContext = dataContext;
        User = new UserRepository(_dataContext);
        transaction = new TransactionRepository(_dataContext);
        AppSettings = appsettings.Value;
    }

    public IUserRepository User { get; set; }


    public ITransactionRepository transaction { get; set; }

    public AppSettings AppSettings { get;  set; }

    public void Dispose()
    {
       _dataContext.Dispose();
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _dataContext.SaveChangesAsync();
    }
}
