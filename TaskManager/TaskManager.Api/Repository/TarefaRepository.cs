using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.Api
{
    public class TarefaRepository : ITarefaRepository
    {
        private readonly AppDbContext _context;

        public TarefaRepository(AppDbContext context)
        {
            _context = context;
        }

        public void AdicionarTarefa(Tarefa tarefa)
        {
            _context.Tarefa.Add(tarefa);
            _context.SaveChanges();
        }

        public Tarefa GetById(int idTarefa)
        {
            return _context.Tarefa.FirstOrDefault(tarefa => tarefa.Id == idTarefa);
        }

        public void RemoverTarefa(Tarefa tarefa)
        {
            _context.Tarefa.Remove(tarefa);
            _context.SaveChanges();
        }

        public void AtualizarTarefa(Tarefa tarefa)
        {
            _context.Entry(tarefa).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            _context.Entry(tarefa).State = Microsoft.EntityFrameworkCore.EntityState.Detached;
        }

        public List<Tarefa> BuscarTarefas(int idUsuario, DateTime? periodoDe, DateTime? periodoAte, StatusTarefaEnum status)
        {
            return _context.Tarefa.Where(tarefa => tarefa.IdUsuario == idUsuario && (periodoDe == null || periodoDe == DateTime.MinValue || tarefa.DataPrevistaConclusao > ((DateTime)periodoDe).Date)).ToList();
        }
    }
}
