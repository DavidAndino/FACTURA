using System;
using System.Windows.Forms;

namespace Vista
{
    public partial class Menu : Form
    {
        public Menu()
        {
            InitializeComponent();
        }

        private void userToolStripButton_Click(object sender, EventArgs e)
        {
            UsersForm userFormulario = new UsersForm();//instanciando objeto de la clase o formulario "Users"
            userFormulario.MdiParent = this;//el formulario "UsersForm" sera hijo de este formulario (menu)
            userFormulario.Show();//llamando y mostrando al formulario luego que se haga click en usuarios en el menu
        }

        private void productToolStripButton_Click(object sender, EventArgs e)
        {
            ProductsForm producto = new ProductsForm();
            producto.MdiParent = this;//elformulario "ProductsForm" sera hijo de este formulario (menu)
            producto.Show();
        }

        private void clientsToolStripButton_Click(object sender, EventArgs e)
        {

        }

        private void salesToolStripButton_Click(object sender, EventArgs e)
        {

        }
    }
}
