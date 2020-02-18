using System;
using System.Collections.Generic;
using System.Text;

namespace Common.ModelGen
{
    public static class TypeMapper
    {
        public static string MapType(this string typeName)
        {
            var returnValue = typeName;
            switch (typeName)
            {
                case "integer":
                    returnValue = "int";
                    break;
            }

            return returnValue;
        }
    }
}
