using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication2.Controller;
using WebApplication2.Model;

namespace WebApplication2.View
{
    public partial class HomeLoja : System.Web.UI.Page
    {

        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DropdownContaMenu.InnerHtml = "<li>" +
                                                 "<button type=\"button\" id =\"BtnPaginaLoginCliente\" runat =\"server\" onclick=\"BtnLoginCliente()\"> Login </ button>" +
                                              "</ li>";
                await GetJogos("https://api.rawg.io/api/games?key=81efaba6814e40658121d32daf691878");
                await GetDesenvolvedorasDeJogo("https://api.rawg.io/api/publishers?key=81efaba6814e40658121d32daf691878");
            }
            else
            {
                DropdownContaMenu.InnerHtml = "<li>" +
                                                 "<button type=\"button\" id =\"BtnPaginaLoginCliente\" runat =\"server\" onclick=\"BtnLoginCliente()\"> Login </ button>" +
                                              "</ li>";
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

        protected void BtnDirecionaParaCadeirasGamer_Click(object sender, EventArgs e)
        {
            Session["filtroDepartamento"] = "Cadeiras";
            Response.Redirect("Loja.aspx");
        }

        protected void BtnDirecionaParaMonitores_Click(object sender, EventArgs e)
        {
            Session["filtroDepartamento"] = "Monitores";
            Response.Redirect("Loja.aspx");
        }

        protected void BtnDirecionaParaHardwares_Click(object sender, EventArgs e)
        {
            Session["filtroDepartamento"] = "Hardwares";
            Response.Redirect("Loja.aspx");
        }

        protected void BtnDirecionaParaPerifericos_Click(object sender, EventArgs e)
        {
            Session["filtroDepartamento"] = "Periféricos";
            Response.Redirect("Loja.aspx");
        }

        protected void BtnDirecionaParaRefrigeracao_Click(object sender, EventArgs e)
        {
            Session["filtroDepartamento"] = "Refrigeração";
            Response.Redirect("Loja.aspx");
        }

        protected void BtnDirecionaParaGabinetes_Click(object sender, EventArgs e)
        {
            Session["filtroDepartamento"] = "Gabinetes";
            Response.Redirect("Loja.aspx");
        }

        [WebMethod]
        public static string MostraProdutoSelecionado(string id)
        {
            HttpContext.Current.Session["idProduto"] = id;
            return "ComprarProduto.aspx";
        }

        protected async Task GetJogos(string uri)
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(uri))
                {
                    string JogoJsonString = await response.Content.ReadAsStringAsync();
                    JogoJsonString = "[" + JogoJsonString + "]";
                    JsonRespostaApiRawgListOfGames[] jsonRespostaListOfGames = JsonConvert.DeserializeObject<JsonRespostaApiRawgListOfGames[]>(JogoJsonString);
                    LoadJogos(jsonRespostaListOfGames);
                }
            }
        }

        protected void LoadJogos(JsonRespostaApiRawgListOfGames[] jsonRespostaListOfGames)
        {
            try
            {
                for (int i = 0; i < 6; i++)
                {
                    DivLojaJogos.InnerHtml += "<div class=\"col-lg-4\">" +
                                                "<img class=\"ImgLojaJogos\" src=\"" + jsonRespostaListOfGames[0].Results[i].Background_Image + "\" />" +
                                                "<h2 class=\"fw-normal\">" + jsonRespostaListOfGames[0].Results[i].Name + "</h2>" +
                                             "</div>";
                }

                DivLojaJogos.InnerHtml += "<button type=\"button\" class=\"btn btn-secondary BtnDirecionaParaLojaJogos\" onclick=\"BtnShowJogos()\">Veja Mais Jogos</button>";
            }
            catch (Exception ex)
            {
                DivAlert.Visible = true;
                LblAlert.InnerHtml = ex.Message;
            }
        }

        protected async Task GetDesenvolvedorasDeJogo(string uri)
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(uri))
                {
                    string JogoDesenvolvedorasJsonString = await response.Content.ReadAsStringAsync();
                    JogoDesenvolvedorasJsonString = "[" + JogoDesenvolvedorasJsonString + "]";
                    JsonRespostaApiRawgListOfPublishers[] jsonRespostaListOfPublishers = JsonConvert.DeserializeObject<JsonRespostaApiRawgListOfPublishers[]>(JogoDesenvolvedorasJsonString);
                    LoadDesenvolvedorasDeJogo(jsonRespostaListOfPublishers);
                }
            }
        }

        protected void LoadDesenvolvedorasDeJogo(JsonRespostaApiRawgListOfPublishers[] jsonRespostaListOfPublishers)
        {
            try 
            {
                DivDesenvolvedorasDeJogo.InnerHtml = "<h1>Desenvolvedoras de Jogos</h1>";

                for (int i = 0; i < jsonRespostaListOfPublishers[0].Results.Count; i++)
                {
                    DivDesenvolvedorasDeJogo.InnerHtml += "<div class=\"col DivColDesenvolvedorasDeJogo mx-3\">" +
                                                            "<img id=\"ImgDevelopmentTeam\" src=\"" + jsonRespostaListOfPublishers[0].Results[i].Image_Background + "\" />" +
                                                            "<p>" + jsonRespostaListOfPublishers[0].Results[i].Name + "</p>" +
                                                          "</div>";
                }

                DivDesenvolvedorasDeJogo.InnerHtml += "<div class=\"col DivColDesenvolvedorasDeJogo mx-3 t\">" +
                                                        "<button type=\"button\" class=\"btn btn-secondary BtnShowDesenvolvedoras\" onclick=\"BtnShowDesenvolvedoras()\">Descubra mais</button" + 
                                                     "</div>";
            }
            catch (Exception ex)
            {
                DivAlert.Visible = true;
                LblAlert.InnerHtml = ex.Message;
            }
        }

        [WebMethod]
        public static string BtnShowJogos()
        {
            return "LojaJogos.aspx";
        }

        [WebMethod]
        public static string BtnShowDesenvolvedoras()
        {
            return "HomePublishers.aspx";
        }
    }
}