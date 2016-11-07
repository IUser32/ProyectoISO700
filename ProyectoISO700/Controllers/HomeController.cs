using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoISO700.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Silux es una aplicación para la centralización de ofertas de trabajo a nivel nacional en la República Dominicana.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Silux fue desarrollador por:";

            return View();
        }

        public ActionResult ErrorPermiso()
        {
            ViewBag.Message = "Usted debe de estar logueado para acceder a esta funcionalidad.";
            return View("~/Views/Shared/Error.cshtml");
        }
    }
}