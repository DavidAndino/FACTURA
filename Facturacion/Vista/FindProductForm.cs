using Datos;
using Entidades;
using System;

namespace Vista
{
    public partial class FindProductForm : Syncfusion.Windows.Forms.Office2010Form
    {
        public FindProductForm()
        {
            InitializeComponent();
        }
        //objetos globales
        ProductDB item = new ProductDB();
        public Product product = new Product();
        private void FindProductForm_Load(object sender, EventArgs e)
        {
            productsDataGridView.DataSource = item.bringProducts();
        }

        private void insertedDescripTextBox_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            productsDataGridView.DataSource = item.bringProductsForDescription(insertedDescripTextBox.Text);
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void acceptButton_Click(object sender, EventArgs e)
        {
            if (productsDataGridView.RowCount > 0)//si el recuento de filas es mayor a 0, o sea, si se elige una fila
            {
                if (productsDataGridView.SelectedRows.Count > 0)//si el usuario selecciona una fila, se le pasan todos los campos del registro seleccionado en el datagrid
                {
                    product.code = productsDataGridView.CurrentRow.Cells["Code"].Value.ToString();
                    product.description = productsDataGridView.CurrentRow.Cells["Description"].Value.ToString();
                    product.stock = Convert.ToInt32(productsDataGridView.CurrentRow.Cells["Stock"].Value);
                    product.price = Convert.ToDecimal(productsDataGridView.CurrentRow.Cells["Price"].Value);
                    product.activeProduct = Convert.ToBoolean(productsDataGridView.CurrentRow.Cells["ActiveProduct"].Value);
                    Close();
                }
            }
        }
    }
}
