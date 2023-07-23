using ProyectoBanco.Models;
using ProyectoBanco.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace ProyectoBanco.Controllers

{
    public class HomeController : Controller
    {
        UsuarioModel model = new UsuarioModel();

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult IniciarSesion(UsuarioEnt entidad)
        {
            var resp = model.IniciarSesion(entidad);

            if (resp != null)
            {

                Session["IdUsuario"] = resp.IdUsuario;
                Session["Correo"] = resp.Correo;
                Session["NombreUsuario"] = resp.Nombre;
                Session["NombreRoleUsuario"] = resp.NombreRole;
                Session["ApellidoUsuario"] = resp.Apellido;
                Session["Direccion"] = resp.Direccion;
                Session["Telefono"] = resp.Telefono;
                Session["RolUsuario"] = resp.IdRole;
                return RedirectToAction("Inicio", "Home");
            }
            else
            {
                ViewBag.MsjPantalla = "No se ha podido validar su información";
                return View("Login");
            }
        }



        [HttpGet]
        public ActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RegistrarUsuario(UsuarioEnt entidad)
        {
            entidad.IdRole = 5;
            entidad.Estado = true;

            var resp = model.RegistrarUsuario(entidad);

            if (resp > 0)
                return RedirectToAction("Login", "Home");
            else
            {
                ViewBag.MsjPantalla = "No se ha podido registrar su información";
                return View("Registro");
            }
        }



        [HttpGet]
        public ActionResult Recuperar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult RecuperarClave(UsuarioEnt entidad)
        {
            var resp = model.RecuperarClave(entidad);

            if (resp)
                return RedirectToAction("Login", "Home");
            else
            {
                ViewBag.MsjPantalla = "No se ha podido recuperar su acceso";
                return View("Recuperar");
            }

        }

        [HttpGet]
        public ActionResult Inicio()
        {
            
            return View();
        }


        [HttpGet]
        public ActionResult CerrarSesion()
        {
            Session.Clear();
            return RedirectToAction("Login", "Home");
        }



        [HttpGet]
        public ActionResult Cambiar()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Cuenta()
        {
            return View();
        }



        [HttpPost]
        public ActionResult CambiarClave(UsuarioEnt entidad)
        {
            entidad.IdUsuario = long.Parse(Session["IdUsuario"].ToString());
            entidad.Correo = Session["Correo"].ToString();
            
            var respValidarClave = model.IniciarSesion(entidad);

            if (respValidarClave == null)
            {
                ViewBag.MsjPantalla = "La actual no coincide con su registro en la base de datos";
                return View("Cambiar");
            }

            if (entidad.ContrasennaNueva != entidad.ConfirmarContrasennaNueva)
            {
                ViewBag.MsjPantalla = "Las nueva contraseña no coincide con su confirmación";
                return View("Cambiar");
            }

            var respCambiarClave = model.CambiarClave(entidad);

            if (respCambiarClave > 0)
                return RedirectToAction("Inicio", "Home");
            else
            {
                ViewBag.MsjPantalla = "No se ha podido cambiar su contraseña actual";
                return View("Cambiar");
            }

        }

    }

}