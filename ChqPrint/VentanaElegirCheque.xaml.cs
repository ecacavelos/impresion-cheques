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
    /// Interaction logic for VentanaElegirCheque.xaml
    /// </summary>
    public partial class VentanaElegirCheque : Window
    {
        private Configuration c0;
        public static bool IsOpen { get; private set; }

        public VentanaElegirCheque()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IsOpen = true;
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            IsOpen = false;
        }

        private void buttonAbrir_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

            // Show open file dialog box.
            Nullable<bool> result = dlg.ShowDialog();

            // Process open file dialog box results.
            if (result == true)
            {
                /* String que contiene el path completo del archivo seleccionado, 
                 * incluyendo el nombre del archivo. */
                string filename = dlg.FileName;
                labelArchivo.Content = filename.ToString();

                // Se trata de leer el archivo xml seleccionado.
                this.c0 = Configuration.Deserialize(filename);

                // Se muestran los datos identificadores obtenidos del archivo abierto.
                labelNombre.Content = c0.ChequeID;
                buttonAceptar.IsEnabled = true;
            }
        }

        private void buttonAceptar_Click(object sender, RoutedEventArgs e)
        {
            // Si se seleccionó previamente un archivo válido, se guarda su ubicación.
            VentanaPrincipal.layoutFilename = labelArchivo.Content.ToString();
            this.Close();
        }

        private void buttonCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();   // Se sale sin hacer nada.
        }
    }
}
