using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AspNetWebAPI.Models;

namespace AspNetWebAPI.Controllers
{
    [RoutePrefix("api/testproject")]
    public class DefaultController : ApiController
    {
        [HttpGet]
        [Route("datahora/consulta")]
        public HttpResponseMessage GetServerDateTime()
        {
            try
            {
                var dateTime = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
                return Request.CreateResponse(HttpStatusCode.OK, dateTime);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpGet]
        [Route("consulta/cliente/{id:int}")]
        public HttpResponseMessage GetClientById(int id)
        {
            try
            {
                var clients = new[]
                {
                    new { Id = 1, Nome = "Pedro", DataNascimento = new DateTime(1954, 2, 1) },
                    new { Id = 2, Nome = "Paulo", DataNascimento = new DateTime(1944, 4, 12) },
                    new { Id = 3, Nome = "Fernando", DataNascimento = new DateTime(1963, 5, 9) },
                    new { Id = 4, Nome = "Maria", DataNascimento = new DateTime(1984, 4, 30) },
                    new { Id = 5, Nome = "João", DataNascimento = new DateTime(1990, 3, 14) },
                    new { Id = 6, Nome = "Joana", DataNascimento = new DateTime(1974, 6, 19) }
                };

                var client = clients.Where(x => x.Id == id).FirstOrDefault();

                if (client == null) throw new Exception("Cliente não encontrado.");
                return Request.CreateResponse(HttpStatusCode.OK, client);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }

        [HttpPost]
        [Route("cadastrar")]
        public HttpResponseMessage CreatePost(Cliente cliente)
        {
            try
            {
                return Request.CreateResponse(HttpStatusCode.OK, string.Format("Cadastro do usuário {0} realizado.", cliente.Nome));
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message);
            }
        }
    }
}
