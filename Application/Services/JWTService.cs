using Application.DTOs;
using Application.Enums;
using Application.Interfaces;
using Infrastructure.Interfaces;
using Infrastructure.Models;
using Infrastructure.Repositories;
using Infrastructure.Security;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class JWTService: IJWTService
    {
        private readonly Security _security;
        private readonly IJWTRepository _jWTRepository;

        public JWTService(Security security, IJWTRepository jWTRepository)
        {
            _security = security;
            _jWTRepository = jWTRepository;
        }
        public string GetToken(UserDto userDto)
        {
            return _security.GetToken(userDto.Id, userDto.Username, (int)userDto.Role);
        }

        public async Task<string> LogIn(string email, string password)
        {
            User user = await _jWTRepository.GetUserByEmail(email);

            if (user == null) throw new Exception("El usuario no fue encontrado");

            if (!BCrypt.Net.BCrypt.Verify(password, user?.Password ?? "")) throw new Exception("Credenciales incorrectas");

            return this.GetToken( new UserDto
            {
                Email = user.Email,
                Id = user.Id,
                Password = user.Password,
                Role = (int)Role.Admin == user.Role ? Role.Admin : Role.Supervisor,
                Username = user.Username
            });
        }

        public async Task ChangePassword(int id, string oldPassword, string newPassword)
        {
            await _jWTRepository.ChangePassword(id, oldPassword, newPassword);
        }

        public async Task CreateUser(UserDto userDto)
        {
            await _jWTRepository.CreateUser(new User
            {
                Email = userDto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password),
                Role = (int)userDto.Role,
                Username = userDto.Username
            });
        }

        public async Task DeleteUser(int id)
        {
            await _jWTRepository.DeleteUser(id);
        }

        public async Task<List<ProductDto>> GetProducts(int userId)
        {
            return (await _jWTRepository.GetProducts(userId)).Select(p => new ProductDto
            {
                Id = p.Id,
                Name = p.Name,
                UserId = p.UserId
            }).ToList();
        }

        public async Task CreateProduct(ProductDto productDto)
        {
            await _jWTRepository.CreateProduct(new Product
            {
                Name = productDto.Name,
                UserId = productDto.UserId
            });
        }

        public async Task DeleteProduct(int userId, int id)
        {
            await _jWTRepository.DeleteProduct(userId, id);
        }
    }
}
