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
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            IsOpen = false;
        }

    }
}
