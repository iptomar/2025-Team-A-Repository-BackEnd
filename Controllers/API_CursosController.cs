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
            var curso = await _context.Cursos
                .Include(c => c.Escola)  // Carregar a entidade Escola associada ao curso
                .Include(c => c.ListaUCs) // Carregar a lista de Unidades Curriculares associadas ao curso
                .FirstOrDefaultAsync(c => c.CodCurso == id); // Buscar pelo id do curso

            if (curso == null)
            {
                return NotFound();
            }

            // Retornar todos os detalhes do curso
            var cursoComDetalhes = new
            {
                curso.CodCurso,
                curso.Nome,
                Escola = new { curso.Escola.Nome, curso.Escola.Id },  // Nome da Escola
                ListaUcs = curso.ListaUCs.Select(uc => new { uc.Nome, uc.Id }).ToList() // Detalhes das Unidades Curriculares (Nome e id)
            };

            return Ok(cursoComDetalhes);
        }

        // PUT: api/API_Cursos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCursos(int id, Cursos curso)
        {
            if (id != curso.CodCurso)
            {
                return BadRequest();
            }

            _context.Entry(curso).State = EntityState.Modified;

            try
            {
                curso.Escola = await _context.Escolas.FindAsync(curso.Escola.Id); // Carrega a Escola associada ao curso

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

        // Criar um Curso
        // POST: api/API_Cursos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Cursos curso)
        {
            // Não é necessário atribuir um valor ao Id ou CodCurso
            if (ModelState.IsValid)
            {
                try
                {
                    curso.Escola = await _context.Escolas.FindAsync(curso.Escola.Id); // Carrega a Escola associada ao curso                    

                    // Adiciona o curso ao banco de dados sem incluir o campo de identidade
                    _context.Add(curso);
                    await _context.SaveChangesAsync();  // Salva o curso

                    return Ok(new { message = "Curso criado com sucesso", id = curso.CodCurso });
                }
                catch (Exception ex)
                {
                    return BadRequest(new { erro = ex.Message });
                }
            }

            return BadRequest(new { erro = "Dados inválidos" });
        }

        // Apagar um curso
        // DELETE: api/API_Cursos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCursos(int id)
        {
            // procura o curso pelo id
            var cursos = await _context.Cursos.FindAsync(id);
            
            // caso o curso não seja encontrado
            if (cursos == null)
            {
                return NotFound();
            }

            // remove o curso da BD
            _context.Cursos.Remove(cursos);
            // efetua COMMIT na BD
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CursosExists(int id)
        {
            return _context.Cursos.Any(e => e.CodCurso == id);
        }
    }
}
