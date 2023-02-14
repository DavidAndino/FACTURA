namespace Entidades //quitar y ordenar usos
{
    public class Login
    {
        //propiedades
        public string codigoUser { get; set; }
        public string password { get; set; }
        public string rol { get; set; }

        public Login()//generando constructor
        {
        }

        public Login(string codigoUser, string password, string rol)//generando constructor con parametros
        {
            this.codigoUser = codigoUser;
            this.password = password;
            this.rol = rol;
        }
    }


}
