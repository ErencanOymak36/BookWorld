using AutoMapper;
using BookWorld.Application.DTOs;
using BookWorld.Application.Interfaces;
using BookWorld.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWorld.Application.Services
{
    public class CategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        // CREATE
        public async Task CreateAsync(CreateCategoryDto dto)
        {
            var entity = _mapper.Map<Category>(dto);
            await _categoryRepository.CreateAsync(entity);
        }

        // UPDATE (DOĞRU YAKLAŞIM)
        public async Task UpdateAsync(UpdateCategoryDto dto)
        {
            var existingCategory = await _categoryRepository.GetByIdAsync(dto.Id);

            if (existingCategory == null)
                throw new Exception("Category not found");

            _mapper.Map(dto, existingCategory);

            await _categoryRepository.UpdateAsync(existingCategory);
        }

        // DELETE
        public async Task<bool> DeleteAsync(int id)
        {
            return await _categoryRepository.DeleteAsync(id);
        }

        // GET BY ID
        public async Task<CategoryDto> GetByIdAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            return _mapper.Map<CategoryDto>(category);
        }

        // GET ALL
        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<CategoryDto>>(categories);
        }
    }
}
