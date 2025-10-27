using Infrastructure.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Interfaces
{
    public interface IJWTRepository
    {
        Task<User> GetUserByEmail(string email);
        Task ChangePassword(int id, string oldPassword, string newPassword);
        Task CreateUser(User user);
        Task DeleteUser(int id);
        Task<List<Product>> GetProducts(int userId);
        Task CreateProduct(Product product);
        Task DeleteProduct(int userId, int id);
    }
}
