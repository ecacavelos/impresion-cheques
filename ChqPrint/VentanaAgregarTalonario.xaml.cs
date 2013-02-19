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
        //private ConfigurationGeneral c2;
        public static bool IsOpen { get; private set; }

        ChqPrint.ChqDatabase2Entities database1Entities = new ChqPrint.ChqDatabase2Entities();

        #region "Funciones relativas a la Inicializacion, Carga y Descarga de la Ventana"

        public VentanaAgregarTalonario()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IsOpen = true;
            // Leemos los datos del formulario actual.
            //this.c2 = ConfigurationGeneral.Deserialize(VentanaPrincipal.layoutFilename);
            labelTalonarioActual.Content += database1Entities.Talonarios.ToArray()[0].Nombre;

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

        #endregion

        #region "Funciones relativas a los Botones Externos"

        private void buttonGuardar_Click(object sender, RoutedEventArgs e)
        {
            database1Entities.Talonarios.ToArray()[0].Nombre = textBoxNuevoTalonario.Text;

            int tempIntToString;
            Int32.TryParse(textBoxPrimerCheque.Text, out tempIntToString);
            database1Entities.Talonarios.ToArray()[0].PrimerCheque = tempIntToString;
            Int32.TryParse(textBoxUltimoCheque.Text, out tempIntToString);
            if (tempIntToString > database1Entities.Talonarios.ToArray()[0].PrimerCheque)
            {
                database1Entities.Talonarios.ToArray()[0].UltimoCheque = tempIntToString;
            }
            else
            {
                MessageBox.Show("El valor de 'Último Cheque' debe ser mayor al valor de 'Primer Cheque'.");
                return;
            }

            // Si se seleccionó previamente un archivo válido, se guarda su ubicación.
            string esql = String.Format("SELECT value f FROM Formatos as f WHERE f.Descripcion = '{0}'", ((ComboBoxItem)comboBoxFormatoCheque.SelectedItem).Content.ToString());
            var formatosVar = database1Entities.CreateQuery<Formatos>(esql);

            //System.Console.WriteLine(esql);

            if (formatosVar.Count() == 1)
            {
                //VentanaPrincipal.layoutFilename = formatosVar.First().Path; // Esta asignacion me parece que no es correcta. no se tiene que tocar esta variable aca. Esteban cacavelos
                VentanaPrincipal.labelTipoChequeHomeScreen.Content = ((ComboBoxItem)comboBoxFormatoCheque.SelectedItem).Content.ToString();
                database1Entities.Talonarios.ToArray()[0].FormatoChequeTalonario = formatosVar.ToArray()[0].Path;
            }

            //ConfigurationGeneral.Serialize("standard.xml", this.c2);
            database1Entities.SaveChanges();

            this.Close();
        }

        private void buttonCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #endregion

    }
}
