using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace EstudoMongoDB
{
    class Program
    {
        static void Main(string[] args)
        {
            //connection string na porta padrão do mongodb
            string connectionString = "mongodb://localhost:27017";

            MongoClient client = new MongoClient(connectionString);

            //nome da base de dados, se não existir antes do primeiro insert
            //é criada automaticamente
            IMongoDatabase db = client.GetDatabase("ExemploCSharp");

            //listando todos os itens da lista
            //List<Cidade> listCidades = db.GetCollection<Cidade>("cidades").Find(_ => true).ToList();

            //buscando item específico
            //Cidade buscaCidade = db.GetCollection<Cidade>("cidades").Find(c => c.Nome == "Roma").ToList().First();

            var cidades = db.GetCollection<Cidade>("cidades");
            //atualizando um documento
            //Cidade atualizaCidade = cidades.Find(c => c.Nome == "Sao Paulo").ToList().First();
            //atualizaCidade.Estado = "RJ";

            //ReplaceOneResult estadoReplace = cidades.ReplaceOne(c => c._id == atualizaCidade._id, atualizaCidade);

            //deletar documento
            //DeleteResult estadoDelete = cidades.DeleteOne(c => c.Nome == "Roma");

            //adiciona novo item
            //Cidade cidade = new Cidade();
            //cidade.Nome = "Charles Lomboni";
            //cidade.Estado = "Rio de Janeiro";
            //cidade.Pais = "Brasil";

            //cidades.InsertOne(cidade);

            //recupera ID da inclusão acima
            //ObjectId _idInclusao = cidade._id;
        }
    }
}
