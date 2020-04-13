﻿using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dm_backend.Models;

namespace dm_backend.Utilities
{
    public class ShouldSerializeContractResolver : DefaultContractResolver
    {

        public new NamingStrategy NamingStrategy =  new CamelCaseNamingStrategy()
            {
                OverrideSpecifiedNames = false
            };
        
        protected override JsonProperty CreateProperty(System.Reflection.MemberInfo member, Newtonsoft.Json.MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);
            if (property.DeclaringType == typeof(BaseEntity) || property.DeclaringType.BaseType == typeof(BaseEntity))
            {
                if (property.PropertyName == "serializableProperties")
                {
                    property.ShouldSerialize = instance => { return false; };
                }
                else
                {
                    property.ShouldSerialize = instance =>
                    {
                        var p = (User)instance;
                        return p.serializableProperties.Contains(property.PropertyName);
                    };
                }
            }
            return property;
        }
    }
}
