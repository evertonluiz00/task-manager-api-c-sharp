using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.Api
{
    public class LoginRequestDto
    {
        public string Login { get; set; }
        public string Senha { get; set; }
    }
}
