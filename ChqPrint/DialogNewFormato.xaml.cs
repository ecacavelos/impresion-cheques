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
    /// Interaction logic for DialogNewFormato.xaml
    /// </summary>
    public partial class DialogNewFormato : Window
    {
        private ConfigurationLayoutCheque c0;

        // Metodos para obtener los Datos del Cuadro de Diálogo.
        public string ResponseText_Descripcion
        {
            get { return textBoxDescripcion.Text; }
            set { textBoxDescripcion.Text = value; }
        }

        public string ResponseText_Path
        {
            get { return textBoxPath.Text; }
            set { textBoxPath.Text = value; }
        }

        public DialogNewFormato()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        // El botón "Explorar" busca el archivo xml adecuado para cargar el nuevo Formato de Cheque.
        private void buttonExplorar_Click(object sender, RoutedEventArgs e)
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
                //labelArchivo.Content = filename.ToString();

                try
                {
                    // Se trata de leer el archivo xml seleccionado.
                    this.c0 = ConfigurationLayoutCheque.Deserialize(filename);
                }
                catch
                {
                    return;
                }

                // Se muestran los datos identificadores obtenidos del archivo abierto.
                textBoxDescripcion.Text = c0.ChequeID;
                textBoxPath.Text = filename;
                buttonAceptar.IsEnabled = true;
            }
        }

        #region "Funciones relativas a los Botones Externos"

        private void buttonAceptar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void buttonCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        #endregion

    }
}
