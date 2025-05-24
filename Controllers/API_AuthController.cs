using GP_Backend.Data;
using GP_Backend.DTOs;
using GP_Backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;
using System.Data;
using Microsoft.EntityFrameworkCore;

namespace GP_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class API_AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Construtor do controller
        /// Invocação do UserManager e do context
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="context"></param>
        public API_AuthController(UserManager<IdentityUser> userManager, ApplicationDbContext context, IConfiguration configuration)
        {
            _userManager = userManager;
            _context = context;
            _configuration = configuration;
        }

        /// <summary>
        /// Endpoint para criação de novo Registo
        /// Criação de novo Utilizador
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            //Cria o IdentityUser
            var identityUser = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(identityUser, model.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            //-------------------

            // Cria o Utilizador
            var utilizador = new Utilizadores
            {
                Nome = model.Nome,
                EscolaFK = model.EscolaFK,
                CursoFK = model.CursoFK,

                UserID = identityUser.Id
            };

            _context.Utilizadores.Add(utilizador);
            await _context.SaveChangesAsync();
            //------------------

            return Ok("Registo efetuado com sucesso.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto model)
        {
            // Procurar o utilizador pelo email
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return BadRequest(new { message = "Email ou password inválidos." });

            // Verificar se o email está confirmado
            if (!user.EmailConfirmed)
                return BadRequest(new { message = "O email ainda não foi confirmado." });

            // Verificar se a password está correta
            var passwordValid = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!passwordValid)
                return BadRequest(new { message = "Email ou password inválidos." });


            // Criar os claims do token
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            // Chave secreta
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Criar o token JWT
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            // Retornar o token
            return Ok(new { token = tokenString });
        }



        [HttpGet("me")]
        [Authorize]  // Só acessível com token válido
        public async Task<IActionResult> GetMe()
        {
           
            // Obter o ID do utilizador a partir dos claims do token   
            var userIdClaim = User.Claims
                .Where(c => c.Type == ClaimTypes.NameIdentifier)
                .FirstOrDefault(c => Guid.TryParse(c.Value, out _));

            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            var userId = userIdClaim.Value;

            // Obter utilizador ASP.NET Identity
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound();
            }

            // Obter registo adicional na tabela Utilizadores
            var utilizador = await _context.Utilizadores
                .FirstOrDefaultAsync(u => u.UserID == userId);

            // Se o utilizador não existir na tabela Utilizadores, retornar erro 404
            if (utilizador == null)
            {
                return NotFound(new { message = "Utilizador não encontrado na tabela Utilizadores." });
            }

            // Obter roles do utilizador
            var roles = await _userManager.GetRolesAsync(user);
                       
            return Ok(new
            {
                user.Id,
                user.Email,
                user.UserName,
                Name = utilizador.Nome,
                Role = roles.FirstOrDefault()
            });
        }
    }
}
