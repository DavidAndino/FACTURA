using Entidades;
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
            int idBill = 0;//esta variable contendra el valor autogenerado en MySql que se almacenara en la Tabla "BillDetail"
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
            }
            catch (System.Exception)
            {
            }
            return inserted;
        }
    }
}
