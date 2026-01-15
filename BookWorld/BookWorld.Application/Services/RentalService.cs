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
    public class RentalService
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IMapper _mapper;

        public RentalService(
            IRentalRepository rentalRepository,
            IBookRepository bookRepository,
            IMapper mapper)
        {
            _rentalRepository = rentalRepository;
            _bookRepository = bookRepository;
            _mapper = mapper;
        }

        public async Task CreateRentalAsync(CreateRentalDto dto,int userId)
        {
            var isRented = await _rentalRepository.IsBookCurrentlyRentedAsync(dto.BookId);

            if (isRented)
                throw new Exception("This book is currently rented.");

            var book = await _bookRepository.GetBookByIdAsync(dto.BookId);

            if (book == null)
                throw new Exception("Book not found");
            if (book.Stock <= 0)
                throw new Exception("Book out of stock");

            book.Stock -= 1;

            var rental = new Rental
            {
                UserId = userId,
                BookId = dto.BookId,
                RentDate = DateTime.UtcNow,
                RentalPeriodDays = dto.RentalPeriodDays,
                RentalPrice = book.Price * dto.RentalPeriodDays
            };

            await _rentalRepository.CreateRentalAsync(rental);
        }

        public async Task<IEnumerable<RentalDto>> GetAllAsync()
        {
            var rentals = await _rentalRepository.GetAllRentalsAsync();
            return _mapper.Map<IEnumerable<RentalDto>>(rentals);
        }

        public async Task<RentalDto> GetByIdAsync(int id)
        {
            var rental = await _rentalRepository.GetRentalByIdAsync(id);
            return _mapper.Map<RentalDto>(rental);
        }

        public async Task<IEnumerable<RentalDto>> GetByUserIdAsync(int userId)
        {
            var rentals = await _rentalRepository.GetRentalsByUserIdAsync(userId);
            return _mapper.Map<IEnumerable<RentalDto>>(rentals);
        }

        public async Task ReturnRentalAsync(int rentalId)
        {
            var rental = await _rentalRepository.GetRentalByIdAsync(rentalId);

            if (rental == null)
                throw new Exception("Rental not found");

            if (rental.ReturnDate != null)
                throw new Exception("Book already returned");

            // İade tarihi
            rental.ReturnDate = DateTime.UtcNow;

            // Gecikme cezası hesapla
            var penalty = CalculatePenalty(rental);

            // Toplam kiralama ücretine ceza ekle
            rental.RentalPrice += penalty;

            // Stok geri artır
            var book = await _bookRepository.GetBookByIdAsync(rental.BookId);

            if (book == null)
                throw new Exception("Book not found");

            book.Stock += 1;

            // DB işlemleri
            await _bookRepository.UpdateBookAsync(book);
            await _rentalRepository.UpdateRentalAsync(rental);
        }

        private decimal CalculatePenalty(Rental rental)
        {
            var dueDate = rental.RentDate.AddDays(rental.RentalPeriodDays);
            var returnDate = rental.ReturnDate ?? DateTime.UtcNow;

            if (returnDate <= dueDate)
                return 0;

            var lateDays = (returnDate - dueDate).Days;
            return lateDays * (rental.RentalPrice * 0.05m);
        }

        public async Task DeleteAsync(int id)
        {
            var rental = await _rentalRepository.GetRentalByIdAsync(id);

            if (rental != null)
                await _rentalRepository.DeleteRentalAsync(rental);
        }
        public async Task<IEnumerable<RentalDto>> GetActiveRentalsAsync()
        {
            var rentals = await _rentalRepository.GetActiveRentalsAsync();
            return _mapper.Map<IEnumerable<RentalDto>>(rentals);
        }

        public async Task<IEnumerable<RentalDto>> GetLateRentalsAsync()
        {
            var rentals = await _rentalRepository.GetLateRentalsAsync();
            return _mapper.Map<IEnumerable<RentalDto>>(rentals);
        }

        public async Task<RentalDto> CancelRentalManuelAsync(int id)
        {
            var rental= await _rentalRepository.CancelRentalAsync(id);
            if(rental == null)
            {
                return null;
            }
            return _mapper.Map<RentalDto>(rental);
        }

        public async Task<IEnumerable<RentalDto>> GetCompletedRentalsAsync()
        {
            var rentals = await _rentalRepository.GetCompletedRentalsAsync();
            return _mapper.Map<IEnumerable<RentalDto>>(rentals);
        }
    }
}
