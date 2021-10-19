using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;

namespace TaskManager.Api
{
    [Authorize]
    public class BaseController : ControllerBase
    {
        protected readonly IUsuarioRepository _usuarioRepository;

        public BaseController(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        protected Usuario ReadToken()
        {
            var idUsuarioStr = User.Claims.Where(c => c.Type == ClaimTypes.Sid).Select(u => u.Value).FirstOrDefault();

            if (!string.IsNullOrEmpty(idUsuarioStr))
            {
                var usuario = _usuarioRepository.GetById(int.Parse(idUsuarioStr));
                return usuario;
            }

            throw new UnauthorizedAccessException("Token não informado ou inválido");
        }
    }
}
