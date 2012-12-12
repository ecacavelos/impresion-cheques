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
        // Elementos accesibles desde otras ventanas.
        public static string layoutFilename;
        public static Label labelTipoChequeHomeScreen;

        private Configuration c2;

        private Window ventanaDesigner = new Window();
        private Window ventanaOpenFile = new Window();
        private Window ventanaVistaCheques = new Window();
        private Window ventanaImprimirCheques = new Window();

        #region "Funciones relativas a la Inicializacion, Carga y Descarga de la Ventana"

        public VentanaPrincipal()
        {
            /* Se intenta leer el archivo de Configuración, antes de mostrar las ventanas.
             * Si el archivo 'config.xml' no existe o no posee la sintaxis correcta, 
             * se arrojan y manejan las excepciones correspondientes. */
            try
            {
                this.c2 = Configuration.Deserialize("standard.xml");
            }
            catch (System.IO.FileNotFoundException ex)
            {
                System.Console.WriteLine(ex.Message);
                System.Windows.MessageBox.Show("No se encontró el archivo de configuración.", "Archivo de Configuración");
                //xmlinvalido = true;
                this.c2 = new ChqPrint.Configuration();
                Configuration.Serialize("standard.xml", this.c2);
            }
            catch (System.InvalidOperationException ex)
            {
                System.Console.WriteLine(ex.Message);
                System.Windows.MessageBox.Show("Existe un error con el archivo de configuración.\nPor favor ingrese a la ventana de Configuración para recuperar las opciones.", "Archivo de Configuración");
                //xmlinvalido = true;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            labelTipoChequeHomeScreen = label1;
            label1.Content = c2.ChequeID;
            layoutFilename = "standard.xml";
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown(); // Cerrar la Aplicación Entera.
        }

        #endregion

        #region "Funciones relativas a los Botones del Toolbar"

        private void buttonDesign_Click(object sender, RoutedEventArgs e)
        {
            if (VentanaElegirCheque.IsOpen) // Se controla que una instancia de esta Ventana no este abierta. 
            {
                this.ventanaOpenFile.Activate(); // Si está abierta entonces activar y mandar al frente.
                return;
            }
            else // No está abierta. Abrir una instancia de la Ventana.
            {
                Type type = this.GetType();
                Assembly assembly = type.Assembly;
                this.ventanaOpenFile = (Window)assembly.CreateInstance("ChqPrint.VentanaElegirCheque");
                this.ventanaOpenFile.Show();
            }
        }

        private void buttonReport_Click(object sender, RoutedEventArgs e)
        {
            if (VentanaVistaCheques.IsOpen) // Se controla que una instancia de esta Ventana no este abierta. 
            {
                this.ventanaVistaCheques.Activate(); // Si está abierta entonces activar y mandar al frente.
                return;
            }
            else // No está abierta. Abrir una instancia de la Ventana.
            {
                Type type = this.GetType();
                Assembly assembly = type.Assembly;
                this.ventanaVistaCheques = (Window)assembly.CreateInstance("ChqPrint.VentanaVistaCheques");
                this.ventanaVistaCheques.Show();
            }
        }

        private void buttonPreview_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonPrint_Click(object sender, RoutedEventArgs e)
        {
            if (VentanaImprimirCheque.IsOpen) // Se controla que una instancia de esta Ventana no este abierta. 
            {
                this.ventanaVistaCheques.Activate(); // Si está abierta entonces activar y mandar al frente.
                return;
            }
            else // No está abierta. Abrir una instancia de la Ventana.
            {
                Type type = this.GetType();
                Assembly assembly = type.Assembly;
                this.ventanaImprimirCheques = (Window)assembly.CreateInstance("ChqPrint.VentanaImprimirCheque");
                this.ventanaImprimirCheques.Show();
            }
        }

        #endregion

        #region "Funciones relativas a la Barra de Menús"

        private void menuItem_Salir(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(); // Cerrar la Aplicación Entera.
        }

        private void menuItem_AdministrarFormatos(object sender, RoutedEventArgs e)
        {

        }

        #endregion

        public void ActualizarLabelTipoCheque(string newTipoCheque)
        {
            label1.Content = newTipoCheque;
        }

    }
}
