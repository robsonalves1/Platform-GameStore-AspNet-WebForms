using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using WebApplication2.Controller;
using WebApplication2.Model;

namespace WebApplication2.View
{
    public partial class ComprarProduto : System.Web.UI.Page
    {
        CntrDB db = new CntrDB();
        int qtd;
        float mediaNotaProduto = 0;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DropdownContaMenu.InnerHtml = "<li>" +
                                                 "<button type=\"button\" id =\"BtnPaginaLoginCliente\" runat =\"server\" onclick=\"BtnLoginCliente()\"> Login </ button>" +
                                              "</ li>";

                CampoProdutoNome.InnerText = (string)HttpContext.Current.Session["idProduto"];
                GetProduto();
                SectionComentarios.InnerHtml = GetComentarios();
                CampoProdutoNotaEstrelas.InnerHtml = GetEstrelas();
                CampoProdutoNotaQuantidadeDeComentarios.InnerHtml = "(" + GetQuantidadeComentarios((string)HttpContext.Current.Session["idProduto"]) + ")";
            } 
            else
            {
                DropdownContaMenu.InnerHtml = "<li>" +
                                                 "<button type=\"button\" id =\"BtnPaginaLoginCliente\" runat =\"server\" onclick=\"BtnLoginCliente()\"> Login </ button>" +
                                              "</ li>";
                SectionComentarios.InnerHtml = GetComentarios();
                CampoProdutoNotaEstrelas.InnerHtml = GetEstrelas();
                CampoProdutoNotaQuantidadeDeComentarios.InnerHtml = "(" + GetQuantidadeComentarios((string)HttpContext.Current.Session["idProduto"]) + ")";
            }

            if (Session["clienteLogado"] != null)
            {
                Cliente c = new Cliente();
                c = (Cliente)Session["clienteLogado"];

                DropdownConta.InnerHtml = c.Nome + "<span><svg xmlns=\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-person-circle\" viewBox=\"0 0 16 16\">" +
                                                    "<path d=\"M11 6a3 3 0 1 1-6 0 3 3 0 0 1 6 0z\" />" +
                                                    "<path fill-rule=\"evenodd\" d=\"M0 8a8 8 0 1 1 16 0A8 8 0 0 1 0 8zm8-7a7 7 0 0 0-5.468 11.37C3.242 11.226 4.805 10 8 10s4.757 1.225 5.468 2.37A7 7 0 0 0 8 1z\" />" +
                                                    "</ svg>▼</span>";
                DropdownContaMenu.InnerHtml = "<li>" +
                                                    "<button type=\"button\" id=\"BtnAcessaContaCliente\" runat=\"server\"><a href=\"HomeContaCliente.aspx\">Conta</a></ button>" +
                                                    "<button type=\"button\" id=\"BtnLogoutContaCliente\" runat=\"server\" onclick=\"BtnLogoutCliente()\">Sair</ button>" +
                                              "</ li>";
            }
        }

        protected void GetProduto()
        {
            try
            {
                string query = "SELECT produtoId, produtoCodigo, produtoQuantidade, produtoNome, " +
                               "Concat('R$ ', " +
                                   "Replace " +
                                     "(Replace" +
                                       "(Replace" +
                                         "(Format(produtoPreco, 2), '.', '|'), ',', '.'), '|', ',')) AS produtoPreco, " +
                                "produtoFoto, produtoCaracteristicas, produtoMarca " +
                                "FROM produto " +
                                "WHERE produtoId = " + (string)HttpContext.Current.Session["idProduto"];
                DataTable dt = db.ExecuteReader(query);

                byte[] bytesImg = (byte[])dt.Rows[0][5];
                string base64StringImg = Convert.ToBase64String(bytesImg);

                //CampoProdutoImg.Src = "data:image/jpg;base64," + base64StringImg;
                imgs.Src = "data:image/jpg;base64," + base64StringImg;

                /**/
                string qry = "SELECT fotoProduto FROM foto WHERE produtoId = " + dt.Rows[0][0];
                DataTable dt2 = db.ExecuteReader(qry);

                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    HtmlGenericControl divImgProduto = new HtmlGenericControl();
                    divImgProduto.Attributes["id"] = "swatch-" + i;
                    divImgProduto.Attributes["class"] = "swatch";

                    byte[] bytesImg2 = (byte[])dt2.Rows[i][0];
                    string base64StringImg2 = Convert.ToBase64String(bytesImg2);

                    divImgProduto.InnerHtml =
                        "<img src=\"data:image/jpg;base64," + base64StringImg2 + "\" />";

                    divImgProduto.Attributes["onclick"] = "updateSelected(this, \"data:image/jpg;base64," + base64StringImg2 + "\")";

                    SwatchesSection.Controls.AddAt(i, divImgProduto);
                }
                /**/

                CampoProdutoNome.InnerText = (string)dt.Rows[0][3];
                CampoProdutoPreco.InnerText = (string)dt.Rows[0][4];
                CampoProdutoQuantidade.InnerText = Convert.ToString(dt.Rows[0][2]);
                CampoCaracteristicas.InnerText = Convert.ToString(dt.Rows[0][6]);
                CampoProdutoMarca.InnerHtml = Convert.ToString(dt.Rows[0][7]);
                qtd = (int)dt.Rows[0][2];
            } 
            catch (Exception ex)
            {
                LblAlert.InnerText = ex.Message;
                DivAlert.Visible = true;
            }
        }

        protected void BtnCancelarCompra_Click(object sender, EventArgs e)
        {
            Response.Redirect("Loja.aspx");
        }

        [WebMethod]
        public static string ResultadoPesquisaProduto(string filtro)
        {
            DataTable dt = new DataTable();
            CntrDB db = new CntrDB();

            string query = "SELECT produtoId, produtoNome, produtoFoto FROM produto WHERE produtoNome LIKE '%" + filtro + "%'";

            dt = db.ExecuteReader(query);
            if (filtro != "")
            {
                string res = "";
                int length = 3;

                if (dt.Rows.Count < 3)
                {
                    length = dt.Rows.Count;
                }

                for (int i = 0; i < length; i++)
                {
                    string foto = "";
                    byte[] bytes = (byte[])dt.Rows[i][2];
                    if (bytes != null && bytes.Length > 0)
                    {
                        string base64String = Convert.ToBase64String(bytes);
                        foto = "data:image/jpg;base64," + base64String;
                    }

                    res += "<button type=\"button\" onclick=\"BtnExibirProduto(" + dt.Rows[i][0] + ")\">" +
                                "<img class=\"imgProdutoBarraDePesquisa\" src=\"" + foto + "\"></img>" +
                                (string)dt.Rows[i][1] +
                            "</button>";
                }

                return res;
            }
            else
            {
                return "";
            }
        }

        [WebMethod]
        public static string BtnCompraProduto()
        {
            Cliente cliente = new Cliente();
            cliente = (Cliente)HttpContext.Current.Session["clienteLogado"];

            try
            {
                if (cliente != null)
                {
                    CntrDB db = new CntrDB();
                    int qtd;

                    // 

                    string query = "SELECT produtoQuantidadeComprada FROM clienteCompras WHERE clienteId = " + cliente.Id + " AND produtoInformaticaId = " + (string)HttpContext.Current.Session["idProduto"];
                    DataTable dt = db.ExecuteReader(query);
                    int quantidadeComprada = 0;

                    if (dt.Rows.Count > 0)
                    {
                        quantidadeComprada = (int)dt.Rows[0][0];
                    }

                    query = "SELECT produtoNome, produtoFoto " +
                                    "FROM produto " +
                                    "WHERE produtoId = " + (string)HttpContext.Current.Session["idProduto"];
                    dt = db.ExecuteReader(query);

                    string produtoNome = (string)dt.Rows[0][0];
                    byte[] bytesImg = (byte[])dt.Rows[0][1];
                    string base64StringImg = Convert.ToBase64String(bytesImg);

                    if (quantidadeComprada == 0)
                    {
                        query = "INSERT INTO clienteCompras(clienteId, produtoInformaticaId, produtoNome, produtoFoto, produtoQuantidadeComprada) VALUES(@CLIENTEID, @PRODUTOID, @NOME, @FOTO, @QUANTIDADECOMPRADA)";
                        using (MySqlCommand cmd = new MySqlCommand(query))
                        {
                            cmd.Parameters.AddWithValue("@CLIENTEID", cliente.Id);
                            cmd.Parameters.AddWithValue("@PRODUTOID", (string)HttpContext.Current.Session["idProduto"]);
                            cmd.Parameters.AddWithValue("@NOME", produtoNome);
                            cmd.Parameters.AddWithValue("@FOTO", base64StringImg);
                            cmd.Parameters.AddWithValue("@QUANTIDADECOMPRADA", ++quantidadeComprada);
                            db.ExecuteNonQuery(query, cmd);
                        }
                    }
                    else
                    {
                        query = "UPDATE clienteCompras SET produtoQuantidadeComprada = @QUANTIDADECOMPRADA WHERE clienteId = @CLIENTEID AND produtoInformaticaId = @PRODUTOID";
                        using (MySqlCommand cmd = new MySqlCommand(query))
                        {
                            cmd.Parameters.AddWithValue("@QUANTIDADECOMPRADA", ++quantidadeComprada);
                            cmd.Parameters.AddWithValue("@CLIENTEID", cliente.Id);
                            cmd.Parameters.AddWithValue("@PRODUTOID", (string)HttpContext.Current.Session["idProduto"]);
                            db.ExecuteNonQuery(query, cmd);
                        }
                    }

                    //

                    string qry = "SELECT produtoQuantidade FROM produto WHERE produtoId = " + (string)HttpContext.Current.Session["idProduto"];
                    dt = db.ExecuteReader(qry);
                    qtd = (int)dt.Rows[0][0];

                    if (qtd > 0)
                    {
                        query = "UPDATE produto SET produtoQuantidade = @QUANTIDADE WHERE produtoId = " + (string)HttpContext.Current.Session["idProduto"];
                        using (MySqlCommand cmd = new MySqlCommand(query))
                        {
                            qtd--;
                            cmd.Parameters.AddWithValue("@QUANTIDADE", qtd);
                            db.ExecuteNonQuery(query, cmd);
                        }
                        return Convert.ToString(qtd);
                    } 
                    else
                    {
                        return "0";
                    }
                }
                else
                {
                    return "Cliente deve estar logado para comprar!";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            
        }

        [WebMethod]
        public static string BtnLoginCliente()
        {
            return "LoginCliente.aspx";
        }

        [WebMethod]
        public static void BtnLogoutCliente()
        {
            HttpContext.Current.Session["clienteLogado"] = null;
        }

        [WebMethod]
        public static string[] EnviarComentario(string comentario, string nota)
        {
            string[] res = new string[4];

            try
            {
                CntrDB db = new CntrDB();
                DataTable dt = new DataTable();
                ComprarProduto cp = new ComprarProduto();
                string qtd = "";
                
                // INSERE O COMENTÁRIO NO BANCO DE DADOS
                string query = "INSERT INTO comentario(produtoId, comentarioTexto, comentarioNota) VALUES(@ID, @COMENTARIO, @NOTA)";
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    cmd.Parameters.AddWithValue("@ID", HttpContext.Current.Session["idProduto"]);
                    cmd.Parameters.AddWithValue("@COMENTARIO", comentario);
                    cmd.Parameters.AddWithValue("@NOTA", nota);
                    db.ExecuteNonQuery(query, cmd);
                }

                // PEGA A QUANTIDADE DE COMENTÁRIOS ATRAVÉS DO ID
                qtd =  cp.GetQuantidadeComentarios((string)HttpContext.Current.Session["idProduto"]);
                
                res[0] = cp.GetComentarios();
                res[1] = cp.GetEstrelas();
                res[2] = qtd;

                return res;
            }
            catch (Exception ex)
            {
                res[3] = ex.Message;
                return res;
            }
        }

        protected string GetComentarios()
        {
            try
            {
                string res = "";
                string query = "SELECT * FROM comentario WHERE produtoId = " + HttpContext.Current.Session["idProduto"] + " ORDER BY comentarioId DESC";
                DataTable dt = db.ExecuteReader(query);
                if (dt.Rows.Count > 0)
                {
                    int totalDeNotas = 0;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string comentario = (string)dt.Rows[i][2];
                        int nota = (int)dt.Rows[i][3];

                        /*CAMPO COMENTÁRIOS*/
                        res += "<div class=\"px - 5 mt - 3\">" +
                                    "<p>" + comentario + "</p>" +
                                    "<p>" + nota + "</p>" +
                                "</div>";

                        /*MÉDIA NOTAS*/
                        mediaNotaProduto = (mediaNotaProduto += nota);
                        totalDeNotas++;
                    }
                    mediaNotaProduto = mediaNotaProduto / totalDeNotas;
                }

                return res;
            }
            catch (Exception ex)
            {
                DivAlert.Visible = true;
                LblAlert.InnerHtml = ex.Message;
                return ex.Message;
            }
        }

        protected string GetEstrelas()
        {
            try
            {
                string res = "";
                if (mediaNotaProduto == 0)
                {
                    res =
                        "<svg xmlns =\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-star\" viewBox=\"0 0 16 16\"> " +
                            "< path d =\"M2.866 14.85c-.078.444.36.791.746.593l4.39-2.256 4.389 2.256c.386.198.824-.149.746-.592l-.83-4.73 3.522-3.356c.33-.314.16-.888-.282-.95l-4.898-.696L8.465.792a.513.513 0 0 0-.927 0L5.354 5.12l-4.898.696c-.441.062-.612.636-.283.95l3.523 3.356-.83 4.73zm4.905-2.767-3.686 1.894.694-3.957a.565.565 0 0 0-.163-.505L1.71 6.745l4.052-.576a.525.525 0 0 0 .393-.288L8 2.223l1.847 3.658a.525.525 0 0 0 .393.288l4.052.575-2.906 2.77a.565.565 0 0 0-.163.506l.694 3.957-3.686-1.894a.503.503 0 0 0-.461 0z\"/> " +
                        "</svg> " +
                        "<svg xmlns =\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-star\" viewBox=\"0 0 16 16\"> " +
                            "< path d =\"M2.866 14.85c-.078.444.36.791.746.593l4.39-2.256 4.389 2.256c.386.198.824-.149.746-.592l-.83-4.73 3.522-3.356c.33-.314.16-.888-.282-.95l-4.898-.696L8.465.792a.513.513 0 0 0-.927 0L5.354 5.12l-4.898.696c-.441.062-.612.636-.283.95l3.523 3.356-.83 4.73zm4.905-2.767-3.686 1.894.694-3.957a.565.565 0 0 0-.163-.505L1.71 6.745l4.052-.576a.525.525 0 0 0 .393-.288L8 2.223l1.847 3.658a.525.525 0 0 0 .393.288l4.052.575-2.906 2.77a.565.565 0 0 0-.163.506l.694 3.957-3.686-1.894a.503.503 0 0 0-.461 0z\"/> " +
                        "</svg> " +
                        "<svg xmlns =\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-star\" viewBox=\"0 0 16 16\"> " +
                            "< path d =\"M2.866 14.85c-.078.444.36.791.746.593l4.39-2.256 4.389 2.256c.386.198.824-.149.746-.592l-.83-4.73 3.522-3.356c.33-.314.16-.888-.282-.95l-4.898-.696L8.465.792a.513.513 0 0 0-.927 0L5.354 5.12l-4.898.696c-.441.062-.612.636-.283.95l3.523 3.356-.83 4.73zm4.905-2.767-3.686 1.894.694-3.957a.565.565 0 0 0-.163-.505L1.71 6.745l4.052-.576a.525.525 0 0 0 .393-.288L8 2.223l1.847 3.658a.525.525 0 0 0 .393.288l4.052.575-2.906 2.77a.565.565 0 0 0-.163.506l.694 3.957-3.686-1.894a.503.503 0 0 0-.461 0z\"/> " +
                        "</svg> " +
                        "<svg xmlns =\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-star\" viewBox=\"0 0 16 16\"> " +
                            "< path d =\"M2.866 14.85c-.078.444.36.791.746.593l4.39-2.256 4.389 2.256c.386.198.824-.149.746-.592l-.83-4.73 3.522-3.356c.33-.314.16-.888-.282-.95l-4.898-.696L8.465.792a.513.513 0 0 0-.927 0L5.354 5.12l-4.898.696c-.441.062-.612.636-.283.95l3.523 3.356-.83 4.73zm4.905-2.767-3.686 1.894.694-3.957a.565.565 0 0 0-.163-.505L1.71 6.745l4.052-.576a.525.525 0 0 0 .393-.288L8 2.223l1.847 3.658a.525.525 0 0 0 .393.288l4.052.575-2.906 2.77a.565.565 0 0 0-.163.506l.694 3.957-3.686-1.894a.503.503 0 0 0-.461 0z\"/> " +
                        "</svg> " +
                        "<svg xmlns =\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-star\" viewBox=\"0 0 16 16\"> " +
                            "< path d =\"M2.866 14.85c-.078.444.36.791.746.593l4.39-2.256 4.389 2.256c.386.198.824-.149.746-.592l-.83-4.73 3.522-3.356c.33-.314.16-.888-.282-.95l-4.898-.696L8.465.792a.513.513 0 0 0-.927 0L5.354 5.12l-4.898.696c-.441.062-.612.636-.283.95l3.523 3.356-.83 4.73zm4.905-2.767-3.686 1.894.694-3.957a.565.565 0 0 0-.163-.505L1.71 6.745l4.052-.576a.525.525 0 0 0 .393-.288L8 2.223l1.847 3.658a.525.525 0 0 0 .393.288l4.052.575-2.906 2.77a.565.565 0 0 0-.163.506l.694 3.957-3.686-1.894a.503.503 0 0 0-.461 0z\"/> " +
                        "</svg> ";
                }
                else if (mediaNotaProduto < 1)
                {
                    res =
                        "<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-star-half\" viewBox=\"0 0 16 16\">" +
                            "<path d =\"M5.354 5.119 7.538.792A.516.516 0 0 1 8 .5c.183 0 .366.097.465.292l2.184 4.327 4.898.696A.537.537 0 0 1 16 6.32a.548.548 0 0 1-.17.445l-3.523 3.356.83 4.73c.078.443-.36.79-.746.592L8 13.187l-4.389 2.256a.52.52 0 0 1-.146.05c-.342.06-.668-.254-.6-.642l.83-4.73L.173 6.765a.55.55 0 0 1-.172-.403.58.58 0 0 1 .085-.302.513.513 0 0 1 .37-.245l4.898-.696zM8 12.027a.5.5 0 0 1 .232.056l3.686 1.894-.694-3.957a.565.565 0 0 1 .162-.505l2.907-2.77-4.052-.576a.525.525 0 0 1-.393-.288L8.001 2.223 8 2.226v9.8z\"/>" +
                        "</svg>" +
                        "<svg xmlns =\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-star\" viewBox=\"0 0 16 16\"> " +
                            "< path d =\"M2.866 14.85c-.078.444.36.791.746.593l4.39-2.256 4.389 2.256c.386.198.824-.149.746-.592l-.83-4.73 3.522-3.356c.33-.314.16-.888-.282-.95l-4.898-.696L8.465.792a.513.513 0 0 0-.927 0L5.354 5.12l-4.898.696c-.441.062-.612.636-.283.95l3.523 3.356-.83 4.73zm4.905-2.767-3.686 1.894.694-3.957a.565.565 0 0 0-.163-.505L1.71 6.745l4.052-.576a.525.525 0 0 0 .393-.288L8 2.223l1.847 3.658a.525.525 0 0 0 .393.288l4.052.575-2.906 2.77a.565.565 0 0 0-.163.506l.694 3.957-3.686-1.894a.503.503 0 0 0-.461 0z\"/> " +
                        "</svg> " +
                        "<svg xmlns =\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-star\" viewBox=\"0 0 16 16\"> " +
                            "< path d =\"M2.866 14.85c-.078.444.36.791.746.593l4.39-2.256 4.389 2.256c.386.198.824-.149.746-.592l-.83-4.73 3.522-3.356c.33-.314.16-.888-.282-.95l-4.898-.696L8.465.792a.513.513 0 0 0-.927 0L5.354 5.12l-4.898.696c-.441.062-.612.636-.283.95l3.523 3.356-.83 4.73zm4.905-2.767-3.686 1.894.694-3.957a.565.565 0 0 0-.163-.505L1.71 6.745l4.052-.576a.525.525 0 0 0 .393-.288L8 2.223l1.847 3.658a.525.525 0 0 0 .393.288l4.052.575-2.906 2.77a.565.565 0 0 0-.163.506l.694 3.957-3.686-1.894a.503.503 0 0 0-.461 0z\"/> " +
                        "</svg> " +
                        "<svg xmlns =\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-star\" viewBox=\"0 0 16 16\"> " +
                            "< path d =\"M2.866 14.85c-.078.444.36.791.746.593l4.39-2.256 4.389 2.256c.386.198.824-.149.746-.592l-.83-4.73 3.522-3.356c.33-.314.16-.888-.282-.95l-4.898-.696L8.465.792a.513.513 0 0 0-.927 0L5.354 5.12l-4.898.696c-.441.062-.612.636-.283.95l3.523 3.356-.83 4.73zm4.905-2.767-3.686 1.894.694-3.957a.565.565 0 0 0-.163-.505L1.71 6.745l4.052-.576a.525.525 0 0 0 .393-.288L8 2.223l1.847 3.658a.525.525 0 0 0 .393.288l4.052.575-2.906 2.77a.565.565 0 0 0-.163.506l.694 3.957-3.686-1.894a.503.503 0 0 0-.461 0z\"/> " +
                        "</svg> " +
                        "<svg xmlns =\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-star\" viewBox=\"0 0 16 16\"> " +
                            "< path d =\"M2.866 14.85c-.078.444.36.791.746.593l4.39-2.256 4.389 2.256c.386.198.824-.149.746-.592l-.83-4.73 3.522-3.356c.33-.314.16-.888-.282-.95l-4.898-.696L8.465.792a.513.513 0 0 0-.927 0L5.354 5.12l-4.898.696c-.441.062-.612.636-.283.95l3.523 3.356-.83 4.73zm4.905-2.767-3.686 1.894.694-3.957a.565.565 0 0 0-.163-.505L1.71 6.745l4.052-.576a.525.525 0 0 0 .393-.288L8 2.223l1.847 3.658a.525.525 0 0 0 .393.288l4.052.575-2.906 2.77a.565.565 0 0 0-.163.506l.694 3.957-3.686-1.894a.503.503 0 0 0-.461 0z\"/> " +
                        "</svg> ";
                }
                else if (mediaNotaProduto == 1)
                {
                    res =
                        "<svg xmlns =\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-star-fill\" viewBox=\"0 0 16 16\">" +
                            "<path d =\"M3.612 15.443c-.386.198-.824-.149-.746-.592l.83-4.73L.173 6.765c-.329-.314-.158-.888.283-.95l4.898-.696L7.538.792c.197-.39.73-.39.927 0l2.184 4.327 4.898.696c.441.062.612.636.282.95l-3.522 3.356.83 4.73c.078.443-.36.79-.746.592L8 13.187l-4.389 2.256z\"/>" +
                        "</svg>" +
                        "<svg xmlns =\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-star\" viewBox=\"0 0 16 16\"> " +
                            "< path d =\"M2.866 14.85c-.078.444.36.791.746.593l4.39-2.256 4.389 2.256c.386.198.824-.149.746-.592l-.83-4.73 3.522-3.356c.33-.314.16-.888-.282-.95l-4.898-.696L8.465.792a.513.513 0 0 0-.927 0L5.354 5.12l-4.898.696c-.441.062-.612.636-.283.95l3.523 3.356-.83 4.73zm4.905-2.767-3.686 1.894.694-3.957a.565.565 0 0 0-.163-.505L1.71 6.745l4.052-.576a.525.525 0 0 0 .393-.288L8 2.223l1.847 3.658a.525.525 0 0 0 .393.288l4.052.575-2.906 2.77a.565.565 0 0 0-.163.506l.694 3.957-3.686-1.894a.503.503 0 0 0-.461 0z\"/> " +
                        "</svg> " +
                        "<svg xmlns =\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-star\" viewBox=\"0 0 16 16\"> " +
                            "< path d =\"M2.866 14.85c-.078.444.36.791.746.593l4.39-2.256 4.389 2.256c.386.198.824-.149.746-.592l-.83-4.73 3.522-3.356c.33-.314.16-.888-.282-.95l-4.898-.696L8.465.792a.513.513 0 0 0-.927 0L5.354 5.12l-4.898.696c-.441.062-.612.636-.283.95l3.523 3.356-.83 4.73zm4.905-2.767-3.686 1.894.694-3.957a.565.565 0 0 0-.163-.505L1.71 6.745l4.052-.576a.525.525 0 0 0 .393-.288L8 2.223l1.847 3.658a.525.525 0 0 0 .393.288l4.052.575-2.906 2.77a.565.565 0 0 0-.163.506l.694 3.957-3.686-1.894a.503.503 0 0 0-.461 0z\"/> " +
                        "</svg> " +
                        "<svg xmlns =\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-star\" viewBox=\"0 0 16 16\"> " +
                            "< path d =\"M2.866 14.85c-.078.444.36.791.746.593l4.39-2.256 4.389 2.256c.386.198.824-.149.746-.592l-.83-4.73 3.522-3.356c.33-.314.16-.888-.282-.95l-4.898-.696L8.465.792a.513.513 0 0 0-.927 0L5.354 5.12l-4.898.696c-.441.062-.612.636-.283.95l3.523 3.356-.83 4.73zm4.905-2.767-3.686 1.894.694-3.957a.565.565 0 0 0-.163-.505L1.71 6.745l4.052-.576a.525.525 0 0 0 .393-.288L8 2.223l1.847 3.658a.525.525 0 0 0 .393.288l4.052.575-2.906 2.77a.565.565 0 0 0-.163.506l.694 3.957-3.686-1.894a.503.503 0 0 0-.461 0z\"/> " +
                        "</svg> " +
                        "<svg xmlns =\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-star\" viewBox=\"0 0 16 16\"> " +
                            "< path d =\"M2.866 14.85c-.078.444.36.791.746.593l4.39-2.256 4.389 2.256c.386.198.824-.149.746-.592l-.83-4.73 3.522-3.356c.33-.314.16-.888-.282-.95l-4.898-.696L8.465.792a.513.513 0 0 0-.927 0L5.354 5.12l-4.898.696c-.441.062-.612.636-.283.95l3.523 3.356-.83 4.73zm4.905-2.767-3.686 1.894.694-3.957a.565.565 0 0 0-.163-.505L1.71 6.745l4.052-.576a.525.525 0 0 0 .393-.288L8 2.223l1.847 3.658a.525.525 0 0 0 .393.288l4.052.575-2.906 2.77a.565.565 0 0 0-.163.506l.694 3.957-3.686-1.894a.503.503 0 0 0-.461 0z\"/> " +
                        "</svg> ";
                }
                else if (mediaNotaProduto < 2)
                {
                    res =
                        "<svg xmlns =\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-star-fill\" viewBox=\"0 0 16 16\">" +
                            "<path d =\"M3.612 15.443c-.386.198-.824-.149-.746-.592l.83-4.73L.173 6.765c-.329-.314-.158-.888.283-.95l4.898-.696L7.538.792c.197-.39.73-.39.927 0l2.184 4.327 4.898.696c.441.062.612.636.282.95l-3.522 3.356.83 4.73c.078.443-.36.79-.746.592L8 13.187l-4.389 2.256z\"/>" +
                        "</svg>" +
                        "<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-star-half\" viewBox=\"0 0 16 16\">" +
                            "<path d =\"M5.354 5.119 7.538.792A.516.516 0 0 1 8 .5c.183 0 .366.097.465.292l2.184 4.327 4.898.696A.537.537 0 0 1 16 6.32a.548.548 0 0 1-.17.445l-3.523 3.356.83 4.73c.078.443-.36.79-.746.592L8 13.187l-4.389 2.256a.52.52 0 0 1-.146.05c-.342.06-.668-.254-.6-.642l.83-4.73L.173 6.765a.55.55 0 0 1-.172-.403.58.58 0 0 1 .085-.302.513.513 0 0 1 .37-.245l4.898-.696zM8 12.027a.5.5 0 0 1 .232.056l3.686 1.894-.694-3.957a.565.565 0 0 1 .162-.505l2.907-2.77-4.052-.576a.525.525 0 0 1-.393-.288L8.001 2.223 8 2.226v9.8z\"/>" +
                        "</svg>" +
                        "<svg xmlns =\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-star\" viewBox=\"0 0 16 16\"> " +
                            "< path d =\"M2.866 14.85c-.078.444.36.791.746.593l4.39-2.256 4.389 2.256c.386.198.824-.149.746-.592l-.83-4.73 3.522-3.356c.33-.314.16-.888-.282-.95l-4.898-.696L8.465.792a.513.513 0 0 0-.927 0L5.354 5.12l-4.898.696c-.441.062-.612.636-.283.95l3.523 3.356-.83 4.73zm4.905-2.767-3.686 1.894.694-3.957a.565.565 0 0 0-.163-.505L1.71 6.745l4.052-.576a.525.525 0 0 0 .393-.288L8 2.223l1.847 3.658a.525.525 0 0 0 .393.288l4.052.575-2.906 2.77a.565.565 0 0 0-.163.506l.694 3.957-3.686-1.894a.503.503 0 0 0-.461 0z\"/> " +
                        "</svg> " +
                        "<svg xmlns =\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-star\" viewBox=\"0 0 16 16\"> " +
                            "< path d =\"M2.866 14.85c-.078.444.36.791.746.593l4.39-2.256 4.389 2.256c.386.198.824-.149.746-.592l-.83-4.73 3.522-3.356c.33-.314.16-.888-.282-.95l-4.898-.696L8.465.792a.513.513 0 0 0-.927 0L5.354 5.12l-4.898.696c-.441.062-.612.636-.283.95l3.523 3.356-.83 4.73zm4.905-2.767-3.686 1.894.694-3.957a.565.565 0 0 0-.163-.505L1.71 6.745l4.052-.576a.525.525 0 0 0 .393-.288L8 2.223l1.847 3.658a.525.525 0 0 0 .393.288l4.052.575-2.906 2.77a.565.565 0 0 0-.163.506l.694 3.957-3.686-1.894a.503.503 0 0 0-.461 0z\"/> " +
                        "</svg> " +
                        "<svg xmlns =\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-star\" viewBox=\"0 0 16 16\"> " +
                            "< path d =\"M2.866 14.85c-.078.444.36.791.746.593l4.39-2.256 4.389 2.256c.386.198.824-.149.746-.592l-.83-4.73 3.522-3.356c.33-.314.16-.888-.282-.95l-4.898-.696L8.465.792a.513.513 0 0 0-.927 0L5.354 5.12l-4.898.696c-.441.062-.612.636-.283.95l3.523 3.356-.83 4.73zm4.905-2.767-3.686 1.894.694-3.957a.565.565 0 0 0-.163-.505L1.71 6.745l4.052-.576a.525.525 0 0 0 .393-.288L8 2.223l1.847 3.658a.525.525 0 0 0 .393.288l4.052.575-2.906 2.77a.565.565 0 0 0-.163.506l.694 3.957-3.686-1.894a.503.503 0 0 0-.461 0z\"/> " +
                        "</svg> ";
                }
                else if (mediaNotaProduto == 2)
                {
                    res =
                        "<svg xmlns =\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-star-fill\" viewBox=\"0 0 16 16\">" +
                            "<path d =\"M3.612 15.443c-.386.198-.824-.149-.746-.592l.83-4.73L.173 6.765c-.329-.314-.158-.888.283-.95l4.898-.696L7.538.792c.197-.39.73-.39.927 0l2.184 4.327 4.898.696c.441.062.612.636.282.95l-3.522 3.356.83 4.73c.078.443-.36.79-.746.592L8 13.187l-4.389 2.256z\"/>" +
                        "</svg>" +
                        "<svg xmlns =\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-star-fill\" viewBox=\"0 0 16 16\">" +
                            "<path d =\"M3.612 15.443c-.386.198-.824-.149-.746-.592l.83-4.73L.173 6.765c-.329-.314-.158-.888.283-.95l4.898-.696L7.538.792c.197-.39.73-.39.927 0l2.184 4.327 4.898.696c.441.062.612.636.282.95l-3.522 3.356.83 4.73c.078.443-.36.79-.746.592L8 13.187l-4.389 2.256z\"/>" +
                        "</svg>" +
                        "<svg xmlns =\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-star\" viewBox=\"0 0 16 16\"> " +
                            "< path d =\"M2.866 14.85c-.078.444.36.791.746.593l4.39-2.256 4.389 2.256c.386.198.824-.149.746-.592l-.83-4.73 3.522-3.356c.33-.314.16-.888-.282-.95l-4.898-.696L8.465.792a.513.513 0 0 0-.927 0L5.354 5.12l-4.898.696c-.441.062-.612.636-.283.95l3.523 3.356-.83 4.73zm4.905-2.767-3.686 1.894.694-3.957a.565.565 0 0 0-.163-.505L1.71 6.745l4.052-.576a.525.525 0 0 0 .393-.288L8 2.223l1.847 3.658a.525.525 0 0 0 .393.288l4.052.575-2.906 2.77a.565.565 0 0 0-.163.506l.694 3.957-3.686-1.894a.503.503 0 0 0-.461 0z\"/> " +
                        "</svg> " +
                        "<svg xmlns =\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-star\" viewBox=\"0 0 16 16\"> " +
                            "< path d =\"M2.866 14.85c-.078.444.36.791.746.593l4.39-2.256 4.389 2.256c.386.198.824-.149.746-.592l-.83-4.73 3.522-3.356c.33-.314.16-.888-.282-.95l-4.898-.696L8.465.792a.513.513 0 0 0-.927 0L5.354 5.12l-4.898.696c-.441.062-.612.636-.283.95l3.523 3.356-.83 4.73zm4.905-2.767-3.686 1.894.694-3.957a.565.565 0 0 0-.163-.505L1.71 6.745l4.052-.576a.525.525 0 0 0 .393-.288L8 2.223l1.847 3.658a.525.525 0 0 0 .393.288l4.052.575-2.906 2.77a.565.565 0 0 0-.163.506l.694 3.957-3.686-1.894a.503.503 0 0 0-.461 0z\"/> " +
                        "</svg> " +
                        "<svg xmlns =\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-star\" viewBox=\"0 0 16 16\"> " +
                            "< path d =\"M2.866 14.85c-.078.444.36.791.746.593l4.39-2.256 4.389 2.256c.386.198.824-.149.746-.592l-.83-4.73 3.522-3.356c.33-.314.16-.888-.282-.95l-4.898-.696L8.465.792a.513.513 0 0 0-.927 0L5.354 5.12l-4.898.696c-.441.062-.612.636-.283.95l3.523 3.356-.83 4.73zm4.905-2.767-3.686 1.894.694-3.957a.565.565 0 0 0-.163-.505L1.71 6.745l4.052-.576a.525.525 0 0 0 .393-.288L8 2.223l1.847 3.658a.525.525 0 0 0 .393.288l4.052.575-2.906 2.77a.565.565 0 0 0-.163.506l.694 3.957-3.686-1.894a.503.503 0 0 0-.461 0z\"/> " +
                        "</svg> ";
                }
                else if (mediaNotaProduto < 3)
                {
                    res =
                        "<svg xmlns =\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-star-fill\" viewBox=\"0 0 16 16\">" +
                            "<path d =\"M3.612 15.443c-.386.198-.824-.149-.746-.592l.83-4.73L.173 6.765c-.329-.314-.158-.888.283-.95l4.898-.696L7.538.792c.197-.39.73-.39.927 0l2.184 4.327 4.898.696c.441.062.612.636.282.95l-3.522 3.356.83 4.73c.078.443-.36.79-.746.592L8 13.187l-4.389 2.256z\"/>" +
                        "</svg>" +
                        "<svg xmlns =\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-star-fill\" viewBox=\"0 0 16 16\">" +
                            "<path d =\"M3.612 15.443c-.386.198-.824-.149-.746-.592l.83-4.73L.173 6.765c-.329-.314-.158-.888.283-.95l4.898-.696L7.538.792c.197-.39.73-.39.927 0l2.184 4.327 4.898.696c.441.062.612.636.282.95l-3.522 3.356.83 4.73c.078.443-.36.79-.746.592L8 13.187l-4.389 2.256z\"/>" +
                        "</svg>" +
                        "<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-star-half\" viewBox=\"0 0 16 16\">" +
                            "<path d =\"M5.354 5.119 7.538.792A.516.516 0 0 1 8 .5c.183 0 .366.097.465.292l2.184 4.327 4.898.696A.537.537 0 0 1 16 6.32a.548.548 0 0 1-.17.445l-3.523 3.356.83 4.73c.078.443-.36.79-.746.592L8 13.187l-4.389 2.256a.52.52 0 0 1-.146.05c-.342.06-.668-.254-.6-.642l.83-4.73L.173 6.765a.55.55 0 0 1-.172-.403.58.58 0 0 1 .085-.302.513.513 0 0 1 .37-.245l4.898-.696zM8 12.027a.5.5 0 0 1 .232.056l3.686 1.894-.694-3.957a.565.565 0 0 1 .162-.505l2.907-2.77-4.052-.576a.525.525 0 0 1-.393-.288L8.001 2.223 8 2.226v9.8z\"/>" +
                        "</svg>" +
                        "<svg xmlns =\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-star\" viewBox=\"0 0 16 16\"> " +
                            "< path d =\"M2.866 14.85c-.078.444.36.791.746.593l4.39-2.256 4.389 2.256c.386.198.824-.149.746-.592l-.83-4.73 3.522-3.356c.33-.314.16-.888-.282-.95l-4.898-.696L8.465.792a.513.513 0 0 0-.927 0L5.354 5.12l-4.898.696c-.441.062-.612.636-.283.95l3.523 3.356-.83 4.73zm4.905-2.767-3.686 1.894.694-3.957a.565.565 0 0 0-.163-.505L1.71 6.745l4.052-.576a.525.525 0 0 0 .393-.288L8 2.223l1.847 3.658a.525.525 0 0 0 .393.288l4.052.575-2.906 2.77a.565.565 0 0 0-.163.506l.694 3.957-3.686-1.894a.503.503 0 0 0-.461 0z\"/> " +
                        "</svg> " +
                        "<svg xmlns =\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-star\" viewBox=\"0 0 16 16\"> " +
                            "< path d =\"M2.866 14.85c-.078.444.36.791.746.593l4.39-2.256 4.389 2.256c.386.198.824-.149.746-.592l-.83-4.73 3.522-3.356c.33-.314.16-.888-.282-.95l-4.898-.696L8.465.792a.513.513 0 0 0-.927 0L5.354 5.12l-4.898.696c-.441.062-.612.636-.283.95l3.523 3.356-.83 4.73zm4.905-2.767-3.686 1.894.694-3.957a.565.565 0 0 0-.163-.505L1.71 6.745l4.052-.576a.525.525 0 0 0 .393-.288L8 2.223l1.847 3.658a.525.525 0 0 0 .393.288l4.052.575-2.906 2.77a.565.565 0 0 0-.163.506l.694 3.957-3.686-1.894a.503.503 0 0 0-.461 0z\"/> " +
                        "</svg> ";
                }
                else if (mediaNotaProduto == 3)
                {
                    res =
                        "<svg xmlns =\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-star-fill\" viewBox=\"0 0 16 16\">" +
                            "<path d =\"M3.612 15.443c-.386.198-.824-.149-.746-.592l.83-4.73L.173 6.765c-.329-.314-.158-.888.283-.95l4.898-.696L7.538.792c.197-.39.73-.39.927 0l2.184 4.327 4.898.696c.441.062.612.636.282.95l-3.522 3.356.83 4.73c.078.443-.36.79-.746.592L8 13.187l-4.389 2.256z\"/>" +
                        "</svg>" +
                        "<svg xmlns =\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-star-fill\" viewBox=\"0 0 16 16\">" +
                            "<path d =\"M3.612 15.443c-.386.198-.824-.149-.746-.592l.83-4.73L.173 6.765c-.329-.314-.158-.888.283-.95l4.898-.696L7.538.792c.197-.39.73-.39.927 0l2.184 4.327 4.898.696c.441.062.612.636.282.95l-3.522 3.356.83 4.73c.078.443-.36.79-.746.592L8 13.187l-4.389 2.256z\"/>" +
                        "</svg>" +
                        "<svg xmlns =\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-star-fill\" viewBox=\"0 0 16 16\">" +
                            "<path d =\"M3.612 15.443c-.386.198-.824-.149-.746-.592l.83-4.73L.173 6.765c-.329-.314-.158-.888.283-.95l4.898-.696L7.538.792c.197-.39.73-.39.927 0l2.184 4.327 4.898.696c.441.062.612.636.282.95l-3.522 3.356.83 4.73c.078.443-.36.79-.746.592L8 13.187l-4.389 2.256z\"/>" +
                        "</svg>" +
                        "<svg xmlns =\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-star\" viewBox=\"0 0 16 16\"> " +
                            "< path d =\"M2.866 14.85c-.078.444.36.791.746.593l4.39-2.256 4.389 2.256c.386.198.824-.149.746-.592l-.83-4.73 3.522-3.356c.33-.314.16-.888-.282-.95l-4.898-.696L8.465.792a.513.513 0 0 0-.927 0L5.354 5.12l-4.898.696c-.441.062-.612.636-.283.95l3.523 3.356-.83 4.73zm4.905-2.767-3.686 1.894.694-3.957a.565.565 0 0 0-.163-.505L1.71 6.745l4.052-.576a.525.525 0 0 0 .393-.288L8 2.223l1.847 3.658a.525.525 0 0 0 .393.288l4.052.575-2.906 2.77a.565.565 0 0 0-.163.506l.694 3.957-3.686-1.894a.503.503 0 0 0-.461 0z\"/> " +
                        "</svg> " +
                        "<svg xmlns =\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-star\" viewBox=\"0 0 16 16\"> " +
                            "< path d =\"M2.866 14.85c-.078.444.36.791.746.593l4.39-2.256 4.389 2.256c.386.198.824-.149.746-.592l-.83-4.73 3.522-3.356c.33-.314.16-.888-.282-.95l-4.898-.696L8.465.792a.513.513 0 0 0-.927 0L5.354 5.12l-4.898.696c-.441.062-.612.636-.283.95l3.523 3.356-.83 4.73zm4.905-2.767-3.686 1.894.694-3.957a.565.565 0 0 0-.163-.505L1.71 6.745l4.052-.576a.525.525 0 0 0 .393-.288L8 2.223l1.847 3.658a.525.525 0 0 0 .393.288l4.052.575-2.906 2.77a.565.565 0 0 0-.163.506l.694 3.957-3.686-1.894a.503.503 0 0 0-.461 0z\"/> " +
                        "</svg> ";
                }
                else if (mediaNotaProduto < 4)
                {
                    res =
                        "<svg xmlns =\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-star-fill\" viewBox=\"0 0 16 16\">" +
                            "<path d =\"M3.612 15.443c-.386.198-.824-.149-.746-.592l.83-4.73L.173 6.765c-.329-.314-.158-.888.283-.95l4.898-.696L7.538.792c.197-.39.73-.39.927 0l2.184 4.327 4.898.696c.441.062.612.636.282.95l-3.522 3.356.83 4.73c.078.443-.36.79-.746.592L8 13.187l-4.389 2.256z\"/>" +
                        "</svg>" +
                        "<svg xmlns =\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-star-fill\" viewBox=\"0 0 16 16\">" +
                            "<path d =\"M3.612 15.443c-.386.198-.824-.149-.746-.592l.83-4.73L.173 6.765c-.329-.314-.158-.888.283-.95l4.898-.696L7.538.792c.197-.39.73-.39.927 0l2.184 4.327 4.898.696c.441.062.612.636.282.95l-3.522 3.356.83 4.73c.078.443-.36.79-.746.592L8 13.187l-4.389 2.256z\"/>" +
                        "</svg>" +
                        "<svg xmlns =\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-star-fill\" viewBox=\"0 0 16 16\">" +
                            "<path d =\"M3.612 15.443c-.386.198-.824-.149-.746-.592l.83-4.73L.173 6.765c-.329-.314-.158-.888.283-.95l4.898-.696L7.538.792c.197-.39.73-.39.927 0l2.184 4.327 4.898.696c.441.062.612.636.282.95l-3.522 3.356.83 4.73c.078.443-.36.79-.746.592L8 13.187l-4.389 2.256z\"/>" +
                        "</svg>" +
                        "<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-star-half\" viewBox=\"0 0 16 16\">" +
                            "<path d =\"M5.354 5.119 7.538.792A.516.516 0 0 1 8 .5c.183 0 .366.097.465.292l2.184 4.327 4.898.696A.537.537 0 0 1 16 6.32a.548.548 0 0 1-.17.445l-3.523 3.356.83 4.73c.078.443-.36.79-.746.592L8 13.187l-4.389 2.256a.52.52 0 0 1-.146.05c-.342.06-.668-.254-.6-.642l.83-4.73L.173 6.765a.55.55 0 0 1-.172-.403.58.58 0 0 1 .085-.302.513.513 0 0 1 .37-.245l4.898-.696zM8 12.027a.5.5 0 0 1 .232.056l3.686 1.894-.694-3.957a.565.565 0 0 1 .162-.505l2.907-2.77-4.052-.576a.525.525 0 0 1-.393-.288L8.001 2.223 8 2.226v9.8z\"/>" +
                        "</svg>" +
                        "<svg xmlns =\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-star\" viewBox=\"0 0 16 16\"> " +
                            "< path d =\"M2.866 14.85c-.078.444.36.791.746.593l4.39-2.256 4.389 2.256c.386.198.824-.149.746-.592l-.83-4.73 3.522-3.356c.33-.314.16-.888-.282-.95l-4.898-.696L8.465.792a.513.513 0 0 0-.927 0L5.354 5.12l-4.898.696c-.441.062-.612.636-.283.95l3.523 3.356-.83 4.73zm4.905-2.767-3.686 1.894.694-3.957a.565.565 0 0 0-.163-.505L1.71 6.745l4.052-.576a.525.525 0 0 0 .393-.288L8 2.223l1.847 3.658a.525.525 0 0 0 .393.288l4.052.575-2.906 2.77a.565.565 0 0 0-.163.506l.694 3.957-3.686-1.894a.503.503 0 0 0-.461 0z\"/> " +
                        "</svg> ";
                }
                else if (mediaNotaProduto == 4)
                {
                    res =
                        "<svg xmlns =\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-star-fill\" viewBox=\"0 0 16 16\">" +
                            "<path d =\"M3.612 15.443c-.386.198-.824-.149-.746-.592l.83-4.73L.173 6.765c-.329-.314-.158-.888.283-.95l4.898-.696L7.538.792c.197-.39.73-.39.927 0l2.184 4.327 4.898.696c.441.062.612.636.282.95l-3.522 3.356.83 4.73c.078.443-.36.79-.746.592L8 13.187l-4.389 2.256z\"/>" +
                        "</svg>" +
                        "<svg xmlns =\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-star-fill\" viewBox=\"0 0 16 16\">" +
                            "<path d =\"M3.612 15.443c-.386.198-.824-.149-.746-.592l.83-4.73L.173 6.765c-.329-.314-.158-.888.283-.95l4.898-.696L7.538.792c.197-.39.73-.39.927 0l2.184 4.327 4.898.696c.441.062.612.636.282.95l-3.522 3.356.83 4.73c.078.443-.36.79-.746.592L8 13.187l-4.389 2.256z\"/>" +
                        "</svg>" +
                        "<svg xmlns =\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-star-fill\" viewBox=\"0 0 16 16\">" +
                            "<path d =\"M3.612 15.443c-.386.198-.824-.149-.746-.592l.83-4.73L.173 6.765c-.329-.314-.158-.888.283-.95l4.898-.696L7.538.792c.197-.39.73-.39.927 0l2.184 4.327 4.898.696c.441.062.612.636.282.95l-3.522 3.356.83 4.73c.078.443-.36.79-.746.592L8 13.187l-4.389 2.256z\"/>" +
                        "</svg>" +
                        "<svg xmlns =\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-star-fill\" viewBox=\"0 0 16 16\">" +
                            "<path d =\"M3.612 15.443c-.386.198-.824-.149-.746-.592l.83-4.73L.173 6.765c-.329-.314-.158-.888.283-.95l4.898-.696L7.538.792c.197-.39.73-.39.927 0l2.184 4.327 4.898.696c.441.062.612.636.282.95l-3.522 3.356.83 4.73c.078.443-.36.79-.746.592L8 13.187l-4.389 2.256z\"/>" +
                        "</svg>" +
                        "<svg xmlns =\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-star\" viewBox=\"0 0 16 16\"> " +
                            "< path d =\"M2.866 14.85c-.078.444.36.791.746.593l4.39-2.256 4.389 2.256c.386.198.824-.149.746-.592l-.83-4.73 3.522-3.356c.33-.314.16-.888-.282-.95l-4.898-.696L8.465.792a.513.513 0 0 0-.927 0L5.354 5.12l-4.898.696c-.441.062-.612.636-.283.95l3.523 3.356-.83 4.73zm4.905-2.767-3.686 1.894.694-3.957a.565.565 0 0 0-.163-.505L1.71 6.745l4.052-.576a.525.525 0 0 0 .393-.288L8 2.223l1.847 3.658a.525.525 0 0 0 .393.288l4.052.575-2.906 2.77a.565.565 0 0 0-.163.506l.694 3.957-3.686-1.894a.503.503 0 0 0-.461 0z\"/> " +
                        "</svg> ";
                }
                else if (mediaNotaProduto < 5)
                {
                    res =
                        "<svg xmlns =\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-star-fill\" viewBox=\"0 0 16 16\">" +
                            "<path d =\"M3.612 15.443c-.386.198-.824-.149-.746-.592l.83-4.73L.173 6.765c-.329-.314-.158-.888.283-.95l4.898-.696L7.538.792c.197-.39.73-.39.927 0l2.184 4.327 4.898.696c.441.062.612.636.282.95l-3.522 3.356.83 4.73c.078.443-.36.79-.746.592L8 13.187l-4.389 2.256z\"/>" +
                        "</svg>" +
                        "<svg xmlns =\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-star-fill\" viewBox=\"0 0 16 16\">" +
                            "<path d =\"M3.612 15.443c-.386.198-.824-.149-.746-.592l.83-4.73L.173 6.765c-.329-.314-.158-.888.283-.95l4.898-.696L7.538.792c.197-.39.73-.39.927 0l2.184 4.327 4.898.696c.441.062.612.636.282.95l-3.522 3.356.83 4.73c.078.443-.36.79-.746.592L8 13.187l-4.389 2.256z\"/>" +
                        "</svg>" +
                        "<svg xmlns =\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-star-fill\" viewBox=\"0 0 16 16\">" +
                            "<path d =\"M3.612 15.443c-.386.198-.824-.149-.746-.592l.83-4.73L.173 6.765c-.329-.314-.158-.888.283-.95l4.898-.696L7.538.792c.197-.39.73-.39.927 0l2.184 4.327 4.898.696c.441.062.612.636.282.95l-3.522 3.356.83 4.73c.078.443-.36.79-.746.592L8 13.187l-4.389 2.256z\"/>" +
                        "</svg>" +
                        "<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-star-half\" viewBox=\"0 0 16 16\">" +
                            "<path d =\"M5.354 5.119 7.538.792A.516.516 0 0 1 8 .5c.183 0 .366.097.465.292l2.184 4.327 4.898.696A.537.537 0 0 1 16 6.32a.548.548 0 0 1-.17.445l-3.523 3.356.83 4.73c.078.443-.36.79-.746.592L8 13.187l-4.389 2.256a.52.52 0 0 1-.146.05c-.342.06-.668-.254-.6-.642l.83-4.73L.173 6.765a.55.55 0 0 1-.172-.403.58.58 0 0 1 .085-.302.513.513 0 0 1 .37-.245l4.898-.696zM8 12.027a.5.5 0 0 1 .232.056l3.686 1.894-.694-3.957a.565.565 0 0 1 .162-.505l2.907-2.77-4.052-.576a.525.525 0 0 1-.393-.288L8.001 2.223 8 2.226v9.8z\"/>" +
                        "</svg>" +
                        "<svg xmlns =\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-star\" viewBox=\"0 0 16 16\"> " +
                            "< path d =\"M2.866 14.85c-.078.444.36.791.746.593l4.39-2.256 4.389 2.256c.386.198.824-.149.746-.592l-.83-4.73 3.522-3.356c.33-.314.16-.888-.282-.95l-4.898-.696L8.465.792a.513.513 0 0 0-.927 0L5.354 5.12l-4.898.696c-.441.062-.612.636-.283.95l3.523 3.356-.83 4.73zm4.905-2.767-3.686 1.894.694-3.957a.565.565 0 0 0-.163-.505L1.71 6.745l4.052-.576a.525.525 0 0 0 .393-.288L8 2.223l1.847 3.658a.525.525 0 0 0 .393.288l4.052.575-2.906 2.77a.565.565 0 0 0-.163.506l.694 3.957-3.686-1.894a.503.503 0 0 0-.461 0z\"/> " +
                        "</svg> ";
                }
                else if (mediaNotaProduto == 5)
                {
                    res =
                        "<svg xmlns =\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-star-fill\" viewBox=\"0 0 16 16\">" +
                            "<path d =\"M3.612 15.443c-.386.198-.824-.149-.746-.592l.83-4.73L.173 6.765c-.329-.314-.158-.888.283-.95l4.898-.696L7.538.792c.197-.39.73-.39.927 0l2.184 4.327 4.898.696c.441.062.612.636.282.95l-3.522 3.356.83 4.73c.078.443-.36.79-.746.592L8 13.187l-4.389 2.256z\"/>" +
                        "</svg>" +
                        "<svg xmlns =\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-star-fill\" viewBox=\"0 0 16 16\">" +
                            "<path d =\"M3.612 15.443c-.386.198-.824-.149-.746-.592l.83-4.73L.173 6.765c-.329-.314-.158-.888.283-.95l4.898-.696L7.538.792c.197-.39.73-.39.927 0l2.184 4.327 4.898.696c.441.062.612.636.282.95l-3.522 3.356.83 4.73c.078.443-.36.79-.746.592L8 13.187l-4.389 2.256z\"/>" +
                        "</svg>" +
                        "<svg xmlns =\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-star-fill\" viewBox=\"0 0 16 16\">" +
                            "<path d =\"M3.612 15.443c-.386.198-.824-.149-.746-.592l.83-4.73L.173 6.765c-.329-.314-.158-.888.283-.95l4.898-.696L7.538.792c.197-.39.73-.39.927 0l2.184 4.327 4.898.696c.441.062.612.636.282.95l-3.522 3.356.83 4.73c.078.443-.36.79-.746.592L8 13.187l-4.389 2.256z\"/>" +
                        "</svg>" +
                        "<svg xmlns =\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-star-fill\" viewBox=\"0 0 16 16\">" +
                            "<path d =\"M3.612 15.443c-.386.198-.824-.149-.746-.592l.83-4.73L.173 6.765c-.329-.314-.158-.888.283-.95l4.898-.696L7.538.792c.197-.39.73-.39.927 0l2.184 4.327 4.898.696c.441.062.612.636.282.95l-3.522 3.356.83 4.73c.078.443-.36.79-.746.592L8 13.187l-4.389 2.256z\"/>" +
                        "</svg>" +
                        "<svg xmlns =\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-star-fill\" viewBox=\"0 0 16 16\">" +
                            "<path d =\"M3.612 15.443c-.386.198-.824-.149-.746-.592l.83-4.73L.173 6.765c-.329-.314-.158-.888.283-.95l4.898-.696L7.538.792c.197-.39.73-.39.927 0l2.184 4.327 4.898.696c.441.062.612.636.282.95l-3.522 3.356.83 4.73c.078.443-.36.79-.746.592L8 13.187l-4.389 2.256z\"/>" +
                        "</svg>";
                } 
                else
                {
                    res =
                        "<svg xmlns=\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-star-half\" viewBox=\"0 0 16 16\">" +
                            "<path d =\"M5.354 5.119 7.538.792A.516.516 0 0 1 8 .5c.183 0 .366.097.465.292l2.184 4.327 4.898.696A.537.537 0 0 1 16 6.32a.548.548 0 0 1-.17.445l-3.523 3.356.83 4.73c.078.443-.36.79-.746.592L8 13.187l-4.389 2.256a.52.52 0 0 1-.146.05c-.342.06-.668-.254-.6-.642l.83-4.73L.173 6.765a.55.55 0 0 1-.172-.403.58.58 0 0 1 .085-.302.513.513 0 0 1 .37-.245l4.898-.696zM8 12.027a.5.5 0 0 1 .232.056l3.686 1.894-.694-3.957a.565.565 0 0 1 .162-.505l2.907-2.77-4.052-.576a.525.525 0 0 1-.393-.288L8.001 2.223 8 2.226v9.8z\"/>" +
                        "</svg>" +
                        "<svg xmlns =\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-star\" viewBox=\"0 0 16 16\"> " +
                            "< path d =\"M2.866 14.85c-.078.444.36.791.746.593l4.39-2.256 4.389 2.256c.386.198.824-.149.746-.592l-.83-4.73 3.522-3.356c.33-.314.16-.888-.282-.95l-4.898-.696L8.465.792a.513.513 0 0 0-.927 0L5.354 5.12l-4.898.696c-.441.062-.612.636-.283.95l3.523 3.356-.83 4.73zm4.905-2.767-3.686 1.894.694-3.957a.565.565 0 0 0-.163-.505L1.71 6.745l4.052-.576a.525.525 0 0 0 .393-.288L8 2.223l1.847 3.658a.525.525 0 0 0 .393.288l4.052.575-2.906 2.77a.565.565 0 0 0-.163.506l.694 3.957-3.686-1.894a.503.503 0 0 0-.461 0z\"/> " +
                        "</svg> " +
                        "<svg xmlns =\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-star\" viewBox=\"0 0 16 16\"> " +
                            "< path d =\"M2.866 14.85c-.078.444.36.791.746.593l4.39-2.256 4.389 2.256c.386.198.824-.149.746-.592l-.83-4.73 3.522-3.356c.33-.314.16-.888-.282-.95l-4.898-.696L8.465.792a.513.513 0 0 0-.927 0L5.354 5.12l-4.898.696c-.441.062-.612.636-.283.95l3.523 3.356-.83 4.73zm4.905-2.767-3.686 1.894.694-3.957a.565.565 0 0 0-.163-.505L1.71 6.745l4.052-.576a.525.525 0 0 0 .393-.288L8 2.223l1.847 3.658a.525.525 0 0 0 .393.288l4.052.575-2.906 2.77a.565.565 0 0 0-.163.506l.694 3.957-3.686-1.894a.503.503 0 0 0-.461 0z\"/> " +
                        "</svg> " +
                        "<svg xmlns =\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-star\" viewBox=\"0 0 16 16\"> " +
                            "< path d =\"M2.866 14.85c-.078.444.36.791.746.593l4.39-2.256 4.389 2.256c.386.198.824-.149.746-.592l-.83-4.73 3.522-3.356c.33-.314.16-.888-.282-.95l-4.898-.696L8.465.792a.513.513 0 0 0-.927 0L5.354 5.12l-4.898.696c-.441.062-.612.636-.283.95l3.523 3.356-.83 4.73zm4.905-2.767-3.686 1.894.694-3.957a.565.565 0 0 0-.163-.505L1.71 6.745l4.052-.576a.525.525 0 0 0 .393-.288L8 2.223l1.847 3.658a.525.525 0 0 0 .393.288l4.052.575-2.906 2.77a.565.565 0 0 0-.163.506l.694 3.957-3.686-1.894a.503.503 0 0 0-.461 0z\"/> " +
                        "</svg> " +
                        "<svg xmlns =\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-star\" viewBox=\"0 0 16 16\"> " +
                            "< path d =\"M2.866 14.85c-.078.444.36.791.746.593l4.39-2.256 4.389 2.256c.386.198.824-.149.746-.592l-.83-4.73 3.522-3.356c.33-.314.16-.888-.282-.95l-4.898-.696L8.465.792a.513.513 0 0 0-.927 0L5.354 5.12l-4.898.696c-.441.062-.612.636-.283.95l3.523 3.356-.83 4.73zm4.905-2.767-3.686 1.894.694-3.957a.565.565 0 0 0-.163-.505L1.71 6.745l4.052-.576a.525.525 0 0 0 .393-.288L8 2.223l1.847 3.658a.525.525 0 0 0 .393.288l4.052.575-2.906 2.77a.565.565 0 0 0-.163.506l.694 3.957-3.686-1.894a.503.503 0 0 0-.461 0z\"/> " +
                        "</svg> ";
                }

                return res;
            }
            catch (Exception ex)
            {
                DivAlert.Visible = true;
                LblAlert.InnerHtml = ex.Message;
                return ex.Message;
            }
        }

        protected string GetQuantidadeComentarios(string id)
        {
            DataTable dt = new DataTable();

            string qry = "SELECT produtoId, COUNT(*) as qtd from comentario WHERE produtoId = " + id;
            dt = db.ExecuteReader(qry);

            return dt.Rows[0][1].ToString();
        }
    }
}