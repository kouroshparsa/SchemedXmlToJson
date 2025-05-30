using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace SchemedXmlToJson
{
    public class XmlToJsonConverter
    {
        public static string XmlToJson(string xml, string xsdContent, bool mustIndent = false, bool omitRootObject = false)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            return XmlToJson(doc.DocumentElement, xsdContent, mustIndent, omitRootObject);
        }
        public static string XmlToJson(XmlNode node, string xsdContent, bool mustIndent=false, bool omitRootObject=false) {
            string json = JsonConvert.SerializeXmlNode(node, Newtonsoft.Json.Formatting.None, false);
            Dictionary<string,string> paths = XsdPathTypeExtractor.ExtractElementPaths(xsdContent);
            JToken token = JObject.Parse(json);

            foreach (KeyValuePair<string, string> pair in paths)
            {
                string jsonPath = "$" + pair.Key.Replace("/", ".");
                if (pair.Value == "boolean")
                {
                    foreach (JToken match in token.SelectTokens(jsonPath))
                    {
                        match.Replace(match.ToString().ToLower().Equals("true"));
                    }
                }
                else if (pair.Value == "number")
                {
                    foreach (JToken match in token.SelectTokens(jsonPath))
                    {
                        int iRes;
                        float fRes;
                        if(int.TryParse(match.ToString(), out iRes))
                        {
                            match.Replace(iRes);
                        }
                        else if (float.TryParse(match.ToString(), out fRes))
                        {
                            match.Replace(fRes);
                        }else
                        {
                            throw new Exception($"Type of {pair.Key} is number but the value: {pair.Value} cannot be converted to a number.");
                        }
                    }
                }
                else if (pair.Value == "array")
                {
                    foreach (JToken match in token.SelectTokens(jsonPath))
                    {
                        bool mustConvert = true;
                        JArray arr = new JArray();
                        foreach(var item in match.Children<JProperty>())
                        {
                            if(item.Value == null || item.Value.Type == JTokenType.Object || item.Count() != 1)
                            {
                                mustConvert = false;
                                break;
                            }

                            arr.Add(item.Value);
                        }

                        if (mustConvert)
                        {
                            match.Replace(arr);
                        }
                    }
                }
                    
            }
            var format = Newtonsoft.Json.Formatting.None;
            if (mustIndent)
            {
                format = Newtonsoft.Json.Formatting.Indented;
            }

            var root = token as JObject;
            if (omitRootObject && root != null && root.Properties().Count() == 1)
            {
                token = root.Properties().First().Value;
            }

            return JsonConvert.SerializeObject(token, format);

        }
    }
}
