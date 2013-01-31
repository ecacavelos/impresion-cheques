﻿using System;
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
using System.Globalization;

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

        private ConfigurationGeneral c0;
        private ConfigurationLayoutCheque c2;

        private Window ventanaDesigner = new Window();
        private Window ventanaOpenFile = new Window();
        // Ventanas abiertas desde el Menú.
        private Window ventanaConfig = new Window();
        private Window ventanaAgregarFormatoCheques = new Window();
        private Window ventanaAgregarTalonarios = new Window();
        private Window ventanaAdministrarClientes = new Window();
        private Window ventanaAdministrarAdmins = new Window();
        // Ventanas abiertas desde el Toolbar de Botones.
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
                this.c0 = ConfigurationGeneral.Deserialize("standard.xml");
            }
            catch (System.IO.FileNotFoundException ex)
            {
                System.Console.WriteLine(ex.Message);
                System.Windows.MessageBox.Show("No se encontró el archivo de configuración GENERAL.\nSe creó un nuevo archivo 'standard.xml'.\nPuede usar el programa normalmente.", "Archivo de Configuración");
                this.c0 = new ChqPrint.ConfigurationGeneral();
                ConfigurationGeneral.Serialize("standard.xml", this.c0);
            }
            catch (System.InvalidOperationException ex)
            {
                System.Console.WriteLine(ex.Message);
                System.Windows.MessageBox.Show("Existe un error con el archivo de configuración GENERAL.\nPor favor ingrese a la ventana de Configuración para recuperar las opciones.", "Archivo de Configuración");
            }
            // Archivo de configuracion para la disposicion de lo distintos formatos de Cheques.
            try
            {
                this.c2 = ConfigurationLayoutCheque.Deserialize(this.c0.FormatoChequeTalonario);
            }
            catch (System.IO.FileNotFoundException ex)
            {
                System.Console.WriteLine(ex.Message);
                System.Windows.MessageBox.Show("No se encontró el archivo de configuración de FORMATO.\n Se creó un nuevo archivo 'continental.xml'.\nPuede usar el programa normalmente.", "Archivo de Configuración");
                this.c2 = new ChqPrint.ConfigurationLayoutCheque();
                ConfigurationLayoutCheque.Serialize(this.c0.FormatoChequeTalonario, this.c2);
            }
            catch (System.InvalidOperationException ex)
            {
                System.Console.WriteLine(ex.Message);
                System.Windows.MessageBox.Show("Existe un error con el archivo de configuración de FORMATO.\nPor favor ingrese a la ventana de Configuración para recuperar las opciones.", "Archivo de Configuración");
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("es-ES");
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("es-ES");

            labelTipoChequeHomeScreen = labelStatusMain;
            labelStatusMain.Content = c2.ChequeID;
            layoutFilename = "standard.xml";
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown(); // Cerrar la Aplicación Entera.
        }

        #endregion

        #region "Funciones relativas a los Botones del Toolbar"

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

        private void menuItem_Configuracion(object sender, RoutedEventArgs e)
        {
            if (VentanaConfiguracion.IsOpen) // Se controla que una instancia de esta Ventana no este abierta. 
            {
                this.ventanaConfig.Activate(); // Si está abierta entonces activar y mandar al frente.
                return;
            }
            else // No está abierta. Abrir una instancia de la Ventana.
            {
                // Se llama a la ventana para hacer login y comprobar que el usuario es admin.
                VentanaLogin winLogin = new VentanaLogin();
                Nullable<bool> result = winLogin.ShowDialog();
                // Si el login es exitoso.
                if (result == true)
                {
                    Type type = this.GetType();
                    Assembly assembly = type.Assembly;
                    this.ventanaConfig = (Window)assembly.CreateInstance("ChqPrint.VentanaConfiguracion");
                    this.ventanaConfig.Show();
                }
                else
                {

                }
            }
        }

        private void menuItem_Salir(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(); // Cerrar la Aplicación Entera.
        }

        private void menuItem_AdministrarClientes(object sender, RoutedEventArgs e)
        {
            if (VentanaVistaClientes.IsOpen) // Se controla que una instancia de esta Ventana no este abierta. 
            {
                this.ventanaAdministrarClientes.Activate(); // Si está abierta entonces activar y mandar al frente.
                return;
            }
            else // No está abierta. Abrir una instancia de la Ventana.
            {
                // Se llama a la ventana para hacer login y comprobar que el usuario es admin.
                VentanaLogin winLogin = new VentanaLogin();
                Nullable<bool> result = winLogin.ShowDialog();
                // Si el login es exitoso.
                if (result == true)
                {
                    Type type = this.GetType();
                    Assembly assembly = type.Assembly;
                    this.ventanaAdministrarClientes = (Window)assembly.CreateInstance("ChqPrint.VentanaVistaClientes");
                    this.ventanaAdministrarClientes.Show();
                }
                else
                {
                    //System.Console.WriteLine("Se canceló el Login.");
                }
            }
        }

        private void menuItem_AdministrarFormatos(object sender, RoutedEventArgs e)
        {
            if (VentanaAgregarFormatoCheque.IsOpen) // Se controla que una instancia de esta Ventana no este abierta. 
            {
                this.ventanaAgregarFormatoCheques.Activate(); // Si está abierta entonces activar y mandar al frente.
                return;
            }
            else // No está abierta. Abrir una instancia de la Ventana.
            {
                Type type = this.GetType();
                Assembly assembly = type.Assembly;
                this.ventanaAgregarFormatoCheques = (Window)assembly.CreateInstance("ChqPrint.VentanaAgregarFormatoCheque");
                this.ventanaAgregarFormatoCheques.Show();
            }
        }

        private void menuItem_AdministrarTalonarios(object sender, RoutedEventArgs e)
        {
            if (VentanaAgregarTalonario.IsOpen) // Se controla que una instancia de esta Ventana no este abierta. 
            {
                this.ventanaAgregarTalonarios.Activate(); // Si está abierta entonces activar y mandar al frente.
                return;
            }
            else // No está abierta. Abrir una instancia de la Ventana.
            {
                // Se llama a la ventana para hacer login y comprobar que el usuario es admin.
                VentanaLogin winLogin = new VentanaLogin();
                Nullable<bool> result = winLogin.ShowDialog();
                // Si el login es exitoso.
                if (result == true)
                {
                    Type type = this.GetType();
                    Assembly assembly = type.Assembly;
                    this.ventanaAgregarTalonarios = (Window)assembly.CreateInstance("ChqPrint.VentanaAgregarTalonario");
                    this.ventanaAgregarTalonarios.Show();
                }
                else
                {
                    //System.Console.WriteLine("Se canceló el Login.");
                }
            }
        }

        private void menuItem_administrarAdmins(object sender, RoutedEventArgs e)
        {
            if (VentanaAdministrarAdmins.IsOpen) // Se controla que una instancia de esta Ventana no este abierta. 
            {
                this.ventanaAdministrarAdmins.Activate(); // Si está abierta entonces activar y mandar al frente.
                return;
            }
            else // No está abierta. Abrir una instancia de la Ventana.
            {
                // Se llama a la ventana para hacer login y comprobar que el usuario es admin.
                VentanaLogin winLogin = new VentanaLogin(true);
                Nullable<bool> result = winLogin.ShowDialog();
                // Si el login es exitoso.
                if (result == true)
                {
                    Type type = this.GetType();
                    Assembly assembly = type.Assembly;
                    this.ventanaAdministrarAdmins = (Window)assembly.CreateInstance("ChqPrint.VentanaAdministrarAdmins");
                    this.ventanaAdministrarAdmins.Show();
                }
                else
                {
                    //System.Console.WriteLine("Se canceló el Login.");
                }
            }
        }

        #endregion

        public void ActualizarLabelTipoCheque(string newTipoCheque)
        {
            labelStatusMain.Content = newTipoCheque;
        }

    }
}
