using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication2.Model;

namespace WebApplication2.View
{
    public partial class LojaJogos : System.Web.UI.Page
    {
        string URI = "https://api.rawg.io/api/games?key=81efaba6814e40658121d32daf691878&page=";
        JsonRespostaApiRawgListOfGames[] jsonResposta = null;

        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DropdownContaMenu.InnerHtml = "<li>" +
                                                 "<button type=\"button\" id =\"BtnPaginaLoginCliente\" runat =\"server\" onclick=\"BtnLoginClient()\">Login</ button>" +
                                              "</ li>";

                DivIconApagaResultados.Visible = false;

                await GetAllGames(URI + 1);
                Session["currentPage"] = 1;
                DivResultados.Visible = false;

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
                                                        "<button type=\"button\" id=\"BtnLogoutContaCliente\" runat=\"server\" onclick=\"BtnLogoutClient()\">Sair</ button>" +
                                                  "</ li>";
                }
            }
        }

        [WebMethod]
        public static string BtnLoginClient()
        {
            return "LoginCliente.aspx";
        }

        [WebMethod]
        public static void BtnLogoutClient()
        {
            HttpContext.Current.Session["clienteLogado"] = null;
        }

        protected async Task GetAllGames(string uri)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    using (HttpResponseMessage response = await client.GetAsync(uri))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            string JogoJsonString = await response.Content.ReadAsStringAsync();
                            JogoJsonString = "[" + JogoJsonString + "]";
                            jsonResposta = JsonConvert.DeserializeObject<JsonRespostaApiRawgListOfGames[]>(JogoJsonString);
                            ShowAllGames(jsonResposta);
                        }
                        else
                        {
                            DivAlert.Visible = true;
                            LblAlert.InnerHtml = "Erro: " + response.StatusCode.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DivAlert.Visible = true;
                LblAlert.InnerHtml = ex.Message;
            }
        }

        protected void ShowAllGames(JsonRespostaApiRawgListOfGames[] jsonRespostaJogos)
        {
            try
            {
                DivJogos.InnerHtml = "";

                string nameJogo = "";
                string imgJogo = "";
                int idJogo = 0;

                for (int i = 0; i < jsonRespostaJogos[0].Results.Count; i++)
                {
                    nameJogo = jsonRespostaJogos[0].Results[i].Name;
                    imgJogo = jsonRespostaJogos[0].Results[i].Background_Image;
                    idJogo = jsonRespostaJogos[0].Results[i].Id;

                    DivJogos.InnerHtml += "<div class=\"col\">" +
                                                "<div class=\"card shadow-sm\">" +
                                                    "<img src=\"" + imgJogo + "\" class=\"d - block w - 100\" alt=\"imagem jogos\" style=\"height: 350px; object-fit: cover; \" />" +
                                                    "<div class=\"card-body DivCardBodyJogos\">" +
                                                        "<h5 class=\"card-text\">" + nameJogo + "</h5>" +
                                                    "</div>" +
                                                    "<button type=\"button\" class=\"btn btn-dark m-2 BtnShowSelectedGame\" onclick=\"BtnShowSelectedGame(" + idJogo + ")\">Detalhes</button>" + 
                                                "</div>" +
                                            "</div>";
                }
            }
            catch (Exception ex)
            {
                DivAlert.Visible = true;
                LblAlert.InnerHtml = ex.Message;
            }
        }

        protected async Task GetListSearchGame(string filter)
        {
            string URIDetails = "https://api.rawg.io/api/games?key=81efaba6814e40658121d32daf691878&search=" + filter;

            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(URIDetails))
                {
                    string JogoJsonStringDetails = await response.Content.ReadAsStringAsync();
                    JogoJsonStringDetails = "[" + JogoJsonStringDetails + "]";
                    jsonResposta = JsonConvert.DeserializeObject<JsonRespostaApiRawgListOfGames[]>(JogoJsonStringDetails);
                    ShowListSearchGame(jsonResposta);
                }
            }
        }

        protected void ShowListSearchGame(JsonRespostaApiRawgListOfGames[] jsonResposta)
        {
            try
            {
                DivResultados.InnerHtml = "";
                DivResultados.Visible = true;
                DivResultados.Attributes["style"] = "visibility: visible";


                for (int i = 0; i < 5; i++)
                {
                    string bgImage = jsonResposta[0].Results[i].Background_Image;
                    string name = jsonResposta[0].Results[i].Name;
                    int id = jsonResposta[0].Results[i].Id;

                    DivResultados.InnerHtml += "<button type=\"button\" onclick=\"BtnShowSelectedGame(" + id + ")\">" +
                                                    "<img class=\"imgProdutoBarraDePesquisa\" src=\"" + bgImage + "\" />" +
                                                    name + 
                                                "</button>";
                }

            }
            catch (Exception ex)
            {
                DivAlert.Visible = true;
                LblAlert.InnerHtml = ex.Message;
            }
        }

        [WebMethod]
        public static string BtnShowSelectedGame(string id)
        {
            HttpContext.Current.Session["idSelectedJogo"] = id;
            return "ComprarJogo.aspx";
        }

        [WebMethod]
        public static async Task<string> ShowSearchedGame(string filtro)
        {
            JsonRespostaApiRawgListOfGames[] jsonRespostaApiRawgListOfGames = null;
            string res = "";

            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync("https://api.rawg.io/api/games?key=81efaba6814e40658121d32daf691878&search=" + filtro))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        string JogoJsonSearchString = await response.Content.ReadAsStringAsync();
                        JogoJsonSearchString = "[" + JogoJsonSearchString + "]";
                        jsonRespostaApiRawgListOfGames = JsonConvert.DeserializeObject<JsonRespostaApiRawgListOfGames[]>(JogoJsonSearchString);

                        for (int i = 0; i < jsonRespostaApiRawgListOfGames[0].Results.Count; i++)
                        {
                            res += "<button type=\"button\">" +
                                        jsonRespostaApiRawgListOfGames[0].Results[i].Name +
                                    "<button>";
                        }
                    }
                        
                    return res;
                }
            }
        }

        protected async void BtnPreviusPage_Click(object sender, EventArgs e)
        {
            Session["currentPage"] = (int)Session["currentPage"] - 1;
            await GetAllGames(URI + Session["currentPage"]);
        }

        protected async void BtnNextPage_Click(object sender, EventArgs e)
        {   
            Session["currentPage"] = (int)Session["currentPage"] + 1;
            await GetAllGames(URI + Session["currentPage"]);
        }

        protected async void BtnSearchGame_Click(object sender, EventArgs e)
        {
            string filter = InputSearchBar.Value;
            await GetListSearchGame(filter);
            DivIconApagaResultados.Visible = true;
        }

        protected void BtnIconEraseSearchedList_Click(object sender, EventArgs e)
        {
            DivResultados.Visible = false;
            DivIconApagaResultados.Visible = false;
        }
    }
}