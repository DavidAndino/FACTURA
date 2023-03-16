using Datos;
using Entidades;
using System;
using System.Windows.Forms;

namespace Vista
{
    public partial class FindClientForm : Syncfusion.Windows.Forms.Office2010Form
    {
        public FindClientForm()
        {
            InitializeComponent();
        }
        //objetos globales
        ClientDB customer = new ClientDB();
        public Client client = new Client();//object que se devolvera cuando se da click en aceptar. Se llevara de este form a FacturaForm

        private void FindClientForm_Load(object sender, EventArgs e)
        {
            clientsDataGridView.DataSource = customer.bringClients();//actualizando lista de registros
        }

        private void acceptButton_Click(object sender, EventArgs e)
        {
            //aqui se enviara el objeto "client" dependiendo de lo que haya seleccionado el usuario (cajero)
            if (clientsDataGridView.SelectedRows.Count > 0)
            {
                client.iD = clientsDataGridView.CurrentRow.Cells["Id"].Value.ToString();
                client.name = clientsDataGridView.CurrentRow.Cells["Name"].Value.ToString();
                client.phone = clientsDataGridView.CurrentRow.Cells["Phone"].Value.ToString();
                client.personalMail = clientsDataGridView.CurrentRow.Cells["PersonalMail"].Value.ToString();
                client.addres = clientsDataGridView.CurrentRow.Cells["Addres"].Value.ToString();
                client.birthDate = Convert.ToDateTime(clientsDataGridView.CurrentRow.Cells["BirthDate"].Value);
                client.active = Convert.ToBoolean(clientsDataGridView.CurrentRow.Cells["Active"].Value);
                this.Close();//cerrando formulario
            }
            else
            {
                MessageBox.Show("None registry has been selected", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void insertedNameTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            //cuando la tecla se deja de pulsar aparecen las coincidencias:
            clientsDataGridView.DataSource = customer.bringClientsForName(insertedNameTextBox.Text);

        }
    }
}
