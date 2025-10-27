using Application.DTOs;
using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IJWTService
    {
        string GetToken(UserDto userDto);
        Task<string> LogIn(string email, string password);
        Task ChangePassword(int id, string oldPassword, string newPassword);
        Task CreateUser(UserDto userDto);
        Task DeleteUser(int id);
        Task<List<ProductDto>> GetProducts(int userId);
        Task CreateProduct(ProductDto productDto);
        Task DeleteProduct(int userId, int id);
    }
}
