using Application.DTOs;
using Application.DTOs.User;
using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Domain.Entities;
using Domain.Exceptions;
using System.ComponentModel.DataAnnotations;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUserRepository userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> CreateUserAsync(CreateUserDto dto)
        {
            var existingUser = await _userRepository.GetUserByEmailAsync(dto.Email);
            if (existingUser != null)
                throw new ValidationException($"Já existe um utilizador registado com o email '{dto.Email}'.");

            var age = CalculateAge(dto.DateOfBirth);
            if (age < 18 || age > 70)
                throw new ValidationException("O utilizador deve ter entre 18 e 70 anos.");

            if (string.IsNullOrWhiteSpace(dto.Password) || dto.Password.Length < 8 || dto.Password.Length > 16)
                throw new ValidationException("A password deve ter entre 8 e 16 caracteres.");

            var newUser = new Player(
                dto.Name,
                dto.DateOfBirth,
                dto.Address,
                dto.Email,
                dto.Password,
                dto.PhoneNumber
            );

            await _userRepository.AddAsync(newUser);
            await _unitOfWork.SaveChangesAsync();

            return newUser.Id;
        }

        private int CalculateAge(DateOnly dateOfBirth)
        {
            var today = DateOnly.FromDateTime(DateTime.Today);
            int age = today.Year - dateOfBirth.Year;
            if (dateOfBirth > today.AddYears(-age)) age--;
            return age;
        }
    }
}