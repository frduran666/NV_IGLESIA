using System.Collections.Generic;
using System.Web.Mvc;
using DS_NotaVenta.Models;
using DS_NotaVenta.DAO;

namespace DS_NotaVenta.Controllers
{
    public class IndexController : Controller
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Login()
        {
            return View();
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Login(FormCollection frm)
        {
            UsuariosModels usuario = new UsuariosModels();

            usuario.Usuario = Request.Form["__usuario"];
            usuario.Password = Request.Form["__password"];

            List<UsuariosModels> listaUsuario = LoginDAO.Login(usuario);

            if (listaUsuario == null || listaUsuario.Count == 0)
            {
                TempData["Mensaje"] = "ERROR - Verifique los datos ingresados <br>";
                return View();
            }
            else
            {
                if (Session["ID"] != null) { Session["ID"] = null; Session["ID"] = listaUsuario[0].id; } else Session["ID"] = listaUsuario[0].id;
                if (Session["Nombre"] != null) { Session["Nombre"] = null; Session["Nombre"] = listaUsuario[0].Usuario; } else Session["Nombre"] = listaUsuario[0].Usuario;
                if (Session["VenCod"] != null) { Session["VenCod"] = null; Session["VenCod"] = listaUsuario[0].VenCod; } else Session["VenCod"] = listaUsuario[0].VenCod;
                if (Session["VenDes"] != null) { Session["VenDes"] = null; Session["VenDes"] = listaUsuario[0].VenDes; } else Session["VenDes"] = listaUsuario[0].VenDes;
                if (Session["TipoId"] != null) { Session["TipoId"] = null; Session["TipoId"] = listaUsuario[0].tipoId; } else Session["TipoId"] = listaUsuario[0].tipoId;
                if (Session["Test"] != null) { Session["Test"] = null; Session["Test"] = "Am new to Session"; } else Session["Test"] = "Am new to Session";

                if (listaUsuario[0].tipoId == 1)
                {
                    return RedirectToAction("Index", "Index");
                }
                else
                {
                    return RedirectToAction("Index", "Index");
                }
            }
        }
    }
}
