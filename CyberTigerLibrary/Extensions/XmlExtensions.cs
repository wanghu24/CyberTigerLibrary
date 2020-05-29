using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace CyberTigerLibrary.Extensions
{
    public static class XmlExtensions
    {
        public static XmlDocument ToXmlDocument(this XDocument xDocument)
        {
            var xmlDocument = new XmlDocument();
            using (var xmlReader = xDocument.CreateReader())
            {
                xmlDocument.Load(xmlReader);
            }
            return xmlDocument;
        }
        public static XDocument ToXDocument(this XmlDocument xmlDocument)
        {
            using (var nodeReader = new XmlNodeReader(xmlDocument))
            {
                nodeReader.MoveToContent();
                return XDocument.Load(nodeReader);
            }
        }
        public static XmlDocument ToXmlDocument(this XElement xElement)
        {
            var sb = new StringBuilder();
            var xws = new XmlWriterSettings { OmitXmlDeclaration = true, Indent = false };
            using (var xw = XmlWriter.Create(sb, xws))
            {
                xElement.WriteTo(xw);
            }
            var doc = new XmlDocument();
            doc.LoadXml(sb.ToString());
            return doc;
        }
        public static Stream ToMemoryStream(this XmlDocument doc)
        {
            var xmlStream = new MemoryStream();
            doc.Save(xmlStream);
            xmlStream.Flush();//Adjust this if you want read your data 
            xmlStream.Position = 0;
            return xmlStream;
        }

        public static T FromXml<T>(this string xml)
        {
            if (string.IsNullOrEmpty(xml)) return default(T);

            T result;
            XmlSerializer xmlSer = new XmlSerializer(typeof(T));
            using (StringReader str = new StringReader(xml))
            {
                result = (T)xmlSer.Deserialize(str);
            }

            return result;
        }

        public static string ToXml<T>(this T obj)
        {
            if (obj == null) return null;

            string result = "";
            XmlSerializer xmlSer = new XmlSerializer(obj.GetType());
            using (MemoryStream m = new MemoryStream())
            {
                xmlSer.Serialize(m, obj);
                result = Encoding.UTF8.GetString(m.GetBuffer()).Replace("\0", "");
            }

            return result;
        }
    }
}
