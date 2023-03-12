namespace Entidades
{
    public class BillDetail
    {
        public int iD { get; set; }
        public int idBill { get; set; }//FK
        public string productCode { get; set; }//FK
        public decimal price { get; set; }
        public int amount { get; set; }
        public decimal total { get; set; }
        public BillDetail()
        {

        }

        public BillDetail(int iD, int idBill, string productCode, decimal price, int amount, decimal total)
        {
            this.iD = iD;
            this.idBill = idBill;
            this.productCode = productCode;
            this.price = price;
            this.amount = amount;
            this.total = total;
        }
    }
}
