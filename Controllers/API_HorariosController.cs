using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GP_Backend.Data;
using GP_Backend.Models;
using Newtonsoft.Json;

namespace GP_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class API_HorariosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public class HorarioDto
        {
            public int Id { get; set; }
            public string AnoLetivo { get; set; }
            public string Semestre { get; set; }
            public int TurmaId { get; set; }
            public string NomeTurma { get; set; }
            public string AnoCurso { get; set; }
            public string TurmaCurso { get; set; }
        }


        public API_HorariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/API_Horarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Horarios>>> GetHorarios()
        {
            var horariosComRelacionamentos = await _context.Horarios
               .Include(h => h.Turma)
               .Select(h => new HorarioDto
               {
                   Id = h.Id,
                   AnoLetivo = h.AnoLetivo,
                   Semestre = h.Semestre,
                   TurmaId = h.TurmaFK,
                   NomeTurma = h.Turma.Nome,
                   AnoCurso = h.Turma.AnoCurso,
                   TurmaCurso = h.Turma.Curso.Nome,
               })
               .ToListAsync();

            return Ok(horariosComRelacionamentos);
            //return await _context.Horarios.ToListAsync();
        }

        // GET: api/API_Horarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Horarios>> GetHorarios(int id)
        {
            var horario = await _context.Horarios.FindAsync(id);

            if (horario == null)
            {
                return NotFound();
            }

            return horario;
        }

        // PUT: api/API_Horarios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHorarios(int id, Horarios horarios)
        {
            if (id != horarios.Id)
            {
                return BadRequest();
            }

            _context.Entry(horarios).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HorariosExists(id))
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

        // POST: api/API_Horarios
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> PostHorarios()
        {
            try
            {
                using (var reader = new StreamReader(Request.Body))
                {
                    var body = await reader.ReadToEndAsync();
                    dynamic data = JsonConvert.DeserializeObject(body);

                    if (data == null)
                    {
                        return BadRequest("Json Inválido, não enviou os dados");
                    }
                    if (data.anoLetivo == null)
                    {
                        return BadRequest("Json Inválido, não enviou o ano letivo");
                    }
                    if (data.semestre == null)
                    {
                        return BadRequest("Json Inválido, não enviou o semestre");
                    }
                    if (data.turmaFK == null)
                    {
                        return BadRequest("Json Inválido, não enviou a Turma");
                    }

                    var horario = new Horarios
                    {
                        AnoLetivo = (string)data.anoLetivo,
                        Semestre = (string)data.semestre,
                        TurmaFK = (int)data.turmaFK
                    };

                    if (ModelState.IsValid)
                    {

                        _context.Add(horario);
                        await _context.SaveChangesAsync();

                        return Ok(horario);
                    }

                    return Ok(horario);
                }
            }
            catch(Exception ex) 
            {
                return BadRequest(new { erro = ex.Message });
            }
        }

        // DELETE: api/API_Horarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHorarios(int id)
        {
            var horarios = await _context.Horarios.FindAsync(id);
            if (horarios == null)
            {
                return NotFound();
            }

            _context.Horarios.Remove(horarios);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HorariosExists(int id)
        {
            return _context.Horarios.Any(e => e.Id == id);
        }
    }
}
