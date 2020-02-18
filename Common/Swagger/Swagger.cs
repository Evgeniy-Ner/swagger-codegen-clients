using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Common.Swagger
{
    public class Swagger
    {
        public IList<SwaggerPath> Paths;

        public Dictionary<string, SwaggerDefenition> Defenitions;

        public Swagger(string json)
        {
            var jObject = JObject.Parse(json);

            this.InitPaths(jObject);
            this.InitDefenitions(jObject);
        }

        private void InitDefenitions(JObject jObject)
        {
            this.Defenitions = new Dictionary<string, SwaggerDefenition>();

            var values = jObject["definitions"];

            var values1 = values.ToObject<Dictionary<string, object>>();

            foreach (var val in values1)
            {
                var swaggerDefenition = new SwaggerDefenition();
                swaggerDefenition.Name = val.Key;

                // Console.WriteLine($"< {val.Key} >");

                var j2 = JObject.Parse(val.Value.ToString());

                var values2 = j2.Properties();


                var properties = new List<Property>();

                foreach (var v in values2)
                {
                    if (v.Name == "properties")
                    {
                        var property = new Property();


                        // Console.WriteLine($"[{v.Name}]");

                        var str = v.Value.ToString();
                        var j3 = JObject.Parse(str);

                        var props = j3.Properties();

                        foreach (var p in props)
                        {
                            {
                                var j4 = JObject.Parse(p.Value.ToString());

                                var values3 = j4.Properties();

                                property.Name = p.Name;

                                //Console.WriteLine($"\t[{p.Name}]");
                                foreach (var val3 in values3)
                                {
                                    
                                    if (val3.Name == "items")
                                    {
                                        var j5 = JObject.Parse(val3.Value.ToString());

                                        foreach (var prop1 in j5.Properties())
                                        {
                                            switch (prop1.Name)
                                            {
                                                case "format":
                                                    property.Items.Format = prop1.Value.ToString();
                                                    break;
                                                case "type":
                                                    property.Items.Type = prop1.Value.ToString();
                                                    break;
                                                case "enum":
                                                    property.Items.Enums = JsonConvert.DeserializeObject<IList<string>>(prop1.Value.ToString());
                                                    break;
                                                case "$ref":
                                                    property.Items.Ref = prop1.Value.ToString();
                                                    break;
                                                case "title":
                                                    property.Items.Title = prop1.Value.ToString();
                                                    break;
                                                default:
                                                    // Console.WriteLine($"\t\t ^[{val3.Name}] {prop1.Name} - {prop1.Value}");
                                                    // Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                                                    // Console.ReadKey();
                                                    break;
                                            }


                                            //Console.WriteLine($"\t\t ^[{val3.Name}] {prop1.Name} - {prop1.Value}");
                                        }



                                        //Console.ReadKey();
                                    }
                                    else
                                    {
                                        switch (val3.Name)
                                        {
                                            case "type":
                                                property.Type = val3.Value.ToString();
                                                break;
                                            case "description":
                                                property.Description = val3.Value.ToString();
                                                break;
                                            case "format":
                                                property.Format = val3.Value.ToString();
                                                break;
                                            case "readOnly":
                                                property.ReadOnly = (bool) val3.Value;
                                                break;
                                            case "$ref":
                                                property.Ref = val3.Value.ToString();
                                                break;
                                            default:
                                                property.Info[val3.Name] = val3.Value.ToString();
                                                break;
                                        }

                                        //Console.WriteLine($"\t * {val3.Name} - {val3.Value}");
                                    }
                                }
                            }
                        }

                        swaggerDefenition.Properties.Add(property);

                    }
                    else
                    {
                        switch (v.Name)
                        {
                            case "type":
                                swaggerDefenition.Type = v.Value.ToString();
                                break;
                            case "description":
                                swaggerDefenition.Description = v.Value.ToString();
                                break;
                            case "required":
                                swaggerDefenition.Requireds = JsonConvert.DeserializeObject<IList<string>>(v.Value.ToString());
                                break;

                        }
                        //Console.WriteLine($"+ [{v.Name} - {v.Value}]");
                    }
                }
                this.Defenitions[val.Key] = swaggerDefenition;
            }
        }

        private void InitPaths(JObject jObject)
        {
            var paths = jObject["paths"];

            this.Paths = paths.Values().Select(x =>
            {
                var path = JsonConvert.DeserializeObject<SwaggerPath>(x.ToString());
                path.Url = x.Path.Substring(7, x.Path.Length - 9);

                this.InitSchema(path, x);

                return path;
            }).ToList();
        }

        private void InitSchema(SwaggerPath path, JToken x)
        {
            if (path.Request.parameters?.Any() == true && path.RequestType == ReqeustType.Post)
            {
                var _ref = x?["post"]?["parameters"]?[0]?["schema"]?["$ref"]?.Value<string>();

                if (_ref != null)
                    for (var i = 0; i < path.Request.parameters.Length; i++)
                    {
                        var r = x["post"]["parameters"][i]["schema"]["$ref"].Value<string>();
                        path.Request.parameters[i].schema = new Schema()
                        {
                            Ref = r.Substring(14, r.Length - 14)
                        };
                    }
            }
        }
    }
}
