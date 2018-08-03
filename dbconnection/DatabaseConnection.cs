using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;


namespace StudentDbManagement.Database
{
    public class DatabaseConnection
    {
        private SqlConnection conn=null;
        //private DatabaseConnection() { }
        public  SqlConnection getconnection
        {
            get
            {
                if (conn != null)
                {
                    return conn;
                }
                else
                {
                    try
                    {
                        conn = new SqlConnection(getConnectionString());
                        return conn;
                    }
                    catch(SqlException)
                    {
                        throw;
                    }
                }
                
            }
        }

        public  string getConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString;
        }
    }
}