using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MODEL;
using MODEL.Entities;
using REPOSITORY.Repositories.IRepositories;

namespace REPOSITORY.Repositories.Repositories;

internal class TransactionRepository:GenericRepository<Transaction>, ITransactionRepository
{
    public TransactionRepository(DataContext context) : base(context)
    {
    }
}


