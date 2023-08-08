using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model
{
    public class Produto
    {
        public int id { get; set; }
        public string Codigo { get; set; }
        public int Quantidade { get; set; }
        public string Nome { get; set; }
        public float Preco { get; set; }
        public string Caracteristicas { get; set; }
        public string Marca { get; set; }
        public string Departamento { get; set; }
        public byte[] Foto { get; set; }
    }
}