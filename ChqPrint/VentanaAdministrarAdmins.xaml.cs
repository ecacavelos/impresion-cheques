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
using System.Windows.Shapes;

namespace ChqPrint
{
    /// <summary>
    /// Interaction logic for VentanaAdministrarAdmins.xaml
    /// </summary>
    public partial class VentanaAdministrarAdmins : Window
    {
        public static bool IsOpen { get; private set; }

        ChqPrint.ChqDatabase2Entities chqDatabase1Entities = new ChqPrint.ChqDatabase2Entities();

        #region "Funciones relativas a la Inicializacion, Carga y Descarga de la Ventana"

        public VentanaAdministrarAdmins()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IsOpen = true;
            textBoxUsuarioActual.Focus();
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            IsOpen = false;
        }

        #endregion

        #region "Funciones relativas a los Botones"

        private void buttonCambiar_Click(object sender, RoutedEventArgs e)
        {

            // Se verifica que ningun TextBox ni PasswordBox esté vacío.
            if (textBoxUsuarioActual.Text.Length < 1)
            {
                System.Windows.MessageBox.Show("Por favor, ingrese el usuario actual.");
                textBoxUsuarioActual.Focus();
                return;
            }
            if (textBoxNuevoUsuario.Text.Length < 1)
            {
                System.Windows.MessageBox.Show("Por favor, ingrese el nuevo usuario.");
                textBoxNuevoUsuario.Focus();
                return;
            }
            if (passwordBoxContraseñaActual.Password.Length < 1)
            {
                System.Windows.MessageBox.Show("Por favor, ingrese la contraseña actual.");
                passwordBoxContraseñaActual.Focus();
                return;
            }
            if (passwordBoxNuevaContraseña.Password.Length < 1 || passwordBoxNuevaContraseñaRepetir.Password.Length < 1)
            {
                System.Windows.MessageBox.Show("Por favor, ingrese la nueva contraseña en los campos correspondientes.");
                passwordBoxNuevaContraseña.Focus();
                return;
            }

            string esql;
            int idAdminActual = 0;

            esql = String.Format("SELECT value a FROM Admins as a WHERE (a.Nombre = '{0}')", textBoxUsuarioActual.Text);
            var adminsVar = chqDatabase1Entities.CreateQuery<Admins>(esql);

            if (adminsVar.ToList().Count == 1)
            {
                esql = String.Format("SELECT value a FROM Admins as a WHERE (a.Nombre = '{0}') AND (a.Password = '{1}')", textBoxUsuarioActual.Text, passwordBoxContraseñaActual.Password);
                adminsVar = chqDatabase1Entities.CreateQuery<Admins>(esql);

                if (adminsVar.ToList().Count == 1)
                {
                    idAdminActual = adminsVar.ToArray()[0].idAdmin;
                    System.Console.WriteLine(idAdminActual);

                    if (passwordBoxNuevaContraseña.Password == passwordBoxNuevaContraseñaRepetir.Password)
                    {
                        chqDatabase1Entities.ExecuteStoreCommand(
                            "UPDATE Admins SET Nombre = {0}, Password = {1} WHERE (Admins.idAdmin = {2})", textBoxNuevoUsuario.Text, passwordBoxNuevaContraseña.Password, idAdminActual);
                        chqDatabase1Entities.SaveChanges();

                        System.Windows.MessageBox.Show("Se cambió el admin satisfactoriamente.", "Cambio Exitoso");
                        this.Close();
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Las nuevas contraseñas introducidas no coinciden.", "Nueva Contraseña no Coincide");
                        passwordBoxNuevaContraseña.Focus();
                        passwordBoxNuevaContraseña.SelectAll();
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show("La contraseña actual ingresada es incorrecta.", "Contraseña Incorrecta");
                    passwordBoxContraseñaActual.Focus();
                    passwordBoxContraseñaActual.SelectAll();
                }

            }
            else
            {
                System.Windows.MessageBox.Show("No existe ningun usuario con el nombre ingresado.", "Usuario No Encontrado");
                textBoxUsuarioActual.Focus();
                textBoxUsuarioActual.SelectAll();
            }

        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #endregion

        #region "Funciones relativas a los Cuadros de Texto"

        private void textBoxUsuarioActual_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString() == "Return" && textBoxUsuarioActual.Text.Length > 0)
            {
                passwordBoxContraseñaActual.Focus();
            }
        }

        private void passwordBoxContraseñaActual_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString() == "Return" && passwordBoxContraseñaActual.Password.Length > 0)
            {
                textBoxNuevoUsuario.Focus();
            }
        }

        private void textBoxNuevoUsuario_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString() == "Return" && textBoxNuevoUsuario.Text.Length > 0)
            {
                passwordBoxNuevaContraseña.Focus();
            }
        }

        private void passwordBoxNuevaContraseña_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString() == "Return" && passwordBoxNuevaContraseña.Password.Length > 0)
            {
                passwordBoxNuevaContraseñaRepetir.Focus();
            }
        }

        private void passwordBoxNuevaContraseñaRepetir_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString() == "Return")
            {
                buttonCambiar_Click(null, null);
            }
        }

        #endregion

    }
}
