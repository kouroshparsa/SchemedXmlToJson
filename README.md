# SchemedXmlToJson
.Net nuget package to convert xml to json using an XSD schema to enforce types such as boolean, array, number.

Example1:
```C#
using SchemedXmlToJson;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            string json = XmlToJsonConverter.XmlToJson(@"<Person>
	<Name>John Smith</Name>
	<Age>40</Age>
    <Male>true</Male>
	<Hobbies>
		<Hobby>Tennis</Hobby>
	</Hobbies>
</Person>",
        @"<?xml version='1.0' encoding='utf-8'?>
        <xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'>
            <xs:element name='Person'>
                <xs:complexType>
                    <xs:sequence>
                        <xs:element name='Name' type='xs:string'/>
                        <xs:element name='Age' type='xs:int'/>
                        <xs:element name='Male' type='xs:boolean'/>
                        <xs:element name=""Hobbies"">
                          <xs:complexType>
	                        <xs:sequence>
	                          <xs:element maxOccurs=""unbounded"" name=""Hobby"" type=""xs:string"" />
	                        </xs:sequence>
                          </xs:complexType>
                        </xs:element>
                    </xs:sequence>
                </xs:complexType>
            </xs:element>
        </xs:schema>");
            Assert.AreEqual(json, "{\"Person\":{\"Name\":\"John Smith\",\"Age\":40,\"Male\":true,\"Hobbies\":[\"Tennis\"]}}");
        }
    }
}
```
As you can see, the type of `Age` is number, the type of `Male` is boolean and the list of `Hobbies` is shown as an array while if you use Newton.soft, it will assume that it is not an array if there is only one item.

Here is the method signature and details:
```C#
        public static string XmlToJson(
            string xml,     # <---- the xml to be converted
            string xsdContent, # <---- the xsd schema file contents
            bool mustIndent = false, # <---- if true, then indent the json result 
            bool omitRootObject = false) # <---- if true, omit the root of the result

```
The example below shows how using the argument `omitRootObject` can change the result:
Example2:
```C#
using SchemedXmlToJson;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            string json = XmlToJsonConverter.XmlToJson(@"<Person>
	<Name>John Smith</Name>
	<Age>40</Age>
    <Male>true</Male>
	<Hobbies>
		<Hobby>Tennis</Hobby>
	</Hobbies>
</Person>",
                @"<?xml version='1.0' encoding='utf-8'?>
        <xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'>
            <xs:element name='Person'>
                <xs:complexType>
                    <xs:sequence>
                        <xs:element name='Name' type='xs:string'/>
                        <xs:element name='Age' type='xs:int'/>
                        <xs:element name='Male' type='xs:boolean'/>
                        <xs:element name=""Hobbies"">
                          <xs:complexType>
	                        <xs:sequence>
	                          <xs:element maxOccurs=""unbounded"" name=""Hobby"" type=""xs:string"" />
	                        </xs:sequence>
                          </xs:complexType>
                        </xs:element>
                    </xs:sequence>
                </xs:complexType>
            </xs:element>
        </xs:schema>", false, true);
            Assert.AreEqual(json, "{\"Name\":\"John Smith\",\"Age\":40,\"Male\":true,\"Hobbies\":[\"Tennis\"]}");
        }
    }
}
```