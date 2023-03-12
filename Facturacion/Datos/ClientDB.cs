using Entidades;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Text;

namespace Datos
{
    public class ClientDB
    {
        string cadenaConexion = "server = localhost; user = root; database = factura; password = 223344";/*se hace esto solo si el server siempre sera el mismo
                                                                                                          */
        public Client bringClientsForID(string id)
        {
            Client customer = null;
            try
            {
                StringBuilder sql = new StringBuilder();//armando sentencia Sql through un objeto
                sql.Append("SELECT * FROM client WHERE Id = @Id");//sentencia para traer registros de la tabla de la base de datos

                using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))//pasando un objeto de la conexion hacia MySql
                {
                    conexion.Open();//abriendo conexion

                    using (MySqlCommand comando = new MySqlCommand(sql.ToString(), conexion))
                    {
                        comando.CommandType = CommandType.Text;//*especificando el tipo de comando que se ejecutara
                        MySqlDataReader dr = comando.ExecuteReader();//trayendo los datos
                        //pasando a cada propiedad los datos que se guardan en el objeto "dr"
                        if (dr.Read())
                        {
                            customer = new Client();//instanciando objeto
                            customer.iD = id;
                            customer.name = dr["Name"].ToString();
                            customer.phone = dr["Phone"].ToString();
                            customer.personalMail = dr["PersonalMail"].ToString();
                            customer.addres = dr["Addres"].ToString();
                            customer.birthDate = Convert.ToDateTime(dr["BirthDate"]);
                            customer.active = Convert.ToBoolean(dr["Active"]);
                        }

                    }//esta sentencia de comando ejecuta la sentencia de sql
                }//esta sentencia ayuda a que, cuando termina la conexion con la DB, cerrar la conexion automatically
            }
            catch (Exception)
            {
            }

            return customer;//se devuelve el objeto "user" de  la clase "Usuario", para validar que todos los datos sean correctos a la hora de ingresar a la DB
        }
        public DataTable bringClients()
        {
            DataTable dt = new DataTable();//instanciando objeto de esta clase
            try
            {
                StringBuilder sql = new StringBuilder();//armando sentencia Sql through un objeto
                sql.Append("SELECT * FROM client");//sentencia para traer registros de la tabla de la base de datos

                using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))//pasando un objeto de la conexion hacia MySql
                {
                    conexion.Open();//abriendo conexion

                    using (MySqlCommand comando = new MySqlCommand(sql.ToString(), conexion))
                    {
                        comando.CommandType = CommandType.Text;//*especificando el tipo de comando que se ejecutara

                        MySqlDataReader dr = comando.ExecuteReader();//trayendo los datos
                        dt.Load(dr);//llenando el objeto de tipo "DataTable" con los registros almacenados en el objeto "dr" 
                    }//esta sentencia de comando ejecuta la sentencia de sql
                }//esta sentencia ayuda a que, cuando termina la conexion con la DB, cerrar la conexion automatically
            }
            catch (System.Exception)
            {
            }

            return dt;//se devuelve el objeto "user" de  la clase "Usuario", para validar que todos los datos sean correctos a la hora de ingresar a la DB
        }
    }


}
