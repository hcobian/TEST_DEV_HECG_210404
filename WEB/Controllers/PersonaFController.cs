using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace WEB.Controllers
{
    [Authorize]
    public class PersonaFController : Controller
    {
        // GET: PersonaF
        public ActionResult Index(string Message = null)
        {
            ViewBag.message = Message;
            ViewBag.Personas = GetPersonas();

            return View("Index");
        }
        public ActionResult Agregar(string nombre, string apPat, string apMat, string rfc)
        {
            if (!string.IsNullOrEmpty(nombre) && !string.IsNullOrEmpty(apPat) && !string.IsNullOrEmpty(apMat) && !string.IsNullOrEmpty(rfc))
            {
                var resp = AgregarAPI(nombre, apPat, apMat, rfc);
                return Index(resp);
            }
            return Index("Ingresa datos validos");
        }
        public ActionResult Editar(int ID, string nombre, string apPat, string apMat, string rfc)
        {
            if (ID != 0 && !string.IsNullOrEmpty(nombre) && !string.IsNullOrEmpty(apPat) && !string.IsNullOrEmpty(apMat) && !string.IsNullOrEmpty(rfc))
            {
                var resp = EditarAPI(ID,nombre, apPat, apMat, rfc);
                return Index(resp);
            }
            return Index("Ingresa datos validos");
        }
        public ActionResult Eliminar(int ID)
        {
            if (ID != 0)
            {
                EliminarAPI(ID);
                return Index("llamado a editar");
            }
            return Index("Ingresa un id valido");
        }
        List<RegistrosModel> GetPersonas()
        {
            List<RegistrosModel> info = new List<RegistrosModel>();
            try
            {
                var client = new RestClient("https://localhost:44387/api/Personas/");
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                IRestResponse response = client.Execute(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                    info = jsonSerializer.Deserialize<List<RegistrosModel>>(response.Content);

                }
            }
            catch (Exception ex) { }
            return info;
        }
        string AgregarAPI(string nombre, string apPat, string apMat, string rfc)
        {
            string Resp = "";
            try
            {
                InsertModel input = new InsertModel();
                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                input.Nombre = nombre;
                input.ApellidoPaterno = apPat;
                input.ApellidoMaterno = apMat;
                input.RFC = rfc;

                string Params = jsonSerializer.Serialize(input);
                var client = new RestClient("https://localhost:44387/api/Personas/");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", Params, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Resp = "Registro Agregado";
                }
                else
                {
                    Resp = "Algo Salio mal";
                }
            }
            catch (Exception ex) { Resp = "Algo Salio mal"; }
            return Resp;
        }
        string EditarAPI(int ID, string nombre, string apPat, string apMat, string rfc)
        {
            string Resp = "";
            try
            {
                InsertModel input = new InsertModel();
                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                input.Nombre = nombre;
                input.ApellidoPaterno = apPat;
                input.ApellidoMaterno = apMat;
                input.RFC = rfc;

                string Params = jsonSerializer.Serialize(input);
                var client = new RestClient($"https://localhost:44387/api/Personas/{ID}");
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", Params, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Resp = "Registro Actualizado";
                }
                else
                {
                    Resp = "Algo Salio mal";
                }
            }
            catch (Exception ex) { Resp = "Algo Salio mal"; }
            return Resp;
        }
        string EliminarAPI(int id)
        {
            string Resp = "";
            var client = new RestClient($"https://localhost:44387/api/Personas/{id}");
            client.Timeout = -1;
            var request = new RestRequest(Method.DELETE);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
            }
            return "";
        }
        public class RegistrosModel
        {
            public int IdPersonaFisica { get; set; }
            public DateTime? FechaRegistro { get; set; }
            public DateTime? FechaActualizacion { get; set; }
            public string Nombre { get; set; }
            public string ApellidoPaterno { get; set; }
            public string ApellidoMaterno { get; set; }
            public string RFC { get; set; }
            public DateTime? FechaNacimiento { get; set; }
            public int? UsuarioAgrega { get; set; }
            public bool? Activo { get; set; }

        }

        public class InsertModel
        {
            public string Nombre { get; set; }
            public string ApellidoPaterno { get; set; }
            public string ApellidoMaterno { get; set; }
            public string RFC { get; set; }
            public DateTime? FechaNacimiento { get; set; }
            public int? UsuarioAgrega { get; set; }

        }

    }
}