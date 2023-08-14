using ProyectoBancoAPI.Entities;
using ProyectoBancoAPI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web.Http;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace ProyectoBancoAPI.Controllers
{
    public class UsuarioController : ApiController
    {
        [HttpPost]
        [Route("api/IniciarSesion")]
        public UsuarioEnt IniciarSesion(UsuarioEnt entidad)
        {
            using (var bd = new ProyectoBancoEntities())
            {
                var datos = (from x in bd.Usuario
                             join y in bd.Role on x.IdRole equals y.IdRole
                             where x.Correo == entidad.Correo
                                      && x.Contrasenna == entidad.Contrasenna
                                      && x.Estado == true
                             select new
                             {
                                 x.IdUsuario,
                                 x.Correo,
                                 x.Nombre,
                                 x.Apellido,
                                 x.Direccion,
                                 x.Telefono,
                                 x.Estado,
                                 x.IdRole,
                                 x.Caducidad,
                                 x.ClaveTemporal,
                                 y.NombreRole
                             }).FirstOrDefault();

                if (datos != null)
                {
                    if (datos.ClaveTemporal.Value && datos.Caducidad < DateTime.Now)
                    {
                        return null;
                    }

                    UsuarioEnt resp = new UsuarioEnt();
                    resp.Correo = datos.Correo;
                    resp.Direccion = datos.Direccion;
                    resp.Telefono = datos.Telefono;
                    resp.Nombre = datos.Nombre;
                    resp.Apellido = datos.Apellido;
                    resp.Estado = datos.Estado;
                    resp.IdRole = datos.IdRole;
                    resp.NombreRole = datos.NombreRole;
                    resp.IdUsuario = datos.IdUsuario;
                    return resp;
                }
                else
                {
                    return null;
                }
            }
        }

        [HttpPost]
        [Route("api/RegistrarUsuario")]
        public int RegistrarUsuario(UsuarioEnt entidad)
        {
            using (var bd = new ProyectoBancoEntities())
            {
                Usuario tabla = new Usuario();
                tabla.Correo = entidad.Correo;
                tabla.Contrasenna = entidad.Contrasenna;
                tabla.Nombre = entidad.Nombre;
                tabla.Apellido = entidad.Apellido;
                tabla.Estado = entidad.Estado;
                tabla.IdRole = entidad.IdRole;
                tabla.Direccion = entidad.Direccion;
                tabla.Telefono = entidad.Telefono;
                tabla.ClaveTemporal = false;
                tabla.Caducidad = DateTime.Now;


                bd.Usuario.Add(tabla);
                return bd.SaveChanges();
            }


        }

        [HttpPost]
        [Route("api/RecuperarClave")]
        public bool RecuperarClave(UsuarioEnt entidad)
        {
            UtilitariosModel util = new UtilitariosModel();

            using (var bd = new ProyectoBancoEntities())
            {
                var datos = (from x in bd.Usuario
                             where x.Correo == entidad.Correo
                                           && x.Estado == true
                             select x).FirstOrDefault();

                if (datos != null)
                {
                    string pass = util.CreatePassword();
                    string mensaje = "Estimado(a): " + datos.Nombre + ". Se ha generado la siguiente contraseña temporal: " + pass + " valida por los siguientes 15 minutos";
                    util.SendEmail(datos.Correo, "Recuperar Contraseña", mensaje);

                    //Update de LiQ
                    datos.Contrasenna = pass;
                    datos.ClaveTemporal = true;
                    datos.Caducidad = DateTime.Now.AddMinutes(15);
                    bd.SaveChanges();
                    return true;
                }
            }

            return false;
        }

        [HttpPut]
        [Route("api/CambiarClave")]
        public int CambiarClave(UsuarioEnt entidad)
        {
            using (var bd = new ProyectoBancoEntities())
            {
                var datos = (from x in bd.Usuario
                             where x.IdUsuario == entidad.IdUsuario
                             select x).FirstOrDefault();

                if (datos != null)
                {
                    datos.IdUsuario = entidad.IdUsuario;
                    datos.Nombre = entidad.Nombre;
                    datos.Apellido = entidad.Apellido;
                    datos.Correo = entidad.Correo;
                    datos.Direccion = entidad.Direccion;
                    datos.Telefono = entidad.Telefono;
                   

                    return bd.SaveChanges();
                }

                return 0;
            }
        }

        [HttpPut]
        [Route("api/CambiarDatos")]
        public int CambiarDatos(UsuarioEnt entidad)
        {
            using (var bd = new ProyectoBancoEntities())
            {
                var datos = (from x in bd.Usuario
                             where x.IdUsuario == entidad.IdUsuario
                             select x).FirstOrDefault();

                if (datos != null)
                {
                    datos.Contrasenna = entidad.ContrasennaNueva;
                    datos.ClaveTemporal = false;
                    datos.Caducidad = DateTime.Now;
                    return bd.SaveChanges();
                }

                return 0;
            }
        }





        [HttpGet]
        [Route("api/ConsultaUsuarios")]
        public List<UsuarioEnt> ConsultaUsuarios()
        {
            using (var bd = new ProyectoBancoEntities())
            {
                var datos = (from x in bd.Usuario
                             select x).ToList();

                if (datos.Count > 0)
                {
                    var resp = new List<UsuarioEnt>();
                    foreach (var item in datos)
                    {
                        resp.Add(new UsuarioEnt
                        {
                            Correo = item.Correo,
                            Nombre = item.Nombre,
                            Apellido = item.Apellido,
                            Estado = item.Estado,
                            IdRole = item.IdRole,
                            IdUsuario = item.IdUsuario
                        });
                    }
                    return resp;
                }
                else
                {
                    return new List<UsuarioEnt>();
                }
            }
        }

        //    [HttpPut]
        //    [Route("api/CambiarEstado")]
        //    public int CambiarEstado(UsuarioEnt entidad)
        //    {
        //        using (var bd = new LN_ProyectoEntities())
        //        {
        //            var datos = (from x in bd.Usuario
        //                         where x.IdUsuario == entidad.IdUsuario
        //                         select x).FirstOrDefault();

        //            if (datos != null)
        //            {
        //                bool estadoActual = datos.Estado;

        //                datos.Estado = (estadoActual == true ? false : true);
        //                return bd.SaveChanges();
        //            }

        //            return 0;
        //        }
        //    }

        //    [HttpGet]
        //    [Route("api/ConsultaUsuario")]
        //    public UsuarioEnt ConsultaUsuario(long q)
        //    {
        //        using (var bd = new LN_ProyectoEntities())
        //        {
        //            var datos = (from x in bd.Usuario
        //                         where x.IdUsuario == q
        //                         select x).FirstOrDefault();

        //            if (datos != null)
        //            {
        //                UsuarioEnt resp = new UsuarioEnt();
        //                resp.CorreoElectronico = datos.CorreoElectronico;
        //                resp.Nombre = datos.Nombre;
        //                resp.Identificacion = datos.Identificacion;
        //                resp.Estado = datos.Estado;
        //                resp.IdRol = datos.IdRol;
        //                resp.IdUsuario = datos.IdUsuario;
        //                return resp;
        //            }
        //            else
        //            {
        //                return null;
        //            }
        //        }
        //    }

        //    [HttpGet]
        //    [Route("api/ConsultaRoles")]
        //    public List<RolEnt> ConsultaRoles()
        //    {
        //        using (var bd = new LN_ProyectoEntities())
        //        {
        //            var datos = (from x in bd.Rol
        //                         where x.Estado == true
        //                         select x).ToList();

        //            if (datos.Count > 0)
        //            {
        //                var resp = new List<RolEnt>();
        //                foreach (var item in datos)
        //                {
        //                    resp.Add(new RolEnt
        //                    {
        //                        IdRol = item.IdRol,
        //                        NombreRol = item.NombreRol,
        //                    });
        //                }
        //                return resp;
        //            }
        //            else
        //            {
        //                return new List<RolEnt>();
        //            }
        //        }
        //    }
        //}
        [HttpGet]
        [Route("api/ConsultaUsuario")]
        public UsuarioEnt ConsultaUsuario(long q)
        {
            using (var bd = new ProyectoBancoEntities())
            {
                var datos = (from x in bd.Usuario
                             where x.IdUsuario == q
                             select x).FirstOrDefault();

                if (datos != null)
                {
                    UsuarioEnt resp = new UsuarioEnt();
                    resp.Apellido = datos.Apellido;
                    resp.Correo = datos.Correo;
                    resp.Direccion = datos.Direccion;
                    resp.Correo = datos.Correo;
                    resp.Telefono = datos.Telefono;
                    resp.Nombre = datos.Nombre;
                    resp.Estado = datos.Estado;
                    resp.IdRole = datos.IdRole;
                    resp.IdUsuario = datos.IdUsuario;
                    return resp;
                }
                else
                {
                    return null;
                }
            }
        }

        [HttpGet]
        [Route("api/ConsultaRoles")]
        public List<RoleEnt> ConsultaRoles()
        {
            using (var bd = new ProyectoBancoEntities())
            {
                var datos = (from x in bd.Role
                             where x.Estado == true
                             select x).ToList();

                if (datos.Count > 0)
                {
                    var resp = new List<RoleEnt>();
                    foreach (var item in datos)
                    {
                        resp.Add(new RoleEnt
                        {
                            IdRol = item.IdRole,
                            NombreRol = item.NombreRole,
                        });
                    }
                    return resp;
                }
                else
                {
                    return new List<RoleEnt>();
                }
            }
        }
    }
}
