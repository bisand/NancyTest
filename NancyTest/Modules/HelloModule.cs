using System;
using System.Linq;
using Biseth.Net.Settee;
using Nancy;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NancyTest.Modules
{
    public class HelloModule : NancyModule
    {
        public HelloModule()
        {
            Get["/"] = parameters =>
                {
                    var indexModel = new IndexModel
                        {
                            Username = "Unknown", 
                            CurrentDate = DateTime.Now
                        };
                    return View["Views/index.html", indexModel];
                };
            Get["/cars/{hp?}"] = parameters =>
                {
                    var database = new CouchDatabase("http://localhost:5984/");
                    using (var session = database.OpenSession("trivial"))
                    {
                        if (parameters.hp)
                        {
                            var hp = (int) parameters.hp;
                            var car = session.Query<Car>().FirstOrDefault(x => x.HorsePowers == hp);
                            return Response.AsJson(car);
                        }
                        var cars = session.Query<Car>().ToList();
                        return Response.AsJson(cars);
                    }
                };
            Get["/init/{count?100}"] = parameters =>
                {
                    var database = new CouchDatabase("http://localhost:5984/");
                    using (var session = database.OpenSession("trivial"))
                    {
                        for (var i = 0; i < parameters.count; i++)
                        {
                            var car = new Car("Saab", (93 + i).ToString(), 170 + i);
                            session.Store(car);
                        }
                        session.SaveChanges();
                    }
                    return string.Format("Generated {0} cars.", parameters.count);
                };
        }
    }

    public class IndexModel
    {
        public string Username { get; set; }
        public DateTime CurrentDate { get; set; }
    }

    public class Car
    {
        public int HorsePowers { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }

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

        #endregion
    }
}