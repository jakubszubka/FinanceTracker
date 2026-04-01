using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinanceTracker.Application.DTOs;

namespace FinanceTracker.Application.Interfaces;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDto>> GetCategories();

    Task<CategoryDto> GetCategoryById(int id);

    Task<CategoryDto> CreateCategory(CreateCategoryDto dto);

    Task<bool> DeleteCategory(int categoryId);

    Task<CategoryDto> UpdateCategory(int categoryId, UpdateCategoryDto dto);
}

