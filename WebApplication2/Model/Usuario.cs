using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Cargo { get; set; }
        public string Departamento { get; set; }
        public DateTime Admissao { get; set; }
        public bool Administrador { get; set; }
        public string HashSenha { get; set; }
    }
}