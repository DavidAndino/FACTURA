using Entidades;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Text;

namespace Datos
{
    public class ProductDB
    {
        string cadenaConexion = "server = localhost; user = root; database = factura; password = 223344";/*creando ruta hacia donde se ira para poderse conectar donde esta el server
                                                                                                          esta cadena trae todos los datos del servidor*/
        //creando metodos para insertar, editar, modificar, eliminar registros en la DB
        public bool insert(Product item)
        {
            bool inserted = false;
            try
            {
                StringBuilder sql = new StringBuilder();//armando sentencia Sql through un objeto, para que deje escribir así: 
                sql.Append(" INSERT INTO product VALUES ");/*seleccionando un dato de la tabla en la DB. 
                                                  * "user" nombre de la tabla en el motor de DB. */
                sql.Append(" (@Code, @Description, @Stock, @Price, @Image, @ActiveProduct); ");//sentencias para insertar un new registro en la DB. Mismo orden que la DB

                using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))//pasando un objeto de la conexion hacia MySql
                {
                    conexion.Open();//abriendo conexion

                    using (MySqlCommand comando = new MySqlCommand(sql.ToString(), conexion))
                    {
                        comando.CommandType = CommandType.Text;/*especificando el tipo de comando que se ejecutara
                                                                            en este caso es un comando de texto, pues ee el codigo varchar el que se ingresa*/
                        //estas sentencias se tienen que hacer en el orden que sale en la tabla del motor de la DB
                        comando.Parameters.Add("@Code", MySqlDbType.VarChar, 80).Value = item.code;//pasando parametros al objeto de comando. 1ro: nombre parametro. 2do:tipo dato
                        comando.Parameters.Add("@Description", MySqlDbType.VarChar, 20).Value = item.description;
                        comando.Parameters.Add("@Stock", MySqlDbType.Int32).Value = item.stock;
                        comando.Parameters.Add("Price", MySqlDbType.Decimal).Value = item.price;
                        comando.Parameters.Add("@Image", MySqlDbType.LongBlob).Value = item.image;
                        comando.Parameters.Add("@ActiveProduct", MySqlDbType.Bit).Value = item.activeProduct;
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
        public bool edit(Product productFromDB)
        {
            bool edited = false;
            try
            {
                StringBuilder sql = new StringBuilder();//armando sentencia Sql through un objeto
                sql.Append(" UPDATE product SET ");//sentencia para editar algun registro

                sql.Append(" Description = @Description, Stock = @Stock, Price = @Price, Image = @Image, ActiveProduct = @ActiveProduct ");//sentencias para editar todos los registros en la DB
                sql.Append(" WHERE Code = @Code; ");//solo permitiendo que se edite un codigo de producto en especifico

                using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))//pasando un objeto de la conexion hacia MySql
                {
                    conexion.Open();//abriendo conexion

                    using (MySqlCommand comando = new MySqlCommand(sql.ToString(), conexion))
                    {
                        comando.CommandType = System.Data.CommandType.Text;/*especificando el tipo de comando que se ejecutara
                                                                            en este caso es un comando de texto, pues ee el codigo varchar el que se ingresa*/
                        //estas sentencias se tienen que hacer en el orden que sale en la tabla del motor de la DB
                        comando.Parameters.Add("@Code", MySqlDbType.VarChar, 80).Value = productFromDB.code;//pasando parametros al objeto de comando. 1ro: nombre parametro. 2do:tipo dato
                        comando.Parameters.Add("@Description", MySqlDbType.VarChar, 20).Value = productFromDB.description;
                        comando.Parameters.Add("@Stock", MySqlDbType.Int32).Value = productFromDB.stock;
                        comando.Parameters.Add("@Price", MySqlDbType.Decimal).Value = productFromDB.price;
                        comando.Parameters.Add("@Image", MySqlDbType.LongBlob).Value = productFromDB.image;
                        comando.Parameters.Add("@ActiveProduct", MySqlDbType.Bit).Value = productFromDB.activeProduct;
                        comando.ExecuteNonQuery();//se va a ejecutar, pero no se devolvera algun registro
                        edited = true;

                    }//esta sentencia de comando ejecuta la sentencia de sql
                }//esta sentencia ayuda a que, cuando termina la conexion con la DB, cerrar la conexion automatically
            }
            catch (System.Exception)
            {
            }

            return edited;//se devuelve la variable "edited"
        }
        public bool delete(string code)
        {
            bool deleted = false;
            try
            {
                StringBuilder sql = new StringBuilder();//armando sentencia Sql through un objeto
                sql.Append(" DELETE FROM product ");//eliminar de la tabla "product" de la base de datos
                sql.Append(" WHERE Code = @Code; ");//aqui si se especifica que elemento de dicha  tabla se  elimna

                using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))//pasando un objeto de la conexion hacia MySql
                {
                    conexion.Open();//abriendo conexion

                    using (MySqlCommand comando = new MySqlCommand(sql.ToString(), conexion))
                    {
                        comando.CommandType = CommandType.Text;/*especificando el tipo de comando que se ejecutara
                                                                            en este caso es un comando de texto*/
                        comando.Parameters.Add("@Code", MySqlDbType.VarChar, 80).Value = code;//pasando parametros al objeto de comando. 1ro: nombre parametro. 2do:tipo dato

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
        //creando metodo que permita traer la imagen del user desde la base de datos. Acoplar todo> ctrl+m+o
        public byte[] photo(string productCode)//se traera mediante el codigo de usuario
        {
            byte[] photo = new byte[0];
            try
            {
                StringBuilder sql = new StringBuilder();//armando sentencia Sql through un objeto
                sql.Append("SELECT Image FROM product WHERE Code = @Code");//sentencia para traer solo la foto de la tabla de la base de datos, tiene que estar escrito como alla

                using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))//pasando un objeto de la conexion hacia MySql
                {
                    conexion.Open();//abriendo conexion

                    using (MySqlCommand comando = new MySqlCommand(sql.ToString(), conexion))
                    {
                        comando.CommandType = System.Data.CommandType.Text;//*especificando el tipo de comando que se ejecutara
                        comando.Parameters.Add("@Code", MySqlDbType.VarChar, 80).Value = productCode;
                        MySqlDataReader dr = comando.ExecuteReader();//trayendo los datos
                        if (dr.Read())//si lee algun archivo existente en el objeto de tipo MySqlDataReader 
                        {
                            photo = (byte[])dr["Photo"];//entonces se le pasa lo que trae el objeto al arreglo de bytes que se devolvera
                        }
                    }//esta sentencia de comando ejecuta la sentencia de sql
                }//esta sentencia ayuda a que, cuando termina la conexion con la DB, cerrar la conexion automatically
            }
            catch (Exception)
            {
            }

            return photo;//se devuelve el objeto "user" de  la clase "Usuario", para validar que todos los datos sean correctos a la hora de ingresar a la DB
        }
        public DataTable bringProducts()
        {
            DataTable dt = new DataTable();//instanciando objeto de esta clase
            try
            {
                StringBuilder sql = new StringBuilder();//armando sentencia Sql through un objeto
                sql.Append("SELECT * FROM product");//sentencia para traer registros de la tabla de la base de datos

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
        public Product bringProductsForCode(string codeNumber)
        {
            Product product = null;
            try
            {
                StringBuilder sql = new StringBuilder();//armando sentencia Sql through un objeto
                sql.Append("SELECT * FROM product WHERE Code = @Code");//sentencia para traer registros de la tabla de la base de datos

                using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))//pasando un objeto de la conexion hacia MySql
                {
                    conexion.Open();//abriendo conexion

                    using (MySqlCommand comando = new MySqlCommand(sql.ToString(), conexion))
                    {
                        comando.CommandType = CommandType.Text;//*especificando el tipo de comando que se ejecutara
                        comando.Parameters.Add("@Code", MySqlDbType.VarChar, 80).Value = codeNumber;
                        MySqlDataReader dr = comando.ExecuteReader();//trayendo los datos
                        //pasando a cada propiedad los datos que se guardan en el objeto "dr"
                        if (dr.Read())
                        {
                            product = new Product();//instanciando objeto
                            product.code = codeNumber;
                            product.description = dr["Description"].ToString();
                            product.stock = Convert.ToInt32(dr["Stock"]);
                            product.price = Convert.ToDecimal(dr["Price"]);
                            product.activeProduct = Convert.ToBoolean((dr["Active"]));
                        }

                    }//esta sentencia de comando ejecuta la sentencia de sql
                }//esta sentencia ayuda a que, cuando termina la conexion con la DB, cerrar la conexion automatically
            }
            catch (Exception)
            {
            }

            return product;//se devuelve el objeto "user" de  la clase "Usuario", para validar que todos los datos sean correctos a la hora de ingresar a la DB
        }
        public DataTable bringProductsForDescription(string description)
        {
            DataTable dt = new DataTable();//instanciando objeto de esta clase
            try
            {
                StringBuilder sql = new StringBuilder();//armando sentencia Sql through un objeto
                sql.Append("SELECT * FROM product WHERE Description LIKE '%" + description + "%'");/*LIKE filtra todos los resultados que sean iguales o contegan 
                                                                           el valor de la description que se trae en el parametro "descripcion" 
                el simbolo de resto duplicado permite buscar varios elementos en el nombre de una Entidad (nombres, apellidos etc) a la vez*/

                using (MySqlConnection conexion = new MySqlConnection(cadenaConexion))//pasando un objeto de la conexion hacia MySql
                {
                    conexion.Open();//abriendo conexion

                    using (MySqlCommand comando = new MySqlCommand(sql.ToString(), conexion))
                    {
                        comando.CommandType = CommandType.Text;//*especificando el tipo de comando que se ejecutara
                        comando.Parameters.Add("@Description", MySqlDbType.VarChar, 20).Value = description;
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
