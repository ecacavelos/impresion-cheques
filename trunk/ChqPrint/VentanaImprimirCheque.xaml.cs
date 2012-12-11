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
                textBoxNumeroCheque.Text = lastNroCheque.ToString("0000000");
            }
            else
            {
                textBoxNumeroCheque.Text = "0000000";
            }

            textBoxMonto.Focus();

        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            IsOpen = false;
        }

        #endregion

        private void buttonImprimir_Click(object sender, RoutedEventArgs e)
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
            tempCheque.nroCheque = textBoxNumeroCheque.Text;
            tempCheque.Fecha = datePickerFecha.SelectedDate;
            tempCheque.Monto = tempMonto;
            tempCheque.PagueseOrdenDe = textBoxPaguese.Text;
            tempCheque.MontoEnLetras = Numalet.ToCardinal((int)(tempCheque.Monto)).ToUpper();
            tempCheque.Anulado = false;

            if (Impresion.ImprimirCheque((DateTime)(tempCheque.Fecha), (int)tempCheque.Monto, tempCheque.PagueseOrdenDe))
            {
                System.Windows.MessageBox.Show("Se imprimió el Cheque.", "Impresión");
                database1Entities.Cheques.AddObject(tempCheque);
                database1Entities.SaveChanges();
                this.Close();
            }

        }

    }
}
