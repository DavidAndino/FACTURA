namespace Entidades
{
    public class BillDetail
    {

        public int idBill { get; set; }//FK
        public string productCode { get; set; }//FK
        public string productDescription { get; set; }//solo se  mostrara en el datagridview, mas no en la base de datos
        public decimal price { get; set; }
        public int amount { get; set; }
        public decimal total { get; set; }
        public BillDetail()
        {

        }

        public BillDetail(int idBill, string productCode, decimal price, int amount, decimal total)
        {

            this.idBill = idBill;
            this.productCode = productCode;
            this.price = price;
            this.amount = amount;
            this.total = total;
        }
    }
}
