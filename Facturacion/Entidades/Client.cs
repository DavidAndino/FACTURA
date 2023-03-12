using System;

namespace Entidades
{
    public class Client
    {
        public string iD { get; set; }/*al referenciar esta propiedad, automaticamente se sabe que metodo se usara (get o set)
                                       se instanciara o modificara respectivamente. el atributo esta internamente*/
        public string name { get; set; }
        public string phone { get; set; }
        public string personalMail { get; set; }
        public string addres { get; set; }
        public DateTime birthDate { get; set; }
        public bool active { get; set; }

        public Client()
        {

        }

        public Client(string iD, string name, string phone, string personalMail, string addres, DateTime birthDate, bool active)
        {
            this.iD = iD;
            this.name = name;
            this.phone = phone;
            this.personalMail = personalMail;
            this.addres = addres;
            this.birthDate = birthDate;
            this.active = active;
        }
    }
}
