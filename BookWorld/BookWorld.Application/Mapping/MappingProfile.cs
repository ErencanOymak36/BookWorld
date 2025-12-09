using AutoMapper;
using BookWorld.Application.DTOs;
using BookWorld.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookWorld.Application.Mapping
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            // Book
            CreateMap<Book, BookDto>()
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.Name))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));
            CreateMap<CreateBookDto, Book>();

            // Author
            CreateMap<Author, AuthorDto>();
            CreateMap<CreateAuthorDto, Author>();

            // Category
            CreateMap<Category, CategoryDto>();
            CreateMap<CreateCategoryDto, Category>();

            // User
            CreateMap<User, UserDto>();
            CreateMap<CreateUserDto, User>();

            // Cart & CartItem
            CreateMap<CartItem, CartItemDto>()
                .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book.Title));
            CreateMap<Cart, CartDto>();

            // Order & OrderItem
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book.Title));
            CreateMap<Order, OrderDto>();

            // Rental
            CreateMap<Rental, RentalDto>()
                .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book.Title))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Username));
            CreateMap<CreateRentalDto, Rental>();
        }
    }
}
