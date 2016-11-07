using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProyectoISO700.Models.ModelView
{
    public class OfertaViewOut
    {
        public int id { get; set; }
        public int category_id { get; set; }
        public int typeID { get; set; }
        public string name { get; set; }
        public string Type { get; set; }
        public string company { get; set; }
        public string position { get; set; }
        public string location { get; set; }
        public DateTime? DateCreated { get; set; }
        public string description { get; set; }
        public string url { get; set; }
        public byte[] logo { get; set; }
        public int userId { get; set; }
    }
}