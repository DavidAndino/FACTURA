using Datos;
using Entidades;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Vista
{
    public partial class FacturaForm : Syncfusion.Windows.Forms.Office2010Form
    {
        public FacturaForm()
        {
            InitializeComponent();
        }
        //objetos globales
        Client myCustomer = null;
        ClientDB clientFromDb = new ClientDB();
        Product myProduct = null;
        ProductDB productFromDB = new ProductDB();
        List<BillDetail> detailsList = new List<BillDetail>();//creando lista
        //creando variables que almacenan subtotal, impuesto y total a pagar
        decimal subtotal = 0, SalesTax = 0, totalPayment = 0, discount = 0;

        private void clientIdTextBox_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            //aqui se presionara "Enter" y se buscara al cliente por ID
            if (e.KeyChar == (char)Keys.Enter && !string.IsNullOrEmpty(clientIdTextBox.Text))//si la pulsasion del teclado es en Enter y si el campo no esta vacio
            {
                myCustomer = new Client();//instanciando objeto
                myCustomer = clientFromDb.bringClientsForID(clientIdTextBox.Text);//pasandole al objet "myCustomer" la id que ingrese el usuario
                clientsNameTextBox.Text = myCustomer.name;//asignando nombre del cliente en el textbox que se hizo para esta accion
            }
            else
            {
                myCustomer = null;//limpiando object whether or not el usuario comete un error
                clientsNameTextBox.Clear();//limpiando el textbox tambien
            }
        }

        private void findClientButton_Click(object sender, System.EventArgs e)
        {
            //abriendo otro formulario donde se buscara por nombre de cliente
            FindClientForm link = new FindClientForm();
            link.ShowDialog();
            //cuando se cierre el dialogo, se recibe el objeto "myCustomer"
            myCustomer = new Client();
            myCustomer = link.client;
            //llenando campos de la factura con los valores encontrados
            clientIdTextBox.Text = myCustomer.iD;
            clientsNameTextBox.Text = myCustomer.name;
        }

        private void productCodeTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            //aqui se presionara "Enter" y se buscara al cliente por ID
            if (e.KeyChar == (char)Keys.Enter && !string.IsNullOrEmpty(productCodeTextBox.Text))//si la pulsasion del teclado es en Enter y el campo de codigo deprodcuto no esta vacio
            {
                myProduct = new Product();//instanciando objeto
                myProduct = productFromDB.bringProductsForCode(productCodeTextBox.Text);//pasandole al object "myProduct" el codigo que ingrese el usuario
                productDescripTextBox.Text = myProduct.description;//asignando descripcion del producto en el textbox que se hizo para esta accion
                stockTextBox.Text = myProduct.stock.ToString();
            }
            else
            {
                myProduct = null;//limpiando object whether or not el usuario comete un error
                productDescripTextBox.Clear();//limpiando el textbox tambien
                stockTextBox.Clear();
            }
        }

        private void FacturaForm_Load(object sender, System.EventArgs e)
        {
            //accediendo al codigo de usuario y llenando el textBox de User de este form
            userTextBox.Text = System.Threading.Thread.CurrentPrincipal.Identity.Name;//en el LoginForm, se le paso el codigo de user al "Identity":
        }

        private void findProductbutton_Click(object sender, System.EventArgs e)
        {
            FindProductForm link = new FindProductForm();
            link.ShowDialog();
            myProduct = new Product();
            myProduct = link.product;//pasando object de FindProductForm al objeto de la Factura
            productCodeTextBox.Text = myProduct.code;
            productDescripTextBox.Text = myProduct.description;
            stockTextBox.Text = myProduct.stock.ToString();
        }

        private void amountTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter && !string.IsNullOrEmpty(amountTextBox.Text))
            {
                BillDetail detail = new BillDetail();
                detail.productCode = myProduct.code;
                detail.price = myProduct.price;
                detail.amount = Convert.ToInt32(amountTextBox.Text);
                detail.total = (myProduct.price * Convert.ToInt32(amountTextBox.Text));//subtotal: precio*cantidad
                //procesos
                subtotal += detail.total;//acumulando el subtotal de cada registro
                SalesTax = (subtotal * 0.15M);
                totalPayment = (subtotal + SalesTax) - discount;

                //añadiendo cada registro de compra de productos a la lista
                detailsList.Add(detail);
                productRegistriesTataGridView.DataSource = null;
                productRegistriesTataGridView.DataSource = detailsList;

                //llenando campos con valores ingresados
                subtotalTextBox.Text = subtotal.ToString();
                StTextBox.Text = SalesTax.ToString();
                totalTextBox.Text = totalPayment.ToString();
                clearControls();//limpiando controles una vez se  haya realizado el registro
                disableControlsAndButtons();
            }

        }
        private void clearControls()
        {
            productCodeTextBox.Text = "";
            productDescripTextBox.Clear();
            stockTextBox.Clear();
            amountTextBox.Text = string.Empty;
        }
        private void disableControlsAndButtons()
        {
            clientIdTextBox.Enabled = false;
            clientsNameTextBox.Enabled = false;
            findClientButton.Enabled = false;
        }

    }
}
