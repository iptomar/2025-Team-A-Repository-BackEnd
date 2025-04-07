using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GP_Backend.Data;
using GP_Backend.Models;
using Microsoft.AspNetCore.Hosting;

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

        // Criar uma Unidade Curricular
        // POST: api/API_UnidadesCurriculares
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UnidadesCurriculares unidadeCurricular)
        {
            bool haErros = false;

            if (unidadeCurricular.ListaCursos.Count() == 0)
            {
                // Verifica se nenhum curso foi selecionado
                haErros = true;
                return BadRequest(new { erro = "Escolha pelo menos um curso, por favor." });
            }

            if (ModelState.IsValid && !haErros)
            {
                try
                {
                    // Lista de cursos válidos associados à UC
                    var listaCursosNaUC = new List<Cursos>();
                    foreach (var curso in unidadeCurricular.ListaCursos)
                    {
                        var c = await _context.Cursos.FirstOrDefaultAsync(m => m.CodCurso == curso.CodCurso);
                        if (c != null)
                        {
                            listaCursosNaUC.Add(c);
                        }
                    }
                    unidadeCurricular.ListaCursos = listaCursosNaUC;

                    // Adiciona a UC ao banco de dados
                    _context.Add(unidadeCurricular);
                    await _context.SaveChangesAsync();  // Salva a UC

                    return Ok(new { message = "Criada com sucesso", id = unidadeCurricular.Id });
                    // Retorna a UC criada com sucesso
                }
                catch (Exception ex)
                {
                    // Em caso de erro crítico, retornar erro com detalhes
                    return BadRequest(new { erro = ex.Message });
                }
            }

            // Se o ModelState não for válido ou houver erro
            return BadRequest(new { erro = "Dados inválidos" });
        }


        // Editar uma Unidade Curricular
        // PUT: api/API_UnidadesCurriculares/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754        
        [HttpPut("{id}")]
        public async Task<IActionResult> Edit(int id, [FromBody] UnidadesCurriculares body)
        {

            var unidadecurricular = await _context.UCs
                .Include(u => u.ListaCursos)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (unidadecurricular == null)
            {
                return NotFound(new { erro = "Unidade Curricular não encontrada" });
            }

            try
            {
                unidadecurricular.Ano = body.Ano;
                unidadecurricular.Plano = body.Plano;
                unidadecurricular.Semestre = body.Semestre;
                unidadecurricular.Nome = body.Nome;

                // Atualizar os cursos associados
                unidadecurricular.ListaCursos.Clear();
                foreach (var curso in body.ListaCursos)
                {
                    var cursoExistente = await _context.Cursos.FindAsync(curso.CodCurso);
                    if (cursoExistente != null)
                    {
                        unidadecurricular.ListaCursos.Add(cursoExistente);
                    }
                }

                _context.Update(unidadecurricular);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Editada com sucesso", id });
            }
            catch (Exception ex)
            {
                // Em caso de erro crítico, retornar erro com detalhes
                return BadRequest(new { erro = ex.Message });
            }
        }


        // Apagar uma Unidade Curricular
        // DELETE: api/API_UnidadesCurriculares/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // procura a unidade curricular pelo id
            var unidadecurricular = await _context.UCs.FindAsync(id);
            
            // caso a unidade curricular não seja encontrada
            if (unidadecurricular == null)
            {
                return NotFound();
            }

            // remove a unidade curricular da BD
            _context.UCs.Remove(unidadecurricular);
            // efetua COMMIT na BD
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UnidadesCurricularesExists(int id)
        {
            return _context.UCs.Any(e => e.Id == id);
        }
    }
}