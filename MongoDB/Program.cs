using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDBApp.Properties;
using MongoRepository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MongoDBApp
{
    class Program
    {
        static MongoRepository<Model> repository = new MongoRepository<Model>();

        static void Main(string[] args)
        {
            //ConnectFromSettings();
            //ConnectFromAppConfig();
            ConnectFromSettingsGridFs();

        }

        static void ConnectFromSettings()
        {
            var client = new MongoClient(Settings.Default.AppConnectionString);
            var server = client.GetServer();
            var database = server.GetDatabase(Settings.Default.DatabaseName);
            var t = database.GetStats();

            var model = new Model() { FirstName = "Jane", LastName = "Doe" };
            var collection = database.GetCollection<Model>("model");
            var write = collection.Insert(model);

        }

        static void ConnectFromAppConfig()
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\"?><root><product>Product</product></root>");

            var john = new Model() { FirstName = "John", LastName = "Doe", config = xmlDoc.InnerXml };
            var jane = new Model() { FirstName = "Jane", LastName = "Doe", config = xmlDoc.InnerXml };
            var jerry = new Model() { FirstName = "Jerry", LastName = "Maguire", config = xmlDoc.InnerXml };
            repository.Add(new[] { john, jane, jerry });

            //Show contents of DB
            //DumpData();

        }

        static void ConnectFromSettingsGridFs()
        {
            var client = new MongoClient(Settings.Default.AppConnectionString);
            var server = client.GetServer();
            var database = server.GetDatabase(Settings.Default.DatabaseName);

            var fileName = "test1.xml";
            var newFileName = "testRES.xml";
            using (var fs = new FileStream(fileName, FileMode.Open))
            {
                var gridFsInfo = database.GridFS.Upload(fs, fileName);
                var fileId = gridFsInfo.Id.AsObjectId;

                var model = new Model() { FirstName = "Jane", LastName = "Doe", xmlFileRef = fileId };
                var collection = database.GetCollection<Model>("model");
                var write = collection.Insert(model);

                var file = database.GridFS.FindOne(Query.EQ("_id", fileId));

                using (var stream = file.OpenRead())
                {
                    var bytes = new byte[stream.Length];
                    stream.Read(bytes, 0, (int)stream.Length);
                    using (var newFs = new FileStream(newFileName, FileMode.Create))
                    {
                        newFs.Write(bytes, 0, bytes.Length);
                    }
                }

            }

        }
    }
}
