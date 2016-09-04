using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PointBlank.API
{
    public class PBConfig
    {
        #region Variables
        private string _path;
        private XmlDocument _doc;
        private XmlElement _root;
        #endregion

        #region Properties
        public string path
        {
            get
            {
                return _path;
            }
        }

        public XmlDocument document
        {
            get
            {
                return _doc;
            }
        }

        public XmlElement rootElement
        {
            get
            {
                return _root;
            }
        }
        #endregion

        public PBConfig(string path)
        {
            _path = path;
            _doc = new XmlDocument();
            _doc.Load(path);
            _root = _doc.DocumentElement;
        }

        public PBConfig()
        {
            _doc = new XmlDocument();
            document.AppendChild(document.CreateXmlDeclaration("1.0", null, null));
            _root = document.CreateElement("PointBlank");
            document.AppendChild(rootElement);
        }

        #region Functions
        public string getText(string nodePath)
        {
            return rootElement.SelectSingleNode(nodePath).InnerText;
        }

        public string[] getChildNodesText(string nodePath)
        {
            List<string> texts = new List<string>();
            XmlNodeList nodeList = rootElement.SelectSingleNode(nodePath).ChildNodes;

            foreach (XmlNode node in nodeList)
            {
                texts.Add(node.InnerText);
            }
            return texts.ToArray();
        }

        public XmlNodeList getChildNodes(string nodePath)
        {
            return rootElement.SelectSingleNode(nodePath).ChildNodes;
        }

        public void addTextElement(string name, string text)
        {
            XmlElement tmp = document.CreateElement(name);
            tmp.InnerText = text;
            rootElement.AppendChild(tmp);
        }

        public void addTextElements(string arrayName, string name, string[] texts)
        {
            XmlElement tmp1 = document.CreateElement(arrayName);
            foreach (string text in texts)
            {
                XmlElement tmp2 = document.CreateElement(name);
                tmp2.InnerText = text;
                tmp1.AppendChild(tmp2);
            }
            rootElement.AppendChild(tmp1);
        }

        public void save(string path)
        {
            document.Save(path);
        }
        #endregion
    }
}
