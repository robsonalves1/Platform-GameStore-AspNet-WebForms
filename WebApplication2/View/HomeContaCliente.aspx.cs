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
    public partial class HomeContaCliente : System.Web.UI.Page
    {
        private static RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();

        protected void Page_Load(object sender, EventArgs e)
        {
            LoadAccount();
        }

        protected void BtnModifyAccountClient_Click(object sender, EventArgs e)
        {
            CntrDB db = new CntrDB();
            DataTable dt = new DataTable();
            Cliente c = new Cliente();
            c = (Cliente)Session["clienteLogado"];
            c.Nome = TxbNomeModificar.Value.ToString().Trim();
            c.Senha = TxbSenhaAntigaModificar.Value.ToString().Trim();
            bool passwordMatch = true;
            DivAlertContaAtualizada.Visible = false;

            try
            {
                if (string.IsNullOrEmpty(TxbNomeModificar.Value.Trim()) ||
                    string.IsNullOrEmpty(TxbSenhaAtualModificar.Value.Trim()))
                {
                    DivAlert.Visible = true;
                    LblAlert.InnerHtml = "Por Favor preencha todos os campos corretamente.";
                }
                else
                {
                    // VERIFY IF THE OLD PASSWORD WAS TYPED CORRECTLY
                    string qry = "SELECT clienteHashSenha FROM cliente WHERE clienteId = " + c.Id;
                    dt = db.ExecuteReader(qry);

                    byte[] storedHashPassword = Convert.FromBase64String(dt.Rows[0][0].ToString());
                    byte[] storedSalt = new byte[16];

                    Array.Copy(storedHashPassword, 0, storedSalt, 0, 16);
                    Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(c.Senha, storedSalt, 1000);
                    byte[] hashTypedOldPassword = pbkdf2.GetBytes(20);

                    for (int i = 0; i < 20; i++)
                    {
                        if (hashTypedOldPassword[i] != storedHashPassword[i + 16])
                        {
                            passwordMatch = false;
                            break;
                        }
                    }

                    if (!passwordMatch)
                    {
                        DivAlert.Visible = true;
                        LblAlert.InnerHtml = "Senha antiga incorreta.";
                    } 
                    else
                    {
                        c.Senha = TxbSenhaAtualModificar.Value.Trim();

                        // CRYPTOGRAPH THE NEW PASSWORD
                        byte[] newPasswordSalt = new byte[16];
                        rngCsp.GetBytes(newPasswordSalt);

                        pbkdf2 = new Rfc2898DeriveBytes(c.Senha, newPasswordSalt, 1000);
                        byte[] newHash = pbkdf2.GetBytes(20);
                        byte[] newHashPassword = new byte[36];

                        Array.Copy(newPasswordSalt, 0, newHashPassword, 0, 16);
                        Array.Copy(newHash, 0, newHashPassword, 16, 20);

                        string newHashPasswordString = Convert.ToBase64String(newHashPassword);

                        string query = "UPDATE cliente SET clienteNome = @NOME, clienteHashSenha = @SENHA WHERE clienteId = @ID";
                        using (MySqlCommand cmd = new MySqlCommand(query))
                        {
                            cmd.Parameters.AddWithValue("@NOME", c.Nome);
                            cmd.Parameters.AddWithValue("@SENHA", newHashPasswordString);
                            cmd.Parameters.AddWithValue("@ID", c.Id);
                            db.ExecuteNonQuery(query, cmd);
                        }

                        DivAlertContaAtualizada.Visible = true;
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
                LoadAccount();
            }
        }

        protected void LoadAccount()
        {
            try
            {
                CntrDB db = new CntrDB();
                DataTable dt = new DataTable();
                Cliente c = new Cliente();
                c = (Cliente)Session["clienteLogado"];

                string query = "SELECT clienteNome, clienteEmail FROM cliente WHERE clienteId = " + c.Id;
                dt = db.ExecuteReader(query);

                TxbNome.InnerHtml = dt.Rows[0][0].ToString();
                TxbEmail.InnerHtml = dt.Rows[0][1].ToString();

                TxbEmailModificar.Value = dt.Rows[0][1].ToString();

                // LOAD PURCHASED PRODUCTS
                string qry = "SELECT produtoNome, produtoFoto, produtoQuantidadeComprada FROM clienteCompras";
                dt = db.ExecuteReader(qry);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TbodyProdutosComprados.InnerHtml +=
                        "<tr>" +
                            "<th scope=\"row\">" + (i + 1) + "</th>";

                    string imgProduto = (string)dt.Rows[i][1];
                    string primeiraPalavraImg = imgProduto.Split('/')[0];
                    if (primeiraPalavraImg == "https:")
                    {
                        TbodyProdutosComprados.InnerHtml +=
                            "<td><img src=\"" + dt.Rows[i][1] + "\"/></td>";
                    }
                    else
                    {
                        TbodyProdutosComprados.InnerHtml +=
                            "<td><img src=\"data:image/jpg;base64," + dt.Rows[i][1] + "\"/></td>";
                    }
                    TbodyProdutosComprados.InnerHtml +=
                        "<td>" + dt.Rows[i][0] + "</td>" +
                        "<td>" + dt.Rows[i][2] + "</td>" +
                    "</tr>";
                }
            } 
            catch (Exception ex)
            {
                DivAlert.Visible = true;
                LblAlert.InnerHtml = ex.Message;
            }
        }

        protected void BtnPreviousPage_Click(object sender, EventArgs e)
        {
            Response.Redirect("HomeLoja.aspx");
        }
    }
}