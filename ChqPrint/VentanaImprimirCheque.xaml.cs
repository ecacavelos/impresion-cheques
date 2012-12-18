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
    /// Interaction logic for VentanaImprimirCheque.xaml
    /// </summary>
    public partial class VentanaImprimirCheque : Window
    {
        public static bool IsOpen { get; private set; }

        ChqPrint.ChqDatabase1Entities database1Entities = new ChqPrint.ChqDatabase1Entities();

        #region "Funciones relativas a la Inicializacion, Carga y Descarga de la Ventana"

        public VentanaImprimirCheque()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IsOpen = true;

            datePickerFecha.SelectedDate = DateTime.Now;    // La fecha del cheque.
            // Hacemos un query a la base de datos para obtener todas los cheques.
            string esql = "SELECT value c FROM Cheques as c";
            var chequesVar = database1Entities.CreateQuery<Cheques>(esql);

            // Si existe al menos un cheque.
            int lastNroCheque = 0;
            if (chequesVar.ToList().Count > 0)
            {
                // Pasamos el string "Nro_Factura" obtenido de la ultima entrada de la tabla Facturas a int
                Int32.TryParse(chequesVar.ToList().ElementAt(chequesVar.ToList().Count - 1).nroCheque, out lastNroCheque);
                // Incrementamos en 1 la ultima factura y desplegamos el valor sugerido en el textbox
                lastNroCheque++;
                textBlockNumeroCheque.Text = lastNroCheque.ToString("000000");
            }
            else
            {
                textBlockNumeroCheque.Text = "000000";
            }

            textBoxMonto.Focus();

            int defaultIndex = 0;
            int i = 0;
            // Cargamos los Cheques en el comboBox            
            string esqlFormatos = String.Format("SELECT value f FROM Formatos as f");
            var formatosVar = database1Entities.CreateQuery<Formatos>(esqlFormatos);
            foreach (Formatos tempFormato in formatosVar)
            {
                ComboBoxItem elementoCombo = new ComboBoxItem();
                elementoCombo.Content = tempFormato.Descripcion;
                comboBoxTipoCheque.Items.Add(elementoCombo);
                if (tempFormato.Path == VentanaPrincipal.layoutFilename)
                {
                    defaultIndex = i;
                }
                i++;
            }

            if (comboBoxTipoCheque.HasItems)
            {
                comboBoxTipoCheque.SelectedIndex = defaultIndex;
            }

        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            IsOpen = false;
        }

        #endregion

        private void buttonImprimir_Click(object sender, RoutedEventArgs e)
        {

            int validarNroCheque = 0;
            int validarMonto = 0;

            // Validamos el campo "Numero de Cheque"
            try
            {
                validarNroCheque = Convert.ToInt32(textBlockNumeroCheque.Text);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                MessageBox.Show("Por favor introduzca un número de Cheque válido (Sólo números).");
            }

            // Validamos el campo de "Monto"
            try
            {
                validarMonto = Convert.ToInt32(textBoxMonto.Text);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.Message);
                MessageBox.Show("Por favor introduzca un Monto válido (Sólo números).");
            }

            if (validarNroCheque > 0 && validarMonto > 0)
            {
                // Si se seleccionó previamente un archivo válido, se obtiene su ubicación.
                string esql0 = String.Format("SELECT value f FROM Formatos as f WHERE f.Descripcion = '{0}'", ((ComboBoxItem)comboBoxTipoCheque.SelectedItem).Content.ToString());
                var formatosVar = database1Entities.CreateQuery<Formatos>(esql0);

                if (formatosVar.Count() == 1)
                {
                    VentanaPrincipal.layoutFilename = formatosVar.First().Path;
                }

                // Hacemos un query a la Base de Datos para obtener todos los Cheques.
                string esql = String.Format("SELECT value c FROM Cheques as c WHERE c.nroCheque = '{0}'", textBlockNumeroCheque.Text);
                var chequesVar = database1Entities.CreateQuery<Cheques>(esql);

                // Si ya no existe un Cheque con ese número.
                if (chequesVar.ToList().Count == 0)
                {
                    int tempMonto = 0;
                    Int32.TryParse(textBoxMonto.Text, out tempMonto);

                    Cheques tempCheque = new Cheques();
                    if (tempCheque.idCheque == 0)                               // Si el ID no existe.
                    {
                        TimeSpan time = (DateTime.UtcNow - new DateTime(1970, 1, 1));
                        int timestamp = (int)time.TotalSeconds;
                        tempCheque.idCheque = timestamp;                        // Nuevo ID = timestamp.
                    }
                    tempCheque.nroCheque = textBlockNumeroCheque.Text;
                    tempCheque.Banco = ((ComboBoxItem)comboBoxTipoCheque.SelectedItem).Content.ToString();
                    tempCheque.Fecha = datePickerFecha.SelectedDate;
                    tempCheque.Monto = tempMonto;
                    tempCheque.PagueseOrdenDe = textBoxPaguese.Text;
                    tempCheque.MontoEnLetras = Numalet.ToCardinal((int)(tempCheque.Monto)).ToUpper();
                    tempCheque.Estado = "Pendiente";

                    // Se intenta imprimir el Cheque.
                    if (Impresion.ImprimirCheque((DateTime)(tempCheque.Fecha), (int)tempCheque.Monto, tempCheque.PagueseOrdenDe))
                    {
                        System.Windows.MessageBox.Show("Se imprimió el Cheque.", "Impresión");
                        // Luego de imprimir el Cheque, agregamos un nuevo registro a la tabla 'Cheques'.
                        database1Entities.Cheques.AddObject(tempCheque);
                        database1Entities.SaveChanges();
                        // Cerramos la ventana.
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("No se imprimió el Cheque.");
                    }
                }
                else
                {
                    MessageBox.Show("Este número de Cheque ya existe.");
                }
            }
            else
            {

            }

        }

    }
}
