using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer
{
    public class HelperClasses
    {
        public static IEnumerable<string> GetGenericEnumDescription(Type enumValue)
        {
            List<string> descriptions = new List<string>();
            string[] names = Enum.GetNames(enumValue);

            foreach (string name in names)
            {
                FieldInfo? field = enumValue.GetField(name);
                DescriptionAttribute[] descriptionAttributes = (DescriptionAttribute[])field.GetCustomAttributes(typeof(DescriptionAttribute), true);
                foreach (DescriptionAttribute attribute in descriptionAttributes)
                {
                    descriptions.Add(attribute.Description);
                } // foreach
            } // foreach

            return descriptions;
        }
    }
}
