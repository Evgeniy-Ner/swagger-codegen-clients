using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Common.ModelGen;
using Common.Swagger;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ConsoleApp
{
    partial class Program
    {
        private const string filePath = @"C:\swagger-gen\moderation-sanctions.json";
        //private const string filePath = @"C:\swagger-gen\ml-rule-engine.json";

        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            var json = File.ReadAllText(filePath);
            
            var swagger = new Swagger(json);

            //PrintSomeThingSwagger(swagger);

            var modelGen = new ModeGen(swagger);

            modelGen.Generate();

        }
    }
}
