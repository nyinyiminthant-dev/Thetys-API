using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MODEL.DTOs;
using REPOSITORY.Repositories.IRepositories;

namespace REPOSITORY.UnitOfWork;

public interface IUnitOfWork : IDisposable
{
    IUserRepository User { get; }
    ITransactionRepository transaction { get; }
    AppSettings AppSettings { get; set; }
    Task<int> SaveChangesAsync();
}
