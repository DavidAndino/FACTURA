using System;

namespace Entidades
{
    public class Bill
    {
        public string iD { get; set; }
        public DateTime date { get; set; }
        public string idClient { get; set; }//FK
        public string userCode { get; set; }//FK
        public Decimal subtotal { get; set; }
        public Decimal salesTax { get; set; }//duda sintaxis "Decimal"
        public Decimal discount { get; set; }
        public Decimal total { get; set; }

        public Bill()
        {

        }

        public Bill(string iD, DateTime date, string idClient, string userCode, decimal salesTax, decimal discount, decimal subtotal, decimal total)
        {
            this.iD = iD;
            this.date = date;
            this.idClient = idClient;
            this.userCode = userCode;
            this.salesTax = salesTax;
            this.discount = discount;
            this.subtotal = subtotal;
            this.total = total;
        }
    }
}
