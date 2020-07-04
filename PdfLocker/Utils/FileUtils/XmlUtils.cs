using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Meridian.Helper
{
    public class XmlUtils
    {
        readonly string XmlPath;
        public XmlUtils(string xmlPath)
        {
            XmlPath = xmlPath;
        }

        #region XmlFile operations
        public string GetElementVal(string elementName)
        {
            try
            {
                if (!new FileInfo(XmlPath).Exists)
                    throw new Exception("No file found at path: " + XmlPath);

                XDocument xDoc = XDocument.Load(XmlPath);

                return xDoc.Root.Element(elementName).Value;
            }
            catch(Exception)
            {
                var errMsg = "Failed to return Config.xml value for property: " + elementName;
                throw new Exception(errMsg);
            }

        }

        /// <summary>
        /// Gets value of element by xmlElement Id
        /// </summary>
        /// <param name="elemId"></param>
        /// <param name="elementName"></param>
        /// <returns>Returns value of element by xmlElement Id</returns>
        public string GetElementValById(string elemId, string elementName)
        {
            if (!new FileInfo(XmlPath).Exists)
                throw new Exception("No file found at path: " + XmlPath);

            XDocument xDoc = XDocument.Load(XmlPath);

            var qElem = xDoc.Root.Descendants("document")
                .Where(c => c.Attribute("ID").Value == elemId)
                .FirstOrDefault();

            if (qElem != null)
                return qElem.Element(elementName).Value;

            return null;
        }

        /// <summary>
        /// Set XmlElement Value by Id
        /// </summary>
        /// <param name="xmlPath"></param>
        /// <param name="elemId"></param>
        /// <param name="elementName"></param>
        /// <param name="elementValue"></param>
        public void SetElementValById(string elemId, string elementName, string elementValue)
        {
            if (!new FileInfo(XmlPath).Exists)
                throw new Exception("No file found at path: " + XmlPath);

            XDocument xDoc = XDocument.Load(XmlPath);

            var qElem = xDoc.Root.Descendants("document")
                .Where(c => c.Attribute("ID").Value == elemId)
                .FirstOrDefault();

            if (qElem != null)
                qElem.Element(elementName).Value = elementValue;

            xDoc.Save(XmlPath);
        }

        public Hashtable XmlProps
        {
            get
            {
                XDocument doc = XDocument.Load(XmlPath);
                Hashtable dataTable = new Hashtable();

                foreach (XElement element in doc.Descendants().Where(p => p.HasElements == false))
                {
                    string keyName = element.Name.LocalName;

                    if (!dataTable.ContainsKey(keyName))
                        if (keyName.ToLower() != "pdf" && keyName.ToLower() != "jpg")
                        dataTable.Add(keyName, element.Value);              
                }

                return dataTable;
            }
        }

        #endregion
    }
}
