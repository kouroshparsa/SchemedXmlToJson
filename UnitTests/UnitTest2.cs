using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SchemedXmlToJson;

namespace UnitTests
{
    [TestClass]
    public class UnitTest2
    {
        [TestMethod]
        public void TestXmlToJson()
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

        [TestMethod]
        public void TestXmlToJsonOmitRoot()
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
