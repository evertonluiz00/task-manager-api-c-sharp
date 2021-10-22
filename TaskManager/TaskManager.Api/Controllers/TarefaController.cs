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

        [HttpDelete("{idTarefa}")]
        public IActionResult DeletarTarefa(int idTarefa)
        {
            try
            {
                var usuario = ReadToken();
                if (usuario == null || idTarefa <= 0)
                {
                    return BadRequest(new ErrorResponseDto()
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Erro = "Usuario ou tarefa inválidos"
                    });
                }

                var tarefa = _tarefaRepository.GetById(idTarefa);
                if (tarefa == null || tarefa.IdUsuario != usuario.Id)
                {
                    return BadRequest(new ErrorResponseDto()
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Erro = "Tarefa não encontrada"
                    });
                }

                _tarefaRepository.RemoverTarefa(tarefa);
                return Ok(new { msg = "Tarefa deletada com sucesso" });
            }
            catch (Exception e)
            {

                _logger.LogError($"Erro ao deletar tarefa: {e.Message}");

                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto()
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Erro = $"Erro ao deletar tarefa, tente novamente. Err: {e.Message}"
                });
            }
        }

        [HttpPut("{idTarefa}")]
        public IActionResult AtualizarTarefa([FromBody] Tarefa model, int idTarefa)
        {
            try
            {
                var usuario = ReadToken();
                if (usuario == null || idTarefa <= 0)
                {
                    return BadRequest(new ErrorResponseDto()
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Erro = "Usuario ou tarefa inválidos"
                    });
                }

                var tarefa = _tarefaRepository.GetById(idTarefa);
                if (tarefa == null || tarefa.IdUsuario != usuario.Id)
                {
                    return BadRequest(new ErrorResponseDto()
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Erro = "Tarefa não encontrada"
                    });
                }

                var erros = new List<string>();
                if (model == null)
                {
                    erros.Add("Favor informar a tarefa ou usuário");
                }
                else if (!string.IsNullOrEmpty(model.Nome.Trim()) && model.Nome.Count() < 4)
                {
                    erros.Add("Favor informar um nome válido");
                }

                if (erros.Count > 0)
                {
                    return BadRequest(new ErrorResponseDto()
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Erros = erros
                    });
                }

                if (!string.IsNullOrEmpty(model.Nome.Trim()))
                {
                    tarefa.Nome = model.Nome;
                }

                if (model.DataPrevistaConclusao != DateTime.MinValue)
                {
                    tarefa.DataPrevistaConclusao = model.DataPrevistaConclusao;
                }

                if (model.DataConclusao != null && model.DataConclusao != DateTime.MinValue)
                {
                    tarefa.DataConclusao = model.DataConclusao;
                }

                _tarefaRepository.AtualizarTarefa(tarefa);
                return Ok( new { msg = "Tarefa atualizada com sucesso."});

            }
            catch (Exception e)
            {

                _logger.LogError($"Erro ao atualizar tarefa: {e.Message}");

                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto()
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Erro = $"Erro ao atualizar tarefa, tente novamente. Err: {e.Message}"
                });
            }
        }


        [HttpGet]
        public IActionResult ListarTarefasUsuario()
        {
            try
            {
                var usuario = ReadToken();
                if (usuario == null)
                {
                    return BadRequest(new ErrorResponseDto()
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Erro = "Usuário não encontrado"
                    });
                }

                var resultado = _tarefaRepository.BuscarTarefas(usuario.Id);
                return Ok(resultado);

            }
            catch (Exception e)
            {

                _logger.LogError($"Erro ao listar tarefas: {e.Message}");

                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto()
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Erro = $"Erro ao listar tarefas, tente novamente. Err: {e.Message}"
                });
            }
        }


    }
}
