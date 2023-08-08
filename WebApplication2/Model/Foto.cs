using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Model
{
    public class Foto
    {
        public int FotoId { get; set; }
        public int ProdutoId { get; set; }
        public byte[] FotoProduto { get; set; }
    }
}