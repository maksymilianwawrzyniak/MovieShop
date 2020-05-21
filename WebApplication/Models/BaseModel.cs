using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace WebApplication.Models
{
    public abstract class BaseModel
    {
        public string Id { get; set; }

        public IEnumerable<string> Properties => PropertyDictionary.Keys;

        private Dictionary<string, PropertyInfo> PropertyDictionary { get; } = new Dictionary<string, PropertyInfo>();


        protected BaseModel()
        {
            var propertyInfos = GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var propertyNames = propertyInfos.Select(x => x.Name);
            foreach (var name in propertyNames)
            {
                if (string.Equals(name, nameof(Properties), StringComparison.InvariantCultureIgnoreCase))
                    continue;
                if (string.Equals(name, nameof(Id), StringComparison.InvariantCultureIgnoreCase))
                    continue;
                PropertyDictionary.Add(name, propertyInfos.First(
                    x => string.Equals(x.Name, name, StringComparison.InvariantCultureIgnoreCase)
                ));
            }
        }

        public object GetPropertyValueByName(string name)
        {
            return !PropertyDictionary.TryGetValue(name, out var propertyInfo) ? null : propertyInfo.GetValue(this);
        }
    }
}