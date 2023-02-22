using System;
using System.Windows.Forms;

namespace Vista
{
    public partial class ProductsForm : Syncfusion.Windows.Forms.Office2010Form
    {
        public ProductsForm()
        {
            InitializeComponent();
        }
        //variable global 
        string operacionProducts = "";

        private void newButton_Click(object sender, EventArgs e)
        {
            enableControls();
            codeTextBox.Focus();
            operacionProducts = "New";
        }

        private void enableControls()
        {
            codeTextBox.Enabled = true;
            descripTextBox.Enabled = true;
            stockTextBox.Enabled = true;
            priceTextBox.Enabled = true;
            attachPicButton.Enabled = true;
            saveButton.Enabled = true;
            cancelButton.Enabled = true;
            //modButton.Enabled = true;
            newButton.Enabled = false;
        }
        private void cancelButton_Click(object sender, EventArgs e)
        {
            disableControls();
            clearControls();
            errorProvider2.Clear();
        }

        private void disableControls()
        {
            codeTextBox.Enabled = false;
            descripTextBox.Enabled = false;
            stockTextBox.Enabled = false;
            priceTextBox.Enabled = false;
            attachPicButton.Enabled = false;
            saveButton.Enabled = false;
            cancelButton.Enabled = false;
            modButton.Enabled = false;
            newButton.Enabled = true;
        }
        private void clearControls()
        {
            codeTextBox.Clear();
            descripTextBox.Clear();
            stockTextBox.Clear();
            priceTextBox.Clear();
            productPictureBox.Image = null;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            if (operacionProducts == "New")
            {
                if (string.IsNullOrEmpty(codeTextBox.Text))//validar en el lado del servidor, backend, porque se puede reutilizar la validacion
                {
                    errorProvider2.SetError(codeTextBox, "Insert a  code");
                    codeTextBox.Focus();
                    return;
                }//validacion de codigo
                errorProvider2.Clear();

                if (string.IsNullOrEmpty(descripTextBox.Text))
                {
                    errorProvider2.SetError(descripTextBox, "Specify a description");
                    descripTextBox.Focus();
                    return;
                }//validacion de descripcion
                errorProvider2.Clear();

                if (string.IsNullOrEmpty(stockTextBox.Text))
                {
                    errorProvider2.SetError(stockTextBox, "Insert the amount of products available");
                    stockTextBox.Focus();
                    return;
                }//validacion existencia
                errorProvider2.Clear();

                if (string.IsNullOrEmpty(priceTextBox.Text))
                {
                    errorProvider2.SetError(priceTextBox, "The product should have a price");
                    priceTextBox.Focus();
                    return;
                }//validacion precio
                errorProvider2.Clear();

            }//decision de boton nuevo


            else if (operacionProducts == "Mod")
            {

            }//decision de boton modificar
        }

        private void modButton_Click(object sender, EventArgs e)
        {
            operacionProducts = "Mod";
        }

        private void stockTextBox_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)//en "e" se guarda la tecla pulsada por el user
        {
            //validando que no se ingresen letras
            if (char.IsLetter(e.KeyChar))
            {
                e.Handled = true;//si es verdadero, no se permite el ingreso del caracter pulsado por el user
                errorProvider2.SetError(stockTextBox, "This blank only admits numbers");
            }
            else
            {
                errorProvider2.Clear();
            }

        }

        private void priceTextBox_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if (char.IsLetter(e.KeyChar) && (e.KeyChar != ',') && (e.KeyChar != '.') || char.IsWhiteSpace(e.KeyChar))//validando precio
            {
                e.Handled = true;
                errorProvider2.SetError(priceTextBox, "This blank only admits numbers and no spaces");
            }
            else
            {
                errorProvider2.Clear();//una vez ingresado un numero y/o punto, se elimina la advertencia
            }

            //validando que solo haya un punto para especificar decimales (TRABAJAR EN VALIDACIONES) NO APARECE LA ADVERTENCIA EN LA VALIDACION DE PRECIO POR ESTA CONDI:
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
                errorProvider2.SetError(priceTextBox, "You can only add one dot to specify amounts of decimals");
            }
            else
            {
                errorProvider2.Clear();
            }
        }
    }
}
