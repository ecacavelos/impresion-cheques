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
        public static bool ImprimirCheque()
        {
            System.Console.WriteLine(VentanaPrincipal.layoutFilename);
            // Creamos la clase pariente 'FixedDocument'.
            FixedDocument fixedDoc = new FixedDocument();

            // Creamos la pagina.
            FixedPage page1 = new FixedPage();
            page1.Width = fixedDoc.DocumentPaginator.PageSize.Width;
            page1.Height = fixedDoc.DocumentPaginator.PageSize.Height;

            // Agregamos los elementos del Cheque.
            TextBlock holaMundo = new TextBlock();
            holaMundo.Text = "Hola Mundo";
            page1.Children.Add(holaMundo);
            FixedPage.SetLeft(holaMundo, 10);
            FixedPage.SetTop(holaMundo, 10);

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
    }
}
