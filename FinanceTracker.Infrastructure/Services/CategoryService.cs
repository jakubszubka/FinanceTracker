using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceTracker.Application.DTOs;
using FinanceTracker.Application.Interfaces;
using FinanceTracker.Domain.Enums;
using FinanceTracker.Domain.Entities;
using FinanceTracker.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FinanceTracker.Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _db;

        public CategoryService(ApplicationDbContext db) { _db = db; }
        public async Task<IEnumerable<CategoryDto>> GetCategories()
        {
            var categories = await _db.Categories
                .Select(t => new CategoryDto
            {
                    Id = t.Id,
                    Name = t.Name
            })
                .ToListAsync();

            return categories;
        }

        public async Task<CategoryDto?> GetCategoryById(int id)
        {
            var category = await _db.Categories
                .Where(c => c.Id == id)
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name
                }).FirstOrDefaultAsync();

            return category;
        }

        public async Task<CategoryDto> CreateCategory(CreateCategoryDto dto)
        {
            if (dto == null) return null;//something

            if (string.IsNullOrWhiteSpace(dto.Name)) throw new Exception("Category name is required");

            if (await _db.Categories.AnyAsync(t => t.Name.ToLower() == dto.Name.ToLower())) throw new Exception("Category already exists");

            var newCategory = new Category
            {
                Name = dto.Name,
            };

            _db.Categories.Add(newCategory);
            await _db.SaveChangesAsync();

            return new CategoryDto
            {
                Id = newCategory.Id,
                Name = newCategory.Name
            };
        }

        public async Task<bool> DeleteCategory(int categoryId)
        {
            var category = await _db.Categories
                .Include(c => c.Transactions)
                .FirstOrDefaultAsync(c => c.Id == categoryId);

            if (category == null) return false;

            if (category.Transactions.Any()) throw new Exception("Cannot deleta category with existing transactions");

            _db.Categories.Remove(category);
            await _db.SaveChangesAsync();

            return true;
        }

        public async Task<CategoryDto?> UpdateCategory(int categoryId, UpdateCategoryDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new Exception("Category name is required");

            var category = await _db.Categories.FindAsync(categoryId);

            if (category == null) return null;

            if (await _db.Categories.AnyAsync(c => c.Name.ToLower() == dto.Name.ToLower() && c.Id != categoryId))
                throw new Exception("Category already exists");

            category.Name = dto.Name;

            await _db.SaveChangesAsync();

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name
            };
        }
    }
}
