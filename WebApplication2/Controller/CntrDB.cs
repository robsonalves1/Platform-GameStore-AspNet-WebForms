using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace WebApplication2.Controller
{
    public class CntrDB
    {
        #region MySql Commands
        private string ConnectionString()
        {
            string cnt = "Data Source=localhost;Initial Catalog=mytestdb; User ID=root;Password=1234; Connection Timeout = 180";
            return cnt;
        }


        public void ExecuteNonQuery(string qry)
        {
            ExecuteNonQuery(qry, null);
        }
        public void ExecuteNonQuery(string qry, MySqlCommand MyCmd)
        {
            using (var connection = new MySqlConnection(ConnectionString()))
            {
                try
                {
                    if (MyCmd == null)
                    {
                        MySqlCommand cmd = new MySqlCommand(qry, connection);
                        cmd.CommandTimeout = 600;
                        connection.Open();
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        MyCmd.Connection = connection;
                        MyCmd.CommandTimeout = 600;
                        connection.Open();
                        MyCmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    connection.Close();
                }
            }
        }


        public DataTable ExecuteReader(string qry)
        {
            using (var connection = new MySqlConnection(ConnectionString()))
            {
                try
                {
                    MySqlCommand cmd = new MySqlCommand(qry, connection);
                    DataTable tb = new DataTable();
                    cmd.CommandTimeout = 600;
                    connection.Open();
                    MySqlDataReader dtAdapter = cmd.ExecuteReader();
                    tb.Load(dtAdapter);
                    return tb;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        #endregion
    }
}