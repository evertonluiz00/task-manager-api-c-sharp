using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.Api
{
    public interface IUsuarioRepository
    {
        public void Salvar(Usuario usuario);
        public bool ExisteUsuarioEmail(string email);
        public Usuario GetByLoginSenha(string login, string senha);
        public Usuario GetById(int idUsuario);
    }
}
