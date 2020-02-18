using System.Collections.Generic;
using System.IO;

namespace Common.ModelGen
{
    public partial class ModeGen
    {
        public string outputFolder { get; set; } = @"C:\code-gen-output";

        public Swagger.Swagger Swagger { get; set; }

        public IList<string> NameSpacesForSkip = new List<string>()
            { "Microsoft", "System" };

        public ModeGen(Swagger.Swagger swagger)
        {
            this.Swagger = swagger;
        }

        public string GetPath(string path)
        {
            return Path.Combine(outputFolder, path);
        }

        public string GetPath(string path, string fileName)
        {
            return Path.Combine(GetPath(path), fileName);
        }

        public string GetPathExceptFile(string path, string fileName)
        {
            return path.Substring(0, path.Length - fileName.Length - 1);
        }

        public string ToPascalCase(string str)
        {
            return char.ToUpper(str[0]) + str.Substring(1);
        }
    }
}
