using GP_Backend.Data;
using GP_Backend.DTOs;
using GP_Backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace GP_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class API_AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// Construtor do controller
        /// Invocação do UserManager e do context
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="context"></param>
        public API_AuthController(UserManager<IdentityUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
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
                UserName = model.Nome
            };

            var result = await _userManager.CreateAsync(identityUser, model.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);
            //-------------------

            // Cria o Utilizador
            var utilizador = new Utilizadores
            {
                //Nome = model.Nome,
                //EscolaFK = model.EscolaFK,
                //CursoFK = model.CursoFK,

                //userID = identityUser.Id
            };

            _context.Utilizadores.Add(utilizador);
            await _context.SaveChangesAsync();
            //------------------

            return Ok("Registo efetuado com sucesso.");
        }

    }
}
