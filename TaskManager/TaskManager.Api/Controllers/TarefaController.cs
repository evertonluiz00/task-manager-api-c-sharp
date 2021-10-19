using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class TarefaController : BaseController
    {
        private readonly ILogger<TarefaController> _logger;
        private readonly ITarefaRepository _tarefaRepository;

        public TarefaController(ILogger<TarefaController> logger, IUsuarioRepository usuarioRepository, ITarefaRepository tarefaRepository) : base(usuarioRepository)
        {
            _logger = logger;
            _tarefaRepository = tarefaRepository;
        }


        [HttpPost]
        public IActionResult AdicionarTarefa([FromBody] Tarefa tarefa)
        {
            try
            {
                var usuario = ReadToken();
                var erros = new List<string>();

                if (tarefa == null)
                {
                    erros.Add("Favor informar a tarefa ou usuário");
                }
                else
                {
                    if (string.IsNullOrEmpty(tarefa.Nome.Trim()) || tarefa.Nome.Count() < 4)
                    {
                        erros.Add("Favor informar a tarefa");
                    }

                    if (tarefa.DataPrevistaConclusao == DateTime.MinValue || tarefa.DataPrevistaConclusao < DateTime.Today)
                    {
                        erros.Add("Data de previsão não pode ser menor que hoje");
                    }
                }

                if (erros.Count > 0)
                {
                    return BadRequest(new ErrorResponseDto()
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Erros = erros
                    });
                }

                tarefa.IdUsuario = usuario.Id;
                tarefa.DataConclusao = null;
                _tarefaRepository.AdicionarTarefa(tarefa);

                return Ok( new { msg = "Tarefa criada com sucesso" } );
            }
            catch (Exception e)
            {

                _logger.LogError($"Erro ao adicionar tarefa: {e.Message}");

                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto()
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Erro = $"Erro ao adicionar tarefa, tente novamente. Err: {e.Message}"
                });
            }
        }
    }
}
