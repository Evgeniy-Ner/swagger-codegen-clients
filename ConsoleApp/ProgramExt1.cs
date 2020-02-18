using System;
using System.Collections.Generic;
using System.Text;
using Common.Swagger;

namespace ConsoleApp
{
    partial class Program
    {
        private static void PrintSomeThingSwagger(Swagger swagger)
        {
            foreach (var path in swagger.Paths)
            {
                if (path.Request.HasParameters)
                {
                    Console.WriteLine($"[{path.Url}] {path.RequestType}");
                    foreach (var p in path.Request.parameters)
                    {
                        Console.WriteLine(p.name);
                        Console.WriteLine(p.required);
                        Console.WriteLine(p.type);
                        Console.WriteLine(p._in);

                        if (p.schema != null)
                            Console.WriteLine("-> schema: " + p.schema.Ref);
                    }
                }
            }
        }
    }
}
