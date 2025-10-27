using Application.DTOs;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace JWTApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class JWTController : ControllerBase
    {
        private readonly IJWTService _jWTService;

        public JWTController(IJWTService jWTService)
        {
            _jWTService = jWTService;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserDto userDto)
        {
            return Ok(await _jWTService.LogIn(userDto.Email, userDto.Password));
        }

        [HttpPost("CambioDeClave")]
        public async Task<IActionResult> CambioDeClave([FromBody] ChangePassword changePassword)
        {
            await _jWTService.ChangePassword(Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value), changePassword.OldPassword, changePassword.NewPassword);
            return Ok();
        }

        [HttpPost("OlvideMiClave")]
        public IActionResult OlvideMiClave()
        {
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("CrearUsuario")]
        public async Task<IActionResult> CreateUser(UserDto userDto)
        {
            await _jWTService.CreateUser(userDto);
            return Ok();
        }

        [HttpDelete("EliminarMiUsuario")]
        public async Task<IActionResult> DeleteUser()
        {
            await _jWTService.DeleteUser(Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value));
            return Ok();
        }

        [HttpGet("ObtenerProductos")]
        public async Task<IActionResult> GetProducts()
        {
            return Ok(await _jWTService.GetProducts(Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value)));
        }

        [HttpPost("CrearProducto")]
        public async Task<IActionResult> CreateProduct(ProductDto productDto)
        {
            productDto.UserId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            await _jWTService.CreateProduct(productDto);
            return Ok();
        }

        [HttpDelete("EliminarProducto/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _jWTService.DeleteProduct(Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value), id);
            return Ok();
        }
    }
}
