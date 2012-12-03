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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reflection;

namespace ChqPrint
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class VentanaPrincipal : Window
    {
        Window ventanaDesigner = new Window();

        public VentanaPrincipal()
        {
            InitializeComponent();
        }

        private void design_Click(object sender, RoutedEventArgs e)
        {
            if (Designer.IsOpen) // Se controla que una instancia de esta Ventana no este abierta. 
            {
                this.ventanaDesigner.Activate(); // Si está abierta entonces activar y mandar al frente.
                return;
            }
            else // No está abierta. Abrir una instancia de la Ventana.
            {
                Type type = this.GetType();
                Assembly assembly = type.Assembly;
                this.ventanaDesigner = (Window)assembly.CreateInstance("ChqPrint.Designer");
                this.ventanaDesigner.Show();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown(); // Cerrar la Aplicación Entera.
        }

        private void buttonPrint_Click(object sender, RoutedEventArgs e)
        {
            Impresion.ImprimirCheque();
        }

    }
}
