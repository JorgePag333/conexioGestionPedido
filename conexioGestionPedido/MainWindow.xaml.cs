using System;
using System.Collections.Generic;
using System.Configuration;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.SqlClient;
using System.Data;

namespace conexioGestionPedido
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            string miConexion = ConfigurationManager.ConnectionStrings["conexioGestionPedido.Properties.Settings.ConectarConnectionString"].ConnectionString;
           sqlConnection = new SqlConnection(miConexion);
          
            MuestraCliente();
            MostrarTodosPedidos();

        }

        private void MostrarTodosPedidos()
        {
            try
            {
                string consulta = "SELECT *, CONCAT(cCliente,' ',fechaPedido,' ',formaPago) AS INFOCOMPLETA  FROM Pedido";
                SqlDataAdapter adapter = new SqlDataAdapter(consulta, sqlConnection);

                using (adapter)
                {
                    DataTable pedidostabla = new DataTable();

                    adapter.Fill(pedidostabla);

                    listatodosP.DisplayMemberPath = "INFOCOMPLETA";
                    listatodosP.SelectedValuePath = "idPedido";
                    listatodosP.ItemsSource = pedidostabla.DefaultView;
                }
            }
            catch (Exception e)
            {

                MessageBox.Show(e.ToString());
            }

        }

        SqlConnection sqlConnection;
        private void MuestraCliente()
        {
            try
            {
                string consulta = "SELECT * FROM Cliente";

                SqlDataAdapter adapter = new SqlDataAdapter(consulta, sqlConnection);
                using (adapter)
                {
                    DataTable clientesTabla = new DataTable();
                    adapter.Fill(clientesTabla);
                    listaC.DisplayMemberPath = "nombre";
                    listaC.SelectedValuePath = "IdCliente";
                    listaC.ItemsSource = clientesTabla.DefaultView;
                }
            }
            catch (Exception e)
            {

                MessageBox.Show(e.ToString());
            }

        }

       

        private void MostrarPedidos()
        {
            try
            {
                string consulta = "SELECT * FROM PEDIDO P INNER JOIN CLIENTE C ON C.idCliente=P.cCLIENTE" + " WHERE C.idCliente=@ClienteId";

                SqlCommand command = new SqlCommand(consulta, sqlConnection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                using (adapter)
                {
                    command.Parameters.AddWithValue("@ClienteId", listaC.SelectedValue);

                    DataTable pedidoTabla = new DataTable();

                    adapter.Fill(pedidoTabla);
                    listaP.DisplayMemberPath = "fechaPedido";
                    listaP.SelectedValuePath = "idPedido";
                    listaP.ItemsSource = pedidoTabla.DefaultView;
                }
            }
            catch (Exception e)
            {

                MessageBox.Show(e.ToString());
            }
        }
        /*private void listaC_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //MessageBox.Show("has clicleado");
            MostrarPedidos();

        }*/

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(listatodosP.SelectedValue.ToString());

            try
            {
                string consulta = "DELETE FROM PEDIDO WHERE idPedido=@PedidoId";
                SqlCommand command = new SqlCommand(consulta, sqlConnection);
                sqlConnection.Open();
                command.Parameters.AddWithValue("@PedidoId", listatodosP.SelectedValue);
                command.ExecuteNonQuery();

                sqlConnection.Close();
                MostrarTodosPedidos();
            }
            catch (Exception r)
            {

                MessageBox.Show(r.ToString());
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                string consulta = "INSERT INTO CLIENTE (nombre) VALUES (@nombre)";
                SqlCommand command = new SqlCommand(consulta, sqlConnection);
                sqlConnection.Open();
                command.Parameters.AddWithValue("@nombre", insertaCliente.Text);
                command.ExecuteNonQuery();

                sqlConnection.Close();
                MuestraCliente();
            }
            catch (Exception r)
            {

                MessageBox.Show(r.ToString());
            }
            insertaCliente.Text = "";
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                string consulta = "DELETE FROM CLIENTE WHERE idCliente=@ClienteId";
                SqlCommand command = new SqlCommand(consulta, sqlConnection);
                sqlConnection.Open();
                command.Parameters.AddWithValue("@ClienteId", listaC.SelectedValue);
                command.ExecuteNonQuery();

                sqlConnection.Close();
                MuestraCliente();
            }
            catch (Exception r)
            {

                MessageBox.Show(r.ToString());
            }
        }

        private void listaC_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MostrarPedidos();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            FormActualizarCliente formActualizar=new FormActualizarCliente();
           
            try
            {
                string consulta = "SELECT * FROM Cliente WHERE IdCliente=@IDC";
                SqlCommand command  = new SqlCommand(consulta,sqlConnection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                using (adapter)
                {
                   command.Parameters.AddWithValue("@IDC",listaC.SelectedValue);

                    DataTable dt = new DataTable();

                    adapter.Fill(dt);

                    formActualizar.Nombre.Text = dt.Rows[0]["nombre"].ToString();
                    formActualizar.Poblacion.Text = dt.Rows[0]["poblacion"].ToString();
                    formActualizar.Telefono.Text = dt.Rows[0]["telefono"].ToString();
                    formActualizar.Direccion.Text = dt.Rows[0]["direccion"].ToString();
                    formActualizar.ID = int.Parse( listaC.SelectedValue.ToString());

                    MuestraCliente();

                }
            }
            catch (Exception ee)
            {

                MessageBox.Show(ee.ToString());
            }
            formActualizar.ShowDialog();
            MuestraCliente();
        }
    }
}
