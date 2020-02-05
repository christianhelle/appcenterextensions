using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace AppCenterExtensions.Extensions
{
    /// <summary>
    /// Exposes extension methods for the object class
    /// </summary>
    public static class ObjectExtensions
    {
        /// <summary>
        /// Gets a string dictionary with key/values that represent
        /// the properties exposed by the specified argument 
        /// </summary>
        /// <param name="obj">Any complex object</param>
        /// <returns>Returns a string dictionary</returns>
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