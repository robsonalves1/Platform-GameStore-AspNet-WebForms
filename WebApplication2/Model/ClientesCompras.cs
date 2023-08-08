using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model
{
    public class ClientesCompras
    {
        public int ComprasId { get; set; }
        public int ClienteId { get; set; }
        public int ProdutoInformaticaId { get; set; }
        public int JogoId { get; set; }
        public string ProdutoNome { get; set; }
        public string JogoNome { get; set; }
        public int ProdutoQuantidadeComprada { get; set; }
    }
}