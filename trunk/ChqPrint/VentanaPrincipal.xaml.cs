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
             if (Designer.IsOpen)// Se controla que una instancia de esta ventana no este abierta. 
            {
                this.ventanaDesigner.Activate();// Si esta abierta entonces activar, mandar al frente
                return;
            }
            else // NO ESTA ABIERTA. Abrir una instancia de la ventana.
            {
                Type type = this.GetType();
                Assembly assembly = type.Assembly;
                this.ventanaDesigner = (Window)assembly.CreateInstance("ChqPrint.Designer");
                this.ventanaDesigner.Show();
            }
        }

    }
}
