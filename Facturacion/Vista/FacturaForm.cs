using Datos;
using Entidades;
using System.Windows.Forms;

namespace Vista
{
    public partial class FacturaForm : Syncfusion.Windows.Forms.Office2010Form
    {
        public FacturaForm()
        {
            InitializeComponent();
        }
        //objetos globales
        Client myCustomer = null;
        ClientDB clientFromDb = new ClientDB();
        private void clientIdTextBox_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            //aqui se presionara "Enter" y se buscara al cliente por ID
            if (e.KeyChar == (char)Keys.Enter)//si la pulsasion del teclado es en Enter
            {
                myCustomer = new Client();//instanciando objeto
                myCustomer = clientFromDb.bringClientsForID(clientIdTextBox.Text);//pasandole al objet "myCustomer" la id que ingrese el usuario
                clientsNameTextBox.Text = myCustomer.name;//asignando nombre del cliente en el textbox que se hizo para esta accion
            }
            else
            {
                myCustomer = null;//limpiando object whether or not el usuario comete un error
                clientsNameTextBox.Clear();//limpiando el textbox tambien
            }
        }

        private void findClientButton_Click(object sender, System.EventArgs e)
        {
            //abriendo otro formulario donde se buscara por nombre de cliente
            FindClientForm link = new FindClientForm();
            link.ShowDialog();
        }
    }
}
