using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using APIConsumption.Models;
using Newtonsoft.Json;
using System.Text;

namespace APIConsumption.Controllers
{
    public class EstudianteController : Controller
    {
        private string baseURL = "https://localhost:44339/";
        // GET: Estudiante
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Lista()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseURL);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = httpClient.GetAsync("/api/estudiantes").Result;
            string data = response.Content.ReadAsStringAsync().Result;
            List<EstudianteCLS> estudiantes = JsonConvert.DeserializeObject<List<EstudianteCLS>>(data);

            return Json(
                new
                {
                    success = true,
                    data = estudiantes,
                    message = "done"
                },
                JsonRequestBehavior.AllowGet
                );
        }

        public JsonResult Guardar(string CIF, string NOMBRE,string APELLIDO,string CARRERA)
        {
            
            try
            {
                EstudianteCLS estudiante = new EstudianteCLS();
                estudiante.CIF= CIF;
                estudiante.NOMBRE= NOMBRE;
                estudiante.APELLIDO= APELLIDO;
                estudiante.CARRERA= CARRERA;

                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(baseURL);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string estudianteJson = JsonConvert.SerializeObject(estudiante);
                HttpContent body = new StringContent(estudianteJson, Encoding.UTF8, "application/json");

                HttpResponseMessage findIdResponse = httpClient.GetAsync($"/api/estudiantes/{CIF}").Result;

                if (!findIdResponse.IsSuccessStatusCode)
                {
                    HttpResponseMessage response = httpClient.PostAsync("/api/estudiantes", body).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return Json(
                            new
                            {
                                success = true,
                                message = "Estudiante creado satisfactoriamente"
                            }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    HttpResponseMessage response = httpClient.PutAsync($"/api/estudiantes/{CIF}", body).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return Json(
                            new
                            {
                                success = true,
                                message = "Estudiante modificado satisfactoriamente"
                            }, JsonRequestBehavior.AllowGet);
                    }
                }
                throw new Exception("Error al guardar");
            }

            catch (Exception ex)
            {
                return Json(
                    new
                    {
                        success = false,
                        message = ex.InnerException
                    }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult Eliminar (string CIF)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseURL);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = httpClient.DeleteAsync($"/api/estudiantes/{CIF}").Result;

            if (response.IsSuccessStatusCode)
            {
                return Json(
                    new
                    {
                        success = true,
                        message = "Estudiante eliminado satisfactoriamente"
                    }, JsonRequestBehavior.AllowGet);
            }

            throw new Exception("Error al eliminar");
        }
    }
}