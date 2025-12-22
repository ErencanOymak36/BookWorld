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
    public class AuthorService
    {
        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;

        public AuthorService(IAuthorRepository authorRepository, IMapper mapper)
        {
            _mapper = mapper;
            _authorRepository = authorRepository;
        }

        public async Task<IEnumerable<AuthorDto>> GetAllAsync()
        {
            var authors= await _authorRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<AuthorDto>>(authors);

        }
        public async Task<AuthorDto> GetByIdAsync(int id)
        {
            var author = await _authorRepository.GetByIdAsync(id);
            return author == null ? null : _mapper.Map<AuthorDto>(author);
        }
        public async Task<bool> DeleteAsync(int id)
        {
            return await _authorRepository.DeleteAsync(id);
        }
        public async Task CreateAsync(CreateAuthorDto dto)
        {
            var entity = _mapper.Map<Author>(dto);
            await _authorRepository.CreateAsync(entity);
        }
        public async Task UpdateAsync(UpdateAuthorDto dto)
        {
            var existingAuthor = await _authorRepository.GetByIdAsync(dto.Id);

            if (existingAuthor == null)
                throw new Exception("Author not found");

            // SADECE ALANLARI GÜNCELLE
            _mapper.Map(dto, existingAuthor);

            await _authorRepository.UpdateAsync(existingAuthor);
        }
    }
}
