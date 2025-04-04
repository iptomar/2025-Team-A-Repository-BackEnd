using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GP_Backend.Data;
using GP_Backend.Models;

namespace GP_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class API_CursosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public API_CursosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/API_Cursos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cursos>>> GetCursos()
        {
            var cursos = await _context.Cursos
                .Include(c => c.Escola)  // Carregar a entidade Escola associada ao curso
                .Include(c => c.ListaUCs) // Carregar a lista de Unidades Curriculares associadas ao curso
                .ToListAsync();

            // Mapear os cursos para retornar apenas as informações necessárias
            var cursosComDetalhes = cursos.Select(c => new
            {
                c.CodCurso,
                c.Nome,
                Escola = new { c.Escola.Nome, c.Escola.Id },  // Nome da Escola
                ListaUcs = c.ListaUCs.Select(uc => uc.Nome).ToList() // Lista de nomes das Unidades Curriculares
            });

            return Ok(cursosComDetalhes);
        }

        // GET: api/API_Cursos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Cursos>> GetCursos(int id)
        {
            var cursos = await _context.Cursos.FindAsync(id);

            if (cursos == null)
            {
                return NotFound();
            }

            return cursos;
        }

        // PUT: api/API_Cursos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCursos(int id, Cursos cursos)
        {
            if (id != cursos.CodCurso)
            {
                return BadRequest();
            }

            _context.Entry(cursos).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CursosExists(id))
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

        // POST: api/API_Cursos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Cursos>> PostCursos(Cursos cursos)
        {
            _context.Cursos.Add(cursos);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCursos", new { id = cursos.CodCurso }, cursos);
        }

        // DELETE: api/API_Cursos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCursos(int id)
        {
            var cursos = await _context.Cursos.FindAsync(id);
            if (cursos == null)
            {
                return NotFound();
            }

            _context.Cursos.Remove(cursos);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CursosExists(int id)
        {
            return _context.Cursos.Any(e => e.CodCurso == id);
        }
    }
}
