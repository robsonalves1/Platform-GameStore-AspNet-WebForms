using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using WebApplication2.Controller;
using WebApplication2.Model;

namespace WebApplication2.View
{
    public partial class Loja : System.Web.UI.Page
    {
        DataTable dt = new DataTable();
        CntrDB db = new CntrDB();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DropdownContaMenu.InnerHtml = "<li>" +
                                                 "<button type=\"button\" id =\"BtnPaginaLoginCliente\" runat =\"server\" onclick=\"BtnLoginCliente()\"> Login </ button>" +
                                              "</ li>";

                DivTodosProdutos.InnerHtml = GetProduto((string)Session["filtro"]);
                //DivFiltros.InnerHtml = LoadFiltro();
            }
            else
            {
                DropdownContaMenu.InnerHtml = "<li>" +
                                                 "<button type=\"button\" id =\"BtnPaginaLoginCliente\" runat =\"server\" onclick=\"BtnLoginCliente()\"> Login </ button>" +
                                              "</ li>";

                DivTodosProdutos.InnerHtml = GetProduto((string)Session["filtro"]);
                //DivFiltros.InnerHtml = LoadFiltro();
            }

            if (Session["clienteLogado"] != null)
            {
                Cliente c = new Cliente();
                c = (Cliente)Session["clienteLogado"];

                DropdownConta.InnerHtml = c.Nome + "<span><svg xmlns=\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-person-circle\" viewBox=\"0 0 16 16\">" +
                                                    "<path d=\"M11 6a3 3 0 1 1-6 0 3 3 0 0 1 6 0z\" />" +
                                                    "<path fill-rule=\"evenodd\" d=\"M0 8a8 8 0 1 1 16 0A8 8 0 0 1 0 8zm8-7a7 7 0 0 0-5.468 11.37C3.242 11.226 4.805 10 8 10s4.757 1.225 5.468 2.37A7 7 0 0 0 8 1z\" />"+
                                                    "</ svg>▼</span>";
                DropdownContaMenu.InnerHtml = "<li>" +
                                                    "<button type=\"button\" id=\"BtnAcessaContaCliente\" runat=\"server\" ><a href=\"HomeContaCliente.aspx\">Conta</a></ button>" +
                                                    "<button type=\"button\" id=\"BtnLogoutContaCliente\" runat=\"server\" onclick=\"BtnLogoutCliente()\">Sair</ button>" +
                                              "</ li>";
            }
        }

        protected string GetProduto(string filter)
        {
            string ret = "";

            try
            {
                string query = "";
                if ((string)Session["filtroDepartamento"] == null)
                {
                    Response.Redirect("HomeLoja.aspx");
                }
                else if (filter == null)
                {
                    query = "SELECT " +
                                   "produtoId, produtoCodigo, produtoQuantidade, produtoNome, " +
                                   "Concat('R$ ', " +
                                       "Replace " +
                                         "(Replace" +
                                           "(Replace" +
                                             "(Format(produtoPreco, 2), '.', '|'), ',', '.'), '|', ',')) AS produtoPreco, " +
                                   "produtoFoto " +
                                   "FROM produto" +
                                   " WHERE produtoDepartamento = '" + (string)Session["filtroDepartamento"] + "'";
                }
                else
                {
                    query = "SELECT " +
                                   "produtoId, produtoCodigo, produtoQuantidade, produtoNome, " +
                                   "Concat('R$ ', " +
                                       "Replace " +
                                         "(Replace" +
                                           "(Replace" +
                                             "(Format(produtoPreco, 2), '.', '|'), ',', '.'), '|', ',')) AS produtoPreco, " +
                                   "produtoFoto " +
                                   "FROM produto " +
                                   filter +
                                   " AND produtoDepartamento = '" + (string)Session["filtroDepartamento"] + "'";
                }

                dt = db.ExecuteReader(query);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string foto = "";
                    byte[] bytes = (byte[])(byte[])dt.Rows[i][5];
                    if (bytes != null && bytes.Length > 0)
                    {
                        string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
                        string b = Convert.ToString(bytes);
                        foto = "data:image/jpg;base64," + base64String;
                    }

                    ret +=
                        "<div class=\"col\" id=\"DivProdutos\" runat=\"server\">" +
                            "<div class=\"card shadow-sm\" id=\"cardProduto\" style=\"height: 514px; \">" +
                                "<img src=\"" + foto + "\" id=\"FotoProduto\" />" +
                                "<div class=\"card-body\" id=\"cardProdutoBody\">" +
                                    "<p class=\"card-text\" id=\"NomeProduto\" runat=\"server\">" + dt.Rows[i][3] + "</p>" +
                                    "<h1 id=\"PrecoProduto\" runat=\"server\">" + dt.Rows[i][4] + "</h1>" +
                                "</div>" +
                                "<button class=\"btn btn-dark m-3\" type=\"button\" id=\"btnComprarProduto\" onclick=\"BtnExibirProduto(" + dt.Rows[i][0] + ")\">Detalhes</button>" +
                            "</div>" +
                        "</div>";
                }

                return ret;
            }
            catch (Exception ex)
            {
                LblAlert.InnerText = ex.Message;
                DivAlert.Visible = true;

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
        public static string FiltraProdutos(string filtro)
        {
            Loja l = new Loja();

            string[] filtroSplit = filtro.Split('=');
            string produto = filtroSplit[1].Trim();

            string filtroQry = filtroSplit[0] + " = \"" + produto + "\"";

            HttpContext.Current.Session["filtro"] = filtroQry;
            
            return l.GetProduto(filtroQry);
        }

        [WebMethod]
        public static string MostraProdutoSelecionado(string id)
        {
            HttpContext.Current.Session["idProduto"] = id;
            return "ComprarProduto.aspx";
        }

        [WebMethod]
        public static string SpanProdutosFiltro(string filtro)
        {
            Loja l = new Loja();
            return l.GetProduto(filtro);
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
        public static string LimpaResultadoPesquisaProduto()
        {
            return "Lost focus!";
        }

        protected void BtnSearchBar_Click(object sender, EventArgs e)
        {
            try
            {
                string txtSearch = InputSearchBar.Value.Trim();
                string filter = "WHERE produtoNome LIKE '%" + txtSearch + "%'";
                GetProduto(filter);
            }
            catch (Exception ex)
            {
                LblAlert.InnerHtml = ex.Message;
                DivAlert.Visible = true;
            }
        }

        protected void BtnLimparFiltro_Click(object sender, EventArgs e)
        {
            Response.Redirect(Request.RawUrl);
        }

        protected void FilterProdutos_Click(object sender, EventArgs e)
        {
            HtmlButton btn = new HtmlButton();
            btn = sender as HtmlButton;
            string filter = btn.Attributes["value"].Trim();
            DivTodosProdutos.InnerHtml = GetProduto(filter);
        }
    }
}