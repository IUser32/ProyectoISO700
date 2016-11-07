using PagedList;
using ProyectoISO700.Models.ModeloEntity;
using ProyectoISO700.Models.ModelView;
using ProyectoISO700.Models.Query;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace ProyectoISO700.Controllers
{
    public class OfertasController : Controller
    {
        private BolsaEmpleoContext _context;

        public OfertasController()
        {
            _context = new BolsaEmpleoContext();
        }

        // GET: Ofertas
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Detalle(int? id)
        {
            if (!id.HasValue)
                return RedirectToAction("Index");

            var oferta = this.ObtenerOferta(id.Value);
            if(oferta == null)
                return RedirectToAction("Index");

            ViewBag.PuedeModificar = this.ObtenerUserId(User.Identity.Name) == oferta.userId ? true : false;
            if (oferta.logo != null)
                ViewBag.File = Convert.ToBase64String(oferta.logo);
            return View(oferta);
        }

        [HttpGet]
        public ActionResult Registrar()
        {
            if(!User.Identity.IsAuthenticated)
                return RedirectToAction("ErrorPermiso", "Home");
            ViewBag.Categoria = ObtenerCategoria();
            ViewBag.Tipo = ObtenerTipo();

            return View();
        }

        [HttpPost]
        public ActionResult Registrar(OfertaViewIn input)
        {
            input.userId = ObtenerUserId(User.Identity.Name);
            var guardado = this.Guardar(input);

            ViewBag.Result = guardado;
            ViewBag.MessageResult = guardado ? "La oferta ha sido registrada correctamente." :
                                                    "Ha ocurrido un error mientras se guardaba.";

            ViewBag.Categoria = ObtenerCategoria();
            ViewBag.Tipo = ObtenerTipo();

            return View();
        }

        [HttpGet]
        public ActionResult Modificar(int? id)
        {
            if (!id.HasValue)
                return RedirectToAction("Index");

            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("ErrorPermiso", "Home");

            var oferta = this.ObtenerOferta(id.Value);
            if (oferta == null)
                return RedirectToAction("Index");

            ViewBag.Categoria = ObtenerCategoria();
            ViewBag.Tipo = ObtenerTipo();

            return View(oferta);
        }

        [HttpPost]
        public ActionResult Modificar(OfertaViewIn input)
        {
            var guardado = this.Update(input);

            ViewBag.Result = guardado;
            ViewBag.MessageResult = guardado ? "La oferta ha sido modificada correctamente." :
                                                    "Ha ocurrido un error mientras se modificaba.";

            ViewBag.Categoria = ObtenerCategoria();
            ViewBag.Tipo = ObtenerTipo();

            var oferta = this.ObtenerOferta(input.OfertaID);

            return View(oferta);
        }

        public PartialViewResult Listado(OfertaCriterio criterio, int pagina = 1, int tamañopagina = 7)
        {
            IEnumerable<OfertaViewOut> ofertas;
            var cantidad = default(int);

            ofertas = ObtenerOfertas(criterio, pagina, tamañopagina);
            cantidad = ObtenerCantidad(criterio);

            ofertas = new StaticPagedList<OfertaViewOut>(ofertas, pagina, tamañopagina, cantidad);

            ViewBag.Criterio = criterio;
            ViewBag.TotalRegistros = cantidad;

            return PartialView("_partialListado", ofertas);
        }

        private IEnumerable<OfertaViewOut> ObtenerOfertas(OfertaCriterio criterio, int pagina, int tamañoPagina)
        {
            List<SqlParameter> parameters;

            var query = createQuery(criterio, out parameters);
            parameters.Add(new SqlParameter()
            {
                SqlDbType = System.Data.SqlDbType.Int,
                ParameterName = "pagina",
                Value = pagina
            });
            parameters.Add(new SqlParameter()
            {
                SqlDbType = System.Data.SqlDbType.Int,
                ParameterName = "tamañoPagina",
                Value = tamañoPagina
            });
            var ofertasDB = _context.Database.SqlQuery<OfertaViewOut>(Consulta.ConsultaPaginado(query), parameters.ToArray());

            return ofertasDB.AsEnumerable();
        }

        private int ObtenerCantidad(OfertaCriterio criterio)
        {
            List<SqlParameter> parameters;

            var query = createQuery(criterio, out parameters);

            var ofertasDB = _context.Database.SqlQuery<int>(Consulta.ConsultaCantidad(query), parameters.ToArray());

            return ofertasDB.AsEnumerable().FirstOrDefault();
        }

        private string createQuery(OfertaCriterio criterio, out List<SqlParameter> parameters)
        {
            parameters = new List<SqlParameter>();

            var query = new StringBuilder(Consulta.ObtenerOfertasListado());

            if (criterio != null)
            {
                if (!string.IsNullOrEmpty(criterio.Nombre))
                {
                    query.Append(" AND j.[position] LIKE @Nombre ");
                    parameters.Add(new SqlParameter()
                    {
                        DbType = System.Data.DbType.String,
                        ParameterName = "Nombre",
                        Value = "%" + criterio.Nombre + "%"
                    });
                }
            }

            return query.ToString();
        }

        private OfertaViewOut ObtenerOferta(int id)
        {
            var ofertaDB = _context.Jobs.Where(e => e.id == id).FirstOrDefault();

            if (ofertaDB == null)
                return null;

            return new OfertaViewOut()
            {
                id = ofertaDB.id,
                category_id = ofertaDB.category_id,
                company = ofertaDB.company,
                DateCreated = ofertaDB.DateCreated,
                location = ofertaDB.location,
                name = ofertaDB.Category.name,
                position = ofertaDB.position,
                Type = ofertaDB.TypeJob.Type,
                typeID = ofertaDB.typeID,
                description = ofertaDB.description,
                logo = ofertaDB.logo,
                url = ofertaDB.url,
                userId = ofertaDB.userId
            };
        }

        private Dictionary<int, string> ObtenerCategoria()
        {
            return _context.Categories.ToDictionary(w => w.id, w => w.name);
        }

        private Dictionary<int, string> ObtenerTipo()
        {
            return _context.TypeJobs.ToDictionary(w => w.typeID, w => w.Type);
        }

        public static byte[] ReadToEnd(System.IO.Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead == readBuffer.Length)
                    {
                        int nextByte = stream.ReadByte();
                        if (nextByte != -1)
                        {
                            byte[] temp = new byte[readBuffer.Length * 2];
                            Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                            Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                            readBuffer = temp;
                            totalBytesRead++;
                        }
                    }
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length != totalBytesRead)
                {
                    buffer = new byte[totalBytesRead];
                    Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                }
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }

        private bool Guardar(OfertaViewIn input)
        {
            if (!validation(input))
                return false;

            byte[] arrayByte = ReadToEnd(input.Logo.InputStream);

            var inputDb = new Job()
            {
                category_id = input.Categoria,
                typeID = input.Tipo,
                company = input.Compañia,
                DateCreated = DateTime.Now,
                DateExpires = DateTime.Now,
                DateUpdated = DateTime.Now,
                description = input.Descripcion,
                location = input.Ubicacion,
                position = input.Posicion,
                url = input.URL,
                logo = arrayByte,
                userId = input.userId
            };

            _context.Jobs.Add(inputDb);

            var guardarCambios = _context.SaveChanges() > 0 ? true : false;

            if (!guardarCambios)
                return false;

            return true;
        }

        private bool Update(OfertaViewIn input)
        {
            if (!validation(input))
                return false;

            var ofertaDb = _context.Jobs.FirstOrDefault(w => w.id == input.OfertaID);

            //byte[] arrayByte = ReadToEnd(input.Logo.InputStream);

            ofertaDb.category_id = input.Categoria;
            ofertaDb.typeID = input.Tipo;
            ofertaDb.company = input.Compañia;
            ofertaDb.DateUpdated = DateTime.Now;
            ofertaDb.description = input.Descripcion;
            ofertaDb.location = input.Ubicacion;
            ofertaDb.position = input.Posicion;
            ofertaDb.url = input.URL;

            var guardarCambios = _context.SaveChanges() > 0 ? true : false;

            if (!guardarCambios)
                return false;

            return true;
        }

        private bool validation(OfertaViewIn input)
        {
            if (input == null)
                return false;

            if (input.Categoria <= 0)
                return false;
            if (input.Tipo <= 0)
                return false;
            if (String.IsNullOrEmpty(input.Compañia))
                return false;
            if (String.IsNullOrEmpty(input.Descripcion))
                return false;
            if (String.IsNullOrEmpty(input.Posicion))
                return false;
            if (String.IsNullOrEmpty(input.Ubicacion))
                return false;

            return true;
        }

        private int ObtenerUserId(string username)
        {
            var user = _context.UserProfiles
                                .FirstOrDefault(w => w.Username.Equals(username, StringComparison.InvariantCultureIgnoreCase));
            if (user == null)
                return 0;

            return user.UserId;
        }
    }
}