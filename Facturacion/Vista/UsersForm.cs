using Datos;
using Entidades;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Vista
{
    public partial class UsersForm : Syncfusion.Windows.Forms.Office2010Form
    {
        public UsersForm()
        {
            InitializeComponent();
        }

        //variables globales
        string operation = "";
        DataTable dt = new DataTable();//objeto que recibira el metodo "bringUsers"
        UserDB userFromDB = new UserDB();//este objeto servira para ejecutar sentencias desde varias partes de codigo
        Usuario user = new Usuario();//creando objeto de la clase "Usuario" que se pasara al metodo que se insertara en la BD

        private void newButton_Click(object sender, System.EventArgs e)
        {
            enableControls();//invocando procedimiento que habilita controles si el usuario da click en "New"
            codeTextBox.Focus();//enviando foco a la primera casilla
            operation = "New";
            modButton.Enabled = false;//desabilitando boton de modificar si se apreta "Nuevo"
        }

        private void enableControls()
        {
            codeTextBox.Enabled = true;
            nameTextBox.Enabled = true;
            passTextBox.Enabled = true;
            mailTextBox.Enabled = true;
            roleComboBox.Enabled = true;
            activeCheckBox.Enabled = true;
            searchPicButton.Enabled = true;
            saveButton.Enabled = true;
            cancelButton.Enabled = true;
        }

        private void cancelButton_Click(object sender, System.EventArgs e)
        {
            disableControls();//invocando metodo que bloquea los controles si el  usuario da click en "cancel"
            cleanControls();//invocando metodo que limpia controles si el usuario cancela
            roleComboBox.Enabled = true;

        }

        private void disableControls()
        {
            codeTextBox.Enabled = false;
            nameTextBox.Enabled = false;
            passTextBox.Enabled = false;
            mailTextBox.Enabled = false;
            roleComboBox.Enabled = false;
            activeCheckBox.Enabled = false;
            searchPicButton.Enabled = false;
            saveButton.Enabled = false;
            cancelButton.Enabled = false;
        }

        private void cleanControls()
        {
            codeTextBox.Clear();
            nameTextBox.Clear();
            passTextBox.Clear();
            mailTextBox.Clear();
            roleComboBox.DataSource = null;
            roleComboBox.Items.Clear();
            activeCheckBox.Checked = false;
            pictureBox1.Image = null; //limpiando imagen
        }

        private void saveButton_Click(object sender, System.EventArgs e)
        {
            if (operation == "New")//decidiendo que hara el boton "save" 
            {
                //validando que no haya campos vacios
                if (string.IsNullOrEmpty(codeTextBox.Text))
                {
                    //validando que no haya campos vacios
                    errorProvider1.SetError(codeTextBox, "Insert a code number");
                    codeTextBox.Focus();
                    return;
                }
                errorProvider1.Clear();//limpiando el "erroProvider" por cada decision

                if (string.IsNullOrEmpty(nameTextBox.Text))
                {
                    errorProvider1.SetError(nameTextBox, "Insert a name");
                    nameTextBox.Focus();
                    return;//hace que el programa no pase de ahi
                }
                errorProvider1.Clear();

                if (string.IsNullOrEmpty(passTextBox.Text))
                {
                    errorProvider1.SetError(passTextBox, "Insert a key");
                    passTextBox.Focus();
                    return;
                }
                errorProvider1.Clear();

                if (string.IsNullOrEmpty(roleComboBox.Text))
                {
                    errorProvider1.SetError(roleComboBox, "Choose a role");
                    roleComboBox.Focus();
                    return;
                }
                errorProvider1.Clear();

                user.userCode = codeTextBox.Text;
                user.name = nameTextBox.Text;
                user.password = passTextBox.Text;
                user.role = roleComboBox.Text;
                user.mail = mailTextBox.Text;
                user.active = activeCheckBox.Checked;//la propiedad "checked" es de tipo booleano

                //psando imagen del pictureBox a la propiedad "Photo" de la clase "Usuario
                if (pictureBox1.Image != null)
                {
                    System.IO.MemoryStream alojadorImagen = new System.IO.MemoryStream();//convirtiendo imagen a arreglo de bytex, como se definio desde el principio
                    pictureBox1.Image.Save(alojadorImagen, System.Drawing.Imaging.ImageFormat.Jpeg);/*pasandole la imagen del pictureBox al objeto de la clase 
                                                                                                     * MemoryStream (alojadorImagen) through metodo "Save" */
                    user.photo = alojadorImagen.GetBuffer();//pasando lo alojado en "alojadorImagen" a la propiedad "Photo" de la clase "Usuario"
                }

                //insertar en la DB
                bool inserts = userFromDB.insert(user);//el objeto "inserts" traera un true o false, desde la clase UserDB

                //validando que el valor de  "inserts" sea true para insertar el registro
                if (inserts)
                {
                    cleanControls();
                    disableControls();
                    bringUsersForm();//actualizando el DataGridView
                    MessageBox.Show("The registry has been saved");
                }
                else
                {
                    MessageBox.Show("The operation failed while trying to save the registry");
                }

            }//decision de boton guardar o save

            else if (operation == "Mod")
            {
                user.userCode = codeTextBox.Text;
                user.name = nameTextBox.Text;
                user.password = passTextBox.Text;
                user.role = roleComboBox.Text;
                user.mail = mailTextBox.Text;
                user.active = activeCheckBox.Checked;//la propiedad "checked" es de tipo booleano

                //psando imagen del pictureBox a la propiedad "Photo" de la clase "Usuario
                if (pictureBox1.Image != null)
                {
                    System.IO.MemoryStream alojadorImagen = new System.IO.MemoryStream();//convirtiendo imagen a arreglo de bytex, como se definio desde el principio
                    pictureBox1.Image.Save(alojadorImagen, System.Drawing.Imaging.ImageFormat.Jpeg);/*pasandole la imagen del pictureBox al objeto de la clase 
                                                                                                     * MemoryStream (alojadorImagen) through metodo "Save" */
                    user.photo = alojadorImagen.GetBuffer();//pasando lo alojado en "alojadorImagen" a la propiedad "Photo" de la clase "Usuario"
                }

                bool mod = userFromDB.edit(user);

                if (mod)
                {
                    cleanControls();
                    disableControls();
                    bringUsersForm();
                    MessageBox.Show("The registry was updated successfully");
                }
                else
                {
                    MessageBox.Show("It was not possible to update the registry");
                }
            }

        }

        private void modButton_Click(object sender, System.EventArgs e)
        {
            operation = "Mod";
            //pasando el registro seleccionado a cada textBox y combo y check
            if (usersDataGridView.SelectedRows.Count > 0)//si el user selecciono al menos una fila
            {
                //el orden de esto de llenar los textbox etc  de nuevo con los datos que hay en la celda seleccionada en el dataGrid, se hace con el de la DB en MySql
                codeTextBox.Text = usersDataGridView.CurrentRow.Cells["UserCode"].Value.ToString();//pasando los datos que contiene la celda de la fila seleccionada
                nameTextBox.Text = usersDataGridView.CurrentRow.Cells["Name"].Value.ToString();
                passTextBox.Text = usersDataGridView.CurrentRow.Cells["Password"].Value.ToString();
                mailTextBox.Text = usersDataGridView.CurrentRow.Cells["Mail"].Value.ToString();
                roleComboBox.Text = usersDataGridView.CurrentRow.Cells["Role"].Value.ToString();
                activeCheckBox.Checked = Convert.ToBoolean(usersDataGridView.CurrentRow.Cells["Active"].Value);
                byte[] myPhoto = userFromDB.photo(usersDataGridView.CurrentRow.Cells["UserCode"].Value.ToString());

                //validando que el vector "myPhoto" traiga una imagen
                if (myPhoto.Length > 0)//si el user no tiene una foto en la DB, este arreglo devuelve un 0
                {
                    MemoryStream mS = new MemoryStream(myPhoto);
                    pictureBox1.Image = System.Drawing.Bitmap.FromStream(mS);
                }
                enableControls();
            }
            else
            {
                MessageBox.Show("You must select a registry");
            }
        }

        private void searchPicButton_Click(object sender, System.EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();//abriendo una ventana para adjuntar
            DialogResult result = dialog.ShowDialog();//capturando el archivo de imagen que elige el usuario

            //validando si el usuario eligio una foto o no
            if (result == DialogResult.OK)
            {
                pictureBox1.Image = Image.FromFile(dialog.FileName);/*asignando imagen al picturebox Metodo "FromFile" convierte la imagen capturada y 
                                                                     se la  pasa al picturebox*/
            }
        }

        private void UsersForm_Load(object sender, System.EventArgs e)
        {
            bringUsersForm();
        }

        private void bringUsersForm()
        {
            dt = userFromDB.bringUsers();//trayendo todos los usuarios
            usersDataGridView.DataSource = dt;//llenando el DataGridView con cada registro que se ingrese, edite o elimine
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            //validando que le user seleccione un registro, es decir, una fila o celda del dataGridView
            if (usersDataGridView.SelectedRows.Count > 0)
            {
                //validando que el usuario realmente quiera eliminar el registro
                DialogResult decision = MessageBox.Show("Are you sure you want to delete the registry?", "Delete Registry", MessageBoxButtons.YesNo);
                //validando si el user presiona "Ok" o "Cancel"
                if (decision == DialogResult.Yes)
                {
                    bool deleted = userFromDB.delete(usersDataGridView.CurrentRow.Cells["UserCode"].Value.ToString());// el codigo de usuario esta en el DataGrid
                    if (deleted)
                    {
                        cleanControls();
                        disableControls();
                        bringUsersForm();//al traer los usuarios actualizados desde la DB, se actualiza el DATAGrid
                        MessageBox.Show("The registry was successfully deleted");
                    }
                }
                else
                {
                    MessageBox.Show("The registry could not be deleted");
                }


            }
            else
            {
                MessageBox.Show("You must select a registry in the table");
            }
        }

        private void refreshButton_Click(object sender, EventArgs e)
        {
            bringUsersForm();//refrescando los posibles datos modificados desde el motor de DB 
        }

    }
}
