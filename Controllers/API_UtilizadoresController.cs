using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GP_Backend.Data;
using GP_Backend.Models;
using GP_Backend.DTOs;
using Microsoft.AspNetCore.Identity;

namespace GP_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class API_UtilizadoresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public API_UtilizadoresController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/API_Utilizadores
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UtilizadorDTO>>> GetUtilizadores()
        {
            var users = await _context.Utilizadores
                .Include(u => u.Escola)
                .Include(u => u.Curso)
                .ToListAsync();

            var aspNetUsers = await _context.Users.ToListAsync();
            var userRoles = await _context.UserRoles.ToListAsync();
            var roles = await _context.Roles.ToListAsync();

            var result = users.Select(u =>
            {
                var aspUser = aspNetUsers.FirstOrDefault(a => a.Id == u.UserID);
                var userRole = userRoles.FirstOrDefault(ur => ur.UserId == u.UserID);
                var roleName = userRole != null
                    ? roles.FirstOrDefault(r => r.Id == userRole.RoleId)?.Name
                    : null;

                return new UtilizadorDTO
                {
                    Id = u.Id,
                    Nome = u.Nome,
                    EscolaNome = u.Escola?.Nome,
                    CursoNome = u.Curso?.Nome,
                    UserID = u.UserID,
                    Role = roleName,
                    Email = aspUser?.Email,
                    EmailConfirmed = aspUser?.EmailConfirmed ?? false
                };
            }).ToList();

            return result;
        }


        // GET: api/API_Utilizadores/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UtilizadorDTO>> GetUtilizadores(int id)
        {
            var utilizador = await _context.Utilizadores
                .Include(u => u.Escola)  
                .Include(u => u.Curso)   
                .FirstOrDefaultAsync(u => u.Id == id);

            if (utilizador == null)
            {
                return NotFound();
            }

            var aspUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == utilizador.UserID);
            var userRole = await _context.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == utilizador.UserID);
            var roleName = userRole != null
                ? await _context.Roles
                    .Where(r => r.Id == userRole.RoleId)
                    .Select(r => r.Name)
                    .FirstOrDefaultAsync()
                : null;
            var roleId = userRole != null
                ? userRole.RoleId
                : null;

            var dto = new UtilizadorDTO
            {
                Id = utilizador.Id,
                Nome = utilizador.Nome,
                EscolaNome = utilizador.Escola?.Nome,  
                EscolaId = utilizador.Escola?.Id ?? null,  
                CursoNome = utilizador.Curso?.Nome,     
                CodCurso = utilizador.Curso?.CodCurso ?? null,  
                UserID = utilizador.UserID,
                Role = roleName,
                RoleId = roleId,
                Email = aspUser?.Email,
                EmailConfirmed = aspUser?.EmailConfirmed ?? false
            };

            return dto;
        }


        // PUT: api/API_Utilizadores/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUtilizador(int id, [FromBody] UtilizadorUpdateDTO dto)
        {
            if (id != dto.Id)
                return BadRequest();

            // Verifica se o utilizador existe
            var utilizador = await _context.Utilizadores.FindAsync(id);
            if (utilizador == null)
                return NotFound();

            // Atualiza os campos do utilizador
            utilizador.Nome = dto.Nome;
            utilizador.CursoFK = dto.CodCurso;
            utilizador.EscolaFK = dto.EscolaId;
            _context.Entry(utilizador).State = EntityState.Modified;

            // Atualiza o utilizador no ASP.NET Identity
            var aspUser = await _context.Users.FindAsync(utilizador.UserID);
            if (aspUser != null)
            {
                aspUser.EmailConfirmed = dto.EmailConfirmed;
                _context.Entry(aspUser).State = EntityState.Modified;
            }
            else
            {
                return NotFound("Usuário não encontrado no ASP.NET Identity.");
            }

            // Se o RoleId for fornecido, remove as roles atuais e adicione a nova role
            if (!string.IsNullOrEmpty(dto.RoleId))
            {
                // Remover todas as roles atuais
                var currentRoles = _context.UserRoles.Where(r => r.UserId == utilizador.UserID);
                _context.UserRoles.RemoveRange(currentRoles);

                // Verifica se a role existe
                var role = await _context.Roles.FindAsync(dto.RoleId);
                if (role == null)
                {
                    return BadRequest("Role não existe.");
                }

                // Adiciona a nova role
                _context.UserRoles.Add(new IdentityUserRole<string>
                {
                    UserId = utilizador.UserID,
                    RoleId = dto.RoleId
                });
            }
            else
            {
                // Se o RoleId for nulo ou vazio, o utilizador não terá roles associadas
                var currentRoles = _context.UserRoles.Where(r => r.UserId == utilizador.UserID);
                _context.UserRoles.RemoveRange(currentRoles);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UtilizadoresExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
            }
            }

            return NoContent();
        }

        // POST: api/API_Utilizadores
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Utilizadores>> PostUtilizadores(Utilizadores utilizadores)
        {
            _context.Utilizadores.Add(utilizadores);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUtilizadores", new { id = utilizadores.Id }, utilizadores);
        }

        // DELETE: api/API_Utilizadores/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUtilizadores(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            var utilizador = await _context.Utilizadores.FindAsync(id);
            if (utilizador == null)
            {
                return NotFound();
            }

            _context.Utilizadores.Remove(utilizador);

            var aspUser = await _context.Users.FindAsync(utilizador.UserID);
            if (aspUser != null)
            {
                _context.Users.Remove(aspUser);
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return NoContent();
        }


        private bool UtilizadoresExists(int id)
        {
            return _context.Utilizadores.Any(e => e.Id == id);
        }
    }
}
