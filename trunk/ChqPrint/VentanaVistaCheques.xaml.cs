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
            database1Entities.SaveChanges();
        }

        #endregion

        private System.Data.Objects.ObjectQuery<Cheques> GetChequesQuery(ChqDatabase1Entities chqDatabase1Entities)
        {
            // Auto generated code
            System.Data.Objects.ObjectQuery<ChqPrint.Cheques> chequesQuery = chqDatabase1Entities.Cheques;
            // Returns an ObjectQuery.
            return chequesQuery;
        }

    }
}
