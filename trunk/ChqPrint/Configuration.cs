using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace ChqPrint
{
    [Serializable]
    public class Configuration
    {
        // Datos Relativos a las Coordenadas de Impresión.
        public struct ImpresionCoords
        {
            public int yFecha;
            public int xFechaDia, xFechaMes, xFechaAño;
            public int xMonto, yMonto;
            public int xPagueseOrdenDe, yPagueseOrdenDe;
            public int xMontoEnLetras, yMontoEnLetras;
        }

        int _Version;
        string _ChequeID;
        string _Talonario;
        ImpresionCoords _ImpresionCoords;
        int _PrimerCheque;
        int _UltimoCheque;

        public Configuration()
        {
            _Version = 1;
            _ChequeID = "Cheque Estándar";
            _Talonario = "T01";
            _PrimerCheque = 1;
            _UltimoCheque = 99999;
            InicializarCoordenadas();
        }

        #region "Funciones Relativas a la Lectura y Escritura del Archivo de Configuración"

        public static void Serialize(string file, Configuration c)
        {
            System.Xml.Serialization.XmlSerializer xs
               = new System.Xml.Serialization.XmlSerializer(c.GetType());
            StreamWriter writer = File.CreateText(file);
            xs.Serialize(writer, c);
            writer.Flush();
            writer.Close();
        }
        public static Configuration Deserialize(string file)
        {
            System.Xml.Serialization.XmlSerializer xs
               = new System.Xml.Serialization.XmlSerializer(
                  typeof(Configuration));
            StreamReader reader = File.OpenText(file);
            Configuration c = (Configuration)xs.Deserialize(reader);
            reader.Close();
            return c;
        }

        #endregion

        public int Version
        {
            get { return _Version; }
            set { _Version = value; }
        }
        public string ChequeID
        {
            get { return _ChequeID; }
            set { _ChequeID = value; }
        }
        public string Talonario
        {
            get { return _Talonario; }
            set { _Talonario = value; }
        }
        public int PrimerCheque
        {
            get { return _PrimerCheque; }
            set { _PrimerCheque = value; }
        }
        public int UltimoCheque
        {
            get { return _UltimoCheque; }
            set { _UltimoCheque = value; }
        }
        // Interfaz para la Estructura con las Coordenadas de Impresión.
        public ImpresionCoords CoordenadasImpresion
        {
            get { return _ImpresionCoords; }
            set { _ImpresionCoords = value; }
        }

        public void InicializarCoordenadas()
        {
            // Creamos e inicializamos la Estructura con las Coordenadas para la Impresión.
            _ImpresionCoords = new ImpresionCoords();
            // Fecha.
            _ImpresionCoords.yFecha = 15;
            _ImpresionCoords.xFechaDia = 120;
            _ImpresionCoords.xFechaMes = 160;
            _ImpresionCoords.xFechaAño = 230;
            // Monto en Números.
            _ImpresionCoords.xMonto = 120;
            _ImpresionCoords.yMonto = 30;
            // Páguese a la Orden De.
            _ImpresionCoords.xPagueseOrdenDe = 120;
            _ImpresionCoords.yPagueseOrdenDe = 45;
            // Monto en Letras.
            _ImpresionCoords.xMontoEnLetras = 120;
            _ImpresionCoords.yMontoEnLetras = 60;
        }
    }
}
