using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Lab04_EDII.Models
{
    public class CompressionsCollections
    {
        public string Nombre_Del_Archivo_Original { get; set; }
        public string Nombre_Del_Archivo_Comprimido { get; set; }
        public string Ruta_Del_Archivo_Comprimido { get; set; }
        public double Razon_De_Compresion { get; set; }
        public double Factor_De_Compresion { get; set; }
        public double Porcentaje { get; set; }

        public static List<CompressionsCollections> HistorialCompresiones() {
            var Linea = string.Empty;
            var ListaArchivoLeido = new List<CompressionsCollections>();
            using (var Lector = new StreamReader(Path.Combine(Environment.CurrentDirectory, "Compressions.txt")))
            {
                while (!Lector.EndOfStream)
                {
                    var TempCompressionCollections = new CompressionsCollections();

                    Linea = Lector.ReadLine();
                    TempCompressionCollections.Nombre_Del_Archivo_Original = Linea;

                    Linea = Lector.ReadLine();
                    TempCompressionCollections.Nombre_Del_Archivo_Comprimido = Linea;

                    Linea = Lector.ReadLine();
                    TempCompressionCollections.Ruta_Del_Archivo_Comprimido = Linea;

                    Linea = Lector.ReadLine();
                    TempCompressionCollections.Razon_De_Compresion = Convert.ToDouble(Linea);

                    Linea = Lector.ReadLine();
                    TempCompressionCollections.Factor_De_Compresion = Convert.ToDouble(Linea);

                    Linea = Lector.ReadLine();
                    TempCompressionCollections.Porcentaje = Convert.ToDouble(Linea);

                    ListaArchivoLeido.Add(TempCompressionCollections);
                }
            }
            return ListaArchivoLeido;
        }

        public static void EscrituraCompresiones(CompressionsCollections CC) {
            var Linea = string.Empty;
            var ListaArchivoHistorial = HistorialCompresiones();
            using (var Escritor = new StreamWriter(Path.Combine(Environment.CurrentDirectory, "Compressions.txt")))
            {
                foreach (var item in ListaArchivoHistorial)
                {
                    Escritor.WriteLine(item.Nombre_Del_Archivo_Original);
                    Escritor.WriteLine(item.Nombre_Del_Archivo_Comprimido);
                    Escritor.WriteLine(item.Ruta_Del_Archivo_Comprimido);
                    Escritor.WriteLine(item.Razon_De_Compresion);
                    Escritor.WriteLine(item.Factor_De_Compresion);
                    Escritor.WriteLine(item.Porcentaje);
                }
                Escritor.WriteLine(CC.Nombre_Del_Archivo_Original);
                Escritor.WriteLine(CC.Nombre_Del_Archivo_Comprimido);
                Escritor.WriteLine(CC.Ruta_Del_Archivo_Comprimido);
                Escritor.WriteLine(CC.Razon_De_Compresion);
                Escritor.WriteLine(CC.Factor_De_Compresion);
                Escritor.WriteLine(CC.Porcentaje);
            }
        }
    }
}
