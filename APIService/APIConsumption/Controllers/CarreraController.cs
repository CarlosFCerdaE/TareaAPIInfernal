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
    public class CarreraController : Controller
    {
        private string baseURL = "https://localhost:44339/";
        // GET: Carrera
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Lista()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseURL);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = httpClient.GetAsync("/api/carreras").Result;
            string data = response.Content.ReadAsStringAsync().Result;
            List<CarreraCLS> carreras = JsonConvert.DeserializeObject<List<CarreraCLS>>(data);

            return Json(
                new
                {
                    success = true,
                    data = carreras,
                    message = "done"
                },
                JsonRequestBehavior.AllowGet
                );
        }

        public JsonResult Guardar(string ID, string NOMBRE,string FACULTAD)
        {
            
            try
            {
                CarreraCLS carrera = new CarreraCLS();
                carrera.ID = ID;
                carrera.NOMBRE = NOMBRE;
                carrera.FACULTAD = FACULTAD;

                Console.WriteLine("id " + carrera.ID + " nombre " + carrera.NOMBRE + " facultad " + carrera.FACULTAD);

                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(baseURL);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string carreraJson = JsonConvert.SerializeObject(carrera);
                HttpContent body = new StringContent(carreraJson, Encoding.UTF8, "application/json");

                HttpResponseMessage findIdResponse = httpClient.GetAsync($"/api/carreras/{ID}").Result;

                if (!findIdResponse.IsSuccessStatusCode)
                {
                    HttpResponseMessage response = httpClient.PostAsync("/api/carreras", body).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return Json(
                            new
                            {
                                success = true,
                                message = "Carrera creada satisfactoriamente"
                            }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    HttpResponseMessage response = httpClient.PutAsync($"/api/carreras/{ID}", body).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return Json(
                            new
                            {
                                success = true,
                                message = "Carrera modificada satisfactoriamente"
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

        public JsonResult Eliminar (string ID)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseURL);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = httpClient.DeleteAsync($"/api/carreras/{ID}").Result;

            if (response.IsSuccessStatusCode)
            {
                return Json(
                    new
                    {
                        success = true,
                        message = "Carrera eliminada satisfactoriamente"
                    }, JsonRequestBehavior.AllowGet);
            }

            throw new Exception("Error al eliminar");
        }
    }
}