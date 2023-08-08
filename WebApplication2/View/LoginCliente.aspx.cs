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
    public partial class LoginCliente : System.Web.UI.Page
    {
        CntrDB db = new CntrDB();
        DataTable dt = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void BtnLoginClient_Click(object sender, EventArgs e)
        {
            Cliente c = new Cliente();
            c.Email = TxbEmail.Value.Trim();
            c.Senha = TxbSenha.Value.Trim();
            bool passwordMatch = true;

            try
            {
                if (string.IsNullOrEmpty(c.Email)
                    || string.IsNullOrEmpty(c.Senha))
                {
                    DivAlert.Visible = true;
                    LblAlert.InnerHtml = "Por favor, preencha todos os campos.";
                }
                else
                {
                    string query = "SELECT * FROM cliente WHERE clienteEmail = '" + c.Email + "'";
                    dt = db.ExecuteReader(query);
                    if (dt.Rows.Count > 0)
                    {
                        byte[] storedHashSenha = Convert.FromBase64String(dt.Rows[0][3].ToString());
                        byte[] storedSalt = new byte[16];

                        Array.Copy(storedHashSenha, 0, storedSalt, 0, 16);

                        Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(c.Senha, storedSalt, 1000);
                        byte[] hashTypedSenha = pbkdf2.GetBytes(20);

                        for (int i = 0; i < hashTypedSenha.Length; i++)
                        {
                            if (hashTypedSenha[i] != storedHashSenha[i + 16])
                            {
                                passwordMatch = false;
                                break;
                            }
                        }

                        if (!passwordMatch)
                        {
                            DivAlert.Visible = true;
                            LblAlert.InnerHtml = "Senha incorreta!";
                        }
                        else
                        {
                            c.Id = (int)dt.Rows[0][0];
                            c.Nome = (string)dt.Rows[0][2];
                            Session["clienteLogado"] = c;
                        }
                    } 
                    else
                    {
                        DivAlert.Visible = true;
                        LblAlert.InnerHtml = "Email não cadastrado!";
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
                if (Session["clienteLogado"] != null)
                {
                    Response.Redirect("HomeLoja.aspx");
                }
            }
        }
    }
}