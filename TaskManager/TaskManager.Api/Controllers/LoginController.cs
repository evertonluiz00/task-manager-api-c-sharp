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

        public LoginController(ILogger<LoginController> logger)
        {
            _logger = logger;
        }


        [HttpPost]
        public IActionResult FazerLogin([FromBody] LoginRequestDto request)
        {
            try
            {
                if (request == null || string.IsNullOrEmpty(request.Login.Trim()) || string.IsNullOrEmpty(request.Senha.Trim()))
                {
                    return BadRequest(new ErrorResponseDto()
                    {
                        Status = StatusCodes.Status400BadRequest,
                        Erro = "Parâmetros de entrada inválidos."
                    });
                }

                return Ok("Usuário autenticado.");
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
