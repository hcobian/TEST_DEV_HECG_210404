using API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace API.Controllers
{
    public class PersonasController : ApiController
    {
        // GET: api/Personas
        public List<RegistrosModel> Get()
        {
            List<RegistrosModel> list = new List<RegistrosModel>();

            using (PruebaTokaEntities db = new PruebaTokaEntities())
            {

                list = (from d in db.Tb_PersonasFisicas
                        select new RegistrosModel
                        {
                            IdPersonaFisica = d.IdPersonaFisica,
                            FechaRegistro = d.FechaRegistro,
                            FechaActualizacion = d.FechaActualizacion,
                            Nombre = d.Nombre,
                            ApellidoPaterno = d.ApellidoPaterno,
                            ApellidoMaterno = d.ApellidoMaterno,
                            RFC = d.RFC,
                            FechaNacimiento = d.FechaNacimiento,
                            UsuarioAgrega = d.UsuarioAgrega,
                            Activo = d.Activo
                        }).ToList();
            }

            return list;
        }

        // GET: api/Personas/5
        /*
          public string Get(int id)
        {
            

            return "value";
        }
        */

        // POST: api/Personas
        public string Post(InsertModel input)
        {
            if(input != null)
            using (PruebaTokaEntities db = new PruebaTokaEntities())
            {
                var resp = db.sp_AgregarPersonaFisica(input.Nombre,input.ApellidoPaterno,input.ApellidoMaterno,input.RFC,input.FechaNacimiento,input.UsuarioAgrega);
                db.SaveChanges();
                    return "Exito";
            }
            return "Error";
            
        }

        public string Post(int id, InsertModel input)
        {
            if (id != 0 && input != null)
                using (PruebaTokaEntities db = new PruebaTokaEntities())
                {
                    var resp = db.sp_ActualizarPersonaFisica(id, input.Nombre, input.ApellidoPaterno, input.ApellidoMaterno, input.RFC, input.FechaNacimiento, input.UsuarioAgrega);
                    db.SaveChanges();
                    
                }
            return "";
        }

        

        // DELETE: api/Personas/5
        public void Delete(int id)
        {
            using (PruebaTokaEntities db = new PruebaTokaEntities())
            {
                var resp = db.sp_EliminarPersonaFisica(id);
                db.SaveChanges();
            }
        }
    }
}
