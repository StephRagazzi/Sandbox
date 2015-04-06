using MongoDB.Bson;
using MongoRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace MongoDBApp
{
    public class Model : Entity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //public XmlDocument config { get; set; }
        public string config { get; set; }
        public ObjectId xmlFileRef { get; set; }
    }
}
