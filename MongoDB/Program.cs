using MongoDB.Driver;
using MongoDBApp.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDBApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var client = new MongoClient(Settings.Default.AppConnectionString);
            var server = client.GetServer();
            var database = server.GetDatabase(Settings.Default.DatabaseName);
            var t=database.GetStats();
        }
    }
}
