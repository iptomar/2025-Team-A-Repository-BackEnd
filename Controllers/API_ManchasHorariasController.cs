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
using Microsoft.IdentityModel.Tokens;

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

            public List<int> HorariosIds { get; set; }
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
                //.Include(m => m.ListaHorarios)
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
        public async Task<IActionResult> PostManchasHorarias([FromBody] ManchaHorariaDto dto)
        {
            if (dto == null)
                return BadRequest(new { erro = "Dados não enviados." });

            bool haErros = false;

            // Validações básicas
            if (string.IsNullOrWhiteSpace(dto.TipoDeAula))
            {
                haErros = true;
                return BadRequest(new { erro = "Tipo de aula é obrigatório." });
            }

            if (dto.NumSlots <= 0)
            {
                haErros = true;
                return BadRequest(new { erro = "Número de slots deve ser maior que zero." });
            }

            if (dto.DocenteFK <= 0 || dto.SalaFK <= 0 || dto.UCFK <= 0)
            {
                haErros = true;
                return BadRequest(new { erro = "Docente, sala ou UC inválidos." });
            }

            if (dto.HorariosIds == null || !dto.HorariosIds.Any())
            {
                haErros = true;
                return BadRequest(new { erro = "Deve associar pelo menos um horário." });
            }

            if (ModelState.IsValid && !haErros)
            {
                try
                {
                    // Buscar os horários válidos
                    var horarios = await _context.Horarios
                        .Where(h => dto.HorariosIds.Contains(h.Id))
                        .ToListAsync();

                    if (horarios.Count != dto.HorariosIds.Count)
                    {
                        return BadRequest(new { erro = "Um ou mais IDs de horários são inválidos." });
                    }

                    // Criação da nova mancha
                    var novaMancha = new ManchasHorarias
                    {
                        TipoDeAula = dto.TipoDeAula,
                        NumSlots = dto.NumSlots,
                        DocenteFK = dto.DocenteFK,
                        SalaFK = dto.SalaFK,
                        UCFK = dto.UCFK,
                        //HoraInicio = dto.HoraInicio ?? new TimeOnly(0, 0),
                       // Dia = dto.DiaSemana ?? DateOnly.FromDateTime(DateTime.Today),
                        ListaHorarios = horarios
                    };

                    _context.ManchasHorarias.Add(novaMancha);
                    await _context.SaveChangesAsync();

                    return Ok(new { message = "Mancha horária criada com sucesso", id = novaMancha.Id });
                }
                catch (Exception ex)
                {
                    return StatusCode(500, new
                    {
                        erro = "Erro ao criar a mancha horária.",
                        detalhes = ex.InnerException?.Message ?? ex.Message
                    });
                }
            }

            return BadRequest(new { erro = "Dados inválidos." });
        }
        // DELETE: api/API_ManchasHorarias/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteManchasHorarias(int id)
        {
            /*var manchasHorarias = await _context.ManchasHorarias.FindAsync(id);
            if (manchasHorarias == null)
            {
                return NotFound();
            }

            _context.ManchasHorarias.Remove(manchasHorarias);
            await _context.SaveChangesAsync();

            return NoContent();*/

            /* var manchasHorarias = await _context.ManchasHorarias
     .Include(m => m.ListaHorarios) // Inclui a lista de horários para verificar se há dependências
     .FirstOrDefaultAsync(m => m.Id == id);

             if (manchasHorarias == null)
             {
                 return NotFound();
             }

             // Remover dependências (se houver)
             if (manchasHorarias.ListaHorarios != null && manchasHorarias.ListaHorarios.Any())
             {
                 _context.Horarios.RemoveRange(manchasHorarias.ListaHorarios);
             }

             _context.ManchasHorarias.Remove(manchasHorarias);
             await _context.SaveChangesAsync();

             return NoContent();*/


            // Encontrar a mancha horária e os horários associados
            var manchasHorarias = await _context.ManchasHorarias
                .Include(m => m.ListaHorarios)  // Carrega os horários associados à mancha
                .FirstOrDefaultAsync(m => m.Id == id);

            if (manchasHorarias == null)
            {
                return NotFound();
            }

            try
            {
                // Remover os horários associados
                if (manchasHorarias.ListaHorarios != null && manchasHorarias.ListaHorarios.Any())
                {
                    // Desassociar os horários da mancha horária sem remover fisicamente
                    manchasHorarias.ListaHorarios.Clear();
                }

                // Deletar a mancha horária
                _context.ManchasHorarias.Remove(manchasHorarias);
                await _context.SaveChangesAsync();

                return NoContent();  // Deletado com sucesso
            }
            catch (Exception ex)
            {
                // Captura erro e retorna mensagem
                return StatusCode(500, new { erro = "Erro ao tentar excluir a mancha horária.", detalhes = ex.Message });
            }
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
        [HttpGet("/Sala/{idSala}")]
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
        [HttpGet("/Docente/{idDocente}")]
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
        [HttpGet("/UC/{idUC}")]
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
