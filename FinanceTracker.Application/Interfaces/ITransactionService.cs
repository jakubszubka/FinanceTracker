using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceTracker.Application.DTOs;

namespace FinanceTracker.Application.Interfaces;

public interface ITransactionService
{
    Task<IEnumerable<TransactionDto>> GetUserTransactions(string userId);

    Task<TransactionDto?> GetById(int id, string userId);

    Task<TransactionDto> CreateTransaction(CreateTransactionDto dto, string userId);

    Task<bool> DeleteTransaction(int id, string userId);
}
