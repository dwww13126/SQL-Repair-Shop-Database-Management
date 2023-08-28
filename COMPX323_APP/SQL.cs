using System;
using Oracle.ManagedDataAccess.Client;
using System.Windows.Forms;

namespace COMPX323_APP
{
    class SQL
    {
        //Connection string for connecting to the Oracle Database
        static string oradb = "Data Source=<DATASOURCE>;DBA Privilege=SYSDBA;User Id=<USERID>;Password=<PASSWORD>;";
        public static OracleConnection con;
        public static OracleCommand cmd;
        public static OracleDataReader dr;

        //Begins the connection with the Database
        public static void initialize()
        {
            con = new OracleConnection(oradb);
            con.Open();
            cmd = new OracleCommand();
            cmd.Connection = con;
            Console.WriteLine("Database initialized");
        }
        //Ends the connection with the database
        public static void end()
        {
            dr.Dispose();
            con.Dispose();
        }

        /// <summary>
        /// Excecutes a query without reading any results of the operation
        /// </summary>
        /// <param name="query"></param>
        public static void executeQuery(string query)
        {
            //Try then catch to catch any unforseen errors gracefully
            try
            {
                cmd.CommandText = query;
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
        }

        /// <summary>
        /// Generates an SQL query based on the passed in input
        /// string e.g. "SELECT * FROM staff"
        /// </summary>
        /// <param name="query"></param>
        public static void selectQuery(string query)
        {
            try
            {
                cmd.CommandText = query;
                dr = cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                return;
            }
        }
    }
}
