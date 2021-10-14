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
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;

        private readonly string loginMock = "admin@admin.com";
        private readonly string senhaMock = "Admin1234@";

        public LoginController(ILogger<LoginController> logger)
        {
            _logger = logger;
        }


        [HttpPost]
        public IActionResult EfetuarLogin([FromBody] LoginRequestDto request)
        {
            try
            {
                if (request == null || string.IsNullOrEmpty(request.Login.Trim()) || string.IsNullOrEmpty(request.Senha.Trim()) || !request.Login.Equals(loginMock) || !request.Senha.Equals(senhaMock))
                {
                    return BadRequest(new ErrorResponseDto() {
                        Status = StatusCodes.Status400BadRequest,
                        Erro = "Parâmetros de entrada inválidos."
                    });
                }

                var usuarioMock = new Usuario()
                {
                    Id = 1,
                    Nome = "Nome usu teste",
                    Email = loginMock,
                    Senha = senhaMock
                };

                return Ok(new LoginResponseDto() {
                    Email = usuarioMock.Email,
                    Nome = usuarioMock.Nome,
                    Token = TokenService.CriarToken(usuarioMock)
                });;
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
