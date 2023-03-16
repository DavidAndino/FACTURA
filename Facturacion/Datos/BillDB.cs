using Entidades;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace Datos
{
    public class BillDB
    {
        string cadenaConexion = "server = localhost; user = root; database = factura; password = 223344";

        public bool savedBill(Bill charge, List<BillDetail> details)
        {
            bool inserted = false;
            int idBill = 0;//esta variable contendra el valor autogenerado del id de factura en MySql que se almacenara en la Tabla "BillDetail"
            try
            {
                StringBuilder sqlBill = new StringBuilder();//sentencia q permite crear varias sentencias separadas, es decir, danto enter

                sqlBill.Append(" INSERT INTO bill VALUES (@Id, @Date, @IdClient, @UserCode, @SalesTax, @Discount, @Subtotal, @Total); ");/*dentro de los ( ) van los atributos 
                                                              de la entidad en la tabla de la DB
                                                            */
                sqlBill.Append(" SELECT LAST_INSERT_ID(); ");/*sentencia de Mysql que permite capturar el Id del producto que se acaba de geenerar. Devuelve la ultima PK (ID) de
                                                               de la tabla o registro que se acaba de  insertar en el programa
                                                              */
                StringBuilder sqlBillDetail = new StringBuilder();
                sqlBillDetail.Append(" INSERT INTO  billdetail VALUES (@Id, @IdBill, @ProductCode, @Price, @Amount, @Total); ");/*sentencia para insertar el detalle, o sea,
                                                                                                                                 los datos de los productos registrados en la factura en cuestion
                                                                                                                                */
                //sentencia para actualizar stock
                StringBuilder sqlStock = new StringBuilder();
                sqlStock.Append(" UPDATE product SET Stock = Stock - @Amount WHERE Code = @Code ");

                using (MySqlConnection connection = new MySqlConnection(cadenaConexion))
                {
                    connection.Open();
                    /*se utilizan transacciones para que no haya registros huerfanos. Si falla la operaion, la transaccion hace un retroceso: 
                     deja el registro como estaba antes de que fallara por cualquier motivo*/
                    //creando transaccion
                    MySqlTransaction transaction = connection.BeginTransaction(System.Data.IsolationLevel.ReadCommitted);/*asignandole la conexion en cuestion a la transaccion. La transaccion bloquea la tabla en cuestion, 
                                                                                   y no pueden trabajar  2 users a la vez en dicha tabla
                                                                                  el "ReadCommitted" la tabla, pero la deja disponible para  otros posibles usuarios*/
                    try
                    {
                        using (MySqlCommand command1 = new MySqlCommand(sqlBill.ToString(), connection, transaction))
                        {
                            command1.CommandType = System.Data.CommandType.Text;
                            command1.Parameters.Add("@Date", MySqlDbType.DateTime).Value = charge.date;
                            command1.Parameters.Add("@IdClient", MySqlDbType.VarChar, 13).Value = charge.idClient;
                            command1.Parameters.Add("@UserCode", MySqlDbType.VarChar, 50).Value = charge.userCode;
                            command1.Parameters.Add("@SalesTax", MySqlDbType.Decimal).Value = charge.salesTax;
                            command1.Parameters.Add("@Discount", MySqlDbType.Decimal).Value = charge.discount;
                            command1.Parameters.Add("@Subtotal", MySqlDbType.Decimal).Value = charge.subtotal;
                            command1.Parameters.Add("@Total", MySqlDbType.Decimal).Value = charge.total;
                            idBill = Convert.ToInt32(command1.ExecuteScalar());//el metodo "ExecuteScalar" trae el valor del id de la ultima factura,
                                                                               //y luego se le asigna ese valor a la variable idBill
                        }

                        //insertando los detalles de productos. cuando el ciclo foreach itere, se insertara el detalle y actualizara el stock del producto en cuestion
                        foreach (BillDetail detail in details)
                        {
                            using (MySqlCommand command2 = new MySqlCommand(sqlBillDetail.ToString(), connection, transaction))
                            {
                                command2.CommandType = System.Data.CommandType.Text;
                                command2.Parameters.Add("@IdBill", MySqlDbType.Int32).Value = idBill;
                                command2.Parameters.Add("@ProductCode", MySqlDbType.VarChar, 80).Value = detail.productCode;//producto code (y el resto) se guarda en el item "detail", en cada iteracion
                                command2.Parameters.Add("@Price", MySqlDbType.Decimal).Value = detail.price;
                                command2.Parameters.Add("@Amount", MySqlDbType.Int32).Value = detail.amount;
                                command2.Parameters.Add("@Total", MySqlDbType.Decimal).Value = detail.total;
                                command2.ExecuteNonQuery();
                            }
                            //actualizando existencia del producto
                            using (MySqlCommand command3 = new MySqlCommand(sqlStock.ToString(), connection, transaction))
                            {
                                command3.CommandType = System.Data.CommandType.Text;
                                command3.Parameters.Add("@Amount", MySqlDbType.Int32).Value = detail.amount;
                                command3.Parameters.Add("@ProductCode", MySqlDbType.VarChar, 80).Value = detail.productCode;//producto code se guarda en el item "detail", en cada iteracion
                                command3.ExecuteNonQuery();
                            }
                        }
                        transaction.Commit();//sentencia para que la transaccion sea ejectuda
                        inserted = true;//si todo se da bien con la transaccion, esta variable se hace verdadera


                    }
                    catch (System.Exception)
                    {
                        inserted = false;//si algo sale mal, esta variable toma el valor falso y no continua el proceso de insercion y actualizacion de detalles
                        transaction.Rollback();//si falla el proceso, en este punto la transaccion hace lo que se especifico anteriormente.
                    }
                }
            }
            catch (System.Exception)
            {
            }
            return inserted;
        }
    }
}
