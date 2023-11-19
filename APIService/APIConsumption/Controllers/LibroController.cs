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
    public class LibroController : Controller
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

            HttpResponseMessage response = httpClient.GetAsync("/api/libros").Result;
            string data = response.Content.ReadAsStringAsync().Result;
            List<LibroCLS> libros= JsonConvert.DeserializeObject<List<LibroCLS>>(data);

            return Json(
                new
                {
                    success = true,
                    data = libros,
                    message = "done"
                },
                JsonRequestBehavior.AllowGet
                );
        }

        public JsonResult Guardar(string ISBN, string NOMBRE,string EDITORIAL, string AUTOR)
        {
            
            try
            {
                LibroCLS libro = new LibroCLS();
                libro.ISBN= ISBN;
                libro.NOMBRE= NOMBRE;
                libro.EDITORIAL=EDITORIAL;
                libro.AUTOR= AUTOR;

                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(baseURL);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string libroJson = JsonConvert.SerializeObject(libro);
                HttpContent body = new StringContent(libroJson, Encoding.UTF8, "application/json");

                HttpResponseMessage findIdResponse = httpClient.GetAsync($"/api/libros/{ISBN}").Result;

                if (!findIdResponse.IsSuccessStatusCode)
                {
                    HttpResponseMessage response = httpClient.PostAsync("/api/libros", body).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return Json(
                            new
                            {
                                success = true,
                                message = "Libro creado satisfactoriamente"
                            }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    HttpResponseMessage response = httpClient.PutAsync($"/api/libros/{ISBN}", body).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return Json(
                            new
                            {
                                success = true,
                                message = "Libro modificado satisfactoriamente"
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

        public JsonResult Eliminar (string ISBN)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseURL);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = httpClient.DeleteAsync($"/api/libros/{ISBN}").Result;

            if (response.IsSuccessStatusCode)
            {
                return Json(
                    new
                    {
                        success = true,
                        message = "Libro eliminado satisfactoriamente"
                    }, JsonRequestBehavior.AllowGet);
            }

            throw new Exception("Error al eliminar");
        }
    }
}