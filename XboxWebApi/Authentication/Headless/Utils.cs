using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Xml;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using XboxWebApi.Common;

namespace XboxWebApi.Authentication.Headless
{
    public class Utils
    {
        static readonly ILogger logger = Logging.Factory.CreateLogger<Utils>();

        /// <summary>
        /// Parses a JS object
        /// </summary>
        /// <param name="definition"></param>
        /// <returns></returns>
        public static Dictionary<string,object> ParseJsObject(string definition)
        {
            logger.LogTrace("ParseJsObject(string definition) called, definition: {}", definition);
            var obj = JObject.Parse(definition);
            return obj.ToObject<Dictionary<string, object>>();
        }

        /// <summary>
        /// Extract a JS object from html body
        /// </summary>
        /// <param name="codeBody">HTML body</param>
        /// <param name="fieldName">Variable to extract (f.e. "var ServerData")</param>
        /// <returns>A collection of regex matches</returns>
        public static MatchCollection ExtractJsViaRegex(string codeBody, string fieldName)
        {
            logger.LogTrace("ExtractJsViaRegex(codeBody, fieldName) called");
            string regexPattern = fieldName + @"(?:.*?)=(?:.*?)({(?:.*?)});";
            logger.LogTrace("ExtractJsViaRegex: Regex pattern set to {}", regexPattern);

            Regex regex = new Regex(regexPattern,
                options: RegexOptions.IgnoreCase
                       | RegexOptions.Multiline
                       | RegexOptions.Singleline);
            return regex.Matches(codeBody);
        }

        /// <summary>
        /// Extract a JS object from html body and convert it to dictionary.
        /// </summary>
        /// <param name="codeBody">HTML body</param>
        /// <param name="fieldName">Variable to extract (f.e. "var ServerData")</param>
        /// <returns>Returns the JS object as a dictionary</returns>
        public static Dictionary<string,object> ExtractJsObject(string codeBody, string fieldName)
        {
            logger.LogTrace("ExtractJsObject called");
            var matches = ExtractJsViaRegex(codeBody, fieldName);
            if (matches.Count <= 0)
            {
                logger.LogError("ExtractJsObject: No matches for {} in string: {}", fieldName, codeBody);
                throw new DataMisalignedException(
                    String.Format("ExtractJsObject: JS object not found: {}", fieldName));
            }
            else if (matches.Count > 1)
            {
                logger.LogWarning("ExtractJsObject: Got than a single regex-match");
            }
            
            string match = matches[0].Groups[1].Value;
            logger.LogDebug("ExtractJsObject: Matched string: {}", match);

            var parsed = ParseJsObject(match);
            return parsed;
        }

        /// <summary>
        /// Parses XML / HTML node from string
        /// </summary>
        /// <param name="htmlNode">HTML / XML node to parse</param>
        /// <returns>Returns the parsed representation of the node</returns>
        public static XmlDocument ParseXmlNode(string htmlNode)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(htmlNode);
            return doc;
        }
    }
}