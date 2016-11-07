using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoISO700.Models.ModelView
{
    public class OfertaViewIn
    {
        public int id { get; set; }
        public int Categoria { get; set; }
        public int Tipo { get; set; }
        public string Compañia { get; set; }
        public HttpPostedFileWrapper Logo { get; set; }
        public string URL { get; set; }
        public string Posicion { get; set; }
        public string Ubicacion { get; set; }
        public string Descripcion { get; set; }
    }
}