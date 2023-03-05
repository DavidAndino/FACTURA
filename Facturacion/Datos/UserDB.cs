using Entidades;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Text;
namespace Datos

{
    public class UserDB
    {
        string cadenaConexion = "server = localhost; user = root; database = factura; password = 223344";/*creando ruta hacia donde se ira para poderse conectar donde esta el server
                                                                                                          esta cadena trae todos los datos del servidor*/

        //metodos para interactuar con la tabla  "User"
        public Usuario Autenticacion(Login login)//este metodo recibe como parametro un objeto de la clase "Login"
        {
            Usuario user = null;//declarando objeto de tipo Usuario de la clase Entidades
            //creando sentencia "try catch" para capturar excepciones o errores, para evitar que la app se cierre bruscamente
            try
            {
                StringBuilder sql = new StringBuilder();//armando sentencia Sql through un objeto
                sql.Append("SELECT * FROM user WHERE UserCode = @UserCode AND Password = @Password");/*seleccionando un dato de la tabla en la DB. 
                                                  * "user" nombre de la tabla en el motor de DB. SELECT devuelve todos los usuarios, WHERE solo un especifico */

                using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))//pasando un objeto de la conexion hacia MySql
                {
                    conexion.Open();//abriendo conexion

                    using (MySqlCommand comando = new MySqlCommand(sql.ToString(), conexion))
                    {
                        comando.CommandType = System.Data.CommandType.Text;/*especificando el tipo de comando que se ejecutara
                                                                            en este caso es un comando de texto, pues el el codigo varchar el que se ingresa*/
                        comando.Parameters.Add("@UserCode", MySqlDbType.VarChar, 50).Value = login.codigoUser;//pasando parametros al objeto de comando. 1ro: nombre parametro. 2do:tipo dato
                        comando.Parameters.Add("@Password", MySqlDbType.VarChar, 50).Value = login.password;

                        MySqlDataReader capturadorRegistros = comando.ExecuteReader();//trayendo o capturando datos o registros por medio de esta instancia
                        //validando si hay algo en el objeto (capturarRegistros) de tipo DataReader
                        if (capturadorRegistros.Read())
                        {
                            user = new Usuario();//instanciando o creando objeto
                            user.userCode = capturadorRegistros["UserCode"].ToString();//pasandole datos o registros al objeto "user" de la clase Usuario
                            user.name = capturadorRegistros["Name"].ToString();
                            user.password = capturadorRegistros["Password"].ToString();
                            user.mail = capturadorRegistros["Mail"].ToString();
                            user.role = capturadorRegistros["Role"].ToString();
                            user.creationDate = (DateTime)capturadorRegistros["CreationDate"];//verificar ese casting
                            user.active = Convert.ToBoolean(capturadorRegistros["Active"]);
                            user.photo = (byte[])capturadorRegistros["Photo"];//haciendo "casting"
                        }
                    }//esta sentencia de comando ejecuta la sentencia de sql
                }//esta sentencia ayuda a que, cuando termina la conexion con la DB, cerrar la conexion automatically
            }
            catch (System.Exception)
            {
            }

            return user;//se devuelve el objeto "user" de  la clase "Usuario", para validar que todos los datos sean correctos a la hora de ingresar a la DB
        }

        //creando metodo que permitira la inserción de datos en la DB
        public Boolean insert(Usuario user)
        {
            bool inserted = false;
            try
            {
                StringBuilder sql = new StringBuilder();//armando sentencia Sql through un objeto
                sql.Append("INSERT INTO user VALUES");/*seleccionando un dato de la tabla en la DB. 
                                                  * "user" nombre de la tabla en el motor de DB. */
                sql.Append("(@UserCode, @Name, @Password, @Mail, @Role, @CreationDate, @Active, @Photo); ");//sentencias para insertar un new registro en la DB

                using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))//pasando un objeto de la conexion hacia MySql
                {
                    conexion.Open();//abriendo conexion

                    using (MySqlCommand comando = new MySqlCommand(sql.ToString(), conexion))
                    {
                        comando.CommandType = CommandType.Text;/*especificando el tipo de comando que se ejecutara
                                                                            en este caso es un comando de texto, pues ee el codigo varchar el que se ingresa*/
                        //estas sentencias se tienen que hacer en el orden que sale en la tabla del motor de la DB
                        comando.Parameters.Add("@UserCode", MySqlDbType.VarChar, 50).Value = user.userCode;//pasando parametros al objeto de comando. 1ro: nombre parametro. 2do:tipo dato
                        comando.Parameters.Add("@Name", MySqlDbType.VarChar, 50).Value = user.name;
                        comando.Parameters.Add("@Password", MySqlDbType.VarChar, 80).Value = user.password;
                        comando.Parameters.Add("@Mail", MySqlDbType.VarChar, 20).Value = user.mail;
                        comando.Parameters.Add("@Role", MySqlDbType.VarChar, 25).Value = user.role;
                        comando.Parameters.Add("@CreationDate", MySqlDbType.DateTime).Value = user.creationDate;
                        comando.Parameters.Add("@Active", MySqlDbType.Bit).Value = user.active;
                        comando.Parameters.Add("@Photo", MySqlDbType.LongBlob).Value = user.photo;
                        comando.ExecuteNonQuery();//se va a ejecutar, pero no se devolver algun registro
                        inserted = true;

                    }//esta sentencia de comando ejecuta la sentencia de sql
                }//esta sentencia ayuda a que, cuando termina la conexion con la DB, cerrar la conexion automatically
            }
            catch (System.Exception)
            {
            }

            return inserted;//
        }
        public bool edit(Usuario user)
        {
            Boolean edited = false;
            try
            {
                StringBuilder sql = new StringBuilder();//armando sentencia Sql through un objeto
                sql.Append("UPDATE user SET");//sentencia para editar algun registro

                sql.Append("Name = @Name, Password = @Password, Mail = @Mail, Role = @Role, Active = @Active, @Photo ");//sentencias para editar todos los registros en la DB
                sql.Append("WHERE UserCode = @UserCode;");//solo permitiendo que se edite un codigo de usuario  en especifico

                using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))//pasando un objeto de la conexion hacia MySql
                {
                    conexion.Open();//abriendo conexion

                    using (MySqlCommand comando = new MySqlCommand(sql.ToString(), conexion))
                    {
                        comando.CommandType = System.Data.CommandType.Text;/*especificando el tipo de comando que se ejecutara
                                                                            en este caso es un comando de texto, pues ee el codigo varchar el que se ingresa*/
                        //estas sentencias se tienen que hacer en el orden que sale en la tabla del motor de la DB
                        comando.Parameters.Add("@UserCode", MySqlDbType.VarChar, 50).Value = user.userCode;//pasando parametros al objeto de comando. 1ro: nombre parametro. 2do:tipo dato
                        comando.Parameters.Add("@Name", MySqlDbType.VarChar, 50).Value = user.name;
                        comando.Parameters.Add("@Password", MySqlDbType.VarChar, 80).Value = user.password;
                        comando.Parameters.Add("@Mail", MySqlDbType.VarChar, 20).Value = user.mail;
                        comando.Parameters.Add("@Role", MySqlDbType.VarChar, 25).Value = user.role;
                        comando.Parameters.Add("@CreationDate", MySqlDbType.DateTime).Value = user.creationDate;
                        comando.Parameters.Add("@Active", MySqlDbType.Bit).Value = user.active;
                        comando.Parameters.Add("@Photo", MySqlDbType.LongBlob).Value = user.photo;
                        comando.ExecuteNonQuery();//se va a ejecutar, pero no se devolver algun registro
                        edited = true;

                    }//esta sentencia de comando ejecuta la sentencia de sql
                }//esta sentencia ayuda a que, cuando termina la conexion con la DB, cerrar la conexion automatically
            }
            catch (System.Exception)
            {
            }

            return edited;//se devuelve la variable "edited"
        }
        public bool delete(string userCode)
        {
            Boolean deleted = false;
            try
            {
                StringBuilder sql = new StringBuilder();//armando sentencia Sql through un objeto
                sql.Append("DELETE FROM user");//eliminar de la tabla "user" de la base de datos
                sql.Append("WHERE UserCode = @UserCode;");

                using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))//pasando un objeto de la conexion hacia MySql
                {
                    conexion.Open();//abriendo conexion

                    using (MySqlCommand comando = new MySqlCommand(sql.ToString(), conexion))
                    {
                        comando.CommandType = System.Data.CommandType.Text;/*especificando el tipo de comando que se ejecutara
                                                                            en este caso es un comando de texto*/
                        comando.Parameters.Add("@UserCode", MySqlDbType.VarChar, 50).Value = userCode;//pasando parametros al objeto de comando. 1ro: nombre parametro. 2do:tipo dato

                        comando.ExecuteNonQuery();//se va a ejecutar, pero no se devolver algun registro
                        deleted = true;

                    }//esta sentencia de comando ejecuta la sentencia de sql
                }//esta sentencia ayuda a que, cuando termina la conexion con la DB, cerrar la conexion automatically
            }
            catch (System.Exception)
            {
            }

            return deleted;//se devuelve el objeto "user" de  la clase "Usuario", para validar que todos los datos sean correctos a la hora de ingresar a la DB
        }
        public DataTable bringUsers()//la clase "DataTable" trae una tabla completa de registros. Recommended para aplicaciones small, es decir, de pocos registros
        {
            DataTable dt = new DataTable();//instanciando objeto de esta clase
            try
            {
                StringBuilder sql = new StringBuilder();//armando sentencia Sql through un objeto
                sql.Append("SELECT * FROM user");//sentencia para traer registros de la tabla de la base de datos

                using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))//pasando un objeto de la conexion hacia MySql
                {
                    conexion.Open();//abriendo conexion

                    using (MySqlCommand comando = new MySqlCommand(sql.ToString(), conexion))
                    {
                        comando.CommandType = System.Data.CommandType.Text;//*especificando el tipo de comando que se ejecutara

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

