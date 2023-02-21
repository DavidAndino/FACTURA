using Entidades;
using System.Drawing;
using System.Windows.Forms;

namespace Vista
{
    public partial class UsersForm : Syncfusion.Windows.Forms.Office2010Form
    {
        public UsersForm()
        {
            InitializeComponent();
        }

        //variable global
        string operation = "";

        private void newButton_Click(object sender, System.EventArgs e)
        {
            enableControls();//invocando procedimiento que habilita controles si el usuario da click en "New"
            codeTextBox.Focus();//enviando foco a la primera casilla
            operation = "New";
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

                Usuario user = new Usuario();//crando objeto de la clase "Usuario" que se pasara al metodo que se insertara en la BD
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
            }//decision de boton guardar o save

            else if (operation == "Mod")
            {

            }

        }

        private void modButton_Click(object sender, System.EventArgs e)
        {
            operation = "Mod";
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
    }
}
