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
    /// Interaction logic for VentanaAgregarTalonario.xaml
    /// </summary>
    public partial class VentanaAgregarTalonario : Window
    {
        private Configuration c2;
        public static bool IsOpen { get; private set; }

        #region "Funciones relativas a la Inicializacion, Carga y Descarga de la Ventana"

        public VentanaAgregarTalonario()
        {
            InitializeComponent();
        }

        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IsOpen = true;
            // Leemos los datos del formulario actual.
            this.c2 = Configuration.Deserialize(VentanaPrincipal.layoutFilename);
            labelTalonarioActual.Content += c2.Talonario;

            textBoxNuevoTalonario.Focus();
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            IsOpen = false;
        }

        #region "Funciones relativas a los Botones Externos"

        private void buttonGuardar_Click(object sender, RoutedEventArgs e)
        {
            this.c2.Talonario = textBoxNuevoTalonario.Text;

            int tempIntToString;                        
            Int32.TryParse(textBoxPrimerCheque.Text, out tempIntToString);
            this.c2.PrimerCheque = tempIntToString;
            Int32.TryParse(textBoxUltimoCheque.Text, out tempIntToString);
            this.c2.UltimoCheque = tempIntToString;

            Configuration.Serialize(VentanaPrincipal.layoutFilename, this.c2);

            this.Close();
        }

        private void buttonCancelar_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion

    }
}
