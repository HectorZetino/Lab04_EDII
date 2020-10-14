using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BibliotecaDeClases.LZW;
using System.IO;
using Lab04_EDII.Models;

namespace Lab04_EDII.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class apiController : ControllerBase
    {
        [HttpPost("compress/{name}")]
        public void Post([FromForm]IFormFile file, string name) {
            LZW.Compresion(file, name);
            var NewFile = new FileInfo(Path.Combine(Environment.CurrentDirectory, "Compressions", $"{name}.lzw"));
            CompressionsCollections.EscrituraCompresiones(
                new CompressionsCollections
                {
                    Nombre_Del_Archivo_Original = file.FileName,
                    Nombre_Del_Archivo_Comprimido = $"{name}.lzw",
                    Ruta_Del_Archivo_Comprimido = Path.Combine(Environment.CurrentDirectory, "Compressions", $"{name}.lzw"),
                    Razon_De_Compresion = (double)NewFile.Length / (double)file.Length,
                    Factor_De_Compresion = (double)file.Length / (double)NewFile.Length,
                    Porcentaje = 100 - (((double)NewFile.Length / (double)file.Length) * 100) // correccion del anterior (lab03), se muestra el porcentaje de reduccion real 
                });
        }
        [HttpPost("decompress")]
        public void Post([FromForm]IFormFile file) {
            var HistorialCompresiones = CompressionsCollections.HistorialCompresiones();
            var OriginalName = HistorialCompresiones.Find(c => Path.GetFileNameWithoutExtension(c.Nombre_Del_Archivo_Comprimido) == Path.GetFileNameWithoutExtension(file.FileName));
            var path = LZW.Decompresion(file, OriginalName.Nombre_Del_Archivo_Original);
            var NewFile = new FileInfo(path);
            CompressionsCollections.EscrituraCompresiones(
                new CompressionsCollections
                {
                    Nombre_Del_Archivo_Original = OriginalName.Nombre_Del_Archivo_Original,
                    Nombre_Del_Archivo_Comprimido = file.FileName,
                    Ruta_Del_Archivo_Comprimido = path,
                    Razon_De_Compresion = 0,
                    Factor_De_Compresion = 0,
                    Porcentaje = 0
                });
        }
        [HttpGet("compressions")]
        public List<CompressionsCollections> Get() {
            var ListadoCompresiones = new List<CompressionsCollections>();
            var PilaCompresiones = new Stack<CompressionsCollections>();
            var Linea = string.Empty;
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

                    PilaCompresiones.Push(TempCompressionCollections);
                }
            }
            while (PilaCompresiones.Count != 0)
            {
                ListadoCompresiones.Add(PilaCompresiones.Pop());
            }
            return ListadoCompresiones;
        }
        [HttpGet]
        public ActionResult GetAction() {
            return Ok();
        }
    }
}
