using System;
using XboxWebApi.Common;

namespace XboxWebApi.Extensions
{
    public interface IStringable
    {
    }

    public static class StringableExtensions
    {
        public static string Stringify(this object instance)
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(instance);
            return $"<{instance.GetType().Name} {json}>";
        }
    }
}