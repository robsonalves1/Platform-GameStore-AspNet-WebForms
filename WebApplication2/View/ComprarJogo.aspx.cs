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
        float mediaNotaProduto = 0;
        string nomeJogo = "";
        string fotoJogo = "";

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
                                                 "<button type=\"button\" id =\"BtnPaginaLoginCliente\" runat =\"server\" onclick=\"BtnLoginClient()\"> Login </ button>" +
                                              "</ li>";

                DivIconApagaResultados.Visible = false;

                await GetSpecificGame("https://api.rawg.io/api/games/" + Session["idSelectedJogo"] + "?key=81efaba6814e40658121d32daf691878");
                await GetScreenshots("https://api.rawg.io/api/games/" + Session["idSelectedJogo"] + "/screenshots?key=81efaba6814e40658121d32daf691878");
                await GetTrailerMovies("https://api.rawg.io/api/games/" + Session["idSelectedJogo"] + "/movies?key=81efaba6814e40658121d32daf691878");
                await GetAchievements("https://api.rawg.io/api/games/" + Session["idSelectedJogo"] + "/achievements?key=81efaba6814e40658121d32daf691878");
                await GetListOfGameSerie("https://api.rawg.io/api/games/" + Session["idSelectedJogo"] + "/game-series?key=81efaba6814e40658121d32daf691878");
                await GetDevelopmentTeam("https://api.rawg.io/api/games/" + Session["idSelectedJogo"] + "/development-team?key=81efaba6814e40658121d32daf691878");

                Session["nomeJogo"] = FieldNameJogo.InnerHtml.Trim();
                Session["fotoJogo"] = imgs.Attributes["src"].Trim();

                SectionComentarios.InnerHtml = GetComments();
                CampoProdutoNotaEstrelas.InnerHtml = GetStars();
                CampoProdutoNotaQuantidadeDeComentarios.InnerHtml = "(" + GetAmountOfComments((string)Session["idSelectedJogo"]) + ")";


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
                                                        "<button type=\"button\" id=\"BtnLogoutContaCliente\" runat=\"server\" onclick=\"BtnLogoutClient()\">Sair</ button>" +
                                                  "</ li>";
                    BtnConfirmPurchase.Visible = true;
                    DivBodyPurchaseDecision.InnerHtml = "Realmente deseja comprar?";

                    ClientAlrearyBoughtProduct();
                }
                else
                {
                    DivContainerAddComentarios.InnerHtml = "<h1>Faça login para comentar</h1>" +
                                                       "<button type=\"button\" class=\"btn btn-success\" onclick=\"BtnLoginClient()\">Login</button>";

                    DivContainerAddComentarios.Attributes["style"] = "background-color: rgba(87, 92, 92, 0.2);";
                }
            }
        }

        protected void ClientAlrearyBoughtProduct()
        {
            Cliente cliente = (Cliente)Session["clienteLogado"];

            string query = "SELECT * " +
                           "FROM clienteCompras " +
                           "WHERE clienteId = " + cliente.Id + " " +
                           "AND produtoJogoId = " + Session["idSelectedJogo"];
            DataTable dt = new DataTable();
            dt = db.ExecuteReader(query);
            if (dt.Rows.Count == 0)
            {
                DivContainerAddComentarios.InnerHtml = "<h1>Para poder comentar você precisa comprar o produto primeiro</h1>";

                DivContainerAddComentarios.Attributes["style"] = "background-color: rgba(87, 92, 92, 0.2);";
            }
            else
            {
                DivContainerAddComentarios.InnerHtml =  "<div class=\"form-floating mb-2\">" +
                                                            "<textarea class=\"form-control mb-2\" placeholder=\"Deixe seu comentário aqui\" style=\"height: 100px\" id=\"TxbComentarioProduto\" runat=\"server\"></textarea>" +
                                                            "<label class=\"form-label\">Comentário</label>" +
                                                        "</div>" +
                                                        "<div class=\"form-floating mb-2\">" +
                                                            "<input type=\"number\" class=\"form-control\" id=\"TxbNotaProduto\" placeholder=\"Nota\" runat=\"server\" min=\"0\" max=\"5\" step=\"1\" />" +
                                                            "<label for=\"floatingNota\">Nota</label>" +
                                                        "</div>" +
                                                        "<div class=\"form-floating mb-2\">" +
                                                            "<button type=\"button\" class=\"btn btn-success\" onclick=\"SendComment()\" runat=\"server\">Enviar</button>" +
                                                        "</div>";

                DivContainerAddComentarios.Attributes["style"] = "";
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

        protected async Task GetSpecificGame(string uri)
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
                        ShowSpecificGame(jsonRespostaDetails);
                    }
                }
            }
            catch (Exception ex)
            {
                DivAlert.Visible = true;
                LblAlert.InnerHtml = ex.Message;
            }
        }

        protected void ShowSpecificGame(JsonRespostaApiRawgDetailsOfGames[] jsonRespostaDetails)
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
                                                    "<button type=\"button\" id=\"BtnSelectGame\" class=\"btn btn-primary my-3\" onclick=\"BtnShowSelectedGame(" + id + ")\">" + jsonRespostaGameSerie[0].Results[i].Name + "</button>" +
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
        public static string BtnShowSelectedGame(string id)
        {
            HttpContext.Current.Session["idSelectedJogo"] = id;
            return "ComprarJogo.aspx";
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

        protected async void BtnSearchGame_Click(object sender, EventArgs e)
        {
            string filter = InputSearchBar.Value;
            await GetListSearchGame(filter);
            DivIconApagaResultados.Visible = false;
        }

        protected void BtnIconEraseResult_Click(object sender, EventArgs e)
        {
            DivResultados.Visible = false;
            DivIconApagaResultados.Visible = false;
        }

        protected void BtnConfirmPurchase_Click(object sender, EventArgs e)
        {
            c = (Cliente)Session["clienteLogado"];

            string query = "SELECT produtoQuantidadeComprada FROM clienteCompras WHERE clienteId = " + c.Id + " AND produtoJogoId = " + Session["idSelectedJogo"];
            DataTable dt = db.ExecuteReader(query);
            int quantidadeComprada = 0;

            if (dt.Rows.Count > 0)
                quantidadeComprada = (int)dt.Rows[0][0];

            //INSERT NEW GAME INTO TABLE  jogo
            query = "SELECT * FROM jogo WHERE jogoId = " + Session["idSelectedJogo"];
            dt = db.ExecuteReader(query);
            if (dt.Rows.Count == 0)
            {
                string nomeJogo = (string)Session["nomeJogo"];
                string fotoJogo = (string)Session["fotoJogo"];
                query = "INSERT INTO jogo(jogoId, jogoNome, jogoFoto) VALUES(@ID, @NOME, @FOTO)";
                using (MySqlCommand cmd = new MySqlCommand(query))
                {
                    cmd.Parameters.AddWithValue("@ID", Session["idSelectedJogo"]);
                    cmd.Parameters.AddWithValue("@NOME", nomeJogo);
                    cmd.Parameters.AddWithValue("@FOTO", fotoJogo);
                    db.ExecuteNonQuery(query, cmd);
                }
            }

            try
            {
                query = "";

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
                ClientAlrearyBoughtProduct();
            }
        }

        [WebMethod]
        public static string[] SendComment(string comentario, string nota) // VERIFICAR SE MÉTODO ESTÁ FUNCIONANDO PERFEITAMENTE
        {
            string[] res = new string[4];

            try
            {
                CntrDB db = new CntrDB();
                DataTable dt = new DataTable();
                ComprarJogo cj = new ComprarJogo();
                string qtd = "";

                Cliente cliente = new Cliente();
                cliente = (Cliente)HttpContext.Current.Session["clienteLogado"];

                if (Convert.ToInt32(nota) >= 0 && Convert.ToInt32(nota) <= 5)
                {
                    // INSERT THE COMMENT IN THE DATABASE
                    string query = "INSERT INTO comentario(jogoId, clienteId, comentarioTexto, comentarioNota) VALUES(@IDJOGO, @IDCLIENTE, @COMENTARIO, @NOTA)";
                    using (MySqlCommand cmd = new MySqlCommand(query))
                    {
                        cmd.Parameters.AddWithValue("@IDJOGO", HttpContext.Current.Session["idSelectedJogo"]);
                        cmd.Parameters.AddWithValue("@IDCLIENTE", cliente.Id);
                        cmd.Parameters.AddWithValue("@COMENTARIO", comentario);
                        cmd.Parameters.AddWithValue("@NOTA", nota);
                        db.ExecuteNonQuery(query, cmd);
                    }
                }
                else
                {
                    res[3] = "Para enviar comentário nota deve estar entre 0 e 5 pontos.";
                }

                // GET THE Nº OF COMMENTS FROM THE ID OF THE PRODUCT
                qtd = cj.GetAmountOfComments((string)HttpContext.Current.Session["idSelectedJogo"]);

                res[0] = cj.GetComments();
                res[1] = cj.GetStars();
                res[2] = qtd;

                return res;
            }
            catch (Exception ex)
            {
                res[2] = ex.Message;
                return res;
            }
        }

        protected string GetComments()
        {
            try
            {
                string res = "";
                string query = "SELECT cm.comentarioId, cm.comentarioTexto, cm.comentarioNota, cl.clienteNome " +
                               "FROM comentario cm " +
                               "JOIN cliente cl ON cm.clienteId = cl.clienteId " +
                               "WHERE cm.jogoId = " + HttpContext.Current.Session["idSelectedJogo"] + " ORDER BY cm.comentarioId DESC";

                DataTable dt = db.ExecuteReader(query);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string comentario = (string)dt.Rows[i][1];
                        int nota = (int)dt.Rows[i][2];
                        string nome = (string)dt.Rows[i][3];

                        /*REVIEWS*/
                        res += "<div class=\"px - 5 mt - 3\">" +
                                    "<p><strong>" + nome + "</strong></p>" +
                                    "<p>" + comentario + "</p>" +
                                    "<p>" + nota + "</p>" +
                                "</div>";
                    }
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

        protected string GetAmountOfComments(string id)
        {
            DataTable dt = new DataTable();

            string qry = "SELECT jogoId, COUNT(*) as qtd from comentario WHERE jogoId = " + id;
            dt = db.ExecuteReader(qry);

            return dt.Rows[0][1].ToString();
        }

        protected string GetStars()
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
    }
}