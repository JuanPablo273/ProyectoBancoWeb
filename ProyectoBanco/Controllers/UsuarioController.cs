using ProyectoBanco.Entities;
using ProyectoBanco.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProyectoBanco.Controllers
{
    public class UsuarioController : Controller
    {
        UsuarioModel model = new UsuarioModel();

        [HttpGet]
        public ActionResult ConsultaUsuarios()
        {
            var resp = model.ConsultaUsuarios();
            return View();
        }

        //[HttpGet]
        //public ActionResult CambiarEstado(long q)
        //{
        //    UsuarioEnt entidad = new UsuarioEnt();
        //    entidad.IdUsuario = q;

        //    var resp = model.CambiarEstado(entidad);

        //    if (resp > 0)
        //        return RedirectToAction("ConsultaUsuarios", "Usuario");
        //    else
        //    {
        //        ViewBag.MsjPantalla = "No se ha podido actualizar el estado del usuario";
        //        return View("ConsultaUsuarios");
        //    }
        //}

        [HttpGet]
        public ActionResult Editar()
        {
            var resp = model.ConsultaUsuario(long.Parse(Session["IdUsuario"].ToString()));
            var respRoles = model.ConsultaRoles();

            var roles = new List<SelectListItem>();
            foreach (var item in respRoles)
            {
                roles.Add(new SelectListItem { Value = item.IdRol.ToString(), Text = item.NombreRol.ToString() });
            }

            ViewBag.ComboRoles = roles;
            return View(resp);
        }

        [HttpPost]
        public ActionResult EditarUsuario(UsuarioEnt entidad)
        {


            var resp = model.EditarUsuario(entidad);

            if (resp > 0)
                return RedirectToAction("ConsultaUsuarios", "Usuario");
            else
            {
                ViewBag.MsjPantalla = "No se ha podido actualizar la información del usuario";
                return View("ConsultaUsuarios");



                //if (datos == null)
                //{
                //    ViewBag.MsjPantalla = "Ingrese Datos";
                //    return View("Editar");
                //}

                //var datosnuevo = model.CambiarDatos(entidad);

                //if (datosnuevo > 0)
                //    return RedirectToAction("Inicio", "Home");
                //else
                //{
                //    ViewBag.MsjPantalla = "No se ha podido cambiar sus datos";
                //    return View("Cambiar");
                //}

            }

        }

    }
}




    
