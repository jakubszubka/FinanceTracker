using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using FinanceTracker.Application.DTOs;
using FinanceTracker.Application.Interfaces;
using FinanceTracker.Domain.Enums;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Application.Services;

public class TransactionService : ITransactionService
{
    public async Task<TransactionDto> CreateTransaction(CreateTransactionDto cDto, string userId)
    {
        if (!await _db.Categories.AnyAsync(t => t.Id == cDto.CategoryId)) throw new Exception("Category doesnt exist");

        var transaction = new Transaction
        {
            Amount = cDto.Amount,
            Description = cDto.Description,
            Date = cDto.Date,
            CategoryId = cDto.CategoryId,
            Type = (Domain.Enums.TransactionType)cDto.Type,
            UserId = userId
        };

        _db.Transactions.Add(transaction);
        await _db.SaveChangesAsync();

        transaction.Category = await _db.Categories.Where(t => t.Id == cDto.CategoryId).FirstOrDefaultAsync();

        return new TransactionDto
        {
            Id = transaction.Id,
            Amount = transaction.Amount,
            Description = transaction.Description,
            Date = transaction.Date,
            CategoryName = transaction.Category.Name,
            Type = transaction.Type.ToString()
        };
    }

    public async Task<TransactionDto?> UpdateTransaction(int id, UpdateTransactionDto uDto, string userId)
    {
        var transaction = await _db.Transactions
            .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

        if (transaction == null) return null;

        if (uDto.CategoryId.HasValue)
        {
            var categoryExists = await _db.Categories.AnyAsync(c => c.Id == uDto.CategoryId.Value);
            if (!categoryExists) throw new Exception("Category does not exist");
        }

        transaction.Amount = uDto.Amount ?? transaction.Amount;
        transaction.Description = uDto.Description ?? transaction.Description;
        transaction.Date = uDto.Date ?? transaction.Date;
        transaction.Type = uDto.Type.HasValue ? (TransactionType)uDto.Type.Value : transaction.Type;
        transaction.CategoryId = uDto.CategoryId ?? transaction.CategoryId;

        await _db.SaveChangesAsync();
        return new TransactionDto
        {
            Id = transaction.Id,
            Amount = transaction.Amount,
            Description = transaction.Description,
            Date = transaction.Date,
            CategoryName = (await _db.Categories.FindAsync(transaction.CategoryId))?.Name,
            Type = transaction.Type.ToString()
        };
    }

    public async Task<bool> DeleteTransaction(int id, string userId)
    {
        var transaction = await _db.Transactions.FirstOrDefaultAsync(t => t.Id == id &&  t.UserId == userId);

        if (transaction == null) return false;

        _db.Transactions.Remove(transaction);
        await _db.SaveChangesAsync();
        return true;
    }
    

    public async Task<TransactionDto?> GetById(int id, string userId)
    {
        TransactionDto? transactionDto = await _db.Transactions
            .Where(t => t.Id == id && t.UserId == userId)
            .Select(t => new TransactionDto
        {
            Id = t.Id,
            Amount = t.Amount,
            Description = t.Description,
            Date = t.Date,
            CategoryName = t.Category.Name,
            Type = t.Type.ToString()
        })
            .FirstOrDefaultAsync();

        return transactionDto;
    }

    public async Task<IEnumerable<TransactionDto>> GetUserTransactions(string userId)
    {
        var transactions = await _db.Transactions.Where(t => t.UserId == userId)
            .Select(t => new TransactionDto
            {
                Id = t.Id,
                Amount = t.Amount,
                Description = t.Description,
                Date = t.Date,
                CategoryName = t.Category.Name,
                Type = t.Type.ToString()
            })
            .ToListAsync();
       
        return transactions;
    }

    private readonly ApplicationDbContext _db;

    public TransactionService(ApplicationDbContext db) { _db = db; }
}