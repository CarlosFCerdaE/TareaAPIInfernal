﻿using System;
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
    public class LibroController : Controller
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

            HttpResponseMessage response = httpClient.GetAsync("/api/libros").Result;
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                return RedirectToAction("Index", "Token");
            }
            else
            {
                string data = response.Content.ReadAsStringAsync().Result;
                List<LibroCLS> libros = JsonConvert.DeserializeObject<List<LibroCLS>>(data);

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
        }

        public ActionResult Guardar(string ISBN, string NOMBRE,string EDITORIAL, string AUTOR)
        {
            
            try
            {
                if (!UsuarioAutenticado() || !TokenValido())
                {
                    Metodos metodos = new Metodos();
                    HttpContext.Session.Add("token", metodos.ObtenerToken());
                    HttpContext.Session.Add("horaToken", DateTime.Now);
                }
                LibroCLS libro = new LibroCLS();
                libro.ISBN= ISBN;
                libro.NOMBRE= NOMBRE;
                libro.EDITORIAL=EDITORIAL;
                libro.AUTOR= AUTOR;

                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(UrlApi);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session["token"].ToString());

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
                    else
                    {
                        return RedirectToAction("Index", "Token");
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
            if (!UsuarioAutenticado() || !TokenValido())
            {
                Metodos metodos = new Metodos();
                HttpContext.Session.Add("token", metodos.ObtenerToken());
                HttpContext.Session.Add("horaToken", DateTime.Now);
            }
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(UrlApi);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", HttpContext.Session["token"].ToString());

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