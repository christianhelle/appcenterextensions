using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ChristianHelle.DeveloperTools.AppCenterExtensions.Extensions
{
    public static class ObjectExtensions
    {
        public static IDictionary<string, string> ToDictionary(this object obj)
        {
            var dictionary = new Dictionary<string, string>();
            ActionExtensions.SafeInvoke(() => ReadObjectProperties(obj, dictionary));
            return dictionary;
        }

        private static void ReadObjectProperties(object obj, IDictionary<string, string> dictionary)
        {
            var objProperties = obj.GetType().GetProperties();
            foreach (var dataProperty in objProperties.Where(c => !string.IsNullOrWhiteSpace(c.Name)))
            {
                if (Attribute.IsDefined(dataProperty, typeof(IgnoreDataMemberAttribute)))
                    continue;
                ActionExtensions.SafeInvoke(() =>
                {
                    var value = dataProperty.GetValue(obj).ToString();
                    dictionary.Add(dataProperty.Name, value);
                });
            }
        }
    }
}