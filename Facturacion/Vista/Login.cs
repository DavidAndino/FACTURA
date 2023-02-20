using System;
using System.Windows.Forms;

namespace Vista
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void acceptButton_Click(object sender, EventArgs e)
        {
            if (userTextBox.Text == string.Empty)
            {
                errorProvider1.SetError(userTextBox, "The user`s name is missing");
                userTextBox.Focus();//devolviendo el foco a la caja de text en usuario
                return;
            }
            errorProvider1.Clear();

            if (string.IsNullOrEmpty(passTextBox.Text))//si el textBox esta nulo o vacio 
            {
                errorProvider1.SetError(passTextBox, "Enter a password");
                passTextBox.Clear();//eliminando la contrasenia incorrecta
                passTextBox.Focus();
                return;
            }
            errorProvider1.Clear();

            //validacion en base de datos


            //Mostrar menu
            Menu menuFormulario = new Menu();//instanciando objeto de la clase Menu
            this.Hide();//ocultando formulario de login
            menuFormulario.Show();//mostrando formulario

        }

        private void hideButton_Click(object sender, EventArgs e)
        {
            if (passTextBox.PasswordChar == '*')//mostrando contrasenia
            {
                passTextBox.PasswordChar = '\0';//enviandola la propiedad "PasswordChar" el valor nulo. Esta contrapleca iguala a null
            }
            else
            {
                passTextBox.PasswordChar = '*';//volviendo a ocultar contra
            }
        }
    }
}
