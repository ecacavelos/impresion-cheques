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
    /// Interaction logic for VentanaImprimirCheque.xaml
    /// </summary>
    public partial class VentanaImprimirCheque : Window
    {
        private ConfigurationGeneral c0;
        private ConfigurationLayoutCheque c2;
        public static bool IsOpen { get; private set; }

        ChqPrint.ChqDatabase1Entities database1Entities = new ChqPrint.ChqDatabase1Entities();

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
            this.c2 = ConfigurationLayoutCheque.Deserialize(this.c0.FormatoChequeTalonario);

            datePickerFecha.SelectedDate = DateTime.Now;    // La fecha del cheque.
            // Hacemos un query a la base de datos para obtener todas los cheques.
            string esql = String.Format("SELECT value c FROM Cheques as c WHERE c.Talonario = '{0}'", this.c0.Talonario);
            var chequesVar = database1Entities.CreateQuery<Cheques>(esql);

            textBlockTalonario.Text = this.c0.Talonario;
            // Si existe al menos un cheque.
            int lastNroCheque = 0;
            if (chequesVar.ToList().Count > 0)
            {
                // Pasamos el string "nroCheque" obtenido de la ultima entrada de la tabla Cheques a int
                Int32.TryParse(chequesVar.ToList().ElementAt(chequesVar.ToList().Count - 1).nroCheque, out lastNroCheque);
                // Incrementamos en 1 el último Cheque y desplegamos el valor correspondiente en el textBlock
                lastNroCheque++;
                textBlockNumeroCheque.Text = lastNroCheque.ToString("000000");
            }
            else
            {
                textBlockNumeroCheque.Text = this.c0.PrimerCheque.ToString();
            }

            textBoxMonto.Focus();

            // Agregamos la lista de sugerencia de Clientes al AutoCompleteTextBox 'Paguese a la Orden de'.
            string esql_clientes = "SELECT value c FROM Clientes as c";
            var clientesVar = database1Entities.CreateQuery<Clientes>(esql_clientes);

            foreach (Clientes tempCliente in clientesVar.ToArray())
            {
                textBoxPaguese.AddItem(new WPFAutoCompleteTextbox.AutoCompleteEntry(tempCliente.Nombre, tempCliente.Nombre));
            }

            textBlockFormatoCheque.Text = c2.ChequeID;

        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            IsOpen = false;
        }

        #endregion

        private void buttonImprimir_Click(object sender, RoutedEventArgs e)
        {

            int montoValidado = 0;

            // Validamos el campo de "Monto"
            if (textBoxMonto.Text.Length > 0)
            {
                try
                {
                    montoValidado = Convert.ToInt32(textBoxMonto.Text);
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine(ex.Message);
                    MessageBox.Show("Por favor introduzca un Monto válido (Sólo números).");
                }
            }

            // Verificamos si el cliente ingresado ya existe en la Base de Datos.

            string esql_clientes = String.Format("SELECT value c FROM Clientes as c WHERE c.Nombre == '{0}'", textBoxPaguese.Text);
            var clientesVar = database1Entities.CreateQuery<Clientes>(esql_clientes);

            if (clientesVar.ToList().Count == 0)
            {
                if (c0.PermitirEscrituraManual == false)
                {
                    MessageBox.Show("Debe elegir un cliente ya existente en la Base de Datos.");
                    return;
                }
                else
                {
                    Clientes newCliente = new Clientes();
                    if (newCliente.idCliente == 0)                               // Si el ID no existe.
                    {
                        var allClientesVar = database1Entities.CreateQuery<Clientes>("SELECT value c FROM Clientes as c");
                        int newID = allClientesVar.ToList().Count + 1;
                        newCliente.idCliente = newID;                        // Nuevo ID = timestamp.
                    }
                    newCliente.Nombre = textBoxPaguese.Text;
                    database1Entities.Clientes.AddObject(newCliente);
                }
            }

            // Hacemos un query a la Base de Datos para obtener todos los Cheques.
            string esql = String.Format("SELECT value c FROM Cheques as c WHERE (c.nroCheque = '{0}' AND c.Talonario = '{1}')", textBlockNumeroCheque.Text, textBlockTalonario.Text);
            var chequesVar = database1Entities.CreateQuery<Cheques>(esql);

            // Si ya no existe un Cheque con ese Talonario y Número de Cheque.
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
                tempCheque.Talonario = textBlockTalonario.Text;
                tempCheque.nroCheque = textBlockNumeroCheque.Text;
                //tempCheque.Banco = ((ComboBoxItem)comboBoxTipoCheque.SelectedItem).Content.ToString();
                tempCheque.Banco = textBlockFormatoCheque.Text;
                tempCheque.Fecha = datePickerFecha.SelectedDate;
                tempCheque.Monto = tempMonto;
                tempCheque.PagueseOrdenDe = textBoxPaguese.Text;
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

        private void textBoxMonto_TextChanged(object sender, TextChangedEventArgs e)
        {
            int tempMonto = 0;


            bool result = Int32.TryParse(((string)textBoxMonto.Text), System.Globalization.NumberStyles.Number, new System.Globalization.CultureInfo("es-ES"), out tempMonto);
            if (result)
            {
                textBoxMonto.Text = tempMonto.ToString("#,##0");
                textBoxMonto.CaretIndex = textBoxMonto.Text.Length;
            }
        }

    }
}
