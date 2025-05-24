using GP_Backend.Models;
using Microsoft.AspNetCore.Mvc;
using GP_Backend.Data;
using GP_Backend.DTOs.Import;

namespace GP_Backend.Controllers
{
    public class API_ImportController : Controller
    {

        private readonly ApplicationDbContext _context;

        public API_ImportController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("importar")]
        public async Task<IActionResult> ImportarDados([FromBody] ImportDto dadosImportacao)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                //inserir escolas
                var mapaEscolas = new Dictionary<string, Escolas>();
                foreach (var escola in dadosImportacao.ListaEscolas)
                {
                    _context.Escolas.Add(escola);
                    //o nome da escola o id temporário
                    mapaEscolas[escola.Nome] = escola;
                }
                await _context.SaveChangesAsync();

                //inserir docentes
                foreach (var docente in dadosImportacao.ListaDocentes)
                {
                    _context.Docentes.Add(docente);
                }
                await _context.SaveChangesAsync();

                //inserir cursos
                var mapaCursos = new Dictionary<string, Cursos>();
                foreach (var curso in dadosImportacao.ListaCursos)
                {
                    //só insere curso se já tiver sido feita a escola com o nome associado
                    var escolaNome = curso.Escola?.Nome;
                    if (escolaNome == null || !mapaEscolas.TryGetValue(escolaNome, out var escola))
                        return BadRequest($"Escola associada ao curso '{curso.Nome}' não encontrada.");

                    curso.EscolaFK = escola.Id;
                    curso.Escola = null; 
                    _context.Cursos.Add(curso);
                    mapaCursos[curso.Nome] = curso;
                }
                await _context.SaveChangesAsync();

                //inserir salas
                foreach (var sala in dadosImportacao.ListaSalas)
                {
                    var escolaNome = sala.Escola?.Nome;
                    if (escolaNome == null || !mapaEscolas.TryGetValue(escolaNome, out var escola))
                        return BadRequest($"Escola associada à sala '{sala.Nome}' não encontrada.");

                    sala.EscolaFK = escola.Id;
                    sala.Escola = null;
                    _context.Salas.Add(sala);
                }
                await _context.SaveChangesAsync();

                //inserir UCs
                foreach (var uc in dadosImportacao.ListaUCs)
                {
                    var novaUC = new UnidadesCurriculares
                    {
                        Nome = uc.Nome,
                        Plano = uc.Plano,
                        Semestre = uc.Semestre,
                        Ano = uc.Ano,
                        ListaCursos = new List<Cursos>()
                    };

                    //só insere se houver curso para inserir a UC
                    foreach (var cursoRef in uc.ListaCursos)
                    {
                        var nomeCurso = cursoRef.Nome;
                        if (nomeCurso == null || !mapaCursos.TryGetValue(nomeCurso, out var curso))
                            return BadRequest($"Curso '{nomeCurso}' referenciado pela UC '{uc.Nome}' não encontrado.");

                        novaUC.ListaCursos.Add(curso);
                    }

                    _context.UCs.Add(novaUC);
                }
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return Ok("Dados importados com sucesso.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, $"Erro ao importar dados: {ex.Message}");
            }
        }
    }
}
