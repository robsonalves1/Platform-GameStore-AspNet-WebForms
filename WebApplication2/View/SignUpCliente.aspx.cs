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

    public partial class SignUpCliente : System.Web.UI.Page
    {
        CntrDB db = new CntrDB();
        DataTable dt = new DataTable();
        private static RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void BtnSignUpClient_Click(object sender, EventArgs e)
        {
            try
            {
                DivAlert.Visible = false;
                LblAlert.InnerHtml = "";

                if (string.IsNullOrEmpty(TxbEmail.Value.Trim())
                    || string.IsNullOrEmpty(TxbNome.Value.Trim())
                    || string.IsNullOrEmpty(TxbSenha.Value.Trim()))
                {
                    DivAlert.Visible = true;
                    LblAlert.InnerHtml = "Por favor, preencha todos os campos.";
                }
                else
                {
                    Cliente c = new Cliente();
                    c.Nome = TxbNome.Value.Trim();
                    c.Email = TxbEmail.Value.Trim();
                    c.Senha = TxbSenha.Value.Trim();

                    string qryChecarEmail = "SELECT clienteEmail FROM cliente WHERE clienteEmail = '" + c.Email + "'";
                    dt = db.ExecuteReader(qryChecarEmail);
                    if (dt.Rows.Count > 0)
                    {
                        DivAlert.Visible = true;
                        LblAlert.InnerHtml = "Já existe esse Email cadastrado!";
                    }
                    else
                    {
                        byte[] salt = new byte[16];
                        rngCsp.GetBytes(salt);

                        Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(c.Senha, salt, 1000);
                        byte[] hash = pbkdf2.GetBytes(20);
                        byte[] hashBytes = new byte[36];

                        Array.Copy(salt, 0, hashBytes, 0, 16);
                        Array.Copy(hash, 0, hashBytes, 16, 20);

                        string hashSenhaString = Convert.ToBase64String(hashBytes);

                        string qry = "INSERT INTO cliente(clienteEmail, clienteNome, clienteHashSenha) VALUES(@EMAIL, @NOME, @HASHSENHA)";
                        using (MySqlCommand cmd = new MySqlCommand(qry))
                        {
                            cmd.Parameters.AddWithValue("@EMAIL", c.Email);
                            cmd.Parameters.AddWithValue("@NOME", c.Nome);
                            cmd.Parameters.AddWithValue("@HASHSENHA", hashSenhaString);
                            db.ExecuteNonQuery(qry, cmd);
                        }

                        divAlertUsuarioCadastrado.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                DivAlert.Visible = true;
                LblAlert.InnerHtml = ex.Message;
            }
        }

        protected void BtnCancelSignUpClient_Click(object sender, EventArgs e)
        {
            Response.Redirect("LoginCliente.aspx");
        }

    }
}