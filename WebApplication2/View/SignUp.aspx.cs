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
    public partial class SignUp : System.Web.UI.Page
    {
        readonly CntrDB db = new CntrDB();
        private static readonly RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();
        
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void BtnSignUpUser_Click(object sender, EventArgs e)
        {
            try
            {
                divAlert.Visible = false;
                divAlertUsuarioCadastrado.Visible = false;

                if (string.IsNullOrEmpty(TxbNome.Value.Trim()) 
                    || string.IsNullOrEmpty(TxbEmail.Value.Trim())
                    || string.IsNullOrEmpty(TxbSenha.Value.Trim())
                    || string.IsNullOrEmpty(TxbCargo.Value.Trim())
                    || string.IsNullOrEmpty(SelectDepartamento.Value.Trim())
                    || string.IsNullOrEmpty(SelectAdministrador.Value.Trim()))
                {
                    divAlert.Visible = true;
                    lblAlert.InnerText = "Por favor, preencha todos os campos.";
                }
                else
                {
                    Usuario u = new Usuario();
                    u.Email = TxbEmail.Value.Trim();
                    
                    string qryEmail = "SELECT usuarioEmail FROM usuario WHERE usuarioEmail = '" + u.Email + "'";
                    DataTable dt = db.ExecuteReader(qryEmail);
                    if (dt.Rows.Count > 0)
                    {
                        divAlert.Visible = true;
                        lblAlert.InnerText = "Já existe esse Email cadastrado!";
                        return;
                    }

                    byte[] salt = new byte[16];
                    rngCsp.GetBytes(salt);
                    u.Senha = TxbSenha.Value.Trim();

                    var pbkdf2 = new Rfc2898DeriveBytes(u.Senha, salt, 1000);
                    byte[] hash = pbkdf2.GetBytes(20);
                    byte[] hashBytes = new byte[36];

                    Array.Copy(salt, 0, hashBytes, 0, 16);
                    Array.Copy(hash, 0, hashBytes, 16, 20);

                    string hashSenha = Convert.ToBase64String(hashBytes);

                    u.Nome = TxbNome.Value.Trim();
                    u.Cargo = TxbCargo.Value.Trim();
                    u.Admissao = DateTime.UtcNow.ToLocalTime();
                    u.Departamento = SelectDepartamento.Value.Trim();
                    u.Administrador = Convert.ToBoolean(SelectAdministrador.Value.Trim());

                    string query = "INSERT INTO usuario(usuarioNome, usuarioEmail, usuarioCargo, usuarioDepartamento, usuarioAdmissao, usuarioAdministrador, usuarioHashSenha, usuarioAprovado)" +
                        "VALUES(@NOME, @EMAIL, @CARGO, @DEPARTAMENTO, @ADMISSAO, @ADMINISTRADOR, @HASHSENHA, @APROVADO)";
                    using (MySqlCommand cmd = new MySqlCommand(query))
                    {
                        cmd.Parameters.AddWithValue("@NOME", u.Nome);
                        cmd.Parameters.AddWithValue("@EMAIL", u.Email);
                        cmd.Parameters.AddWithValue("@CARGO", u.Cargo);
                        cmd.Parameters.AddWithValue("@DEPARTAMENTO", u.Departamento);
                        cmd.Parameters.AddWithValue("@ADMISSAO", u.Admissao);
                        cmd.Parameters.AddWithValue("@ADMINISTRADOR", u.Administrador);
                        cmd.Parameters.AddWithValue("@HASHSENHA", hashSenha);
                        cmd.Parameters.AddWithValue("@APROVADO", false);
                        db.ExecuteNonQuery(query, cmd);
                    }
                }

                divAlertUsuarioCadastrado.Visible = true;
            }
            catch (Exception ex)
            {
                divAlert.Visible = true;
                lblAlert.InnerText = ex.Message;
            }
        }

        protected void BtnCancelSignUpUser_Click(object sender, EventArgs e)
        {
            Response.Redirect("Login.aspx");
        }
    }
}