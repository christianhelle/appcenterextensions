using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace ChristianHelle.DeveloperTools.AppCenterExtensions.Extensions
{
    public static class ObjectExtensions
    {
        private static readonly JsonSerializerSettings Settings
            = new JsonSerializerSettings()
            {
                Converters = new List<JsonConverter> { new StringEnumConverter() }
            };

        public static string ToJson(this object obj)
            => obj != null
                ? JsonConvert.SerializeObject(obj, Settings)
                : null;

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
                    dictionary.Add(
                        dataProperty.Name,
                        dataProperty.GetValue(obj) as string ?? dataProperty.GetValue(obj).ToJson()));
            }
        }
    }
}