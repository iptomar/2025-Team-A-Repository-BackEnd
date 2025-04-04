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
    public class API_TurmasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public API_TurmasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/API_Turmas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Turmas>>> GetTurmas()
        {
            return await _context.Turmas.Include(t => t.Curso).ToListAsync();
        }

        // GET: api/API_Turmas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Turmas>> GetTurmas(int id)
        {
            var turmas = await _context.Turmas.Include(t => t.Curso).FirstOrDefaultAsync(t => t.Id == id);

            if (turmas == null)
            {
                return NotFound();
            }

            return turmas;
        }

        // PUT: api/API_Turmas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTurmas(int id, Turmas turmas)
        {
            if(TurmasExists(id) == false)
            {
                return NotFound();
            }

            if(turmas == null) 
            { 
                return BadRequest("O campo turma não pode estar vazio.");
            }

            if (string.IsNullOrWhiteSpace(turmas.Nome))
            {
                return BadRequest("O campo nome não pode estar vazio.");
            }

            if (string.IsNullOrWhiteSpace(turmas.AnoLetivo))
            {
                return BadRequest("O campo ano letivo não pode estar vazio.");
            }

            if (turmas.CursoFK == 0)
            {
                return BadRequest("O campo curso não pode estar vazio.");
            }

            if(turmas.CursoFK != 0)
            {
                var curso = await _context.Cursos.FindAsync(turmas.CursoFK);
                if (curso == null)
                {
                    return BadRequest("O curso não existe.");
                }
            }

            if (turmas.Id != id)
            {
                return BadRequest("O ID da turma não corresponde ao ID fornecido na URL.");
            }

            var existingTurma = await _context.Turmas
                .FirstOrDefaultAsync(t => t.Nome == turmas.Nome && t.AnoLetivo == turmas.AnoLetivo && t.CursoFK == turmas.CursoFK && t.Id != id);
            if (existingTurma != null)
            {
                return Conflict("A turma já existe.");
            }

            _context.Entry(turmas).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TurmasExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(turmas);
        }

        // POST: api/API_Turmas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Turmas>> PostTurmas(Turmas turmas)
        {

            if(turmas == null)
            {
                return BadRequest("O campo turma não pode estar vazio.");
            }

            if (string.IsNullOrWhiteSpace(turmas.Nome))
            {
                return BadRequest("O campo nome não pode estar vazio.");
            }

            if (string.IsNullOrWhiteSpace(turmas.AnoLetivo))
            {
                return BadRequest("O campo ano letivo não pode estar vazio.");
            }

            if (turmas.CursoFK == 0)
            {
                return BadRequest("O campo curso não pode estar vazio.");
            }

            if(turmas.CursoFK != 0)
            {
                var curso = await _context.Cursos.FindAsync(turmas.CursoFK);
                if (curso == null)
                {
                    return BadRequest("O curso não existe.");
                }
            }

            var existingTurma = await _context.Turmas
                .FirstOrDefaultAsync(t => t.Nome == turmas.Nome && t.AnoLetivo == turmas.AnoLetivo && t.CursoFK == turmas.CursoFK);
            if (existingTurma != null)
            {
                return Conflict("A turma já existe.");
            }

            _context.Turmas.Add(turmas);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTurmas", new { id = turmas.Id }, turmas);
        }

        // DELETE: api/API_Turmas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTurmas(int id)
        {
            var turmas = await _context.Turmas.FindAsync(id);
            if (turmas == null)
            {
                return NotFound();
            }

            _context.Turmas.Remove(turmas);
            await _context.SaveChangesAsync();

            return Ok(turmas);
        }

        private bool TurmasExists(int id)
        {
            return _context.Turmas.Any(e => e.Id == id);
        }
    }
}
