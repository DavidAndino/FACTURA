using Datos;
using Entidades;
using System;
using System.Data;
using System.Windows.Forms;

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
        Client customer = new Client();
        string operation = "";

        private void bringClientsForm()
        {
            dtClients = clientFromDB.bringClients();
            clientsDataGridView.DataSource = dtClients;
        }
        private void refreshButton_Click(object sender, System.EventArgs e)
        {
            bringClientsForm();
        }

        private void newButton_Click(object sender, System.EventArgs e)
        {
            idTextBox.Focus();
            operation = "New";
            enableControls();
        }
        private void enableControls()
        {
            if (operation == "New")
            {
                modButton.Enabled = false;
            }
            else if (operation == "Edit")
            {
                newButton.Enabled = false;
            }
            idTextBox.Enabled = true;
            nameTextBox.Enabled = true;
            phoneTextBox.Enabled = true;
            addresTextBox.Enabled = true;
            mailTextBox.Enabled = true;
            clientsDateTimePicker1.Enabled = true;
            activeCheckBox.Enabled = true;
            saveButton.Enabled = true;
            cancelButton.Enabled = true;
            deleteButton.Enabled = false;

        }
        private void disableControls()
        {
            if (modButton.Enabled == false)
            {
                modButton.Enabled = true;
            }
            if (newButton.Enabled == false)
            {
                newButton.Enabled = true;
            }
            idTextBox.Enabled = false;
            nameTextBox.Enabled = false;
            phoneTextBox.Enabled = false;
            addresTextBox.Enabled = false;
            mailTextBox.Enabled = false;
            clientsDateTimePicker1.Enabled = false;
            activeCheckBox.Enabled = false;
            saveButton.Enabled = false;
            cancelButton.Enabled = false;
            deleteButton.Enabled = true;

        }
        private void clearControls()
        {
            idTextBox.Clear();
            nameTextBox.Text = string.Empty;
            phoneTextBox.Clear();
            mailTextBox.Text = "";
            addresTextBox.Text = "";
            activeCheckBox.Checked = false;
            clientsDateTimePicker1.Value = DateTime.Now;
        }
        private void saveButton_Click(object sender, System.EventArgs e)
        {
            //modificando estados de atributos de la clase Client
            customer.iD = idTextBox.Text;
            customer.name = nameTextBox.Text;
            customer.phone = phoneTextBox.Text;
            customer.personalMail = mailTextBox.Text;
            customer.addres = addresTextBox.Text;
            customer.birthDate = Convert.ToDateTime(clientsDateTimePicker1.Text);
            customer.active = activeCheckBox.Checked;

            if (operation == "New")
            {
                if (string.IsNullOrEmpty(idTextBox.Text))
                {
                    errorProvider1.SetError(idTextBox, "Type the customer's ID");
                    idTextBox.Focus();
                    return;
                }
                errorProvider1.Clear();
                if (string.IsNullOrEmpty(nameTextBox.Text))
                {
                    errorProvider1.SetError(nameTextBox, "Type the customer's name");
                    nameTextBox.Focus();
                    return;
                }
                errorProvider1.Clear();

                //insertando datos en la tabla de la DB
                bool inserts = clientFromDB.insert(customer);

                if (inserts)
                {
                    clearControls();
                    disableControls();
                    bringClientsForm();
                    MessageBox.Show("The registry has been saved", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("It has not been possible to save the registry", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }//decision de boton  "New"
            else if (operation == "Edit")
            {
                bool edits = clientFromDB.edit(customer);
                if (edits)
                {
                    disableControls();
                    clearControls();
                    bringClientsForm();
                    MessageBox.Show("The registry has been updated successfully", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("It has not been possible to update the registry", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }//decision de boton "Modify"
        }

        private void modButton_Click(object sender, System.EventArgs e)
        {

            operation = "Edit";
            if (clientsDataGridView.SelectedRows.Count > 0)
            {
                idTextBox.Text = clientsDataGridView.CurrentRow.Cells["Id"].Value.ToString();
                nameTextBox.Text = clientsDataGridView.CurrentRow.Cells["Name"].Value.ToString();
                phoneTextBox.Text = clientsDataGridView.CurrentRow.Cells["Phone"].Value.ToString();
                mailTextBox.Text = clientsDataGridView.CurrentRow.Cells["PersonalMail"].Value.ToString();
                addresTextBox.Text = clientsDataGridView.CurrentRow.Cells["Addres"].Value.ToString();
                //validando que la fecha no traiga un valor nulo. Esta validacion permite al IDE aceptar valores nulos de fecha de nacimiento de la base de datos, y no mostrara una excepcion
                if (clientsDataGridView.CurrentRow.Cells["BirthDate"].Value != DBNull.Value)
                {
                    clientsDateTimePicker1.Value = Convert.ToDateTime(clientsDataGridView.CurrentRow.Cells["BirthDate"].Value);
                }
                activeCheckBox.Checked = Convert.ToBoolean(clientsDataGridView.CurrentRow.Cells["Active"].Value);
                enableControls();
            }
            else
            {
                MessageBox.Show("You must select a registry to edit", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                deleteButton.Enabled = true;
            }
        }

        private void cancelButton_Click(object sender, System.EventArgs e)
        {
            DialogResult decision = MessageBox.Show("Do you want to cancel the process", "Cancel Process", MessageBoxButtons.YesNo);
            if (decision == DialogResult.Yes)
            {
                MessageBox.Show("The process has been cancelled", "Cancel operation", MessageBoxButtons.OK, MessageBoxIcon.Error);
                disableControls();
                clearControls();
                if (modButton.Enabled == false)//validando que el boton de modify quede activo si se apreta "New", pero luego se cancela
                {
                    modButton.Enabled = true;
                }
                errorProvider1.Clear();//eliminando advertencia que podrtia quedar en determinados casos
            }


        }//programar pregunta

        private void ClientsForm_Load(object sender, System.EventArgs e)
        {
            clientsDataGridView.DataSource = clientFromDB.bringClients();
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (clientsDataGridView.SelectedRows.Count > 0)
            {
                DialogResult decision = MessageBox.Show("Are you sure you want to delete the registry?", "Delete Registry", MessageBoxButtons.YesNo);

                if (decision == DialogResult.Yes)
                {
                    bool deleted = clientFromDB.delete(clientsDataGridView.CurrentRow.Cells["Id"].Value.ToString());// el Id del cliente esta en el DataGrid

                    if (deleted)
                    {
                        clearControls();
                        disableControls();
                        bringClientsForm();//al traer los usuarios actualizados desde la DB, se actualiza el DATAGrid
                        MessageBox.Show("The registry was successfully deleted", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("The registry could not be deleted", "Information", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("You must select a registry to delete", "Information", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
