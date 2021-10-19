using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.Api
{
    public interface ITarefaRepository
    {
        public void AdicionarTarefa(Tarefa tarefa);
    }
}
