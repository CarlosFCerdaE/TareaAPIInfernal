using Microsoft.IdentityModel.Tokens;
using System;
using System.Configuration;
using System.Net;
using System.Text;
using System.Web.Http;
using APIToken.Models;
using System.IdentityModel.Tokens.Jwt;
using DAL;
using DAL.Clases;

namespace APIToken.Controllers
{
    public class TokenController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Authenticate(Usuario Usuario)
        {
            if (Usuario == null)
                throw new HttpResponseException(HttpStatusCode.BadRequest);

            Nullable<bool> result = APISecurity.UsuarioValidar(Usuario.Username, Usuario.Password);


            bool isCredentialValid = ((bool)result);

            if (isCredentialValid)
            {
                var token = GenerateTokenJWT();
                return Ok(token);
            }
            else
            {
                return Unauthorized();
            }
        }

        private Token GenerateTokenJWT()
        {
            var now = DateTime.UtcNow;

            //appsetting for Token JWT
            var secretKey = ConfigurationManager.AppSettings["JWT_SECRET_KEY"];
            var audienceToken = ConfigurationManager.AppSettings["JWT_AUDIENCE_TOKEN"];
            var issuerToken = ConfigurationManager.AppSettings["JWT_ISSUER_TOKEN"];
            var expireTime = ConfigurationManager.AppSettings["JWT_EXPIRE_MINUTES"];

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //create token
            var tokenHandler = new JwtSecurityTokenHandler();

            var jwtSecurityToken = tokenHandler.CreateJwtSecurityToken(
                audience: audienceToken,
                issuer: issuerToken,
                notBefore: now,
                expires: now.AddMinutes(Convert.ToInt32(expireTime)),
                signingCredentials:signingCredentials
                );

            var jwtTokenString = tokenHandler.WriteToken(jwtSecurityToken);

            Token token = new Token();
            token.AccessToken = jwtTokenString;
            token.ExpiresIn = Convert.ToInt32(expireTime)*60;

            return token;

        }
    }
}
