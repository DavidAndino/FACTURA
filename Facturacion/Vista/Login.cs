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
        }
    }
}
