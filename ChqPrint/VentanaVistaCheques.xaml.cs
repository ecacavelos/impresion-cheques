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

using System.Globalization;
using ExportToExcelTools;

namespace ChqPrint
{
    /// <summary>
    /// Interaction logic for VentanaVistaCheques.xaml
    /// </summary>
    public partial class VentanaVistaCheques : Window
    {
        //private Configuration c2;
        public static bool IsOpen { get; private set; }

        ChqPrint.ChqDatabase1Entities database1Entities = new ChqPrint.ChqDatabase1Entities();

        int timestampDesde;
        int timestampHasta;
        //string[] arrayClientesID;

        #region "Funciones relativas a la Inicializacion, Carga y Descarga de la Ventana"

        public VentanaVistaCheques()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IsOpen = true;
            // Load data into Cheques. You can modify this code as needed.
            System.Windows.Data.CollectionViewSource chequesViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("chequesViewSource")));
            System.Data.Objects.ObjectQuery<ChqPrint.Cheques> chequesQuery = this.GetChequesQuery(database1Entities);
            chequesViewSource.Source = chequesQuery.Execute(System.Data.Objects.MergeOption.AppendOnly);

            eliminarFiltros(true);
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            IsOpen = false;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            MessageBoxResult result;

            if (buttonGuardar.IsEnabled == true)
            {
                result = MessageBox.Show("Desea guardar los cambios efectuados?", "Confirmar modificaciones", MessageBoxButton.YesNoCancel);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        database1Entities.SaveChanges();
                    }
                    catch (System.Data.UpdateException ex)
                    {
                        System.Console.WriteLine(ex.InnerException.GetType());
                        System.Console.WriteLine(ex.InnerException.Message);
                    }
                    //label1.Content = "Se guardaron los cambios.";                
                }
                else if (result == MessageBoxResult.No)
                {
                    //label1.Content = "NO se guardaron los cambios.";
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }

        #endregion

        private System.Data.Objects.ObjectQuery<Cheques> GetChequesQuery(ChqDatabase1Entities chqDatabase1Entities)
        {
            // Auto generated code
            System.Data.Objects.ObjectQuery<ChqPrint.Cheques> chequesQuery = chqDatabase1Entities.Cheques;
            // Returns an ObjectQuery.
            return chequesQuery;
        }

        #region "Funciones relativas a los Botones Externos"

        private void buttonExportar_Click(object sender, RoutedEventArgs e)
        {
            ExportToExcelTools.DataGridExcelTools.SetFormatForExport(dataGridCheques.Columns[3], "dd.MM.yyyy");
            dataGridCheques.ExportToExcel();
        }

        private void buttonGuardar_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Desea guardar los cambios efectuados?", "Confirmar modificaciones", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    database1Entities.SaveChanges();
                }
                catch (System.Data.UpdateException ex)
                {
                    System.Console.WriteLine(ex.InnerException.GetType());
                    System.Console.WriteLine(ex.InnerException.Message);
                }
                buttonGuardar.IsEnabled = false;
                labelStatusBar.Content = "Se guardaron los cambios.";
            }
            else if (result == MessageBoxResult.No)
            {
                labelStatusBar.Content = "NO se guardaron los cambios.";
            }
        }

        private void buttonSalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #endregion

        #region "Funciones varias relativas a la edición y guardado"

        private void dataGridCheques_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            // Si el Cheque está en estado 'Cobrado' o 'Anulado', no se permiten modificaciones en el mismo.
            if ((((Cheques)e.Row.Item).Estado == "Cobrado") || (((Cheques)e.Row.Item).Estado == "Anulado"))
            {
                e.Cancel = true;
            }
        }

        private void dataGrid1_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (((Cheques)e.Row.Item).Estado == null)
            {
                ((Cheques)e.Row.Item).Estado = "Pendiente";
            }
            buttonGuardar.IsEnabled = true;
        }

        // Se habilita la opción de Guardar cuando se cambia la opción en el ComboBox.
        private void comboBoxEstadoCheque_DropDownClosed(object sender, EventArgs e)
        {
            buttonGuardar.IsEnabled = true;
        }

        #endregion

        #region "Funciones relativas al filtrado y la busqueda"

        private void checkBoxDesde_Checked(object sender, RoutedEventArgs e)
        {
            datePickerDesde.IsEnabled = true;
            buttonBuscar.IsEnabled = true;
        }

        private void checkBoxDesde_Unchecked(object sender, RoutedEventArgs e)
        {
            datePickerDesde.IsEnabled = false;
            if (checkBoxHasta.IsChecked == false && checkBoxOrdenDe.IsChecked == false)
            {
                buttonBuscar.IsEnabled = false;
                eliminarFiltros(false);
            }
            else
            {
                aplicarFiltros();
            }
        }

        private void checkBoxHasta_Checked(object sender, RoutedEventArgs e)
        {
            datePickerHasta.IsEnabled = true;
            buttonBuscar.IsEnabled = true;
        }

        private void checkBoxHasta_Unchecked(object sender, RoutedEventArgs e)
        {
            datePickerHasta.IsEnabled = false;
            if (checkBoxDesde.IsChecked == false && checkBoxOrdenDe.IsChecked == false)
            {
                buttonBuscar.IsEnabled = false;
                eliminarFiltros(false);
            }
            else
            {
                aplicarFiltros();
            }
        }

        private void checkBoxOrdenDe_Checked(object sender, RoutedEventArgs e)
        {
            autoCompleteTextBoxOrdenDe.IsEnabled = true;
            buttonBuscar.IsEnabled = true;
            autoCompleteTextBoxOrdenDe.FocusTextBox();
        }

        private void checkBoxOrdenDe_Unchecked(object sender, RoutedEventArgs e)
        {
            autoCompleteTextBoxOrdenDe.IsEnabled = false;
            autoCompleteTextBoxOrdenDe.Text = "";
            if (checkBoxDesde.IsChecked == false && checkBoxHasta.IsChecked == false)
            {
                buttonBuscar.IsEnabled = false;
                eliminarFiltros(false);
            }
            else
            {
                aplicarFiltros();
            }
        }

        private void datePickerDesde_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            TimeSpan epochTime = ((DateTime)datePickerDesde.SelectedDate - new DateTime(1970, 1, 1));
            timestampDesde = (int)epochTime.TotalSeconds;
        }

        private void datePickerHasta_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            TimeSpan epochTime = ((DateTime)datePickerHasta.SelectedDate + new TimeSpan(23, 0, 0) - new DateTime(1970, 1, 1));
            timestampHasta = (int)epochTime.TotalSeconds;
        }

        private void autoCompleteTextBoxOrdenDe_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            //System.Console.WriteLine(e.Key.ToString());
            if (e.Key.ToString() == "Down")
            {
                e.Handled = true;
                autoCompleteTextBoxOrdenDe.ChangeComboBoxIndexDown();
            }
            else if (e.Key.ToString() == "Up")
            {
                e.Handled = true;
                autoCompleteTextBoxOrdenDe.ChangeComboBoxIndexUp();
            }
            else if (e.Key.ToString() == "Back")
            {
                if (autoCompleteTextBoxOrdenDe.SuggestionListActive() == true && autoCompleteTextBoxOrdenDe.FindComboBoxIndex() != -1)
                {
                    autoCompleteTextBoxOrdenDe.FocusTextBox();
                }
            }
            else if (e.Key.ToString() == "Return")
            {
                autoCompleteTextBoxOrdenDe.FocusTextBox();
            }
        }

        private void buttonBuscar_Click(object sender, RoutedEventArgs e)
        {
            aplicarFiltros();
        }

        private void aplicarFiltros()
        {
            string esql = "SELECT value c FROM Cheques as c";

            if (autoCompleteTextBoxOrdenDe.IsEnabled == true)
            {
                esql += " WHERE ";
                esql += String.Format("(c.PagueseOrdenDe LIKE '%{0}%')", autoCompleteTextBoxOrdenDe.Text);
            }

            if (datePickerDesde.IsEnabled == true)
            {
                if (datePickerDesde.SelectedDate != null)
                {
                    if (esql.Contains("WHERE"))
                    {
                        esql += " AND ";
                    }
                    else
                    {
                        esql += " WHERE ";
                    }
                    esql += String.Format("(c.idCheque >= {0})", timestampDesde);
                }
            }

            if (datePickerHasta.IsEnabled == true)
            {
                if (datePickerHasta.SelectedDate != null)
                {
                    if (esql.Contains("WHERE"))
                    {
                        esql += " AND ";
                    }
                    else
                    {
                        esql += " WHERE ";
                    }
                    esql += String.Format("(c.idCheque <= {0})", timestampHasta);
                }
            }

            System.Console.WriteLine(esql);

            var chequesVar = database1Entities.CreateQuery<Cheques>(esql);
            dataGridCheques.ItemsSource = chequesVar;
            if (chequesVar.ToList().Count > 0)
            {
                labelStatusBar.Content = "Búsqueda Completa.";
            }
            else
            {
                labelStatusBar.Content = "La búsqueda no arrojó resultados.";
            }

            buttonEliminarFiltros.IsEnabled = true;

        }

        private void eliminarFiltros(bool firstTime)
        {
            string esql = "SELECT value c FROM Cheques as c";
            var pagosVar = database1Entities.CreateQuery<Cheques>(esql);
            dataGridCheques.ItemsSource = pagosVar;

            buttonEliminarFiltros.IsEnabled = false;

            if (firstTime)
            {
                string esql_clientes = "SELECT value c FROM Clientes as c";
                var clientesVar = database1Entities.CreateQuery<Clientes>(esql_clientes);

                //System.Console.WriteLine(clientesVar.ToList().Count.ToString() + " clientes.");

                int i = 0;
                foreach (ChqPrint.Clientes tempCliente in clientesVar.ToArray())
                {
                    autoCompleteTextBoxOrdenDe.AddItem(new WPFAutoCompleteTextbox.AutoCompleteEntry(tempCliente.Nombre, tempCliente.Nombre));
                    i++;
                }
            }
        }

        private void textBoxTimbrado_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textBoxTimbrado.Text.Length > 0)
            {
                buttonBuscarTimbrado.IsEnabled = true;
            }
            else
            {
                buttonBuscarTimbrado.IsEnabled = false;
            }
        }

        private void buttonBuscarTimbrado_Click(object sender, RoutedEventArgs e)
        {
            string esql = "SELECT value c FROM Cheques as c";
            esql += " WHERE ";
            esql += String.Format("(c.nroCheque = '{0}')", textBoxTimbrado.Text);

            System.Console.WriteLine(esql);

            var chequesVar = database1Entities.CreateQuery<Cheques>(esql);
            dataGridCheques.ItemsSource = chequesVar;

            if (chequesVar.ToList().Count > 0)
            {
                labelStatusBar.Content = "Búsqueda Completa.";
            }
            else
            {
                labelStatusBar.Content = "La búsqueda no arrojó resultados.";
            }

            buttonEliminarFiltros.IsEnabled = true;

        }

        private void buttonEliminarFiltros_Click(object sender, RoutedEventArgs e)
        {
            // Colocamos todos los controles de búsqueda en sus estados iniciales.
            checkBoxDesde.IsChecked = false;
            datePickerDesde.IsEnabled = false;
            checkBoxHasta.IsChecked = false;
            datePickerHasta.IsEnabled = false;
            checkBoxOrdenDe.IsChecked = false;
            autoCompleteTextBoxOrdenDe.IsEnabled = false;
            autoCompleteTextBoxOrdenDe.Text = "";
            buttonBuscar.IsEnabled = false;

            textBoxTimbrado.Text = "";
            buttonBuscarTimbrado.IsEnabled = false;

            eliminarFiltros(false);
        }

        #endregion

    }

    #region "Conversores y Recursos para los data bindings de esta ventana."

    [ValueConversion(typeof(int), typeof(string))]
    public class IntToNumericStringConverter : IValueConverter
    {
        #region IValueConverter Members
        public object Convert(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            //string myFormattedNumericString = value.ToString();
            string myFormattedNumericString = ((int)value).ToString("#,##0");
            return myFormattedNumericString;
        }
        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }
        #endregion
    }

    public enum enumEstadoCheque { Pendiente, Emitido, Cobrado, Rechazado, Anulado };

    #endregion

}
