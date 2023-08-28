using MySql.Data.MySqlClient;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication2.Controller;
using WebApplication2.Model;

namespace WebApplication2.View
{
    public partial class ApproveAccessPage : System.Web.UI.Page
    {
        CntrDB db = new CntrDB();
        Usuario u = new Usuario();
        private static RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();

        protected void Page_Load(object sender, EventArgs e)
        {
            u = (Usuario)Session["userLogged"];

            if (!IsPostBack && Session["userLogged"] == null)
            {
                Response.Redirect("Login.aspx");
            }
            else
            {
                LoadUserAccountDetails(u);
                GetUsersAccount();
                GetClientAccounts();
                DropdownConta.InnerHtml = u.Nome + "<span><svg xmlns=\"http://www.w3.org/2000/svg\" width=\"16\" height=\"16\" fill=\"currentColor\" class=\"bi bi-person-circle\" viewBox=\"0 0 16 16\">" +
                                                        "<path d=\"M11 6a3 3 0 1 1-6 0 3 3 0 0 1 6 0z\" />" +
                                                        "<path fill-rule=\"evenodd\" d=\"M0 8a8 8 0 1 1 16 0A8 8 0 0 1 0 8zm8-7a7 7 0 0 0-5.468 11.37C3.242 11.226 4.805 10 8 10s4.757 1.225 5.468 2.37A7 7 0 0 0 8 1z\" />" +
                                                        "</ svg>▼</span>";
                DropdownContaMenu.InnerHtml = "<li>" +
                                                    "<button type=\"button\" id=\"BtnAcessaContaCliente\" runat=\"server\" data-bs-toggle=\"modal\" data-bs-target=\"#ModalContaUserDetails\">Conta</button>" +
                                                    "<button type=\"button\" id=\"BtnLogoutContaCliente\" runat=\"server\" onclick=\"BtnLogoutUsuario()\">Sair</button>" +
                                              "</ li>";
            }
        }

        protected void LoadUserAccountDetails(Usuario usuario)
        {
            try
            {
                string qry = "SELECT * FROM usuario WHERE usuarioId = " + usuario.Id;
                DataTable dt = db.ExecuteReader(qry);

                TxbContaNome.Value = dt.Rows[0][2].ToString();
                TxbContaCargo.Value = dt.Rows[0][3].ToString();

                ListItem liValue = SelectDepartamento.Items.FindByValue(dt.Rows[0][4].ToString());
                liValue.Selected = true;

                TxbContaAdmissao.Value = dt.Rows[0][5].ToString();
            }
            catch (Exception ex)
            {
                DivAlert.Visible = true;
                LblAlert.InnerHtml = ex.Message;
            }
        }

        protected void GetUsersAccount()
        {
            string res = "";

            try
            {
                string query = "SELECT * FROM usuario";
                DataTable dt = db.ExecuteReader(query);

                res += "<tr>" +
                            "<th scope\"col\">#</th>" +
                            "<th scope\"col\">Email</th>" +
                            "<th scope=\"col\">Nome</th>" +
                            "<th scope=\"col\">Cargo</th>" +
                            "<th scope=\"col\">Departamento</th>" +
                            "<th scope=\"col\">Aprovar/Reprovar</th>" +
                       "</tr>";

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    res += "<tr>" +
                                "<td scope=\"row\">" + (i + 1) + "</td>" +
                                "<td>" + dt.Rows[i][1] + "</td>" +
                                "<td>" + dt.Rows[i][2] + "</td>" +
                                "<td>" + dt.Rows[i][3] + "</td>" +
                                "<td>" + dt.Rows[i][4] + "</td>" +
                                "<td>" +
                                "<button type=\"button\" class=\"btn btn-danger mx-1\" onclick=\"btnApproveOrRejectAccount_click(event)\" data-id=\"" + dt.Rows[i][0] + "\" data-bs-toggle=\"modal\" data-bs-target=\"#ModalRejectAccount\">reprovar</button>" +
                                "<button type=\"button\" class=\"btn btn-success\" onclick=\"btnApproveOrRejectAccount_click(event)\" data-id=\"" + dt.Rows[i][0] + "\" data-bs-toggle=\"modal\" data-bs-target=\"#ModalApproveAccount\">aprovar</button>" + 
                                "</td>" +
                           "</tr>";
                }

                TBodyAccountsUser.InnerHtml = res;
            }
            catch (Exception ex)
            {
                DivAlert.Visible = true;
                LblAlert.InnerHtml = ex.Message;
            }
        }

        protected void GetClientAccounts()
        {
            try
            {
                string res = "";

                string query = "SELECT * FROM cliente";
                DataTable dt = db.ExecuteReader(query);

                res += "<tr>" +
                            "<th scope\"col\">#</th>" +
                            "<th scope\"col\">Email</th>" +
                            "<th scope=\"col\">Nome</th>" +
                            "<th scope=\"col\">Aprovar/Reprovar</th>" +
                       "</tr>";

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    res += "<tr>" +
                                "<td scope=\"row\">" + (i + 1) + "</td>" +
                                "<td>" + dt.Rows[i][1] + "</td>" +
                                "<td>" + dt.Rows[i][2] + "</td>" +
                                "<td>" +
                                "<button type=\"button\" class=\"btn btn-danger mx-1\" onclick=\"btnApproveOrRejectAccount_click(event)\" data-id=\"" + dt.Rows[i][0] + "\" data-bs-toggle=\"modal\" data-bs-target=\"#ModalRejectAccount\">reprovar</button>" +
                                "<button type=\"button\" class=\"btn btn-success\" onclick=\"btnApproveOrRejectAccount_click(event)\" data-id=\"" + dt.Rows[i][0] + "\" data-bs-toggle=\"modal\" data-bs-target=\"#ModalApproveAccount\">aprovar</button>" +
                                "</td>" +
                           "</tr>";
                }

                TBodyAccountsClient.InnerHtml = res;
            }
            catch (Exception ex)
            {
                DivAlert.Visible = true;
                LblAlert.InnerHtml = ex.Message;
            }
        }

        protected void BtnConfirmAccount_Click(object sender, EventArgs e)
        {
            try
            {
                int idRow = Convert.ToInt16(ValueHiddenField.Value);
                string query = "SELECT usuarioAprovado FROM usuario WHERE usuarioId = " + idRow;
                DataTable dt = db.ExecuteReader(query);

                if ((bool)dt.Rows[0][0] == true)
                {
                    DivAlert.Visible = true;
                    LblAlert.InnerHtml = "Usuário já foi aprovado!";
                }
                else
                {
                    query = "UPDATE usuario SET usuarioAprovado = true WHERE usuarioId = " + idRow;
                    db.ExecuteNonQuery(query);
                }
            } catch (Exception ex)
            {
                DivAlert.Visible = true;
                LblAlert.InnerHtml = ex.Message;
            }
        }

        protected void BtnRejectAccount_Click(object sender, EventArgs e)
        {
            try
            {
                int idRow = Convert.ToInt16(ValueHiddenField.Value);
                string query = "SELECT usuarioAprovado FROM usuario WHERE usuarioId = " + idRow;
                DataTable dt = db.ExecuteReader(query);

                if ((bool)dt.Rows[0][0] == false)
                {
                    DivAlert.Visible = true;
                    LblAlert.InnerHtml = "Usuário já foi rejeitado!";
                }
                else
                {
                    query = "UPDATE usuario SET usuarioAprovado = false WHERE usuarioId = " + idRow;
                    db.ExecuteNonQuery(query);
                }
            }
            catch (Exception ex)
            {
                DivAlert.Visible = true;
                LblAlert.InnerHtml = ex.Message;
            }
            finally
            {
                Response.Redirect(Request.RawUrl);
            }

        }

        protected void BtnChangeAccountDetails_Click(object sender, EventArgs e)
        {
            u = (Usuario)Session["userLogged"];

            try
            {
                if (string.IsNullOrEmpty(TxbContaNome.Value.Trim())
                    || string.IsNullOrEmpty(TxbContaSenha.Value.Trim())
                    || string.IsNullOrEmpty(TxbContaCargo.Value.Trim()))
                {
                    DivAlert.Visible = true;
                    LblAlert.InnerText = "Preencha todos os campos para atualizar a conta";
                }
                else
                {
                    byte[] salt = new byte[16];
                    rngCsp.GetBytes(salt);

                    u.Senha = TxbContaSenha.Value.Trim();

                    byte[] key = new byte[20];

                    using (var deriveBytes = new Rfc2898DeriveBytes(u.Senha, salt, 1000))
                    {
                        key = deriveBytes.GetBytes(20);
                    }

                    byte[] hashBytes = new byte[36];

                    Array.Copy(salt, 0, hashBytes, 0, 16);
                    Array.Copy(key, 0, hashBytes, 16, 20);

                    string hashSenha = Convert.ToBase64String(hashBytes);

                    string qry = "UPDATE usuario SET usuarioNome = @NOME, usuarioHashSenha = @SENHA, usuarioCargo = @CARGO, usuarioDepartamento = @DEPARTAMENTO WHERE usuarioId = " + u.Id;
                    using (MySqlCommand cmd = new MySqlCommand(qry))
                    {
                        cmd.Parameters.AddWithValue("@NOME", TxbContaNome.Value.Trim());
                        cmd.Parameters.AddWithValue("@SENHA", hashSenha);
                        cmd.Parameters.AddWithValue("@CARGO", TxbContaCargo.Value.Trim());
                        cmd.Parameters.AddWithValue("@DEPARTAMENTO", SelectDepartamento.Value.Trim());
                        db.ExecuteNonQuery(qry, cmd);
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
                LoadUserAccountDetails(u);
                GetUsersAccount();
                GetClientAccounts();
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