using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RedditStoreApp
{
    public static class ExtensionMethods
    {
        public static string GetDescription(this Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);

            FieldInfo field = type.GetRuntimeField(name);
            DisplayAttribute disp = field.GetCustomAttribute<DisplayAttribute>();

            if (disp != null)
            {
                return disp.Name;
            }
            else
            {
                return name;
            }
        }
    }
}
