using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ChqPrint
{
    /// <summary>
    /// Interaction logic for VentanaVistaClientes.xaml
    /// </summary>
    public partial class VentanaVistaClientes : Window
    {
        public static bool IsOpen { get; private set; }

        ChqPrint.ChqDatabase1Entities chqDatabase1Entities = new ChqPrint.ChqDatabase1Entities();

        #region "Funciones relativas a la Inicializacion, Carga y Descarga de la Ventana"

        public VentanaVistaClientes()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IsOpen = true;
            // Load data into Clientes. You can modify this code as needed.
            System.Windows.Data.CollectionViewSource clientesViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("clientesViewSource")));
            System.Data.Objects.ObjectQuery<ChqPrint.Clientes> clientesQuery = this.GetClientesQuery(chqDatabase1Entities);
            clientesViewSource.Source = clientesQuery.Execute(System.Data.Objects.MergeOption.AppendOnly);
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            IsOpen = false;
        }

        #endregion

        #region "Funciones relativas a los Botones Externos"

        private void buttonSalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #endregion

        private System.Data.Objects.ObjectQuery<Clientes> GetClientesQuery(ChqDatabase1Entities chqDatabase1Entities)
        {
            // Auto generated code
            System.Data.Objects.ObjectQuery<ChqPrint.Clientes> clientesQuery = chqDatabase1Entities.Clientes;
            // Returns an ObjectQuery.
            return clientesQuery;
        }

        private void buttonGuardar_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Desea guardar los cambios efectuados?", "Confirmar modificaciones", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    chqDatabase1Entities.SaveChanges();
                }
                catch (System.Data.UpdateException ex)
                {
                    System.Console.WriteLine(ex.InnerException.GetType());
                    System.Console.WriteLine(ex.InnerException.Message);
                }
                buttonGuardar.IsEnabled = false;
                labelStatusBar.Content = "Se guardaron los cambios.";
            }
            else if (result == MessageBoxResult.No)
            {
                labelStatusBar.Content = "NO se guardaron los cambios.";
            }
        }

        private void dataGridClientes_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            buttonGuardar.IsEnabled = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result;

            if (buttonGuardar.IsEnabled == true)
            {
                result = MessageBox.Show("Desea guardar los cambios efectuados?", "Confirmar modificaciones", MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        chqDatabase1Entities.SaveChanges();
                    }
                    catch (System.Data.UpdateException ex)
                    {
                        System.Console.WriteLine(ex.InnerException.GetType());
                        System.Console.WriteLine(ex.InnerException.Message);
                    }
                    //label1.Content = "Se guardaron los cambios.";                
                }
                else if (result == MessageBoxResult.No)
                {
                    //label1.Content = "NO se guardaron los cambios.";
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

    }
}
