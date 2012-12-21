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
    /// Interaction logic for VentanaConfiguracion.xaml
    /// </summary>
    public partial class VentanaConfiguracion : Window
    {
        private ConfigurationGeneral c2;
        public static bool IsOpen { get; private set; }

        #region "Funciones relativas a la Inicializacion, Carga y Descarga de la Ventana"

        public VentanaConfiguracion()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IsOpen = true;
            // Cargamos el archivo de configuración.
            try
            {
                this.c2 = ConfigurationGeneral.Deserialize(VentanaPrincipal.layoutFilename);
            }
            catch
            {
            }
            checkBoxPermitirEscrituraManual.IsChecked = this.c2.PermitirEscrituraManual;
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            IsOpen = false;
        }

        #endregion

        #region "Funciones relativas a los Botones Externos"

        private void buttonAceptar_Click(object sender, RoutedEventArgs e)
        {
            this.c2.PermitirEscrituraManual = (bool)(checkBoxPermitirEscrituraManual.IsChecked);
            ConfigurationGeneral.Serialize(VentanaPrincipal.layoutFilename, this.c2);
            this.Close();
        }

        private void buttonCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #endregion

    }
}
