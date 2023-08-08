using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
    public partial class HomePublishers : System.Web.UI.Page
    {
        string URI = "https://api.rawg.io/api/publishers?key=81efaba6814e40658121d32daf691878&page_size=30&page=1";
        JsonRespostaApiRawgListOfPublishers[] jsonRespostaPublishers = null;

        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                DropdownContaMenu.InnerHtml = "<li>" +
                                                 "<button type=\"button\" id =\"BtnPaginaLoginCliente\" runat =\"server\" onclick=\"BtnLoginCliente()\">Login</ button>" +
                                              "</ li>";

                DivIconApagaResultados.Visible = false;

                Session["currentPageHomePublishers"] = 1;
                await GetAllPublishers(URI);
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

        protected async Task GetAllPublishers(string uri)
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(uri))
                {
                    string PublisherJsonString = await response.Content.ReadAsStringAsync();
                    PublisherJsonString = "[" + PublisherJsonString + "]";
                    jsonRespostaPublishers = JsonConvert.DeserializeObject<JsonRespostaApiRawgListOfPublishers[]>(PublisherJsonString);
                    LoadAllPublishers(jsonRespostaPublishers);
                }
            }
        }

        protected void LoadAllPublishers(JsonRespostaApiRawgListOfPublishers[] jsonRespostaP)
        {
            try
            {
                DivAlert.Visible = false;
                DivResultPublishers.InnerHtml = "";

                for (int i = 0; i < jsonRespostaP[0].Results.Count; i++)
                {
                    DivResultPublishers.InnerHtml += "<div class=\"item\">" +
                                                        "<div class=\"thumbnail\">" +
                                                            "<img id=\"ImgPublisher\" class=\"group list-group-image\" src=\"" + jsonRespostaP[0].Results[i].Image_Background + "\" alt=\"\" />" +
                                                            "<div class=\"caption\">" +
                                                                "<h4 class=\"group inner list-group-item-heading\">" + jsonRespostaP[0].Results[i].Name + "</h4>" +
                                                                "<p class=\"group inner list-group-item-text\">Números de Jogos: " + jsonRespostaP[0].Results[i].Games_Count + "</p>" +
                                                            "</div>" +
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

        protected async void BtnLastPageHomePublishers_Click(object sender, EventArgs e)
        {
            if ((int)Session["currentPageHomePublishers"] > 0)
            {
                Session["currentPageHomePublishers"] = (int)Session["currentPageHomePublishers"] - 1;
                string uri = "https://api.rawg.io/api/publishers?key=81efaba6814e40658121d32daf691878&page_size=30&page=";
                await GetAllPublishers(uri + Session["currentPageHomePublishers"]);
            }
        }

        protected async void BtnNextPageHomePublishers_Click(object sender, EventArgs e)
        {
            Session["currentPageHomePublishers"] = (int)Session["currentPageHomePublishers"] + 1;
            string uri = "https://api.rawg.io/api/publishers?key=81efaba6814e40658121d32daf691878&page_size=30&page=";
            await GetAllPublishers(uri + Session["currentPageHomePublishers"]);
        }

        protected async void BtnSearchGames_Click(object sender, EventArgs e)
        {
            string uri = "https://api.rawg.io/api/games?key=81efaba6814e40658121d32daf691878&search=" + InputSearchBar.Value;
            await GetGameSearched(uri);
            DivIconApagaResultados.Visible = true;
        }

        protected async Task GetGameSearched(string uri)
        {
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(uri))
                {
                    string GameSearchedJsonString = await response.Content.ReadAsStringAsync();
                    GameSearchedJsonString = "[" + GameSearchedJsonString + "]";
                    var jsonListOfGamesSearched = JsonConvert.DeserializeObject<JsonRespostaApiRawgListOfGames[]>(GameSearchedJsonString);
                    LoadListOfGamesSearched(jsonListOfGamesSearched);
                }
            }
        }

        protected void LoadListOfGamesSearched(JsonRespostaApiRawgListOfGames[] jsonListOfGamesSearched)
        {
            try
            {
                DivResultados.InnerHtml = "";
                DivResultados.Visible = true;
                DivResultados.Attributes["style"] = "visibility: visible";

                for (int i = 0; i < 5; i++)
                {
                    string bgImage = jsonListOfGamesSearched[0].Results[i].Background_Image;
                    string name = jsonListOfGamesSearched[0].Results[i].Name;
                    int id = jsonListOfGamesSearched[0].Results[i].Id;

                    DivResultados.InnerHtml += "<button type=\"button\" onclick=\"BtnShowSelectedJogo(" + id + ")\">" +
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
        public static string BtnShowSelectedJogo(int id)
        {
            HttpContext.Current.Session["idSelectedJogo"] = id;
            return "ComprarJogo.aspx";
        }

        protected void BtnIconApagaResultado_Click(object sender, EventArgs e)
        {
            DivResultados.Visible = false;
            DivIconApagaResultados.Visible = false;
        }
    }
}