using GP_Backend.Models;
using Microsoft.AspNetCore.Mvc;
using GP_Backend.Data;
using GP_Backend.DTOs.Import;
using Microsoft.AspNetCore.OutputCaching;
using static GP_Backend.Controllers.API_ManchasHorariasController;

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
            //inicia transação
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                //inserir a lista de escolas

                //cria um array com os nomes das escolas inseridas
                var listaEscolasCriadas = new List<Escolas>();

                foreach (var escolaDto in dadosImportacao.ListaEscolas)
                {
                    //cria objeto do tipo escola
                    var escola = new Escolas();

                    //adiciona os parâmetros
                    escola.Nome = escolaDto.Nome;

                    //adiciona à bd
                    _context.Escolas.Add(escola);

                    //adiciona o nome da escola ao array
                    listaEscolasCriadas.Add(escola);
                }
                await _context.SaveChangesAsync();

                //inserir docentes
                foreach (var docenteDto in dadosImportacao.ListaDocentes)
                {
                    //cria objeto do tipo Docente
                    var docente = new Docentes();

                    //adiciona os parâmetros
                    docente.Nome = docenteDto.Nome;
                    docente.Email = docenteDto.Email;

                    _context.Docentes.Add(docente);
                }
                await _context.SaveChangesAsync();

                //inserir cursos

                //cria um array com os nomes dos cursos inseridos
                var listaCursosCriados = new List<Cursos>();

                foreach (var cursoDto in dadosImportacao.ListaCursos)
                {
                    //só insere curso se já tiver sido feita a escola com o nome associado

                    //tenta ir buscar o nome da escola no curso
                    var escolaNome = cursoDto.NomeEscola;

                    //se este for null ou não coincidir com nenhuma escola inserida, dá erro
                    if (escolaNome == null || !listaEscolasCriadas.Any(e => e.Nome == escolaNome))
                        return BadRequest($"Escola associada ao curso '{cursoDto.Nome}' não encontrada.");

                    //cria o objeto escola com base na escola associada ao curso
                    var escola = listaEscolasCriadas.FirstOrDefault(e => e.Nome == escolaNome);

                    //cria o objeto curso com base no cursoDto
                    var curso = new Cursos();

                    //adiciona os parâmetros
                    curso.Nome = cursoDto.Nome;
                    curso.Escola = escola;
                    curso.EscolaFK = escola.Id;

                    //adiciona o curso à bd
                    _context.Cursos.Add(curso);

                    //adiciona o curso à lista de cursos criados
                    listaCursosCriados.Add(curso);
                }
                await _context.SaveChangesAsync();

                //inserir salas
                foreach (var salaDto in dadosImportacao.ListaSalas)
                {
                    var escolaNome = salaDto.NomeEscola;
                    if (escolaNome == null || !listaEscolasCriadas.Any(e => e.Nome == escolaNome))
                        return BadRequest($"Escola associada à sala '{salaDto.Nome}' não encontrada.");

                    //cria o objeto escola com base na escola associada à sala
                    var escola = listaEscolasCriadas.FirstOrDefault(e => e.Nome == escolaNome);

                    //cria o objeto curso com base no salaDto
                    var sala = new Salas();

                    //adiciona os parâmetros
                    sala.Nome = salaDto.Nome;
                    sala.Escola = escola;
                    sala.EscolaFK = escola.Id;

                    //adiciona o curso à bd
                    _context.Salas.Add(sala);
                }
                await _context.SaveChangesAsync();

                //inserir UCs
                foreach (var ucDto in dadosImportacao.ListaUCs)
                {
                    //cria um objeto com os parâmetros
                    var novaUC = new UnidadesCurriculares
                    {
                        Nome = ucDto.Nome,
                        Plano = ucDto.Plano,
                        Semestre = ucDto.Semestre,
                        Ano = ucDto.Ano,
                        //cria uma lista de cursos ainda vazia
                        ListaCursos = new List<Cursos>()
                    };

                    //só insere se houver curso para inserir a UC
                    foreach (var nomeCurso in ucDto.ListaCursos)
                    {
                        //procura os cursos na lista de nomes de curso na ucDto
                        if (nomeCurso == null || !listaCursosCriados.Any(e => e.Nome == nomeCurso))
                            return BadRequest($"Curso '{nomeCurso}' referenciado pela UC '{ucDto.Nome}' não encontrado.");

                        //cria um novo objeto do tipo Curso com base no nome do curso
                        var curso = listaCursosCriados.FirstOrDefault(e => e.Nome == nomeCurso);

                        //adiciona à lista de cursos
                        novaUC.ListaCursos.Add(curso);
                    }

                    //adiciona à bd
                    _context.UCs.Add(novaUC);
                }
                await _context.SaveChangesAsync();

                //faz commit de todas as mudanças
                await transaction.CommitAsync();
                return Ok("Dados importados com sucesso.");
            }
            catch (Exception ex)
            {
                //caso alguma tenha dado erro, descarta todas as mudanças e retorna ao ponto inicial
                await transaction.RollbackAsync();
                return StatusCode(500, $"Erro ao importar dados: {ex.Message}");
            }
        }
    }
}
