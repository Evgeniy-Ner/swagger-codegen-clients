using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Common.ModelGen
{
    public class ModeGen
    {
        public Swagger.Swagger Swagger { get; set; }

        public IList<string> NameSpacesForSkip = new List<string>()
            { "Microsoft", "System" };

        public ModeGen(Swagger.Swagger swagger)
        {
            this.Swagger = swagger;
        }

        public void Generate()
        {
            foreach (var def in Swagger.Defenitions.OrderBy(x=>x.Key))
            {
                var name = def.Key;

                if (NameSpacesForSkip.Any(x => name.StartsWith(x)))
                    continue;

                Console.WriteLine(name);
            }
        }
    }
}
