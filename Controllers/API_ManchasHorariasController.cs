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
    public class API_ManchasHorariasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public API_ManchasHorariasController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/API_ManchasHorarias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ManchasHorarias>>> GetManchasHorarias()
        {
            return await _context.ManchasHorarias.ToListAsync();
        }

        // GET: api/API_ManchasHorarias/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ManchasHorarias>> GetManchasHorarias(int id)
        {
            var manchasHorarias = await _context.ManchasHorarias.FindAsync(id);

            if (manchasHorarias == null)
            {
                return NotFound();
            }

            return manchasHorarias;
        }

        // PUT: api/API_ManchasHorarias/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutManchasHorarias(int id, ManchasHorarias manchasHorarias)
        {
            if (id != manchasHorarias.Id)
            {
                return BadRequest();
            }

            _context.Entry(manchasHorarias).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ManchasHorariasExists(id))
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

        // POST: api/API_ManchasHorarias
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> PostManchasHorarias([FromForm] string tipoAula, [FromForm] int numSlots, [FromForm] int docenteFK, [FromForm] int salaFK, [FromForm] int ucFK)
        {
            // Verifificar se os campos obrigatórios estão preenchidos
            if (tipoAula == null || numSlots == 0 || docenteFK <= 0 || salaFK <= 0 || ucFK <= 0)
            {
                return BadRequest("Preencha os campos corretamente!");
            }

            var manchaHoraria = new ManchasHorarias
            {
                TipoDeAula = tipoAula,
                NumSlots = numSlots,
                DocenteFK = docenteFK,
                SalaFK = salaFK,
                UCFK = ucFK
            };

            if (ModelState.IsValid)
            {

                _context.Add(manchaHoraria);
                await _context.SaveChangesAsync();
                return Ok(manchaHoraria);
            }

            return BadRequest("Não deu!");


        }

        // DELETE: api/API_ManchasHorarias/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteManchasHorarias(int id)
        {
            var manchasHorarias = await _context.ManchasHorarias.FindAsync(id);
            if (manchasHorarias == null)
            {
                return NotFound();
            }

            _context.ManchasHorarias.Remove(manchasHorarias);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ManchasHorariasExists(int id)
        {
            return _context.ManchasHorarias.Any(e => e.Id == id);
        }

        /// <summary>
        /// Endpoint para efetuar um GET passando o id da Sala como parametro
        /// </summary>
        /// <param name="idSala"></param>
        /// <returns></returns>
        // GET: api/API_ManchasHorarias/5
        [HttpGet("{idSala}")]
        public async Task<IActionResult> GetManchasHorariasPorSala(int idSala)
        {
            var manchasHorarias = await _context.ManchasHorarias
                .Where(m => m.SalaFK == idSala)
                .ToListAsync();

            if (manchasHorarias == null)
            {
                return NotFound();
            }

            return Ok(manchasHorarias);
        }

        /// <summary>
        /// Endpoint para efetuar um GET passando o id do Docente como parametro
        /// </summary>
        /// <param name="idDocente"></param>
        /// <returns></returns>
        // GET: api/API_ManchasHorarias/5
        [HttpGet("{idDocente}")]
        public async Task<IActionResult> GetManchasHorariasPorDocente(int idDocente)
        {
            var manchasHorarias = await _context.ManchasHorarias
                .Where(m => m.DocenteFK == idDocente)
                .ToListAsync();

            if (manchasHorarias == null)
            {
                return NotFound();
            }

            return Ok(manchasHorarias);
        }

        /// <summary>
        /// Endpoint para efetuar um GET passando o id da Unidade Curricular como parametro
        /// </summary>
        /// <param name="idDocente"></param>
        /// <returns></returns>
        // GET: api/API_ManchasHorarias/5
        [HttpGet("{idDocente}")]
        public async Task<IActionResult> GetManchasHorariasPorUC(int idUC)
        {
            var manchasHorarias = await _context.ManchasHorarias
                .Where(m => m.UCFK == idUC)
                .ToListAsync();

            if (manchasHorarias == null)
            {
                return NotFound();
            }

            return Ok(manchasHorarias);
        }
    }
}
