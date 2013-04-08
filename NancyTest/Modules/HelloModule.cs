using System.Collections.Generic;
using System.Linq;
using Divan;
using Nancy;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NancyTest.Modules
{
    public class HelloModule : NancyModule
    {
        public HelloModule()
        {
            Get["/"] = parameters => "Hello World";
            Get["/cars/{hp?}"] = parameters =>
                {
                    var server = new CouchServer();
                    var db = server.GetDatabase("trivial");
                    var tempView = db.NewTempView("test", "test", "if (doc.docType && doc.docType == 'car') emit(doc.Hps, doc);");
                    var linqCars = tempView.LinqQuery<Car>();
                    if (parameters.hp)
                    {
                        var hp = (int) parameters.hp;
                        var car = linqCars.Where(c => c.HorsePowers == hp).ToList();
                        return Response.AsJson(car);
                    }
                    var cars = linqCars.Where(x=>x.HorsePowers <= 500).ToList();
                    return Response.AsJson(cars);
                };
            Get["/init/{count?100}"] = parameters =>
                {
                    var server = new CouchServer();
                    var db = server.GetDatabase("trivial");
                    for (var i = 0; i < parameters.count; i++)
                    {
                        var car = new Car("Saab", "93", 170 + i);
                        db.SaveDocument(car);
                    }
                    return string.Format("Generated {0} cars.", parameters.count);
                };
        }
    }

    public class Car : CouchDocument
    {
        public string Make;
        public string Model;
        public int HorsePowers;

        public Car()
        {
            // This constructor is needed by Divan
        }

        public Car(string make, string model, int hps)
        {
            Make = make;
            Model = model;
            HorsePowers = hps;
        }
        #region CouchDocument Members

        public override void WriteJson(JsonWriter writer)
        {
            // This will write id and rev
            base.WriteJson(writer);

            writer.WritePropertyName("docType");
            writer.WriteValue("car");
            writer.WritePropertyName("Make");
            writer.WriteValue(Make);
            writer.WritePropertyName("Model");
            writer.WriteValue(Model);
            writer.WritePropertyName("Hps");
            writer.WriteValue(HorsePowers);
        }

        public override void ReadJson(JObject obj)
        {
            // This will read id and rev
            base.ReadJson(obj);

            Make = obj["Make"].Value<string>();
            Model = obj["Model"].Value<string>();
            HorsePowers = obj["Hps"].Value<int>();
        }

        #endregion
    }

}