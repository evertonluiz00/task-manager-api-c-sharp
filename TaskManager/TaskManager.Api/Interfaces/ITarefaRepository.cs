using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.Api
{
    public interface ITarefaRepository
    {
        public void AdicionarTarefa(Tarefa tarefa);
        public Tarefa GetById(int idTarefa);
        public void RemoverTarefa(Tarefa tarefa);
        public void AtualizarTarefa(Tarefa tarefa);
        public List<Tarefa> BuscarTarefas(int idUsuario);
    }
}
