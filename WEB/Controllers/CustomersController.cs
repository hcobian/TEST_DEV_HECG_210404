using NPOI.SS.UserModel;
using NPOI.Util;
using NPOI.XSSF.UserModel;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace WEB.Controllers
{
    [Authorize]
    public class CustomersController : Controller
    {
        [HttpGet]
        public ActionResult GetFile()
        {
            XSSFWorkbook wb = new XSSFWorkbook();
            ISheet sheet = wb.CreateSheet("Archivo");
            #region Escribe Archivo
            int col_Actual = 0;
            int row_Actual = 0;
            var row_ = sheet.CreateRow(row_Actual++);
            row_.CreateCell(col_Actual).SetCellValue("IdCliente");
            col_Actual++;
            row_.CreateCell(col_Actual).SetCellValue("FechaRegistroEmpresa");
            col_Actual++;
            row_.CreateCell(col_Actual).SetCellValue("RazonSocial");
            col_Actual++;
            row_.CreateCell(col_Actual).SetCellValue("RFC");
            col_Actual++;
            row_.CreateCell(col_Actual).SetCellValue("Sucursal");
            col_Actual++;
            row_.CreateCell(col_Actual).SetCellValue("IdEmpleado");
            col_Actual++;
            row_.CreateCell(col_Actual).SetCellValue("Nombre");
            col_Actual++;
            row_.CreateCell(col_Actual).SetCellValue("Paterno");
            col_Actual++;
            row_.CreateCell(col_Actual).SetCellValue("Materno");
            col_Actual++;
            row_.CreateCell(col_Actual).SetCellValue("IdViaje");            
            col_Actual = 0;

            InfoCustomer info = GetData();
            if (info.Data != null && info.Data.Count > 0)
                foreach (var elm in info.Data)
                {
                    var row_d = sheet.CreateRow(row_Actual++);
                    row_d.CreateCell(col_Actual).SetCellValue(elm.IdCliente.ToString().Trim());
                    col_Actual++;
                    row_d.CreateCell(col_Actual).SetCellValue(elm.FechaRegistroEmpresa.ToString().Trim()); ;
                    col_Actual++;
                    row_d.CreateCell(col_Actual).SetCellValue(elm.RazonSocial.Trim());
                    col_Actual++;
                    row_d.CreateCell(col_Actual).SetCellValue(elm.RFC.Trim());
                    col_Actual++;
                    row_d.CreateCell(col_Actual).SetCellValue(elm.Sucursal);
                    col_Actual++;
                    row_d.CreateCell(col_Actual).SetCellValue(elm.IdEmpleado.ToString());
                    col_Actual++;
                    row_d.CreateCell(col_Actual).SetCellValue(elm.Nombre);
                    col_Actual++;
                    row_d.CreateCell(col_Actual).SetCellValue(elm.Paterno);
                    col_Actual++;
                    row_d.CreateCell(col_Actual).SetCellValue(elm.Materno);
                    col_Actual++;
                    row_d.CreateCell(col_Actual).SetCellValue(elm.IdViaje.ToString());
                    col_Actual = 0;
                }
            

            #endregion

            ByteArrayOutputStream bos = new ByteArrayOutputStream();
            try
            {
                wb.Write(bos);
            }
            finally
            {
                bos.Close();
            }
            byte[] bytes = bos.ToByteArray();

            string filename = "customers.xlsx";

            var cd = new System.Net.Mime.ContentDisposition
            {
                FileName = filename,
                Inline = true,
            };

            Response.AppendHeader("Content-Disposition", cd.ToString());

            return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }
        // GET: Profile
        public ActionResult Index()
        {
            ViewBag.Lista = GetData();

            return View();
        }
        InfoCustomer GetData()
        {
            InfoCustomer info = new InfoCustomer();
            string token = GetToken();

            var client = new RestClient(WebConfigurationManager.AppSettings["Url"]);
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", "Bearer " + token);
            //request.AddHeader("Cookie", "__cfduid=d62bc70a5f219096e34a7155514ce14f51617241370");
            IRestResponse response = client.Execute(request);
            //Console.WriteLine(response.Content);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                info = jsonSerializer.Deserialize<InfoCustomer>(response.Content);
                
            }
            return info;

        }
        string GetToken()
        {

            System.Net.ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
            Token tokenInfo = new Token();
            var client = new RestClient(WebConfigurationManager.AppSettings["UrlToken"]);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            //request.AddHeader("Cookie", "__cfduid=d62bc70a5f219096e34a7155514ce14f51617241370");
            request.AddParameter("Username", WebConfigurationManager.AppSettings["Username"]);
            request.AddParameter("Password", WebConfigurationManager.AppSettings["Password"]);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                tokenInfo = jsonSerializer.Deserialize<Token>(response.Content);
                return tokenInfo.Data;
            }
            else
                return  "";
        }
        public class Token
        {
            public string Data { get; set; }
        }
        public class InfoCustomer
        {
            public List<Customer> Data { get; set; }
        }
        public class Customer
        {
            public int? IdCliente { get; set; }
            public DateTime? FechaRegistroEmpresa { get; set; }
            public string RazonSocial { get; set; }
            public string RFC { get; set; }
            public string Sucursal { get; set; }
            public int? IdEmpleado { get; set; }
            public string Nombre { get; set; }
            public string Paterno { get; set; }
            public string Materno { get; set; }
            public int? IdViaje { get; set; }
        }


    }
}
