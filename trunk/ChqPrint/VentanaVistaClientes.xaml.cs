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

using System.Configuration;

namespace ChqPrint
{
    /// <summary>
    /// Interaction logic for VentanaVistaClientes.xaml
    /// </summary>
    public partial class VentanaVistaClientes : Window
    {
        public static bool IsOpen { get; private set; }

        //ChqPrint.ChqDatabase1Entities chqDatabase1Entities = new ChqPrint.ChqDatabase1Entities();
        ChqPrint.ChqDatabase2Entities chqDatabase1Entities;

        private System.Data.Objects.ObjectQuery<Clientes> GetClientesQuery(ChqDatabase2Entities chqDatabase1Entities)
        {
            // Auto generated code
            System.Data.Objects.ObjectQuery<ChqPrint.Clientes> clientesQuery = chqDatabase1Entities.Clientes;
            // Returns an ObjectQuery.
            return clientesQuery;
        }

        #region "Funciones relativas a la Inicializacion, Carga y Descarga de la Ventana"

        public VentanaVistaClientes()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IsOpen = true;

            chqDatabase1Entities = new ChqPrint.ChqDatabase2Entities();

            // Load data into Clientes. You can modify this code as needed.
            System.Windows.Data.CollectionViewSource clientesViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("clientesViewSource")));
            System.Data.Objects.ObjectQuery<ChqPrint.Clientes> clientesQuery = this.GetClientesQuery(chqDatabase1Entities);
            clientesViewSource.Source = clientesQuery.Execute(System.Data.Objects.MergeOption.AppendOnly);
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            IsOpen = false;
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

        #endregion

        #region "Funciones relativas a los Botones Externos"

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

        private void buttonSalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #endregion

        private void dataGridClientes_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            Clientes obj = e.Row.Item as Clientes;
            // Si el registro no tiene id es porque es nuevo.
            if (obj.idCliente == 0)
            {
                /*// Se crea un id para el nuevo cliente a partir de la fecha actual.
                TimeSpan time = (DateTime.UtcNow - new DateTime(1970, 1, 1));
                int timestamp = (int)time.TotalSeconds;
                obj.idCliente = timestamp;*/

                // Se crea un ID para el nuevo Cliente de manera secuencial.
                var allClientesVar = chqDatabase1Entities.CreateQuery<Clientes>("SELECT value c FROM Clientes as c");
                int newID = allClientesVar.ToList().Count + 1;
                obj.idCliente = newID;
            }
            buttonGuardar.IsEnabled = true;
        }

        private void dataGridClientes_UnloadingRow(object sender, DataGridRowEventArgs e)
        {
            // System.Console.WriteLine("Borramos.");            
            buttonGuardar.IsEnabled = true;
        }

    }
}
