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
    /// Interaction logic for VentanaAgregarTalonario.xaml
    /// </summary>
    public partial class VentanaAgregarTalonario : Window
    {
        private ConfigurationGeneral c2;
        public static bool IsOpen { get; private set; }

        ChqPrint.ChqDatabase1Entities database1Entities = new ChqPrint.ChqDatabase1Entities();

        #region "Funciones relativas a la Inicializacion, Carga y Descarga de la Ventana"

        public VentanaAgregarTalonario()
        {
            InitializeComponent();
        }

        #endregion

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IsOpen = true;
            // Leemos los datos del formulario actual.
            this.c2 = ConfigurationGeneral.Deserialize(VentanaPrincipal.layoutFilename);
            labelTalonarioActual.Content += c2.Talonario;

            // Cargamos los Cheques en el comboBox            
            int defaultIndex = 0;
            int i = 0;

            string esql = String.Format("SELECT value f FROM Formatos as f");
            var formatosVar = database1Entities.CreateQuery<Formatos>(esql);
            foreach (Formatos tempFormato in formatosVar)
            {
                ComboBoxItem elementoCombo = new ComboBoxItem();
                elementoCombo.Content = tempFormato.Descripcion;
                comboBoxFormatoCheque.Items.Add(elementoCombo);
                if (tempFormato.Path == VentanaPrincipal.layoutFilename)
                {
                    defaultIndex = i;
                }
                i++;
            }

            if (comboBoxFormatoCheque.HasItems)
            {
                comboBoxFormatoCheque.SelectedIndex = defaultIndex;
            }

            textBoxNuevoTalonario.Focus();
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            IsOpen = false;
        }

        #region "Funciones relativas a los Botones Externos"

        private void buttonGuardar_Click(object sender, RoutedEventArgs e)
        {
            this.c2.Talonario = textBoxNuevoTalonario.Text;

            int tempIntToString;
            Int32.TryParse(textBoxPrimerCheque.Text, out tempIntToString);
            this.c2.PrimerCheque = tempIntToString;
            Int32.TryParse(textBoxUltimoCheque.Text, out tempIntToString);
            this.c2.UltimoCheque = tempIntToString;

            // Si se seleccionó previamente un archivo válido, se guarda su ubicación.
            string esql = String.Format("SELECT value f FROM Formatos as f WHERE f.Descripcion = '{0}'", ((ComboBoxItem)comboBoxFormatoCheque.SelectedItem).Content.ToString());
            var formatosVar = database1Entities.CreateQuery<Formatos>(esql);

            System.Console.WriteLine(esql);

            if (formatosVar.Count() == 1)
            {
                VentanaPrincipal.layoutFilename = formatosVar.First().Path;
                VentanaPrincipal.labelTipoChequeHomeScreen.Content = ((ComboBoxItem)comboBoxFormatoCheque.SelectedItem).Content.ToString();
                this.c2.FormatoChequeTalonario = formatosVar.ToArray()[0].Path;
            }

            ConfigurationGeneral.Serialize("standard.xml", this.c2);

            this.Close();
        }

        private void buttonCancelar_Click(object sender, RoutedEventArgs e)
        {

        }

        #endregion

    }
}
