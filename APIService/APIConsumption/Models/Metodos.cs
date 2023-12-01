using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Configuration;

namespace WebCRUDapi.Models
{
    public class Metodos
    {
        private string UrlToken = ConfigurationManager.AppSettings["UrlToken"];

        public string ObtenerToken()
        {


            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(UrlToken);
            var contentType = new MediaTypeWithQualityHeaderValue("application/json");
            client.DefaultRequestHeaders.Accept.Add(contentType);

            Usuario user = new Usuario();

            user.Username = "Carlos";
            user.Password = "123";

            string stringData = JsonConvert.SerializeObject(user);
            var contentData = new StringContent(stringData, Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync("/api/Token", contentData).Result;
            string stringJWT = response.Content.ReadAsStringAsync().Result;
            Token token = JsonConvert.DeserializeObject<Token>(stringJWT);

            return token.AccessToken.ToString();

        }
    }
}