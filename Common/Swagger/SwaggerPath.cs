using System.Runtime.Serialization;

namespace Common.Swagger
{

    public enum ReqeustType
    {
        [EnumMember(Value = "get")]
        Get = 1,

        [EnumMember(Value = "post")]
        Post = 2,
    }

    [DataContract]
    public class SwaggerPath
    {
        public string Url { get; set; }

        [DataMember(Name = "get")]
        public Request Get { get; set; }

        [DataMember(Name = "post")]
        public Request Post { get; set; }

        
        public Request Request => Get ?? this.Post;

        
        public ReqeustType RequestType => Get != null ? ReqeustType.Get : ReqeustType.Post;
    }

    public class Request
    {
        public string[] tags { get; set; }
        public string summary { get; set; }
        public string operationId { get; set; }
        public object[] consumes { get; set; }
        public string[] produces { get; set; }
        public Parameter[] parameters { get; set; }
        public Responses responses { get; set; }

        public bool HasParameters => parameters != null;
    }

    public class Responses
    {
        public _200 _200 { get; set; }
    }

    public class _200
    {
        public string description { get; set; }
        public Schema schema { get; set; }
    }

    [DataContract]
    public class Schema
    {
        [DataMember(Name = "$ref")]
        public string Ref { get; set; }
    }

    [DataContract]
    public class Parameter
    {
        [DataMember(Name = "name")]
        public string name { get; set; }

        [DataMember(Name = "in")]
        public string _in { get; set; }

        [DataMember(Name = "request")]
        public bool required { get; set; }

        [DataMember(Name = "type")]
        public string type { get; set; }

        [DataMember(Name = "format")]
        public string format { get; set; }

        [DataMember(Name = "schema")]
        public Schema schema { get; set; }
    }




}
