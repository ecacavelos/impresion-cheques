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
    /// Interaction logic for VentanaAgregarFormatoCheque.xaml
    /// </summary>
    public partial class VentanaAgregarFormatoCheque : Window
    {
        public static bool IsOpen { get; private set; }

        ChqPrint.ChqDatabase2Entities chqDatabase1Entities = new ChqPrint.ChqDatabase2Entities();

        private System.Data.Objects.ObjectQuery<Formatos> GetFormatosQuery(ChqDatabase2Entities chqDatabase1Entities)
        {
            // Auto generated code

            System.Data.Objects.ObjectQuery<ChqPrint.Formatos> formatosQuery = chqDatabase1Entities.Formatos;
            // Returns an ObjectQuery.
            return formatosQuery;
        }

        #region "Funciones relativas a la Inicializacion, Carga y Descarga de la Ventana"

        public VentanaAgregarFormatoCheque()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IsOpen = true;
            // Load data into Formatos. You can modify this code as needed.
            System.Windows.Data.CollectionViewSource formatosViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("formatosViewSource")));
            System.Data.Objects.ObjectQuery<ChqPrint.Formatos> formatosQuery = this.GetFormatosQuery(chqDatabase1Entities);
            formatosViewSource.Source = formatosQuery.Execute(System.Data.Objects.MergeOption.AppendOnly);
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e)
        {
            IsOpen = false;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                chqDatabase1Entities.SaveChanges();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex.InnerException.GetType());
                System.Console.WriteLine(ex.InnerException.Message);
            }
        }

        #endregion

        #region "Funciones relativas a los Botones Externos"

        private void buttonAgregar_Click(object sender, RoutedEventArgs e)
        {
            Formatos newFormato = new Formatos();
            Formatos lastFormato = (Formatos)(dataGridFormatos.Items[dataGridFormatos.Items.Count - 1]);

            DialogNewFormato dlg = new DialogNewFormato();
            Nullable<bool> result = dlg.ShowDialog();

            if (result == true)
            {
                newFormato.idFormato = lastFormato.idFormato + 1;
                newFormato.Descripcion = dlg.ResponseText_Descripcion;
                newFormato.Path = dlg.ResponseText_Path;
                // Agregamos el nuevo Formato a la Tabla correspondiente.
                chqDatabase1Entities.Formatos.AddObject(newFormato);
                chqDatabase1Entities.SaveChanges();
                // Refrescamos el origen de los datos mostrados en el DataGrid, para mostrar el nuevo Formato.
                System.Windows.Data.CollectionViewSource formatosViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("formatosViewSource")));
                System.Data.Objects.ObjectQuery<ChqPrint.Formatos> formatosQuery = this.GetFormatosQuery(chqDatabase1Entities);
                formatosViewSource.Source = formatosQuery.Execute(System.Data.Objects.MergeOption.AppendOnly);
            }
        }

        private void buttonSalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #endregion

    }
}
