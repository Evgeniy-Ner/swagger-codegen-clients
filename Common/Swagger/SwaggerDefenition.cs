using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Common.Swagger
{
    [DataContract]
    public class SwaggerDefenition
    {
        public string Name { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "properties")]
        public IList<Property> Properties { get; set; } = new List<Property>();

        public IList<string> Requireds { get; set; }
    }

    [DataContract]
    public class Property
    {
        public string Name { get; set; }

        public string Format { get; set; }

        public string Description { get; set; }

        public string Type { get; set; }

        public bool ReadOnly { get; set; }

        public string Ref { get; set; }

        public Dictionary<string, string> Info = new Dictionary<string, string>();

        public Items Items { get; set; } = new Items();
    }

    public class Items
    {
        public IList<string> Enums { get; set; }

        public string Format { get; set; }

        public string Type { get; set; }

        public string Ref { get; set; }

        public string Title { get; set; }
    }
}
