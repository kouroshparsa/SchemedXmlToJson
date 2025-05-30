using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Schema;


namespace SchemedXmlToJson
{
    public class XsdPathTypeExtractor
    {
        public static Dictionary<string, string> ExtractElementPaths(string xsdContent)
        {
            var pathTypeMap = new Dictionary<string, string>();
            var schemaSet = new XmlSchemaSet();
            using (StringReader stringReader = new StringReader(xsdContent))
            {
                using (XmlReader xmlReader = XmlReader.Create(stringReader))
                {
                    XmlSchema schema = XmlSchema.Read(xmlReader, ValidationCallback);
                    schemaSet.Add(schema);
                }
            }
            schemaSet.Compile();
            foreach (XmlSchema schema in schemaSet.Schemas()) {
                foreach (XmlSchemaElement element in schema.Elements.Values)
                {
                    string currentPath = "/" + element.Name;
                    ProcessElement(element, currentPath, pathTypeMap);
                }
            }
            return pathTypeMap;
        }
 
        private static void ProcessElement(XmlSchemaElement element, string path, Dictionary<string, string> pathTypeMap)
        {
            // Handle arrays
            if (element.MaxOccurs > 1 || element.MaxOccursString == "unbounded")
            {
                int ind = path.LastIndexOf("/");
                string arrPath = path.Substring(0, ind);
                pathTypeMap[arrPath] = "array";
            }

            // Handle primitive types
            if (element.SchemaTypeName != null && !element.SchemaTypeName.IsEmpty)
            {
                string typeName = element.SchemaTypeName.Name;
                if (typeName == "boolean")
                    pathTypeMap[path] = "boolean";
                else if (typeName == "int" || typeName == "integer" || typeName == "long" || typeName == "short" || typeName == "float" || typeName == "decimal")
                    pathTypeMap[path] = "number";
            }

            // Recurse if complex type
            var complexType = element.ElementSchemaType as XmlSchemaComplexType;
            if (complexType != null)
            {
                var sequence = complexType.ContentTypeParticle as XmlSchemaSequence;
                if (sequence != null)
                {
                    foreach (XmlSchemaObject item in sequence.Items)
                    {
                        var childElement = item as XmlSchemaElement;
                        if (childElement != null)
                        {
                            string childPath = path + "/" + childElement.Name;
                            ProcessElement(childElement, childPath, pathTypeMap);
                        }
                    }
                }
            }
        }

        private static void ValidationCallback(object sender, ValidationEventArgs args)
        {
            Console.WriteLine($"Validation error: {args.Message}");
        }
    }
}
