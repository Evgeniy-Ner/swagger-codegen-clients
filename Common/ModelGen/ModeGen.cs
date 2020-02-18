using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Common.ModelGen
{
    public partial class ModeGen
    {
        public string TemplatePath = @"C:\template\template.cs";

        public string GetTemplateModel()
        {
            return File.ReadAllText(this.TemplatePath);
        }

        public void Generate()
        {
            foreach (var def in Swagger.Defenitions.OrderBy(x=>x.Key))
            {
                var name = def.Key;

                name = Regex.Replace(name, @"\[(.*)\]", "");

                if (NameSpacesForSkip.Any(x => name.StartsWith(x)))
                    continue;

                var fileName = name.Substring(name.LastIndexOf('.') + 1, name.Length - name.LastIndexOf('.') - 1);

                var path = GetPathExceptFile(name, fileName);

                path = path.Replace(".", @"\");

                if (!Directory.Exists(this.GetPath(path)))
                {
                    Directory.CreateDirectory(this.GetPath(path));
                }

                using (var file = File.CreateText($"{GetPath(path, fileName)}.cs"))
                {
                    file.WriteLine();
                };

                Console.WriteLine($"{name} - {fileName}");

                Console.WriteLine(Environment.NewLine);

                var newFile = this.GetTemplateModel();

                newFile = newFile.Replace("#NameSpace", name);
                newFile = newFile.Replace("#ClassName", fileName);
                newFile = newFile.Replace("#ClassSummary", def.Value.Description);
                newFile = newFile.Replace("#AdditionalNamepsaces\r\n", "");

                Console.WriteLine($"props: {def.Value.Properties.Count}");

                var properties = new StringBuilder();

                foreach (var prop in def.Value.Properties)
                {
                    var property = new StringBuilder();

                    var propertySummary = "\t/// <summary>\n\t/// #propertySummary\n\t/// </summary>\n";

                    propertySummary = propertySummary.Replace("#propertySummary", prop.Description);

                    property.Append(propertySummary);


                    var datamember = $"\t[DataMember(Name = \"{prop.Name}\")]";

                    property.Append(datamember + Environment.NewLine + "\t");
                    
                    var type = "";
                    switch (prop.Type)
                    {
                        case "array":
                            type = "public IList<#itemType> #propertyName {get; set;}";
                            type = type.Replace("#itemType", prop.Items.Type.MapType());
                            type = type.Replace("#propertyName", this.ToPascalCase(prop.Name));
                            break;

                    }
                    property.Append(type);

                    Console.WriteLine($"{prop.Name}");
                    Console.WriteLine($"{prop.Type}");
                    Console.WriteLine($"{prop.Description}");

                    if (prop.Items != null)
                    {
                        Console.WriteLine($"{prop.Items.Type}");
                    }

                    properties.Append(property);

                    Console.WriteLine($"{prop.Items}");
                }

                newFile = newFile.Replace("#Properties", properties.ToString());

                Console.WriteLine(newFile);

                

                Console.ReadKey();
            }
        }
    }
}
