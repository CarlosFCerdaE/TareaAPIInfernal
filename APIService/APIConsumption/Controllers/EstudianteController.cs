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
using System.Configuration;

namespace APIConsumption.Controllers
{
    public class EstudianteController : Controller
    {
        private string UrlApi = ConfigurationManager.AppSettings["UrlApi"];
        private int DuracionToken = int.Parse(ConfigurationManager.AppSettings["DuracionToken"]);
        private DateTime HoraToken;

        private bool UsuarioAutenticado()
        {
            return HttpContext.Session["token"] != null;
        }

        private bool TokenValido()
        {
            if (UsuarioAutenticado())
            {
                HoraToken = (DateTime)HttpContext.Session["horaToken"];
                return DuracionToken >= DateTime.Now.Subtract(HoraToken).Minutes;
            }
            return false;
        }
        // GET: Estudiante
        public ActionResult Index()
        {
            if (!UsuarioAutenticado() || !TokenValido())
            {
                Metodos metodos = new Metodos();
                HttpContext.Session.Add("token", metodos.ObtenerToken());
                HttpContext.Session.Add("horaToken", DateTime.Now);
            }
            return View();
        }

        public ActionResult Lista()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(UrlApi);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session["token"].ToString());

            HttpResponseMessage response = httpClient.GetAsync("/api/estudiantes").Result;
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Index", "Token");
            }
            else
            {
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