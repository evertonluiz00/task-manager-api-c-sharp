using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace TaskManager.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : BaseController
    {
        private readonly ILogger<LoginController> _logger;

        public LoginController(ILogger<LoginController> logger, IUsuarioRepository usuarioRepository) : base(usuarioRepository)
        {
            _logger = logger;
        }


        [HttpPost]
        [AllowAnonymous]
        public IActionResult EfetuarLogin([FromBody] LoginRequestDto request)
        {
            try
            {
                if (request == null || string.IsNullOrEmpty(request.Login.Trim()) || string.IsNullOrEmpty(request.Senha.Trim()))
                {
                    return BadRequest(new ErrorResponseDto() {
                        Status = StatusCodes.Status400BadRequest,
                        Erro = "Parâmetros de entrada inválidos."
                    });
                }


                var usuario = _usuarioRepository.GetByLoginSenha(request.Login, MD5Utils.GerarHashMD5(request.Senha));

                if (usuario == null)
                {
                    return BadRequest(new ErrorResponseDto()
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Erro = "Usuário eu senha inválidos."
                    });
                }

                var token = TokenService.CriarToken(usuario);

                return Ok(new LoginResponseDto() {
                    Email = usuario.Email,
                    Nome = usuario.Nome,
                    Token = token
                });
            }
            catch (Exception e)
            {
                _logger.LogError($"Erro ao efetuar login: {e.Message}");

                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto() { 
                    Status = StatusCodes.Status500InternalServerError,
                    Erro = $"Erro ao efetuar login, tente novamente."
                });
            }
        }
    }
}
