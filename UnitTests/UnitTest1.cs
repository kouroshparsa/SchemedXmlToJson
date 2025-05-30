using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SchemedXmlToJson;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestPathExtraction()
        {
            string xsdContent = @"<?xml version='1.0' encoding='utf-8'?>
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
        </xs:schema>";
            var res = XsdPathTypeExtractor.ExtractElementPaths(xsdContent);
            Assert.AreEqual(res.Count, 3);
            Assert.AreEqual(res["/Person/Age"], "number");
            Assert.AreEqual(res["/Person/Male"], "boolean");
            Assert.AreEqual(res["/Person/Hobbies"], "array");
        }
    }
}
