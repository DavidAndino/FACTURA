using System;

namespace Entidades
{
    public class Usuario
    {
        //propiedades
        public string userCode { get; set; }
        public String name { get; set; }
        public string password { get; set; }
        public string mail { get; set; }
        public string role { get; set; }
        public DateTime creationDate { get; set; }
        public bool active { get; set; }
        public byte[] photo { get; set; }

        //creando constructor vaio
        public Usuario()
        {

        }

        //creando constructor con parametros
        public Usuario(string userCode, string name, string password, string mail, string role, DateTime creationDate, bool active, byte[] photo)
        {
            this.userCode = userCode;
            this.name = name;
            this.password = password;
            this.mail = mail;
            this.role = role;
            this.creationDate = creationDate;
            this.active = active;
            this.photo = photo;
        }
    }
}
