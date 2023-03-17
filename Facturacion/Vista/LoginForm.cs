using Datos;
using Entidades;
using System;
using System.Windows.Forms;

namespace Vista
{
    public partial class LoginForm : Form
    {
        public LoginForm()
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
            Login login = new Login(userTextBox.Text, passTextBox.Text);
            Usuario user = new Usuario();
            UserDB userDB = new UserDB();

            user = userDB.Autenticacion(login);

            //validando que el usuario exista en la base de datos
            if (user != null)
            {
                if (user.active)
                {
                    //codigo necesario para que el codigo de usuario se muestre en el textBox "User" de la factura
                    System.Security.Principal.GenericIdentity iD = new System.Security.Principal.GenericIdentity(user.userCode);/*la clase "GenericId"
                                                                                                       permite generar una autenticacion. El parametro es el codigo de user
                                                                                                       que se ingresa en el login*/
                    //pasando el GenericIdentity al GenericPrincipal
                    System.Security.Principal.GenericPrincipal principal = new System.Security.Principal.GenericPrincipal(iD, new string[] { user.role });//el 2do parametro es el rol o arreglo de roles, este cuando el user tiene varios roles (admin y user)
                    System.Threading.Thread.CurrentPrincipal = principal;//pasando el GenericPrincipal al CurrentPrincipal

                    //Mostrar menu
                    MenuForm menuFormulario = new MenuForm();//instanciando objeto de la clase Menu
                    this.Hide();//ocultando formulario de login
                    menuFormulario.Show();//mostrando formulario
                }
                else
                {
                    MessageBox.Show("The user is not active right now", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
            {
                MessageBox.Show("The user's name or password is incorrect", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void hideButton_Click(object sender, EventArgs e)
        {
            if (passTextBox.PasswordChar == '*')//mostrando contrasenia
            {
                passTextBox.PasswordChar = '\0';//enviando a la propiedad "PasswordChar" el valor nulo. Esta contrapleca iguala a null
                passTextBox.Focus();
            }
            else
            {
                passTextBox.PasswordChar = '*';//volviendo a ocultar contra
                passTextBox.Focus();
            }
        }
    }
}
