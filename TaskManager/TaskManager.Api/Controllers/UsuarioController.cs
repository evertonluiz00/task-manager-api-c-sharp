using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuarioController : BaseController
    {
        private readonly ILogger<UsuarioController> _logger;

        private readonly string loginMock = "admin@admin.com";
        private readonly string senhaMock = "Admin1234@";

        public UsuarioController(ILogger<UsuarioController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult BuscarUsuario()
        {
            try
            {
                var usuarioMock = new Usuario()
                {
                    Id = 1,
                    Nome = "Nome usu teste",
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
                return Ok(usuario);
            }
            catch (Exception e)
            {

                _logger.LogError($"Erro ao salvar usuário: {e.Message}");

                return StatusCode(StatusCodes.Status500InternalServerError, new ErrorResponseDto()
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Erro = $"Erro ao salvar usuário, tente novamente."
                });
            }
        }
    }
}
