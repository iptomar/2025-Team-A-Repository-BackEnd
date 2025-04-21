using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GP_Backend.Data;
using GP_Backend.Models;
using GP_Backend.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace GP_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class API_ManchasHorariasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<HorarioHub> _hubContext;
        public class ManchaHorariaUpdateHoraDTO
        {
            public TimeOnly HoraInicio { get; set; }
            public DateOnly Dia { get; set; }
        }
        public class ManchaHorariaDto
        {
            public string TipoDeAula { get; set; }
            public int NumSlots { get; set; }
            public int DocenteFK { get; set; }
            public int SalaFK { get; set; }
            public int UCFK { get; set; }
        }
        public API_ManchasHorariasController(ApplicationDbContext context,
            IHubContext<HorarioHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

            // GET: api/API_ManchasHorarias
            [HttpGet]
        public async Task<ActionResult<IEnumerable<ManchasHorarias>>> GetManchasHorarias()
        {
            var manchasComRelacionamentos = await _context.ManchasHorarias
                .Include(m => m.Docente)
                .Include(m => m.Sala)
                .Include(m => m.UC)
                .ToListAsync();

            return Ok(manchasComRelacionamentos);
            // return await _context.ManchasHorarias.ToListAsync();
        }

        // GET: api/API_ManchasHorarias/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ManchasHorarias>> GetManchasHorarias(int id)
        {
            var manchaHoraria = await _context.ManchasHorarias
                .Include(m => m.Docente)
                .Include(m => m.Sala)
                .Include(m => m.UC)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (manchaHoraria == null)
            {
                return NotFound();
            }

            return manchaHoraria;
        }

        // PUT: api/API_ManchasHorarias/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutManchasHorarias(int id, [FromBody] ManchaHorariaDto body)
        {
            var manchaHoraria = await _context.ManchasHorarias
                .FirstOrDefaultAsync(m => m.Id == id);

            if (manchaHoraria == null)
            {
                return NotFound(new { erro = "Mancha Horária não encontrada" });
            }


            try
            {
                manchaHoraria.TipoDeAula = body.TipoDeAula;
                manchaHoraria.NumSlots = body.NumSlots;
                manchaHoraria.DocenteFK = body.DocenteFK;
                manchaHoraria.SalaFK = body.SalaFK;
                manchaHoraria.UCFK = body.UCFK;

                _context.Update(manchaHoraria);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Editada com sucesso", id });
            }
            catch (Exception ex)
            {
                // Em caso de erro crítico, retornar erro com detalhes
                return BadRequest(new { erro = ex.Message });
            }
           
        }


        [HttpPut]
        [Route("drag-bloco/{id}")]
        public async Task<IActionResult> PutHoursManchasHorarias(int id, [FromBody] ManchaHorariaUpdateHoraDTO update)
        {
            var mancha = await _context.ManchasHorarias.FindAsync(id);

            if (mancha == null)
            {
                return NotFound("A mancha horária não foi encontrada.");
            }

            mancha.HoraInicio = update.HoraInicio;
            mancha.Dia = update.Dia;

            await _context.SaveChangesAsync();

            await _hubContext.Clients.All.SendAsync("AulaAtualizada", new
            {
                id = mancha.Id,
                horaInicio = update.HoraInicio.ToString(),
                dia = update.Dia.ToString()
            });

            return Ok(mancha);
        }

        // POST: api/API_ManchasHorarias
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> PostManchasHorarias([FromForm] string tipoAula, [FromForm] int numSlots, [FromForm] TimeOnly horaInicio, [FromForm] DateOnly diaSemana, [FromForm] int docenteFK, [FromForm] int salaFK, [FromForm] int ucFK)
        {
            // Verifificar se os campos obrigatórios estão preenchidos
            if (tipoAula == null || numSlots == 0 || docenteFK <= 0 || salaFK <= 0 || ucFK <= 0)
            {
                return BadRequest("Preencha os campos corretamente!");
            }

            var manchaHoraria = new ManchasHorarias
            {
                TipoDeAula = tipoAula,
                HoraInicio = horaInicio,
                Dia = diaSemana,
                NumSlots = numSlots,
                DocenteFK = docenteFK,
                SalaFK = salaFK,
                UCFK = ucFK,
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
        [HttpGet("sala/{idSala}")]
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
        [HttpGet("docente/{idDocente}")]
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
        [HttpGet("uc/{idUC}")]
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
