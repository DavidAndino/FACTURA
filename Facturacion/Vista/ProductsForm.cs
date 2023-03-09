using Datos;
using Entidades;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Vista
{
    public partial class ProductsForm : Syncfusion.Windows.Forms.Office2010Form
    {
        public ProductsForm()
        {
            InitializeComponent();
        }
        //variable y objetos globales 
        string operacionProducts = "";
        Product productEntity;//solo declarando objeto
        ProductDB productFromDB = new ProductDB();
        DataTable productDT = new DataTable();

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
            //validando que luego de cancelar una modificacion de registro, el codeTextBox, quede habilitado para los otros botones
            if (codeTextBox.ReadOnly == true)
            {
                codeTextBox.ReadOnly = false;
            }
            descripTextBox.Enabled = false;
            stockTextBox.Enabled = false;
            priceTextBox.Enabled = false;
            attachPicButton.Enabled = false;
            saveButton.Enabled = false;
            cancelButton.Enabled = false;
            modButton.Enabled = true;
            newButton.Enabled = true;
        }
        private void clearControls()
        {
            codeTextBox.Clear();
            descripTextBox.Clear();
            stockTextBox.Clear();
            priceTextBox.Clear();
            activeCheckBox1.Checked = false;
            productPictureBox.Image = null;
            productEntity = null;
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            productEntity = new Product();//instanciando objeto "productEntity"
            productEntity.code = codeTextBox.Text;
            productEntity.description = descripTextBox.Text;
            productEntity.price = Convert.ToDecimal(priceTextBox.Text);
            productEntity.stock = Convert.ToInt32(stockTextBox.Text);
            productEntity.activeProduct = activeCheckBox1.Checked;

            //pasando imagen del pictureBox a la propiedad "image" de la clase "Product", si realmente se eligio imagen, esto para evitar que salten excepciones
            if (productPictureBox.Image != null)
            {
                System.IO.MemoryStream alojadorImagen = new System.IO.MemoryStream();//convirtiendo imagen a arreglo de bytex, como se definio desde el principio
                productPictureBox.Image.Save(alojadorImagen, System.Drawing.Imaging.ImageFormat.Jpeg);/*pasandole la imagen del pictureBox al objeto de la clase 
                                                                                                     * MemoryStream (alojadorImagen) through metodo "Save" */
                productEntity.image = alojadorImagen.GetBuffer();//pasando lo alojado en "alojadorImagen" a la propiedad "image" de la clase "Product"
            }

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

                //insertar producto en la DB
                bool inserts = productFromDB.insert(productEntity);//el objeto "inserts" traera un true o false, desde la clase ProductDB

                //validando que el valor de  "inserts" sea true para insertar el registro
                if (inserts)
                {
                    clearControls();
                    disableControls();
                    bringProductsForm();//actualizando el DataGridView
                    MessageBox.Show("The registry has been saved successfully", "Registry save", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("The operation failed while trying to save the registry", "Failure", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                }
            }//decision de boton nuevo


            else if (operacionProducts == "Mod")
            {
                bool mod = productFromDB.edit(productEntity);
                if (mod)
                {
                    codeTextBox.ReadOnly = false;//habilitando control para que, despues de guardar un registro modded, se pueda editar a la hora de crear un nuevo  registro
                    clearControls();
                    disableControls();
                    bringProductsForm();
                    MessageBox.Show("The registry has been modified successfully", "Registry modification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("The operation failed while trying to modify the registry", "Failure", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                }
            }//decision de boton modificar
        }

        private void modButton_Click(object sender, EventArgs e)
        {
            operacionProducts = "Mod";
            //pasando el registro seleccionado a cada textBox y combo y check
            if (productsDataGridView.SelectedRows.Count > 0)//si el user selecciono al menos una fila
            {
                //el orden de esto de llenar los textbox etc  de nuevo con los datos que hay en la celda seleccionada en el dataGrid, se hace con el de la DB en MySql
                codeTextBox.Text = productsDataGridView.CurrentRow.Cells["Code"].Value.ToString();//pasando los datos que contiene la celda de la fila seleccionada
                descripTextBox.Text = productsDataGridView.CurrentRow.Cells["Description"].Value.ToString();
                stockTextBox.Text = productsDataGridView.CurrentRow.Cells["Stock"].Value.ToString();
                priceTextBox.Text = productsDataGridView.CurrentRow.Cells["Price"].Value.ToString();
                activeCheckBox1.Checked = Convert.ToBoolean(productsDataGridView.CurrentRow.Cells["ActiveProduct"].Value);
                byte[] image = productFromDB.photo(productsDataGridView.CurrentRow.Cells["Code"].Value.ToString());

                //validando que el vector "myPhoto" traiga una imagen
                if (image.Length > 0)//si el user no tiene una foto en la DB, este arreglo devuelve un 0
                {
                    MemoryStream mS = new MemoryStream(image);
                    productPictureBox.Image = System.Drawing.Bitmap.FromStream(mS);
                }
                enableControls();//habilitando los textBoxes y checkBoxes para que el usuario pueda editar
                codeTextBox.ReadOnly = true;//no permitiendo que la PK se  pueda modificar
            }
            else
            {
                MessageBox.Show("You must select a registry");
            }
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

        private void attachPicButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();//abriendo una ventana para adjuntar
            DialogResult result = dialog.ShowDialog();//capturando el archivo de imagen que elige el usuario

            //validando si el usuario eligio una foto o no
            if (result == DialogResult.OK)
            {
                productPictureBox.Image = Image.FromFile(dialog.FileName);/*asignando imagen al picturebox Metodo "FromFile" convierte la imagen capturada y 
                                                                     se la  pasa al picturebox*/
            }
        }

        //creando metodo que actualiza o trae los nuevos datos de la tabla de la DB
        private void bringProductsForm()
        {
            productDT = productFromDB.bringProducts();//trayendo todos los productos
            productsDataGridView.DataSource = productDT;//llenando el DataGridView con cada registro que se ingrese, edite o elimine
        }

        private void ProductsForm_Load(object sender, EventArgs e)
        {
            bringProductsForm();
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            //validando que le user seleccione un registro, es decir, una fila o celda del dataGridView
            if (productsDataGridView.SelectedRows.Count > 0)
            {
                //validando que el usuario realmente quiera eliminar el registro
                DialogResult decision = MessageBox.Show("Are you sure you want to delete the registry?", "Delete Registry", MessageBoxButtons.YesNo);
                //validando si el user presiona "Ok" o "Cancel"
                if (decision == DialogResult.Yes)
                {
                    bool deleted = productFromDB.delete(productsDataGridView.CurrentRow.Cells["Code"].Value.ToString());// el codigo de producto esta en el DataGrid
                    if (deleted)
                    {
                        clearControls();
                        disableControls();
                        bringProductsForm();//al traer los productos actualizados desde la DB, se actualiza el DATAGrid
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
            bringProductsForm();
        }
    }
}
