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
        private string baseURL = "http://localhost:44339/";
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

        public JsonResult Guardar(string CarreraID, string Nombre,string Facultad)
        {
            try
            {
                CarreraCLS carrera = new CarreraCLS();
                carrera.ID = CarreraID;
                carrera.NOMBRE = Nombre;
                carrera.FACULTAD = Facultad;

                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(baseURL);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string tarjetaJson = JsonConvert.SerializeObject(carrera);
                HttpContent body = new StringContent(tarjetaJson, Encoding.UTF8, "application/json");

                if (CarreraID == "" || CarreraID == null)
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
                    HttpResponseMessage response = httpClient.PutAsync($"/api/carreras/{CarreraID}", body).Result;
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

        public JsonResult Eliminar (int CarreraID)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseURL);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = httpClient.DeleteAsync($"/api/creditcards/{CarreraID}").Result;

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