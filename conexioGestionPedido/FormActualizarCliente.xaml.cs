using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace conexioGestionPedido
{
    /// <summary>
    /// Lógica de interacción para FormActualizarCliente.xaml
    /// </summary>
    public partial class FormActualizarCliente : Window
    {
        public int ID;
        public FormActualizarCliente()
        {
            InitializeComponent();
            string miConexion = ConfigurationManager.ConnectionStrings["conexioGestionPedido.Properties.Settings.ConectarConnectionString"].ConnectionString;
              sqlConnection = new SqlConnection(miConexion);
        }
        SqlConnection sqlConnection;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string consulta = "UPDATE CLIENTE SET nombre=@nom, poblacion=@pob, telefono=@tel, direccion=@dire WHERE IdCliente=" + ID;
                SqlCommand cmd = new SqlCommand(consulta, sqlConnection);
                sqlConnection.Open();

                cmd.Parameters.AddWithValue("@nom", Nombre.Text);
                cmd.Parameters.AddWithValue("@pob", Poblacion.Text);
                cmd.Parameters.AddWithValue("@tel", Telefono.Text);
                cmd.Parameters.AddWithValue("@dire", Direccion.Text);
                cmd.ExecuteNonQuery();
                sqlConnection.Close();
                this.Close();
                MessageBox.Show("Registro actualizado");
            }
            catch (Exception r)
            {
                MessageBox.Show(r.Message);
                MessageBox.Show("Registro no actualizado");
            }
        }
    }
}
