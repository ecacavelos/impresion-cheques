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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChqPrint
{
    public static class Impresion
    {

        public static bool ImprimirCheque(DateTime fecha, int monto, string beneficiario)
        {
            System.Console.WriteLine(VentanaPrincipal.layoutFilename);
            Configuration c2 = Configuration.Deserialize(VentanaPrincipal.layoutFilename);

            System.Console.WriteLine(c2.ChequeID);
            // Creamos la clase pariente 'FixedDocument'.
            FixedDocument fixedDoc = new FixedDocument();

            // Creamos la pagina.
            FixedPage page1 = new FixedPage();
            page1.Width = fixedDoc.DocumentPaginator.PageSize.Width;
            page1.Height = fixedDoc.DocumentPaginator.PageSize.Height;

            // Agregamos los elementos del Cheque.

            // --- Fecha ---
            TextBlock chqFechaDia = new TextBlock();
            chqFechaDia.Text = fecha.Day.ToString();
            page1.Children.Add(chqFechaDia);
            FixedPage.SetLeft(chqFechaDia, c2.CoordenadasImpresion.xFechaDia);
            FixedPage.SetTop(chqFechaDia, c2.CoordenadasImpresion.yFecha);

            TextBlock chqFechaMes = new TextBlock();
            chqFechaMes.Text = IntToMes(fecha.Month);
            page1.Children.Add(chqFechaMes);
            FixedPage.SetLeft(chqFechaMes, c2.CoordenadasImpresion.xFechaMes);
            FixedPage.SetTop(chqFechaMes, c2.CoordenadasImpresion.yFecha);

            TextBlock chqFechaAño = new TextBlock();
            chqFechaAño.Text = fecha.Year.ToString();
            page1.Children.Add(chqFechaAño);
            FixedPage.SetLeft(chqFechaAño, c2.CoordenadasImpresion.xFechaAño);
            FixedPage.SetTop(chqFechaAño, c2.CoordenadasImpresion.yFecha);

            // --- Monto ---
            TextBlock chqMonto = new TextBlock();
            chqMonto.Text = monto.ToString();
            page1.Children.Add(chqMonto);
            FixedPage.SetLeft(chqMonto, c2.CoordenadasImpresion.xMonto);
            FixedPage.SetTop(chqMonto, c2.CoordenadasImpresion.yMonto);

            // --- Páguese a la Orden De ---
            TextBlock chqPagueseOrdenDe = new TextBlock();
            chqPagueseOrdenDe.Text = beneficiario;
            page1.Children.Add(chqPagueseOrdenDe);
            FixedPage.SetLeft(chqPagueseOrdenDe, c2.CoordenadasImpresion.xPagueseOrdenDe);
            FixedPage.SetTop(chqPagueseOrdenDe, c2.CoordenadasImpresion.yPagueseOrdenDe);

            // --- Monto en Letras ---
            TextBlock chqMontoEnLetras = new TextBlock();
            chqMontoEnLetras.Text = Numalet.ToCardinal((int)monto).ToUpper();
            page1.Children.Add(chqMontoEnLetras);
            FixedPage.SetLeft(chqMontoEnLetras, c2.CoordenadasImpresion.xMontoEnLetras);
            FixedPage.SetTop(chqMontoEnLetras, c2.CoordenadasImpresion.yMontoEnLetras);

            // Agregamos la Pagina al Documento.
            PageContent page1Content = new PageContent();
            ((System.Windows.Markup.IAddChild)page1Content).AddChild(page1);
            fixedDoc.Pages.Add(page1Content);

            DocumentPaginator aDocPage = ((IDocumentPaginatorSource)fixedDoc).DocumentPaginator;
            PrintDialog pDialog = new PrintDialog();

            // Display the dialog. This returns true if the user presses the Print button.
            Nullable<Boolean> print = pDialog.ShowDialog();
            if (print == true)
            {
                fixedDoc.DocumentPaginator.PageSize = new Size(pDialog.PrintableAreaWidth, pDialog.PrintableAreaHeight);
                try
                {
                    pDialog.PrintDocument(aDocPage, "ChqPrint - Cheque");
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Función que convierte un int al mes correspondiente en formato cadena.
        /// </summary>
        /// <param name="mymes">El entero que correspondiente al mes.</param>
        /// <returns></returns>
        public static string IntToMes(int mymes)
        {
            switch (mymes)
            {
                case 1:
                    return "Enero";
                case 2:
                    return "Febrero";
                case 3:
                    return "Marzo";
                case 4:
                    return "Abril";
                case 5:
                    return "Mayo";
                case 6:
                    return "Junio";
                case 7:
                    return "Julio";
                case 8:
                    return "Agosto";
                case 9:
                    return "Septiembre";
                case 10:
                    return "Octubre";
                case 11:
                    return "Noviembre";
                case 12:
                    return "Diciembre";
                default:
                    return "Undefined";
            }
        }

    }
}
