using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using EasyNetQ;

namespace RabbitMq.Test.WebAPI.Controllers
{
    public class OrderController : ApiController
    {
        public HttpResponseMessage Post([FromBody] TeslaOrder order)
        {
            var messageBus = RabbitHutch.CreateBus("host=localhost");

            using (var publishChannel = messageBus.OpenPublishChannel())
            {
                publishChannel.Publish(order);
            }

            return Request.CreateResponse(HttpStatusCode.Created);
        }
    }

    public class TeslaOrder
    {
    }
}