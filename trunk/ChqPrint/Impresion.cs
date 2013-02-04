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

        public static bool ImprimirCheque(DateTime fecha, long monto, ChqPrint.Clientes beneficiario, string tempConcepto)
        {
            System.Console.WriteLine(VentanaPrincipal.layoutFilename);
            ConfigurationGeneral c0 = ConfigurationGeneral.Deserialize(VentanaPrincipal.layoutFilename);
            ConfigurationLayoutCheque c2 = ConfigurationLayoutCheque.Deserialize(c0.FormatoChequeTalonario);

            bool montoBlanco = false;
            if (monto == 0)
            {
                montoBlanco = true;
            }

            System.Console.WriteLine(c2.ChequeID);
            // Creamos la clase pariente 'FixedDocument'.
            FixedDocument fixedDoc = new FixedDocument();

            // Creamos la pagina.
            FixedPage page1 = new FixedPage();
            page1.Width = fixedDoc.DocumentPaginator.PageSize.Width;
            page1.Height = fixedDoc.DocumentPaginator.PageSize.Height;
            //page1.Height = 300;

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
            if (montoBlanco == false)
            {
                TextBlock chqMonto = new TextBlock();
                chqMonto.Text = monto.ToString("#,##0");
                page1.Children.Add(chqMonto);
                FixedPage.SetLeft(chqMonto, c2.CoordenadasImpresion.xMonto);
                FixedPage.SetTop(chqMonto, c2.CoordenadasImpresion.yMonto);
            }

            // --- Páguese a la Orden De ---
            TextBlock chqPagueseOrdenDe = new TextBlock();
            chqPagueseOrdenDe.Text = beneficiario.Nombre;
            page1.Children.Add(chqPagueseOrdenDe);
            FixedPage.SetLeft(chqPagueseOrdenDe, c2.CoordenadasImpresion.xPagueseOrdenDe);
            FixedPage.SetTop(chqPagueseOrdenDe, c2.CoordenadasImpresion.yPagueseOrdenDe);

            // --- Monto en Letras ---
            if (montoBlanco == false)
            {
                TextBlock chqMontoEnLetras = new TextBlock();
                string firstLineIndentation = "";
                for (int i = 0; i < 36; i++)
                {
                    firstLineIndentation += " ";
                }
                chqMontoEnLetras.Text = firstLineIndentation + Numalet.ToCardinal((long)monto).ToUpper();
                chqMontoEnLetras.Width = 580;
                chqMontoEnLetras.TextWrapping = TextWrapping.Wrap;
                chqMontoEnLetras.LineHeight = 30;

                page1.Children.Add(chqMontoEnLetras);
                FixedPage.SetLeft(chqMontoEnLetras, c2.CoordenadasImpresion.xMontoEnLetras - 118);
                FixedPage.SetTop(chqMontoEnLetras, c2.CoordenadasImpresion.yMontoEnLetras);
            }

            // --- Fecha Abreviada en el Talon ---
            TextBlock chqTalonFecha = new TextBlock();
            chqTalonFecha.Text = string.Format("{0}/{1}/{2}", fecha.Day.ToString(), fecha.Month.ToString(), fecha.Year.ToString());
            page1.Children.Add(chqTalonFecha);
            FixedPage.SetLeft(chqTalonFecha, c2.CoordenadasImpresion.xTalonFecha);
            FixedPage.SetTop(chqTalonFecha, c2.CoordenadasImpresion.yTalonFecha);

            // --- Orden Abreviada en el Talon ---
            TextBlock chqTalonAlias = new TextBlock();
            chqTalonAlias.Text = beneficiario.Alias;
            chqTalonAlias.Width = 90;
            page1.Children.Add(chqTalonAlias);
            FixedPage.SetLeft(chqTalonAlias, c2.CoordenadasImpresion.xTalonAlias);
            FixedPage.SetTop(chqTalonAlias, c2.CoordenadasImpresion.yTalonAlias);

            // --- Concepto Abreviado en el Talon ---
            TextBlock chqTalonConcepto = new TextBlock();
            chqTalonConcepto.Text = tempConcepto;
            chqTalonConcepto.Width = 90;
            chqTalonConcepto.TextWrapping = TextWrapping.Wrap;
            chqTalonConcepto.LineHeight = 30;
            page1.Children.Add(chqTalonConcepto);
            FixedPage.SetLeft(chqTalonConcepto, c2.CoordenadasImpresion.xTalonConcepto);
            FixedPage.SetTop(chqTalonConcepto, c2.CoordenadasImpresion.yTalonConcepto);

            // --- Monto Abreviado en el Talon ---
            if (montoBlanco == false)
            {
                TextBlock chqTalonMonto = new TextBlock();
                chqTalonMonto.Text = monto.ToString("#,##0");
                page1.Children.Add(chqTalonMonto);
                FixedPage.SetLeft(chqTalonMonto, c2.CoordenadasImpresion.xTalonMonto);
                FixedPage.SetTop(chqTalonMonto, c2.CoordenadasImpresion.yTalonMonto);
            }

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
