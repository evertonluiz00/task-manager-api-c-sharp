using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TaskManager.Api
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }

        [JsonIgnore]
        public virtual ICollection<Tarefa> Tarefas { get; private set; }
    }
}
