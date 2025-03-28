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
    public class API_UnidadesCurricularesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public API_UnidadesCurricularesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/API_UnidadesCurriculares
        [HttpGet]
        public async Task<ActionResult<IEnumerable<object>>> GetUCs()
        {
            var ucs = await _context.UCs
                .Select(uc => new
                {
                    uc.Id,
                    uc.Nome,
                    uc.Plano,
                    uc.Semestre,
                    uc.Ano,
                    Cursos = uc.ListaCursos.Select(c => c.Nome).ToList() // Apenas os nomes dos cursos
                })
                .ToListAsync();

            return Ok(ucs);
        }

        // GET: api/API_UnidadesCurriculares/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UnidadesCurriculares>> GetUnidadesCurriculares(int id)
        {
            var unidadesCurriculares = await _context.UCs
                .Where(uc => uc.Id == id)
                .Select(uc => new
                {
                    uc.Id,
                    uc.Nome,
                    uc.Plano,
                    uc.Semestre,
                    uc.Ano,
                    Cursos = uc.ListaCursos.Select(c => c.Nome).ToList() // Apenas os nomes dos cursos
                })
                .FirstOrDefaultAsync();

            if (unidadesCurriculares == null)
            {
                return NotFound();
            }

            return Ok(unidadesCurriculares);
        }

        // PUT: api/API_UnidadesCurriculares/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUnidadesCurriculares(int id, UnidadesCurriculares unidadesCurriculares)
        {
            if (id != unidadesCurriculares.Id)
            {
                return BadRequest();
            }

            _context.Entry(unidadesCurriculares).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UnidadesCurricularesExists(id))
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

        // POST: api/API_UnidadesCurriculares
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UnidadesCurriculares>> PostUnidadesCurriculares(UnidadesCurriculares unidadesCurriculares)
        {
            _context.UCs.Add(unidadesCurriculares);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUnidadesCurriculares", new { id = unidadesCurriculares.Id }, unidadesCurriculares);
        }

        // DELETE: api/API_UnidadesCurriculares/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUnidadesCurriculares(int id)
        {
            var unidadesCurriculares = await _context.UCs.FindAsync(id);
            if (unidadesCurriculares == null)
            {
                return NotFound();
            }

            _context.UCs.Remove(unidadesCurriculares);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UnidadesCurricularesExists(int id)
        {
            return _context.UCs.Any(e => e.Id == id);
        }
    }
}