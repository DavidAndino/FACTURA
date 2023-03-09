namespace Entidades
{
    public class Product
    {
        //creando propiedades
        public string code { get; set; }
        public string description { get; set; }
        public int stock { get; set; }
        public decimal price { get; set; }
        public byte[] image { get; set; }
        public bool activeProduct { get; set; }

        //creando metodos constructores
        public Product()
        {

        }
        public Product(string code, string description, int stock, decimal price, byte[] image, bool activeProduct)
        {
            this.code = code;
            this.description = description;
            this.stock = stock;
            this.price = price;
            this.image = image;
            this.activeProduct = activeProduct;
        }

    }
}
