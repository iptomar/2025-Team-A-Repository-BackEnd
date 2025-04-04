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
    public class API_DocentesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public API_DocentesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Docentes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Docentes>>> GetDocentes()
        {
            return await _context.Docentes.ToListAsync();
        }

        // GET: api/Docentes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Docentes>> GetDocentes(int id)
        {
            var docentes = await _context.Docentes.FindAsync(id);

            if (docentes == null)
            {
                return NotFound();
            }

            return docentes;
        }

        // PUT: api/Docentes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDocentes(int id)
        {
            if (!DocentesExists(id))
            {
                return NotFound();
            }
            using (var reader = new StreamReader(Request.Body))
            {
                var body = await reader.ReadToEndAsync();
                dynamic data = JsonConvert.DeserializeObject(body);

                if (data == null || data.nome == null || data.email == null || data.id == null)
                {
                    return BadRequest("Não forneceu todos os parâmetros");
                }
                var docente = new Docentes
                {
                    Id = (int)data.id,
                    Nome = (string)data.nome,
                    Email = (string)data.email
                };

                if (id != docente.Id)
                {
                    return NotFound();
                }

                try
                {
                    _context.Update(docente);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {

                    throw;

                }

                return Ok(docente);
            }
            
        }

        // POST: api/Docentes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> PostDocentes()
        {
            try
            {
                using (var reader = new StreamReader(Request.Body))
                {
                    var body = await reader.ReadToEndAsync();
                    dynamic data = JsonConvert.DeserializeObject(body);

                    if (data == null)
                    {
                        return BadRequest("Json Inválido, não enviou o nome e o email do docente");
                    }
                    if (data.nome == null)
                    {
                        return BadRequest("Json Inválido, não enviou o nome do docente");
                    }
                    if (data.email == null)
                    {
                        return BadRequest("Json Inválido, não enviou o email do docente");
                    }

                    var docente = new Docentes
                    {
                        Nome = (string)data.nome,
                        Email = (string)data.email,
                    };

                    _context.Docentes.Add(docente);
                    await _context.SaveChangesAsync();

                    return Ok(docente);
                }
            }
            catch
            {
                return BadRequest();
            }
           
        }

        // DELETE: api/Docentes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDocentes(int id)
        {
            var docentes = await _context.Docentes.FindAsync(id);
            if (docentes == null)
            {
                return NotFound();
            }

            _context.Docentes.Remove(docentes);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DocentesExists(int id)
        {
            return _context.Docentes.Any(e => e.Id == id);
        }
    }
}
