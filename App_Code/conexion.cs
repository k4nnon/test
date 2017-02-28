using MySql.Data.MySqlClient;
using System;
using System.Diagnostics;

namespace Namespace
{

    /// <summary>
    /// se conecta a la base de datos interna de metacontrol, el cual es un motor de base de datos MYsql
    /// en caso de moficiar la conexion, solo basta con modificar el mysqlConnString
    /// </summary>
    public static class Conexion
    {
        /**
         * @autor Gonzalo Zeballos Alvarez mail: maxpayne_ga@hotmail.com
         * */
        static MySqlConnection connection = new MySqlConnection();
        static MySqlCommand Comando = new MySqlCommand();
        static MySqlDataAdapter Adaptador = new MySqlDataAdapter();
        static MySqlDataReader Reader;
        static string mysqlConnString = "Server=192.168.1.7; Database=sgsmtc2; userid=root; password='s0p0rte'; ";
        /**
         * inicia la conexion con la base de datos
         * @throw en caso que ocurra un error al conectar la base de datos
         * */
        private static void iniciarConexion()
        {
            try
            {
                connection.ConnectionString = mysqlConnString;
                connection.Open();
            }
            catch (Exception ex) {
                Debug.Print(ex.Message);
            }
        }

        /**
         * desconecta la ultima conexicion con la base de datos
         * */
        public static void Desconectar()
        {
            connection.Close();
           
        }

        /**
         * inicia una conexion con la base de datos y genera un adapter el cual lo retorna
         * @param queyr: la consulta sql que se realizara en la base de datos
         * @return un adapter con los datos solicitados
         * @throw en caso que la base de datos falle.
         * */
        public static MySqlDataAdapter adapter(string query)
        {

            try
            {

                connection.ConnectionString = mysqlConnString;
                connection.Open();
            }
            catch (Exception ex)
            {
                Debug.Print(ex.Message);
            }
            MySqlConnection cn = new MySqlConnection(mysqlConnString);
            return new MySqlDataAdapter(query, cn);
        }

        /**
         * recibe un comando sql el cual se ingresara en la base de datos
         * @param sqlQuery: los datos que se ingresaran en la base de datos
         * */
        public static void insert(string sqlQuery)
        {
            iniciarConexion();
            Comando.CommandText = sqlQuery;
            Comando.Connection = connection;
            Comando.ExecuteNonQuery();
        }

        /**
        * recibe un comando sql el cual buscara la informacion en la base de datos
        * @param sqlQuery: los datos que se buscaran en la base de datos
        * @return un reader para luego obtener informacion de este
        * */
        public static MySqlDataReader reader(string sqlQuery)
        {
            iniciarConexion();
            Comando.CommandText = sqlQuery;
            Comando.Connection = connection;
            return Reader = Comando.ExecuteReader();
        }

        /**
        * recibe un comando sql el cual actualiza lo datos en la base de datos
        * @param sqlQuery: los datos que se acutalizaran en la base de datos
        * */
        public static void update (string sqlQuery)
        {
            iniciarConexion();
            Comando.CommandText = sqlQuery;
            Comando.Connection = connection;
            Comando.ExecuteNonQuery();
        }
    }
        
}