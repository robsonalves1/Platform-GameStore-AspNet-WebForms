using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using WebApplication2.Controller;
using WebApplication2.Model;

namespace WebApplication2.View
{
    public partial class Home : System.Web.UI.Page
    {
        CntrDB db = new CntrDB();
        Usuario u = new Usuario();

        protected void Page_Load(object sender, EventArgs e)
        {
            u = (Usuario)Session["userLogged"];

            if (!IsPostBack && Session["userLogged"] == null)
            {
                Response.Redirect("Login.aspx");
            }
            else if (!IsPostBack)
            {

                DivFiltersProducts.InnerHtml = LoadFilters();
                TBodyProdutos.InnerHtml = GetProduct("");

                LoadAccountDetails(u);

                DropdownConta.InnerHtml = u.Nome + "<span><svg xmlns=\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-person-circle\" viewBox=\"0 0 16 16\">" +
                                                    "<path d=\"M11 6a3 3 0 1 1-6 0 3 3 0 0 1 6 0z\" />" +
                                                    "<path fill-rule=\"evenodd\" d=\"M0 8a8 8 0 1 1 16 0A8 8 0 0 1 0 8zm8-7a7 7 0 0 0-5.468 11.37C3.242 11.226 4.805 10 8 10s4.757 1.225 5.468 2.37A7 7 0 0 0 8 1z\" />" +
                                                    "</ svg>▼</span>";
                DropdownContaMenu.InnerHtml = "<li>" +
                                                    "<button type=\"button\" id=\"BtnAcessaContaCliente\" runat=\"server\" data-bs-toggle=\"modal\" data-bs-target=\"#modalContaDetalhes\">Conta</button>" +
                                                    "<button type=\"button\" id=\"BtnLogoutContaCliente\" runat=\"server\" onclick=\"BtnLogoutUsuario()\">Sair</button>" +
                                              "</ li>";
            }
            else
            {
                DivFiltersProducts.InnerHtml = LoadFilters();
                TBodyProdutos.InnerHtml = GetProduct("");
            }
        }

        protected void LoadAccountDetails(Usuario usuario)
        {
            try
            {
                string qry = "SELECT * FROM usuario WHERE usuarioId = " + usuario.Id;
                DataTable dt = db.ExecuteReader(qry);

                TxbContaNome.Value = dt.Rows[0][2].ToString();
                TxbContaCargo.Value = dt.Rows[0][3].ToString();
                TxbContaDepartamento.Value = dt.Rows[0][4].ToString();
                TxbContaAdmissao.Value = dt.Rows[0][5].ToString();
            }
            catch (Exception ex)
            {
                divAlert.Visible = true;
                lblAlert.InnerHtml = ex.Message;
            }
        }

        protected string LoadFilters()
        {
            try
            {
                string res = "";

                // LOAD MARCAS
                string query = "SELECT DISTINCT produtoMarca FROM produto";
                DataTable dt = db.ExecuteReader(query);
                res += "<div>";
                res += "<div>";
                res += "<p>Marcas</p>";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    res +=
                        "<div>" +
                            "<input type=\"checkbox\" class=\"checkboxMarcaFiltroEstoque\" id=\"" + dt.Rows[i][0] + "\" name=\"" + dt.Rows[i][0] + "\" onclick=\"CheckboxMarcaFiltroEstoque(this.id, this.checked)\" />" +
                            "<label for=\"" + dt.Rows[i][0] + "\">" + dt.Rows[i][0] +
                        "</div>";
                }
                res += "</div>";
                
                
                //LOAD PREÇOS
                res += "<div>" +
                            "<p>Preço</p>" + 
                            "<label> Selecione o preço: </label>" +
                            "<select name=\"preco\" class=\"form-select mb-2\" id=\"SelectPreco\" onclick=\"SelectPrecoFiltroEstoque(this.value)\">" +
                                 "<option value=\"\"> --Escolha uma opção-- </ option >" +
                                 "<option value=\"<= 100\">Menos de 100</ option>" +
                                 "<option value=\"BETWEEN 101 AND 499\">De 101 as 499</ option>" +
                                 "<option value=\"BETWEEN 500 AND 999\">De 500 as 999</ option>" +
                                 "<option value=\">= 1000\">mais de 1000</ option>" +
                            "</select>" +
                        "</div>";
                
                res += "<button type=\"button\" class=\"btn btn-primary\" id=\"BtnFiltraProdutoEstoque\" onclick=\"FiltraProdutoEstoque()\">Filtrar</button>";

                res += "</div>";


                return res;
            }
            catch (Exception ex)
            {
                divAlert.Visible = true;
                lblAlert.InnerHtml = ex.Message;
                return ex.Message;
            }
        }

        [WebMethod]
        public static string FiltraProdutoEstoque(string filtroEstoque)
        {

            Home home = new Home();
            CntrDB db = new CntrDB();
            DataTable dt = new DataTable();

            string[] f = filtroEstoque.Split(',');
            string query = "SELECT " +
                                    "row_number() over (order by produtoId) linha, produtoId, produtoCodigo, produtoQuantidade, produtoMarca, produtoNome, " +
                                    "Concat('R$ ', " +
                                        "Replace " +
                                            "(Replace" +
                                            "(Replace" +
                                                "(Format(produtoPreco, 2), '.', '|'), ',', '.'), '|', ',')) AS produtoPreco, " +
                                    "produtoDepartamento " +
                                    "FROM produto " +
                                    filtroEstoque;

            dt = db.ExecuteReader(query);

            string dtInJSONFormat = home.DataTable_JSON_JavaSerializer(dt);

            return dtInJSONFormat;

        }


        protected string DataTable_JSON_JavaSerializer(DataTable tabela)
        {
            try
            {
                JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
                List<Dictionary<string, object>> parentRow = new List<Dictionary<string, object>>();
                Dictionary<string, object> childRow;
                foreach (DataRow row in tabela.Rows)
                {
                    childRow = new Dictionary<string, object>();
                    foreach (DataColumn col in tabela.Columns)
                    {
                        childRow.Add(col.ColumnName, row[col]);
                    }
                    parentRow.Add(childRow);
                }
                return jsSerializer.Serialize(parentRow);
            }
            catch
            {
                throw;
            }
        }

        protected string GetProduct(string filtro)
        {
            string res = "";

            try
            {
                if (filtro == "")
                {
                    string query = "SELECT " +
                                   "row_number() over (order by produtoId) linha, produtoId, produtoCodigo, produtoQuantidade, produtoMarca, produtoNome, " +
                                   "Concat('R$ ', " +
                                       "Replace " +
                                         "(Replace" +
                                           "(Replace" +
                                             "(Format(produtoPreco, 2), '.', '|'), ',', '.'), '|', ',')) AS produtoPreco, " +
                                   "produtoDepartamento " +
                                   "FROM produto";
                    DataTable dt = db.ExecuteReader(query);

                    res += "<tr>" +
                                "<th scope=\"col\">linha</ th>" +
                                "<th scope=\"col\">id</ th>" +
                                "<th scope=\"col\">código</ th>" +
                                "<th scope=\"col\">quantidade</ th>" +
                                "<th scope=\"col\">marca</ th>" +
                                "<th scope=\"col\">Nome</ th>" +
                                "<th scope=\"col\">preço</ th>" +
                                "<th scope=\"col\">Departamento </th>" +
                                "<th scope=\"col\">botões</ th>" +
                            "</tr>";

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        res += "<tr>" +
                                    "<td>" + dt.Rows[i][0] + "</td>" +
                                    "<td>" + dt.Rows[i][1] + "</td>" +
                                    "<td>" + dt.Rows[i][2] + "</td>" +
                                    "<td>" + dt.Rows[i][3] + "</td>" +
                                    "<td>" + dt.Rows[i][4] + "</td>" +
                                    "<td>" + dt.Rows[i][5] + "</td>" +
                                    "<td>" + dt.Rows[i][6] + "</td>" +
                                    "<td>" + dt.Rows[i][7] + "</td>" +
                                    "<td>";
                        if (u.Administrador == true)
                            res +=      "<button type =\"button\" id =\"" + dt.Rows[i][1] + "\" class=\"btn btn-danger col-12\" onclick=\"btnExcluir_click(event)\" data-id=\"" + dt.Rows[i][0] + "\" data-bs-toggle=\"modal\" data-bs-target=\"#modalExluirProduto\">Excluir</button>";
                        res +=          "<button type =\"button\" class=\"btn btn-info col-12\" id=\"" + dt.Rows[i][1] + "\" onclick=\"btnDescricao_click(event)\" data-id=\"" + dt.Rows[i][0] + "\" data-bs-toggle=\"modal\" data-bs-target=\"#modalDescricao\">" +
                                            "Detalhes" +
                                        "</button>" +
                                        "<button type =\"button\" class=\"btn btn-primary col-12\" id=\"" + dt.Rows[i][1] + "\" onclick=\"btnEditar_click(event)\" data-id=\"" + dt.Rows[i][0] + "\" data-bs-toggle=\"modal\" data-bs-target=\"#modalEditarProduto\">" +
                                            "Editar" +
                                        "</button>" +
                                    "</td>" +
                               "</tr>"; 
                    }

                    return res;
                }
                else
                {
                    string query = "SELECT " +
                                   "row_number() over (order by produtoId) linha, produtoId, produtoCodigo, produtoQuantidade, produtoMarca, produtoNome, " +
                                   "Concat('R$ ', " +
                                       "Replace " +
                                         "(Replace" +
                                           "(Replace" +
                                             "(Format(produtoPreco, 2), '.', '|'), ',', '.'), '|', ',')) AS produtoPreco, " +
                                   "produtoFoto, " +
                                   "produtoDepartamento " +
                                   "FROM produto" +
                                   "WHERE " + filtro;
                    DataTable dt = db.ExecuteReader(query);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        res += "<tr>" +
                                    "<td>" + dt.Rows[i][0] + "</td>" +
                                    "<td>" + dt.Rows[i][1] + "</td>" +
                                    "<td>" + dt.Rows[i][2] + "</td>" +
                                    "<td>" + dt.Rows[i][3] + "</td>" +
                                    "<td>" + dt.Rows[i][4] + "</td>" +
                                    "<td>" + dt.Rows[i][5] + "</td>" +
                                "</tr>";
                    }

                    return res;
                }
            }
            catch (Exception ex)
            {
                lblAlert.InnerText = ex.Message;
                divAlert.Visible = true;
                return ex.Message;
            }
        }

        #region Add product
        protected void BtnAddProduto_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TxbCodigo.Value.Trim()) 
                    || string.IsNullOrEmpty(TxbQuantidade.Value.Trim()) 
                    || string.IsNullOrEmpty(TxbNome.Value.Trim()) 
                    || string.IsNullOrEmpty(TxbPreco.Value.ToString())
                    || string.IsNullOrEmpty(TxbCaracteristica.Value.Trim())
                    || string.IsNullOrEmpty(TxbMarca.Value.Trim())
                    || string.IsNullOrEmpty(TxbDepartamento.Value.Trim()))
                {
                    lblAlert.InnerText = "Campo não pode estar vazio.";
                    divAlert.Visible = true;
                }
                else
                {
                    Produto p = new Produto();
                    p.Codigo = TxbCodigo.Value.Trim().ToUpper();
                    p.Quantidade = Convert.ToInt16(TxbQuantidade.Value.Trim());
                    p.Nome = TxbNome.Value.Trim();
                    p.Preco = float.Parse(TxbPreco.Value.Trim());
                    p.Caracteristicas = TxbCaracteristica.InnerText.Trim();
                    p.Marca = TxbMarca.Value.Trim();
                    p.Departamento = TxbDepartamento.Value.Trim();

                    if (FotoUpload.HasFile)
                    {
                        HttpFileCollection uploadedFiles = HttpContext.Current.Request.Files;

                        for (int i = 0; i < uploadedFiles.Count - 1; i++)
                        {
                            HttpPostedFile uploadedFile = uploadedFiles[i];

                            using (BinaryReader binaryReader = new BinaryReader(uploadedFile.InputStream))
                            {
                                p.Foto = binaryReader.ReadBytes((int)uploadedFile.InputStream.Length);

                                if (i == 0)
                                {
                                    string query = "INSERT INTO produto (produtoCodigo, produtoQuantidade, produtoNome, produtoPreco, produtoCaracteristicas, produtoMarca, produtoDepartamento, produtoFoto)" +
                                    "VALUES (@CODIGO, @QUANTIDADE, @NOME, @PRECO, @CARACTERISTICAS, @MARCA, @DEPARTAMENTO, @FOTO)";
                                    using (MySqlCommand cmd = new MySqlCommand(query))
                                    {
                                        cmd.Parameters.AddWithValue("@CODIGO", p.Codigo);
                                        cmd.Parameters.AddWithValue("@QUANTIDADE", p.Quantidade);
                                        cmd.Parameters.AddWithValue("@NOME", p.Nome);
                                        cmd.Parameters.AddWithValue("@PRECO", p.Preco);
                                        cmd.Parameters.AddWithValue("@CARACTERISTICAS", p.Caracteristicas);
                                        cmd.Parameters.AddWithValue("@MARCA", p.Marca);
                                        cmd.Parameters.AddWithValue("@DEPARTAMENTO", p.Departamento);
                                        cmd.Parameters.AddWithValue("@FOTO", p.Foto);
                                        db.ExecuteNonQuery(query, cmd);
                                    }
                                }

                                string q = "SELECT produtoId FROM produto WHERE produtoCodigo = '" + p.Codigo + "'";
                                DataTable dt = db.ExecuteReader(q);
                                p.id = (int)dt.Rows[0][0];

                                string qry = "INSERT INTO foto (produtoId, fotoProduto) VALUES(@ID, @FOTO)";
                                using (MySqlCommand cmd = new MySqlCommand(qry))
                                {
                                    cmd.Parameters.AddWithValue("@ID", p.id);
                                    cmd.Parameters.AddWithValue("@FOTO", p.Foto);
                                    db.ExecuteNonQuery(qry, cmd);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                divAlert.Visible = true;
                lblAlert.InnerText = ex.Message;
            }
            finally
            { 
                TBodyProdutos.InnerHtml = GetProduct("");
                DivFiltersProducts.InnerHtml = LoadFilters();
            }
        }

        protected void BtnCancelarAddProduto_Click(object sender, EventArgs e)
        {
            TxbCodigo.Value = "";
            TxbQuantidade.Value = "";
            TxbNome.Value = "";
            TxbPreco.Value = "";
        }
        #endregion

        #region Delete Product
        protected void BtnExcluirProduto_Click(object sender, EventArgs e)
        {
            try
            {
                string id = ValueHiddenField.Value;
                
                //DELETE EM FOTO
                string query = "DELETE FROM foto WHERE produtoId = " + id;
                db.ExecuteNonQuery(query);
                
                //DELETE EM PRODUTO
                query = "DELETE FROM produto WHERE produtoId = " + id;
                db.ExecuteNonQuery(query);


            }
            catch (Exception ex)
            {
                divAlert.Visible = true;
                divAlert.InnerText = ex.Message;
            }
            finally
            {
                TBodyProdutos.InnerHtml = GetProduct("");
                DivFiltersProducts.InnerHtml = LoadFilters();
            }
        }
        #endregion

        #region Edit Product
        protected void BtnEditarProduto_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(TxbCodigoEditar.Value.Trim())
                     || string.IsNullOrEmpty(TxbNomeEditar.Value.Trim())
                     || string.IsNullOrEmpty(TxbQuantidadeEditar.Value.Trim())
                     || string.IsNullOrEmpty(TxbValorEditar.Value.Trim())
                     || string.IsNullOrEmpty(TxbMarcaEditar.Value.Trim())
                     || string.IsNullOrEmpty(TxbDepartamentoEditar.Value.Trim()))
                {
                    lblAlertaEditar.InnerText = "Campo não pode estar vazio.";
                    divAlert.Visible = true;
                }
                else
                {
                    Produto p = new Produto();
                    p.id = int.Parse(HiddenFieldIdEditar.Value.Trim());
                    p.Codigo = TxbCodigoEditar.Value.Trim().ToUpper();
                    p.Quantidade = Convert.ToInt16(TxbQuantidadeEditar.Value.Trim());
                    p.Nome = TxbNomeEditar.Value.Trim();
                    p.Preco = float.Parse(TxbValorEditar.Value.Trim());
                    p.Caracteristicas = TxbCaracteristicasEditar.Value.Trim();
                    p.Marca = TxbMarcaEditar.Value.Trim();
                    p.Departamento = TxbDepartamentoEditar.Value.Trim();

                    HttpFileCollection uploadedFiles = HttpContext.Current.Request.Files;

                    for (int i = 1; i < uploadedFiles.Count; i++)
                    {
                        HttpPostedFile uploadedFile = uploadedFiles[i];

                        using (BinaryReader binaryReader = new BinaryReader(uploadedFile.InputStream))
                        {
                            p.Foto = binaryReader.ReadBytes((int)uploadedFile.InputStream.Length);
                        }

                        if (i == 1)
                        {
                            string query = "UPDATE produto SET produtoCodigo = @CODIGO, produtoQuantidade = @QUANTIDADE, produtoNome = @NOME, produtoPreco = @PRECO, produtoCaracteristicas = @CARACTERISTICAS, produtoMarca = @MARCA, produtoDepartamento = @DEPARTAMENTO,  produtoFoto = @FOTO WHERE produtoId = " + p.id;
                            using (MySqlCommand cmd = new MySqlCommand(query))
                            {
                                cmd.Parameters.AddWithValue("@CODIGO", p.Codigo);
                                cmd.Parameters.AddWithValue("@QUANTIDADE", p.Quantidade);
                                cmd.Parameters.AddWithValue("@NOME", p.Nome);
                                cmd.Parameters.AddWithValue("@PRECO", p.Preco);
                                cmd.Parameters.AddWithValue("@CARACTERISTICAS", p.Caracteristicas);
                                cmd.Parameters.AddWithValue("@MARCA", p.Marca);
                                cmd.Parameters.AddWithValue("@DEPARTAMENTO", p.Departamento);
                                cmd.Parameters.AddWithValue("@FOTO", p.Foto);
                                db.ExecuteNonQuery(query, cmd);
                            }

                            //NECESSÁRIO DELETAR FOTOS ANTIGAS NA TABELA FOTO PARA INSERIR NOVAS
                            string q = "DELETE FROM foto WHERE produtoId = " + p.id;
                            db.ExecuteNonQuery(q);
                        }


                        string qry = "INSERT INTO foto (produtoId, fotoProduto) VALUES (@ID, @FOTO)";
                        using (MySqlCommand cmd = new MySqlCommand(qry))
                        {
                            cmd.Parameters.AddWithValue("@ID", p.id);
                            cmd.Parameters.AddWithValue("@FOTO", p.Foto);
                            db.ExecuteNonQuery(qry, cmd);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                lblAlert.InnerText = ex.Message;
                divAlertaEditar.Visible = true;
            }
            finally
            {
                TBodyProdutos.InnerHtml = GetProduct("");
                DivFiltersProducts.InnerHtml = LoadFilters();
            }
        }
        #endregion

        protected void BtnChangeAccountDetails_Click(object sender, EventArgs e)
        {
            u = (Usuario)Session["userLogged"];
            
            try
            {
                if (string.IsNullOrEmpty(TxbContaNome.Value.Trim())
                    || string.IsNullOrEmpty(TxbContaSenha.Value.Trim())
                    || string.IsNullOrEmpty(TxbContaCargo.Value.Trim())
                    || string.IsNullOrEmpty(TxbContaDepartamento.Value.Trim()))
                {
                    divAlert.Visible = true;
                    lblAlert.InnerText = "Preencha todos os campos para atualizar a conta";
                }
                else
                {
                    string qry = "UPDATE usuario SET usuarioNome = @NOME, usuarioSenha = @SENHA, usuarioCargo = @CARGO, usuarioDepartamento = @DEPARTAMENTO WHERE usuarioId = " + u.Id;
                    using (MySqlCommand cmd = new MySqlCommand(qry))
                    {
                        cmd.Parameters.AddWithValue("@NOME", TxbContaNome.Value);
                        cmd.Parameters.AddWithValue("@SENHA", TxbContaSenha.Value);
                        cmd.Parameters.AddWithValue("@CARGO", TxbContaCargo.Value);
                        cmd.Parameters.AddWithValue("@DEPARTAMENTO", TxbContaDepartamento.Value);
                        db.ExecuteNonQuery(qry, cmd);
                    }
                }
            }
            catch (Exception ex)
            {
                divAlert.Visible = true;
                lblAlert.InnerHtml = ex.Message;
            }
            finally
            {
                LoadAccountDetails(u);
            }
        }

        [WebMethod]
        public static string BtnLogoutUsuario()
        {
            HttpContext.Current.Session["userLogged"] = null;
            return "Login.aspx";
        }
    }
}