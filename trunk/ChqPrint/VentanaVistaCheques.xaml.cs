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

        private void dataGrid1_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            if (((Cheques)e.Row.Item).Estado == null)
            {
                ((Cheques)e.Row.Item).Estado = "Pendiente";
            }
            buttonGuardar.IsEnabled = true;
        }

        #region "Funciones relativas a los Botones Externos"

        private void buttonExportar_Click(object sender, RoutedEventArgs e)
        {
            ExportToExcelTools.DataGridExcelTools.SetFormatForExport(dataGridCheques.Columns[2], "dd.MM.yyyy");
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
