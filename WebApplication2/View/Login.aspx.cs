using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication2.Controller;
using WebApplication2.Model;

namespace WebApplication2.View
{
    public partial class Login : System.Web.UI.Page
    {
        readonly CntrDB db = new CntrDB();
        
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void BtnLogin_Click(object sender, EventArgs e)
        {
            Usuario u = new Usuario();
            u.Email = TxbEmail.Value.Trim();
            u.Senha = TxbSenha.Value.Trim();
            bool emailMatch = true;
            bool passwordMatch = true;

            // CHECK IF THE USER IS APPROVED
            string qry = "SELECT usuarioAprovado FROM usuario WHERE usuarioEmail = '" + u.Email + "'";
            DataTable dt = db.ExecuteReader(qry);
            bool usuarioApproved = (bool)dt.Rows[0][0];

            try
            {
                if (string.IsNullOrEmpty(u.Email)
                    || string.IsNullOrEmpty(u.Senha))
                {
                    divAlert.Visible = true;
                    lblAlert.InnerHtml = "Por favor, preencha todos os campos.";
                }
                else if (usuarioApproved)
                {
                    string query = "SELECT usuarioId, usuarioNome, usuarioEmail, usuarioCargo, usuarioAdmissao, usuarioHashSenha, usuarioAdministrador FROM usuario WHERE usuarioEmail = '" + u.Email + "'";
                    dt = db.ExecuteReader(query);

                    if (dt.Rows.Count > 0)
                    {
                        byte[] storedHashSenhaBytes = Convert.FromBase64String((string)dt.Rows[0][5]);

                        byte[] storedSalt = new byte[16];
                        Array.Copy(storedHashSenhaBytes, 0, storedSalt, 0, 16);

                        Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(u.Senha, storedSalt, 1000);
                        byte[] hashTypedSenha = pbkdf2.GetBytes(20);

                        for (int i = 0; i < 20; i++)
                        {
                            if (storedHashSenhaBytes[i + 16] != hashTypedSenha[i])
                            {
                                passwordMatch = false;
                                break;
                            }
                        }

                        if (!passwordMatch)
                        {
                            divAlert.Visible = true;
                            lblAlert.InnerHtml = "Senha incorreta! ";
                        }
                        else
                        {
                            u.Id = (int)dt.Rows[0][0];
                            u.Nome = (string)dt.Rows[0][1];
                            u.Administrador = (bool)dt.Rows[0][6];
                            Session["userLogged"] = u;
                        }
                    } 
                    else
                    {
                        emailMatch = false;
                        divAlert.Visible = true;
                        lblAlert.InnerHtml = "Email não cadastrado!";
                    }
                }
                else
                {
                    divAlert.Visible = true;
                    lblAlert.InnerHtml = "Cadastro de usuário ainda precisa ser aprovado.";
                }
            }
            catch (Exception ex)
            {
                divAlert.Visible = true;
                lblAlert.InnerText = ex.Message;
            }
            finally
            {
                if (passwordMatch && emailMatch && usuarioApproved)
                    Response.Redirect("Home.aspx");
            }
        }
    }
}