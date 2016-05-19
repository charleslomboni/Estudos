using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;

namespace EstudoMongoDB
{
    class Cidade
    {
        public ObjectId _id { get; set; }
        public string Nome { get; set; }
        public string Estado { get; set; }
        public string Pais { get; set; }
    }
}
