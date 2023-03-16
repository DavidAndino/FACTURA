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
                        comando.Parameters.Add("@Id", MySqlDbType.VarChar, 13).Value = id;
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
        //creando metodo que permitira la inserción de datos en la DB
        public bool insert(Client customer)
        {
            bool inserted = false;
            try
            {
                StringBuilder sql = new StringBuilder();//armando sentencia Sql through un objeto
                sql.Append(" INSERT INTO client VALUES ");/*seleccionando un dato de la tabla en la DB. 
                                                  * "user" nombre de la tabla en el motor de DB. */
                sql.Append(" (@Id, @Name, @Phone, @PersonalMail, @Addres, @BirthDate, @Active); ");//sentencias para insertar un new registro en la DB

                using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))//pasando un objeto de la conexion hacia MySql
                {
                    conexion.Open();//abriendo conexion

                    using (MySqlCommand comando = new MySqlCommand(sql.ToString(), conexion))
                    {
                        comando.CommandType = CommandType.Text;/*especificando el tipo de comando que se ejecutara
                                                                            en este caso es un comando de texto, pues ee el codigo varchar el que se ingresa*/
                        //estas sentencias se tienen que hacer en el orden que sale en la tabla del motor de la DB
                        comando.Parameters.Add("@Id", MySqlDbType.VarChar, 13).Value = customer.iD;//pasando parametros al objeto de comando. 1ro: nombre parametro. 2do:tipo dato
                        comando.Parameters.Add("@Name", MySqlDbType.VarChar, 50).Value = customer.name;
                        comando.Parameters.Add("@Phone", MySqlDbType.VarChar, 15).Value = customer.phone;
                        comando.Parameters.Add("@PersonalMail", MySqlDbType.VarChar, 45).Value = customer.personalMail;
                        comando.Parameters.Add("@Addres", MySqlDbType.VarChar, 100).Value = customer.addres;
                        comando.Parameters.Add("@BirthDate", MySqlDbType.Date).Value = customer.birthDate;//ojo Date
                        comando.Parameters.Add("@Active", MySqlDbType.Bit).Value = customer.active;
                        comando.ExecuteNonQuery();//se va a ejecutar, pero no se devolvera algun registro
                        inserted = true;

                    }//esta sentencia de comando ejecuta la sentencia de sql
                }//esta sentencia ayuda a que, cuando termina la conexion con la DB, cerrar la conexion automatically
            }
            catch (System.Exception)
            {
            }

            return inserted;
        }
        public bool edit(Client customer)
        {
            bool edited = false;
            try
            {
                StringBuilder sql = new StringBuilder();//armando sentencia Sql through un objeto
                sql.Append(" UPDATE client SET ");//sentencia para editar algun registro

                sql.Append(" Name = @Name, Phone = @Phone, PersonalMail = @PersonalMail, Addres = @Addres, BirthDate = @BirthDate, Active = @Active ");//sentencias para editar todos los registros en la DB
                sql.Append(" WHERE Id = @Id; ");//solo permitiendo que se edite un cliente por un Id especificado

                using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))//pasando un objeto de la conexion hacia MySql
                {
                    conexion.Open();//abriendo conexion

                    using (MySqlCommand comando = new MySqlCommand(sql.ToString(), conexion))
                    {
                        comando.CommandType = System.Data.CommandType.Text;/*especificando el tipo de comando que se ejecutara
                                                                            en este caso es un comando de texto, pues ee el codigo varchar el que se ingresa*/
                        //estas sentencias se tienen que hacer en el orden que sale en la tabla del motor de la DB
                        comando.Parameters.Add("@Id", MySqlDbType.VarChar, 13).Value = customer.iD;//pasando parametros al objeto de comando. 1ro: nombre parametro. 2do:tipo dato
                        comando.Parameters.Add("@Name", MySqlDbType.VarChar, 50).Value = customer.name;
                        comando.Parameters.Add("@Phone", MySqlDbType.VarChar, 15).Value = customer.phone;
                        comando.Parameters.Add("@PersonalMail", MySqlDbType.VarChar, 45).Value = customer.personalMail;
                        comando.Parameters.Add("@Addres", MySqlDbType.VarChar, 100).Value = customer.addres;
                        comando.Parameters.Add("@BirthDate", MySqlDbType.Date).Value = customer.birthDate;
                        comando.Parameters.Add("@Active", MySqlDbType.Bit).Value = customer.active;
                        comando.ExecuteNonQuery();//se va a ejecutar, pero no se devolvera algun registro
                        edited = true;

                    }//esta sentencia de comando ejecuta la sentencia de sql
                }//esta sentencia ayuda a que, cuando termina la conexion con la DB, cerrar la conexion automatically
            }
            catch (Exception)
            {
            }

            return edited;//se devuelve la variable "edited"
        }
        public bool delete(string clientId)
        {
            bool deleted = false;
            try
            {
                StringBuilder sql = new StringBuilder();//armando sentencia Sql through un objeto
                sql.Append(" DELETE FROM client ");//eliminar de la tabla "user" de la base de datos
                sql.Append(" WHERE Id = @Id; ");

                using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))//pasando un objeto de la conexion hacia MySql
                {
                    conexion.Open();//abriendo conexion

                    using (MySqlCommand comando = new MySqlCommand(sql.ToString(), conexion))
                    {
                        comando.CommandType = CommandType.Text;/*especificando el tipo de comando que se ejecutara
                                                                            en este caso es un comando de texto*/
                        comando.Parameters.Add("@Id", MySqlDbType.VarChar, 13).Value = clientId;//pasando parametros al objeto de comando. 1ro: nombre parametro. 2do:tipo dato

                        comando.ExecuteNonQuery();//se va a ejecutar, pero no se devolver algun registro
                        deleted = true;

                    }//esta sentencia de comando ejecuta la sentencia de sql
                }//esta sentencia ayuda a que, cuando termina la conexion con la DB, cerrar la conexion automatically
            }
            catch (Exception)
            {
            }

            return deleted;//se devuelve el objeto "user" de  la clase "Usuario", para validar que todos los datos sean correctos a la hora de ingresar a la DB
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
                }//esta sentencia ayuda a que, cuando termina la conexion con la DB, se cierre la conexion automatically
            }
            catch (System.Exception)
            {
            }

            return dt;
        }
        //creando metodo que consultara el FindClientForm, para que lo que el cajero escriba coincida con registros de clientes en la tabla
        public DataTable bringClientsForName(string name)
        {
            DataTable dt = new DataTable();//instanciando objeto de esta clase
            try
            {
                StringBuilder sql = new StringBuilder();//armando sentencia Sql through un objeto
                sql.Append("SELECT * FROM client WHERE Name LIKE ('%@Name%'); ");/*LIKE filtra todos los resultados que sean iguales o contegan 
                                                                           el valor del nombre que se trae en el parametro "name@ 
                el simbolo de resto duplicado permite buscar varios elementos en el nombre de una Entidad (nombres, apellidos etc) a la vez*/

                using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))//pasando un objeto de la conexion hacia MySql
                {
                    conexion.Open();//abriendo conexion

                    using (MySqlCommand comando = new MySqlCommand(sql.ToString(), conexion))
                    {
                        comando.CommandType = CommandType.Text;//*especificando el tipo de comando que se ejecutara
                        comando.Parameters.Add("@Name", MySqlDbType.VarChar, 50).Value = name;
                        MySqlDataReader dr = comando.ExecuteReader();//trayendo los datos
                        dt.Load(dr);//llenando el objeto de tipo "DataTable" con los registros almacenados en el objeto "dr" 
                    }//esta sentencia de comando ejecuta la sentencia de sql
                }//esta sentencia ayuda a que, cuando termina la conexion con la DB, se cierre la conexion automatically
            }
            catch (System.Exception)
            {
            }

            return dt;
        }
    }


}
