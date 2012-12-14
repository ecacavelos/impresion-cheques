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
    /// Interaction logic for VentanaElegirFormatoCheque.xaml
    /// </summary>
    public partial class VentanaElegirFormatoCheque : Window
    {
        private Configuration c0;
        public static bool IsOpen { get; private set; }

        ChqPrint.ChqDatabase1Entities database1Entities = new ChqPrint.ChqDatabase1Entities();

        #region "Funciones relativas a la Inicializacion, Carga y Descarga de la Ventana"

        public VentanaElegirFormatoCheque()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IsOpen = true;
            // Cargamos los Cheques en el comboBox            
            int defaultIndex = 0;
            int i = 0;

            string esql = String.Format("SELECT value f FROM Formatos as f");
            var formatosVar = database1Entities.CreateQuery<Formatos>(esql);
            foreach (Formatos tempFormato in formatosVar)
            {
                ComboBoxItem elementoCombo = new ComboBoxItem();
                elementoCombo.Content = tempFormato.Descripcion;
                comboBoxFormatoCheque.Items.Add(elementoCombo);
                if (tempFormato.Path == VentanaPrincipal.layoutFilename)
                {
                    defaultIndex = i;
                }
                i++;
            }

            if (comboBoxFormatoCheque.HasItems)
            {
                comboBoxFormatoCheque.SelectedIndex = defaultIndex;
            }
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            IsOpen = false;
        }

        #endregion

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
                //labelArchivo.Content = filename.ToString();

                // Se trata de leer el archivo xml seleccionado.
                this.c0 = Configuration.Deserialize(filename);

                // Se muestran los datos identificadores obtenidos del archivo abierto.
                labelNombre.Content = c0.ChequeID;
                buttonAceptar.IsEnabled = true;
            }
        }

        #region "Funciones relativas a los botones Aceptar, Cancelar, etc."

        private void buttonAceptar_Click(object sender, RoutedEventArgs e)
        {
            // Si se seleccionó previamente un archivo válido, se guarda su ubicación.
            string esql = String.Format("SELECT value f FROM Formatos as f WHERE f.Descripcion = '{0}'", ((ComboBoxItem)comboBoxFormatoCheque.SelectedItem).Content.ToString());
            var formatosVar = database1Entities.CreateQuery<Formatos>(esql);

            System.Console.WriteLine(esql);

            if (formatosVar.Count() == 1)
            {
                VentanaPrincipal.layoutFilename = formatosVar.First().Path;
                VentanaPrincipal.labelTipoChequeHomeScreen.Content = ((ComboBoxItem)comboBoxFormatoCheque.SelectedItem).Content.ToString();
                this.Close();
            }

        }

        private void buttonCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();   // Se sale sin hacer nada.
        }

        #endregion

        private void comboBoxFormatoCheque_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Si se seleccionó previamente un archivo válido, se obtiene su ubicación.
            string esql0 = String.Format("SELECT value f FROM Formatos as f WHERE f.Descripcion = '{0}'", ((ComboBoxItem)comboBoxFormatoCheque.SelectedItem).Content.ToString());
            var formatosVar = database1Entities.CreateQuery<Formatos>(esql0);

            if (formatosVar.Count() != 1)
            {
                MessageBox.Show("Existe un conflicto con el archivo xml asociado a este Formato.");
                return;
            }

            /* String que contiene el path completo del archivo seleccionado, 
            * incluyendo el nombre del archivo. */
            string filename = formatosVar.First().Path;
            // Se trata de leer el archivo xml seleccionado.
            this.c0 = Configuration.Deserialize(filename);

            // Se muestran los datos identificadores obtenidos del archivo abierto.
            labelNombre.Content = c0.ChequeID;
            buttonAceptar.IsEnabled = true;
        }

    }
}
