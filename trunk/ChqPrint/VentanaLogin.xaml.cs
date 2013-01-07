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
    /// Interaction logic for VentanaLogin.xaml
    /// </summary>
    public partial class VentanaLogin : Window
    {

        public static bool IsOpen { get; private set; }
        ChqPrint.ChqDatabase1Entities database1Entities = new ChqPrint.ChqDatabase1Entities();

        public VentanaLogin()
        {
            InitializeComponent();
        }

        public VentanaLogin(bool cambiarAdmin)
        {
            if (cambiarAdmin == true)
            {
                InitializeComponent();
                this.Height = 200;
                this.textBlock1.Visibility = Visibility.Visible;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IsOpen = true;
            textBoxUsuario.Focus();
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            IsOpen = false;
        }

        private void buttonOK_Click(object sender, RoutedEventArgs e)
        {
            string esql = "SELECT value a FROM Admins as a WHERE (a.Nombre = '" + textBoxUsuario.Text + "') AND (a.Password = '" + passwordBoxContraseña.Password + "')";
            var adminsVar = database1Entities.CreateQuery<ChqPrint.Admins>(esql);

            if (adminsVar.ToList().Count == 1)
            {
                this.DialogResult = true;
            }
            else
            {
                System.Windows.MessageBox.Show("El usuario y/o la contraseña son incorrectos.", "Contraseña Incorrecta");
                passwordBoxContraseña.Focus();
                passwordBoxContraseña.SelectAll();
            }
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void textBoxUsuario_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString() == "Return" && textBoxUsuario.Text.Length > 0)
            {
                passwordBoxContraseña.Focus();
            }
        }

        private void passwordBoxContraseña_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString() == "Return")
            {
                buttonOK_Click(null, null);
            }
        }

    }
}
