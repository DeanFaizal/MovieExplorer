using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieExplorer.Core.BusinessLayer
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            return value.GetAttribute(typeof(EnumDescription));
        }
        
        public static string GetAttribute(this Enum value, Type attributeType)
        {
            var fieldInfo = value.GetType().GetRuntimeField(value.ToString());
            ValueAttribute[] attributes = (ValueAttribute[])fieldInfo.GetCustomAttributes(attributeType, false);

            if (attributes != null && attributes.Length > 0)
            {
                return attributes[0].Value;
            }
            else
            {
                return value.ToString();
            }
        }
    }

    public class ValueAttribute : Attribute
    {
        public string Value { get; set; }
    }

    public class EnumDescription : ValueAttribute
    {
        public EnumDescription(string description)
        {
            Value = description;
        }
    }
}
