using MySql.Data.MySqlClient;
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
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using WebApplication2.Controller;
using WebApplication2.Model;

namespace WebApplication2.View
{
    public partial class ComprarJogo : System.Web.UI.Page
    {
        JsonRespostaApiRawgListOfGames[] jsonResposta = null;
        JsonRespostaApiRawgDetailsOfGames[] jsonRespostaDetails = null;
        CntrDB db = new CntrDB();
        GameController gameController = new GameController();
        Cliente c = new Cliente();

        protected async void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack && Session["idSelectedJogo"] == null)
            {
                DivAlert.Visible = true;
                LblAlert.InnerHtml = "Por favor, selecione um jogo antes de vir para a página de compra.";
            }
            else if (!IsPostBack)
            {
                DropdownContaMenu.InnerHtml = "<li>" +
                                                 "<button type=\"button\" id =\"BtnPaginaLoginCliente\" runat =\"server\" onclick=\"BtnLoginCliente()\"> Login </ button>" +
                                              "</ li>";

                DivIconApagaResultados.Visible = false;

                await GetSpecificJogo("https://api.rawg.io/api/games/" + Session["idSelectedJogo"] + "?key=81efaba6814e40658121d32daf691878");
                await GetScreenshots("https://api.rawg.io/api/games/" + Session["idSelectedJogo"] + "/screenshots?key=81efaba6814e40658121d32daf691878");
                await GetTrailerMovies("https://api.rawg.io/api/games/" + Session["idSelectedJogo"] + "/movies?key=81efaba6814e40658121d32daf691878");
                await GetAchievements("https://api.rawg.io/api/games/" + Session["idSelectedJogo"] + "/achievements?key=81efaba6814e40658121d32daf691878");
                await GetListOfGameSerie("https://api.rawg.io/api/games/" + Session["idSelectedJogo"] + "/game-series?key=81efaba6814e40658121d32daf691878");
                await GetDevelopmentTeam("https://api.rawg.io/api/games/" + Session["idSelectedJogo"] + "/development-team?key=81efaba6814e40658121d32daf691878");
                
                DivResultados.Visible = false;


                if (Session["clienteLogado"] != null)
                {
                    c = (Cliente)Session["clienteLogado"];

                    DropdownConta.InnerHtml = c.Nome + "<span><svg xmlns=\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-person-circle\" viewBox=\"0 0 16 16\">" +
                                                        "<path d=\"M11 6a3 3 0 1 1-6 0 3 3 0 0 1 6 0z\" />" +
                                                        "<path fill-rule=\"evenodd\" d=\"M0 8a8 8 0 1 1 16 0A8 8 0 0 1 0 8zm8-7a7 7 0 0 0-5.468 11.37C3.242 11.226 4.805 10 8 10s4.757 1.225 5.468 2.37A7 7 0 0 0 8 1z\" />" +
                                                        "</ svg>▼</span>";
                    DropdownContaMenu.InnerHtml = "<li>" +
                                                        "<button type=\"button\" id=\"BtnAcessaContaCliente\" runat=\"server\"><a href=\"HomeContaCliente.aspx\">Conta</a></ button>" +
                                                        "<button type=\"button\" id=\"BtnLogoutContaCliente\" runat=\"server\" onclick=\"BtnLogoutCliente()\">Sair</ button>" +
                                                  "</ li>";
                    BtnConfirmPurchase.Visible = true;
                    DivBodyPurchaseDecision.InnerHtml = "Realmente deseja comprar?";
                }
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

        protected async Task GetSpecificJogo(string uri)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    using (HttpResponseMessage response = await client.GetAsync(uri))
                    {
                        string JogoJsonString = await response.Content.ReadAsStringAsync();
                        JogoJsonString = "[" + JogoJsonString + "]";
                        jsonRespostaDetails = JsonConvert.DeserializeObject<JsonRespostaApiRawgDetailsOfGames[]>(JogoJsonString);
                        LoadSpecificJogo(jsonRespostaDetails);
                    }
                }
            }
            catch (Exception ex)
            {
                DivAlert.Visible = true;
                LblAlert.InnerHtml = ex.Message;
            }
        }

        protected void LoadSpecificJogo(JsonRespostaApiRawgDetailsOfGames[] jsonRespostaDetails)
        {
            try
            {
                string nameJogo = jsonRespostaDetails[0].Name;
                string imgJogo = jsonRespostaDetails[0].Background_Image;
                string descriptionJogo = jsonRespostaDetails[0].Description;
                int metacriticJogo = jsonRespostaDetails[0].Metacritic;
                double ratingJogo = jsonRespostaDetails[0].Rating;
                string releasedString = jsonRespostaDetails[0].Released;
                string[] releasedArray = releasedString.Split('-').Reverse().ToArray();
                string releasedReverseString = String.Join("-", releasedArray);

                c = (Cliente)Session["clienteLogado"];
                int quantidadeComprada = 0;
                
                if (c != null)
                {
                    string query = "SELECT produtoQuantidadeComprada FROM clienteCompras WHERE clienteId = " + c.Id + " AND produtoJogoId = " + Session["idSelectedJogo"];
                    DataTable dt = db.ExecuteReader(query);
                    if (dt.Rows.Count > 0)
                        quantidadeComprada = (int)dt.Rows[0][0];
                }


                FieldQuantityPurchased.InnerText = quantidadeComprada.ToString();

                FieldNameJogo.InnerHtml = nameJogo;
                imgs.Src = imgJogo;
                FieldDescriptionJogo.InnerHtml = descriptionJogo;
                FieldMetacriticJogo.InnerHtml = metacriticJogo.ToString();
                FieldRatingJogo.InnerHtml= ratingJogo.ToString();
                FieldReleasedJogo.InnerHtml= releasedReverseString;                
            }
            catch (Exception ex)
            {
                DivAlert.Visible = true;
                LblAlert.InnerHtml = ex.Message;
            }
        }

        protected async Task GetScreenshots(string uri)
        {
            string JogoScreenshotsJsonString = await gameController.ConsumeExternalApi(uri);
            JogoScreenshotsJsonString = "[" + JogoScreenshotsJsonString + "]";
            JsonRespostaApiRawgListOfScreenshots[] jsonRespostaScreenshots = JsonConvert.DeserializeObject<JsonRespostaApiRawgListOfScreenshots[]>(JogoScreenshotsJsonString);
            LoadScreenshots(jsonRespostaScreenshots);
        }

        protected void LoadScreenshots(JsonRespostaApiRawgListOfScreenshots[] jsonRespostaScreenshots)
        {
            try
            {
                for (int i = 0; i < jsonRespostaScreenshots[0].Results.Count; i++)
                {
                    string imgScreenshot = jsonRespostaScreenshots[0].Results[i].Image;

                    HtmlGenericControl divImgScreenshotJogo = new HtmlGenericControl();
                    divImgScreenshotJogo.Attributes["id"] = "swatch-" + i;
                    divImgScreenshotJogo.Attributes["class"] = "swatch";
                    divImgScreenshotJogo.InnerHtml = "<img src=\"" + imgScreenshot + "\" />";
                    divImgScreenshotJogo.Attributes["onclick"] = "updateSelected(this, \"" + imgScreenshot + "\")";
                    SwatchesSection.Controls.AddAt(i, divImgScreenshotJogo);
                }
            }
            catch (Exception ex)
            {
                DivAlert.Visible = true;
                LblAlert.InnerHtml = ex.Message;
            }
        }

        protected async Task GetTrailerMovies(string uri)
        {
            
            string JogoTrailerMovieJsonString = await gameController.ConsumeExternalApi(uri);
            JogoTrailerMovieJsonString = "[" + JogoTrailerMovieJsonString + "]";
            JsonRespostaApiRawgListOfTrailerMovies[] jsonRespostaTrailerMovies = JsonConvert.DeserializeObject<JsonRespostaApiRawgListOfTrailerMovies[]>(JogoTrailerMovieJsonString);
            LoadTrailerMovies(jsonRespostaTrailerMovies);

        }

        protected void LoadTrailerMovies(JsonRespostaApiRawgListOfTrailerMovies[] jsonRespostaTrailerMovies)
        {
            try
            {

                if (jsonRespostaTrailerMovies[0].Results.Count > 0)
                {
                    DivTrailerMovies.Visible = true;
                    DivTrailerMovies.InnerHtml = "<h1>Trailers</h1>";
                    
                    for (int i = 0; i < jsonRespostaTrailerMovies[0].Results.Count; i++)
                    {
                        DivTrailerMovies.InnerHtml += "<video class=\"mx-3 mb-4\" controls width=\"400\">" +
                                                        "<source src=\"" + jsonRespostaTrailerMovies[0].Results[i].Data.Max + "\" type=\"video/mp4\"/>" +
                                                      "</video>";
                    }
                }
            }
            catch (Exception ex)
            {
                DivAlert.Visible = true;
                LblAlert.InnerHtml = ex.Message;
            }
        }

        protected async Task GetAchievements(string uri)
        {
            string JogoAchievementsJsonString = await gameController.ConsumeExternalApi(uri);
            JogoAchievementsJsonString = "[" + JogoAchievementsJsonString + "]";
            JsonRespostaApiRawgListAchievements[] jsonRespostaAchievements = JsonConvert.DeserializeObject<JsonRespostaApiRawgListAchievements[]>(JogoAchievementsJsonString);
            LoadAchivements(jsonRespostaAchievements);

        }

        protected void LoadAchivements(JsonRespostaApiRawgListAchievements[] jsonRespostaAchievements)
        {
            try
            {
                DivAchievements.Visible = true;
                DivAchievements.InnerHtml = "<h1>Conquistas do Jogo</h1>";

                for (int i = 0; i < jsonRespostaAchievements[0].Results.Count; i++)
                {
                    DivAchievements.InnerHtml += "<div class=\"col DivColAchievements mx-3\">" +
                                                    "<img id=\"ImgAchievements\" src=\"" + jsonRespostaAchievements[0].Results[i].Image + "\" />" +
                                                    "<p>" + jsonRespostaAchievements[0].Results[i].Name + "</p>" +
                                                 "</div>";
                }
            }
            catch (Exception ex)
            {
                DivAlert.Visible = true;
                LblAlert.InnerHtml = ex.Message;
            }
        }

        protected async Task GetListOfGameSerie(string uri)
        {
            string JogoSerieJsonString = await gameController.ConsumeExternalApi(uri);
            JogoSerieJsonString = "[" + JogoSerieJsonString + "]";
            JsonRespostaApiRawgListOfGameSerie[] jsonRespostaGameSerie = JsonConvert.DeserializeObject<JsonRespostaApiRawgListOfGameSerie[]>(JogoSerieJsonString);
            LoadGameSameSerie(jsonRespostaGameSerie);
        }

        protected void LoadGameSameSerie(JsonRespostaApiRawgListOfGameSerie[] jsonRespostaGameSerie)
        {
            try
            {
                DivGameSameSerie.Visible = true;
                DivGameSameSerie.InnerHtml = "<h1>Jogos da Mesma Série</h1>";

                for (int i = 0; i < jsonRespostaGameSerie[0].Results.Count; i++)
                {
                    int id = jsonRespostaGameSerie[0].Results[i].Id;

                    DivGameSameSerie.InnerHtml += "<div class=\"col DivColGameSameSerie mx-3\">" +
                                                    "<img id=\"ImgGameSameSerie\" src=\"" + jsonRespostaGameSerie[0].Results[i].Background_Image + "\" />" +
                                                    "<button type=\"button\" id=\"BtnSelectGame\" class=\"btn btn-primary my-3\" onclick=\"BtnShowSelectedJogo(" + id + ")\">" + jsonRespostaGameSerie[0].Results[i].Name + "</button>" +
                                                 "</div>";
                }
            }
            catch (Exception ex)
            {
                DivAlert.Visible = true;
                LblAlert.InnerHtml = ex.Message;
            }
        }

        protected async Task GetDevelopmentTeam(string uri)
        {
                    string JogoDevelopmentTeamJsonString = await gameController.ConsumeExternalApi(uri);
                    JogoDevelopmentTeamJsonString = "[" + JogoDevelopmentTeamJsonString + "]";
                    JsonRespostaApiRawgListOfDevelopmentTeam[] jsonRespostaDevelopmentTeams = JsonConvert.DeserializeObject<JsonRespostaApiRawgListOfDevelopmentTeam[]>(JogoDevelopmentTeamJsonString);
                    LoadDevelopmentTeam(jsonRespostaDevelopmentTeams);
        }

        protected void LoadDevelopmentTeam(JsonRespostaApiRawgListOfDevelopmentTeam[] jsonRespostaDevelopmentTeams)
        {
            try
            {
                DivDevelopmentTeam.Visible = true;
                DivDevelopmentTeam.InnerHtml = "<h1>Time de Desenvolvedores </h1>";

                for (int i = 0; i < jsonRespostaDevelopmentTeams[0].Results.Count; i++)
                {
                    if (jsonRespostaDevelopmentTeams[0].Results[i].Image != null)
                        DivDevelopmentTeam.InnerHtml += "<div class=\"col DivColDevelopmentTeam mx-3\">" +
                                                            "<img id=\"ImgDevelopmentTeam\" src=\"" + jsonRespostaDevelopmentTeams[0].Results[i].Image + "\" />" +
                                                            "<p>" + jsonRespostaDevelopmentTeams[0].Results[i].Name + "" +
                                                        "</div>";
                    else
                        DivDevelopmentTeam.InnerHtml += "<div class=\"col DivColDevelopmentTeam mx-3\">" +
                                                            "<img id=\"ImgDevelopmentTeam\" src=\"" + jsonRespostaDevelopmentTeams[0].Results[i].Image_Background + "\" />" +
                                                            "<p>" + jsonRespostaDevelopmentTeams[0].Results[i].Name + "" +
                                                        "</div>";
                }
            }
            catch (Exception ex)
            {
                DivAlert.Visible = true;
                LblAlert.InnerHtml = ex.Message;
            }
        }


        [WebMethod]
        public static string BtnShowSelectedJogo(string id)
        {
            HttpContext.Current.Session["idSelectedJogo"] = id;
            return "ComprarJogo.aspx";
        }

        protected async Task GetListSearchJogo(string filter)
        {
            string URIDetails = "https://api.rawg.io/api/games?key=81efaba6814e40658121d32daf691878&search=" + filter;

            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(URIDetails))
                {
                    string JogoJsonStringDetails = await response.Content.ReadAsStringAsync();
                    JogoJsonStringDetails = "[" + JogoJsonStringDetails + "]";
                    jsonResposta = JsonConvert.DeserializeObject<JsonRespostaApiRawgListOfGames[]>(JogoJsonStringDetails);
                    LoadListSearchJogo(jsonResposta);
                }
            }
        }

        protected void LoadListSearchJogo(JsonRespostaApiRawgListOfGames[] jsonResposta)
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

        protected async void BtnSearchJogo_Click(object sender, EventArgs e)
        {
            string filter = InputSearchBar.Value;
            await GetListSearchJogo(filter);
            DivIconApagaResultados.Visible = false;
        }

        protected void BtnIconApagaResultado_Click(object sender, EventArgs e)
        {
            DivResultados.Visible = false;
            DivIconApagaResultados.Visible = false;
        }

        protected void BtnConfirmPurchase_Click(object sender, EventArgs e)
        {
            c = (Cliente)Session["clienteLogado"];

            string qry = "SELECT produtoQuantidadeComprada FROM clienteCompras WHERE clienteId = " + c.Id + " AND produtoJogoId = " + Session["idSelectedJogo"];
            DataTable dt = db.ExecuteReader(qry);
            int quantidadeComprada = 0;

            if (dt.Rows.Count > 0)
                quantidadeComprada = (int)dt.Rows[0][0];

            try
            {
                string query = "";

                if (quantidadeComprada == 0)
                {
                    query = "INSERT INTO clienteCompras(clienteId, produtoJogoId, produtoNome, produtoFoto, produtoQuantidadeComprada) VALUES(@CLIENTEID, @JOGOID, @NOME, @FOTO, @QUANTIDADECOMPRADA)";
                    using (MySqlCommand cmd = new MySqlCommand(query))
                    {
                        cmd.Parameters.AddWithValue("@CLIENTEID", c.Id);
                        cmd.Parameters.AddWithValue("@JOGOID", (string)Session["idSelectedJogo"]);
                        cmd.Parameters.AddWithValue("@NOME", FieldNameJogo.InnerText.Trim());
                        cmd.Parameters.AddWithValue("@FOTO", imgs.Src.Trim());
                        cmd.Parameters.AddWithValue("@QUANTIDADECOMPRADA", ++quantidadeComprada);
                        db.ExecuteNonQuery(query, cmd);
                    }
                } 
                else
                {
                    query = "UPDATE clienteCompras SET produtoQuantidadeComprada = @QUANTIDADECOMPRADA WHERE clienteId = @CLIENTEID AND produtoJogoId = @JOGOID";
                    using (MySqlCommand cmd = new MySqlCommand(query))
                    {
                        cmd.Parameters.AddWithValue("@QUANTIDADECOMPRADA", ++quantidadeComprada);
                        cmd.Parameters.AddWithValue("@CLIENTEID", c.Id);
                        cmd.Parameters.AddWithValue("@JOGOID", (string)Session["idSelectedJogo"]);
                        db.ExecuteNonQuery(query, cmd);
                    }
                }
            }
            catch (Exception ex)
            {
                DivAlert.Visible = true;
                LblAlert.InnerHtml = ex.Message;
            }
            finally
            {
                FieldQuantityPurchased.InnerHtml = quantidadeComprada.ToString();
            }
        }
    }
}