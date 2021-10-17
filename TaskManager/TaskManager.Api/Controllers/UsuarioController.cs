using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TaskManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : BaseController
    {
        private readonly ILogger<UsuarioController> _logger;
        private readonly IUsuarioRepository _usuarioRepository;

        private readonly string loginMock = "admin@admin.com";
        private readonly string senhaMock = "Admin1234@";

        public UsuarioController(ILogger<UsuarioController> logger, IUsuarioRepository usuarioRepository)
        {
            _logger = logger;
            _usuarioRepository = usuarioRepository;
        }

        [HttpGet]
        public IActionResult BuscarUsuario()
        {
            try
            {
                var usuarioMock = new Usuario()
                {
                    Id = 1,
                    Nome = "UsuarioTeste",
                    Email = loginMock,
                    Senha = senhaMock
                };

                return Ok(usuarioMock);
            }
            catch (Exception e)
            {
                _logger.LogError($"Erro ao buscar usuário: {e.Message}");

                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto()
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Erro = $"Erro ao buscar usuário, tente novamente."
                });
            }
        }


        [HttpPost]
        [AllowAnonymous]
        public IActionResult SalvarUsuario([FromBody]Usuario usuario)
        {
            try
            {
                var erros = new List<string>();

                if (string.IsNullOrEmpty(usuario.Nome.Trim()) || usuario.Nome.Length < 2)
                {
                    erros.Add("Nome inválido");
                }

                if (string.IsNullOrEmpty(usuario.Senha.Trim()) || usuario.Nome.Length < 4)
                {
                    erros.Add("Senha inválido");
                }

                Regex regex = new Regex(@"^([\w\.\-\+\d]+)@([\w\-]+)((\.(\w){2,4})+)$");
                if (string.IsNullOrEmpty(usuario.Email.Trim()) || !regex.IsMatch(usuario.Email))
                {
                    erros.Add("E-mail inválido");
                }

                if (erros.Count > 0)
                {
                    return BadRequest(new ErrorResponseDto()
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Erros = erros
                    });
                }

                _usuarioRepository.Salvar(usuario);

                return Ok( new { msg = "Usuário criado com sucesso"} );
            }
            catch (Exception e)
            {
                _logger.LogError($"Erro ao salvar usuário: {e.Message}");

                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto()
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Erro = $"Erro ao salvar usuário, tente novamente. Err: {e.Message}"
                });
            }
        }
    }
}
