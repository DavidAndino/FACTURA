using Datos;
using System.Data;

namespace Vista
{
    public partial class ClientsForm : Syncfusion.Windows.Forms.Office2010Form
    {
        public ClientsForm()
        {
            InitializeComponent();
        }
        //variables y objetos globales
        DataTable dtClients = new DataTable();
        ClientDB clientFromDB = new ClientDB();

        private void bringClientsForm()
        {
            dtClients = clientFromDB.bringClients();
            clientsDataGridView.DataSource = dtClients;
        }
        private void refreshButton_Click(object sender, System.EventArgs e)
        {
            bringClientsForm();
        }
    }
}
