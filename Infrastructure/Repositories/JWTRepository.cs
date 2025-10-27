using Infrastructure.Interfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class JWTRepository: IJWTRepository
    {
        private readonly ILogger<JWTRepository> _logger;
        public JWTRepository(ILogger<JWTRepository> logger)
        {
            _logger = logger;
        }

        private RailwayContext GetContext()
        {
            string connectionString = "Host=nozomi.proxy.rlwy.net;Port=10353;Database=railway;Username=postgres;Password=zAcuqwqvZgmBkBtoLBedNynumLfLhkou";
            var optionsBuilder = new DbContextOptionsBuilder<RailwayContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new RailwayContext(optionsBuilder.Options);
        }

        public async Task<User> GetUserByEmail(string email)
        {
            _logger.LogInformation("Se ejecuto GetUserByEmail");
            using (var context = GetContext())
            {
                return await context.Users.FirstOrDefaultAsync(x => x.Email == email);
            }
        }

        public async Task ChangePassword(int id, string oldPassword, string newPassword)
        {
            _logger.LogInformation("Se ejecuto ChangePassword");
            using (var context = GetContext())
            {
                var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id);
                if (!BCrypt.Net.BCrypt.Verify(oldPassword, user?.Password ?? "")) throw new Exception("La contraseña anterior is incorrecta");

                user.Password = BCrypt.Net.BCrypt.HashPassword(newPassword);
                await context.SaveChangesAsync();
            }
        }

        public async Task CreateUser(User user)
        {
            try
            {
                _logger.LogInformation("Se ejecuto CreateUser");
                using (var context = GetContext())
                {
                    await context.Users.AddAsync(user);
                    await context.SaveChangesAsync();
                }
            } catch (Exception ex)
            {
                throw new Exception("Ya existe un usuario con ese email o ese username");
            }
        }

        public async Task DeleteUser(int id)
        {
            _logger.LogInformation("Se ejecuto DeleteUser");
            using (var context = GetContext())
            {
                var user = await context.Users.Include(u => u.Products).FirstOrDefaultAsync(u => u.Id == id);

                if (user == null) throw new Exception("El usuario no fue encontrado");

                if (user.Products.Any()) throw new Exception("El usuario tiene productos asociados"); 

                if (user == null) throw new Exception("El usuario no fue encontrado");
                context.Remove(user);
                await context.SaveChangesAsync();
            }
        }

        public async Task<List<Product>> GetProducts(int userId)
        {
            _logger.LogInformation("Se ejecuto GetProducts");
            using (var context = GetContext())
            {
                return await context.Products.Where(p => p.UserId == userId).ToListAsync();
            }
        }

        public async Task CreateProduct(Product product)
        {
            _logger.LogInformation("Se ejecuto CreateProduct");
            using (var context = GetContext())
            {
                await context.AddAsync(product);
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteProduct(int userId, int id)
        {
            _logger.LogInformation("Se ejecuto DeleteProduct");
            using (var context = GetContext())
            {
                var product = await context.Products.FirstOrDefaultAsync(p => p.UserId == userId && p.Id == id);

                if (product == null) throw new Exception("El producto no fue encontrado");
                context.Remove(product);
                await context.SaveChangesAsync();
            }
        }
    }
}
