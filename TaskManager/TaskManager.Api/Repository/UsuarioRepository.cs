using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.Api
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _context;

        public UsuarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public void Salvar(Usuario usuario)
        {
            _context.Usuario.Add(usuario);
            _context.SaveChanges();
        }

        public bool ExisteUsuarioEmail(string email)
        {
            return _context.Usuario.Any(usuario => usuario.Email.ToLower() == email.ToLower());
        }

        public Usuario GetByLoginSenha(string login, string senha)
        {
            return _context.Usuario.FirstOrDefault(usuario => usuario.Email == login.ToLower() && usuario.Senha == senha);
        }

        public Usuario GetById(int idUsuario)
        {
            return _context.Usuario.FirstOrDefault(usuario => usuario.Id == idUsuario);
        }
    }
}
