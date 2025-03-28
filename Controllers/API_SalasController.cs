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
    public class API_SalasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public API_SalasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/API_Salas
        [HttpGet]
		public async Task<ActionResult<IEnumerable<object>>> GetSalas()
		{
			var salas = await _context.Salas
				.Include(s => s.Escola)
				.Select(s => new
				{
					s.Id,
					s.Nome,
					s.EscolaFK,
					Escola = s.Escola != null ? s.Escola.Nome : null
				})
				.ToListAsync();

			return Ok(salas);
		}

		// GET: api/API_Salas/5
		[HttpGet("{id}")]
		public async Task<ActionResult<object>> GetSala(int id)
		{
			var sala = await _context.Salas
				.Include(s => s.Escola)
				.Where(s => s.Id == id)
				.Select(s => new
				{
					s.Id,
					s.Nome,
					s.EscolaFK,
					Escola = s.Escola != null ? s.Escola.Nome : null
				})
				.FirstOrDefaultAsync();

			if (sala == null)
			{
				return NotFound();
			}

			return Ok(sala);
		}


		// PUT: api/API_Salas/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		[HttpPut("{id}")]
        public async Task<IActionResult> PutSalas(int id, Salas salas)
        {
            if (id != salas.Id)
            {
                return BadRequest();
            }

            _context.Entry(salas).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SalasExists(id))
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

        // POST: api/API_Salas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Salas>> PostSalas(Salas salas)
        {
            _context.Salas.Add(salas);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSalas", new { id = salas.Id }, salas);
        }

        // DELETE: api/API_Salas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSalas(int id)
        {
            var salas = await _context.Salas.FindAsync(id);
            if (salas == null)
            {
                return NotFound();
            }

            _context.Salas.Remove(salas);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SalasExists(int id)
        {
            return _context.Salas.Any(e => e.Id == id);
        }
    }
}
