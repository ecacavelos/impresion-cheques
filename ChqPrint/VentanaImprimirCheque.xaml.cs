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
        private ConfigurationGeneral c0;
        private ConfigurationLayoutCheque c2;
        public static bool IsOpen { get; private set; }

        ChqPrint.ChqDatabase2Entities database1Entities = new ChqPrint.ChqDatabase2Entities();

        //private bool AutoCompleteHasFocus = false;

        #region "Funciones relativas a la Inicializacion, Carga y Descarga de la Ventana"

        public VentanaImprimirCheque()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IsOpen = true;
            this.c0 = ConfigurationGeneral.Deserialize("standard.xml");
            this.c2 = ConfigurationLayoutCheque.Deserialize(database1Entities.Talonarios.ToArray()[0].FormatoChequeTalonario);

            datePickerFecha.SelectedDate = DateTime.Now;    // La fecha del cheque.

            inicializarNroCheque();

            textBoxMonto.Focus();

            // Agregamos la lista de sugerencias de Clientes al AutoCompleteTextBox 'Paguese a la Orden de'.
            string esql_clientes = "SELECT value c FROM Clientes as c";
            var clientesVar = database1Entities.CreateQuery<Clientes>(esql_clientes);
            foreach (Clientes tempCliente in clientesVar.ToArray())
            {
                textBoxPaguese.AddItem(new WPFAutoCompleteTextbox.AutoCompleteEntry(tempCliente.Nombre, tempCliente.Nombre));
            }

            // Habilitamos el textBox de Alias, si corresponde.
            if (c0.PermitirEscrituraManual == true)
            {
                textBoxAlias.IsEnabled = true;
            }

            textBoxConcepto.SetTextBoxMaxLength(55);
            // Agregamos la lista de sugerencias de Conceptos al AutoCompleteTextBox 'Concepto'.
            string esql_conceptos = "SELECT value c FROM Conceptos as c";
            var conceptosVar = database1Entities.CreateQuery<Conceptos>(esql_conceptos);
            foreach (Conceptos tempConcepto in conceptosVar.ToArray())
            {
                textBoxConcepto.AddItem(new WPFAutoCompleteTextbox.AutoCompleteEntry(tempConcepto.Descripcion, tempConcepto.Descripcion));
            }

            // Indicamos el Formato de Cheque Actual en el Label correspondiente.
            textBlockFormatoCheque.Text = c2.ChequeID;

        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            IsOpen = false;
        }

        #endregion

        private void inicializarNroCheque()
        {
            // Hacemos un query a la base de datos para obtener todas los cheques.
            string esql = String.Format("SELECT value c FROM Cheques as c WHERE c.Talonario = '{0}' ORDER BY c.nroCheque", database1Entities.Talonarios.ToArray()[0].Nombre);
            var chequesVar = database1Entities.CreateQuery<Cheques>(esql);

            textBlockTalonario.Text = database1Entities.Talonarios.ToArray()[0].Nombre;
            // Si existe al menos un cheque.
            int lastNroCheque = 0;
            if (chequesVar.ToList().Count > 0)
            {
                // Pasamos el string "nroCheque" obtenido de la ultima entrada de la tabla Cheques a int
                Int32.TryParse(chequesVar.ToList().ElementAt(chequesVar.ToList().Count - 1).nroCheque, out lastNroCheque);
                // Incrementamos en 1 el último Cheque y desplegamos el valor correspondiente en el textBlock
                lastNroCheque++;
                textBlockNumeroCheque.Text = lastNroCheque.ToString("00000000");
            }
            else
            {
                textBlockNumeroCheque.Text = ((long)database1Entities.Talonarios.ToArray()[0].PrimerCheque).ToString("00000000");
            }

            // Verificamos que no se haya sobrepasado el ultimo cheque del talonario.
            if (lastNroCheque > database1Entities.Talonarios.ToArray()[0].UltimoCheque)
            {
                MessageBox.Show(String.Format("Se han agotado todos los cheques de este talonario ({0}).\nPor favor agregue un nuevo talonario.", database1Entities.Talonarios.ToArray()[0].Nombre));
                this.Close();
            }
        }

        // Función de Impresión.
        private void buttonImprimir_Click(object sender, RoutedEventArgs e)
        {
            long montoValidado = 0;

            // Validamos el campo de "Monto".
            if (textBoxMonto.Text.Length > 0)
            {
                try
                {
                    montoValidado = Convert.ToInt64(textBoxMonto.Text.Replace(".", ""));
                    //System.Console.WriteLine(montoValidado);
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine(ex.Message);
                    MessageBox.Show("Por favor introduzca un Monto válido (Sólo números).");
                }
            }

            // Verificamos si el Cliente ingresado ya existe en la Base de Datos.
            string esql_clientes = String.Format("SELECT value c FROM Clientes as c WHERE (c.Nombre == '{0}' AND c.Alias == '{1}')", textBoxPaguese.Text, textBoxAlias.Text);
            System.Console.WriteLine(esql_clientes);
            var clientesVar = database1Entities.CreateQuery<Clientes>(esql_clientes);
            System.Console.WriteLine(String.Format("Buscando clientes... {0} resultado(s).", clientesVar.ToList().Count));

            // Si el Cliente NO existe.
            if (clientesVar.ToList().Count == 0)
            {
                // Si no está habilitado el ingreso manual de nuevos clientes.
                if (c0.PermitirEscrituraManual == false)
                {
                    MessageBox.Show("Debe elegir un cliente ya existente en la Base de Datos.");
                    return;
                }
                // Agregamos el nuevo Cliente a la Base de Datos.
                else
                {
                    System.Console.WriteLine("Registrando el nuevo cliente...");
                    Clientes newCliente = new Clientes();
                    // Si el ID no existe.
                    if (newCliente.idCliente == 0)
                    {
                        var allClientesVar = database1Entities.CreateQuery<Clientes>("SELECT value c FROM Clientes as c");
                        int newID = allClientesVar.ToList().Count + 1;
                        newCliente.idCliente = newID;
                    }
                    newCliente.Nombre = textBoxPaguese.Text;
                    newCliente.Alias = textBoxAlias.Text;
                    database1Entities.Clientes.AddObject(newCliente);
                }
            }

            // Grabamos el nuevo Cliente y el nuevo Concepto si hiciese falta.
            database1Entities.SaveChanges();

            // Hacemos Queries a la Base de Datos para obtener todos los Cheques y el Cliente correspondiente.
            string esql = String.Format("SELECT value c FROM Cheques as c WHERE (c.nroCheque = '{0}' AND c.Talonario = '{1}')", textBlockNumeroCheque.Text, textBlockTalonario.Text);
            var chequesVar = database1Entities.CreateQuery<Cheques>(esql);

            // Si ya no existe un Cheque con ese Talonario y Número de Cheque.
            if (chequesVar.ToList().Count == 0)
            {

            }
            else
            {
                MessageBox.Show("Este número de Cheque ya existe.\nSe pasará al siguiente cheque en el Talonario.\nPuede volver a presionar el botón 'Imprimir'.");
                inicializarNroCheque();
                return;
            }

            clientesVar = database1Entities.CreateQuery<Clientes>(esql_clientes);
            System.Console.WriteLine(String.Format("Individualizando cliente... {0} resultado(s).", clientesVar.ToList().Count));

            // Si el receptor del cheque existe y fue individualizado.
            if (clientesVar.ToList().Count == 1)
            {
                Clientes tempCliente = clientesVar.ToArray()[0];

                // Creamos la estructura para el nuevo cheque.
                Cheques tempCheque = new Cheques();
                if (tempCheque.idCheque == 0)
                {
                    TimeSpan time = (/*(DateTime)datePickerFecha.SelectedDate*/DateTime.Now - new DateTime(1970, 1, 1));
                    int timestamp = (int)time.TotalSeconds;
                    tempCheque.idCheque = timestamp;                        // Nuevo ID = timestamp.
                }
                tempCheque.Talonario = textBlockTalonario.Text;
                tempCheque.nroCheque = textBlockNumeroCheque.Text;
                //tempCheque.Banco = ((ComboBoxItem)comboBoxTipoCheque.SelectedItem).Content.ToString();
                tempCheque.Banco = textBlockFormatoCheque.Text;
                tempCheque.Fecha = datePickerFecha.SelectedDate;
                tempCheque.Monto = montoValidado;
                tempCheque.PagueseOrdenDe = textBoxPaguese.Text;
                tempCheque.concepto = textBoxConcepto.Text;
                tempCheque.MontoEnLetras = Numalet.ToCardinal((int)(tempCheque.Monto)).ToUpper();
                // Emitimos el cheque en un estado distinto de acuerdo a los datos completados.
                if (textBoxMonto.Text.Length == 0 || textBoxPaguese.Text.Length == 0)
                {
                    MessageBoxResult result = MessageBox.Show("Desea imprimir el cheque con campos en blanco (Estado: Pendiente)?", "Imprimir", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.No)
                    {
                        return;
                    }
                    tempCheque.Estado = "Pendiente";
                }
                else
                {
                    tempCheque.Estado = "Emitido";
                }

                if (checkBoxImprimir.IsChecked == true)
                {
                    // Se intenta imprimir el Cheque.
                    if (Impresion.ImprimirCheque((DateTime)(tempCheque.Fecha), (long)tempCheque.Monto, tempCliente, tempCheque.concepto))
                    {

                        MessageBoxResult result = MessageBox.Show("Se imprimió correctamente el Cheque?", "Impresión", MessageBoxButton.YesNo);
                        if (result == MessageBoxResult.Yes)
                        {
                            // Se imprime el Cheque, agregamos un nuevo registro a la tabla 'Cheques'.
                            try
                            {
                                database1Entities.Cheques.AddObject(tempCheque);
                                database1Entities.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Hubo un problema ingresando el Cheque a la Base de Datos. Por favor cierre la ventana e intente nuevamente.\nExcepción: " + ex.Message);
                            }

                            // Cerramos la ventana.
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("El usuario indicó que el Cheque no se imprimió correctamente.");
                        }

                    }
                    else
                    {
                        MessageBox.Show("No se imprimió el Cheque.");
                    }
                }
                else
                {
                    // Esperamos confirmación de que no se desea imprimir el cheque, sólo guardarlo.
                    MessageBoxResult result = MessageBox.Show("Está seguro de que no desea imprimir el Cheque?", "Sólo Guardar", MessageBoxButton.YesNo);
                    if (result == MessageBoxResult.No)
                    {
                        return;
                    }
                    // Intentamos guardar el cheque en la Base de Datos.
                    try
                    {
                        database1Entities.Cheques.AddObject(tempCheque);
                        database1Entities.SaveChanges();

                        System.Windows.MessageBox.Show("Se guardó el Cheque.", "Impresión");
                        // Cerramos la ventana.
                        this.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Hubo un problema ingresando el Cheque a la Base de Datos. Por favor cierre la ventana e intente nuevamente.\nExcepción: " + ex.Message);
                    }

                }

            }
            else
            {
                MessageBox.Show(String.Format("No se pudo determinar el receptor del Cheque.\n{0} posible(s) receptore(s).", clientesVar.ToList().Count));
            }

        }

        #region "Funciones relativas a los Cuadros de Texto con Sugerencias"

        // Esta funcion es la que pone los puntos en los miles, millones, etc.  
        private void textBoxMonto_TextChanged(object sender, TextChangedEventArgs e)
        {
            long tempMonto = 0;

            bool result = Int64.TryParse(((string)textBoxMonto.Text), System.Globalization.NumberStyles.Number, new System.Globalization.CultureInfo("es-ES"), out tempMonto);
            if (result)
            {
                //textBoxMonto.Text = ((Int64)tempMonto).ToString("#,##0");                                
                System.Globalization.NumberFormatInfo nfi = new System.Globalization.CultureInfo("es-ES", false).NumberFormat;
                textBoxMonto.Text = ((Int64)tempMonto).ToString("N0", nfi);
                textBoxMonto.CaretIndex = textBoxMonto.Text.Length;
            }
        }

        private void textBoxMonto_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString() == "Return")
            {
                textBoxPaguese.FocusTextBox();
            }
        }

        private void textBoxPaguese_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //System.Console.WriteLine(e.Key.ToString());
            if (e.Key.ToString() == "Down")
            {
                e.Handled = true;
                textBoxPaguese.ChangeComboBoxIndexDown();
            }
            else if (e.Key.ToString() == "Up")
            {
                e.Handled = true;
                textBoxPaguese.ChangeComboBoxIndexUp();
            }
            else if (e.Key.ToString() == "Back")
            {
                if (textBoxPaguese.SuggestionListActive() == true && textBoxPaguese.FindComboBoxIndex() != -1)
                {
                    textBoxPaguese.FocusTextBox();
                }
            }
            else if (e.Key.ToString() == "Return")
            {
                string esql_clientes = String.Format("SELECT value c FROM Clientes as c WHERE c.Nombre == '{0}'", textBoxPaguese.Text);
                var clientesVar = database1Entities.CreateQuery<Clientes>(esql_clientes);

                if (textBoxAlias.IsEnabled == true)
                {
                    System.Console.WriteLine(String.Format("{0} resultado(s) posible(s).", clientesVar.ToList().Count));
                    // Si no hay clientes con ese nombre.
                    if (clientesVar.ToList().Count == 0)
                    {
                        textBoxAlias.Focus();
                    }
                    // Si existe un cliente con dicho nombre.
                    if (clientesVar.ToList().Count == 1)
                    {
                        // Si el Alias no está en blanco.
                        if (clientesVar.ToArray()[0].Alias.Length > 0)
                        {
                            textBoxAlias.Text = clientesVar.ToArray()[0].Alias;
                            textBoxConcepto.FocusTextBox();
                        }
                        // Si el Alias está en blanco hacemos focus en el textbox correspondiente.
                        else
                        {
                            textBoxAlias.Focus();
                        }
                    }
                    if (clientesVar.ToList().Count > 1)
                    {
                        textBoxAlias.Focus();
                    }
                }
                else
                {
                    textBoxConcepto.FocusTextBox();
                }
            }
            else if (e.Key.ToString() == "Escape")
            {
                textBoxConcepto.FocusTextBox();
            }
        }

        private void textBoxAlias_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key.ToString() == "Return")
            {
                textBoxConcepto.FocusTextBox();
            }
        }

        private void textBoxConcepto_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //System.Console.WriteLine(e.Key.ToString());
            if (e.Key.ToString() == "Down")
            {
                e.Handled = true;
                textBoxConcepto.ChangeComboBoxIndexDown();
            }
            else if (e.Key.ToString() == "Up")
            {
                e.Handled = true;
                textBoxConcepto.ChangeComboBoxIndexUp();
            }
            else if (e.Key.ToString() == "Back")
            {
                if (textBoxConcepto.SuggestionListActive() == true && textBoxConcepto.FindComboBoxIndex() != -1)
                {
                    textBoxConcepto.FocusTextBox();
                }
            }
            else if (e.Key.ToString() == "Return")
            {
                //textBoxConcepto.FocusTextBox();
                e.Handled = true;
                buttonImprimir.Focus();
            }
        }

        #endregion

        private void checkBoxImprimir_Checked(object sender, RoutedEventArgs e)
        {
            buttonImprimir.Content = "Imprimir";
        }

        private void checkBoxImprimir_Unchecked(object sender, RoutedEventArgs e)
        {
            buttonImprimir.Content = "Guardar";
        }

    }
}
