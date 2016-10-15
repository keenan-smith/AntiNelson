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
        /// <summary>
        /// Path to the config file.
        /// </summary>
        public string path
        {
            get
            {
                return _path;
            }
        }

        /// <summary>
        /// Xml document of the config file.
        /// </summary>
        public XmlDocument document
        {
            get
            {
                return _doc;
            }
        }

        /// <summary>
        /// The xml root element of the config file.
        /// </summary>
        public XmlElement rootElement
        {
            get
            {
                return _root;
            }
        }
        #endregion

        /// <summary>
        /// Creates a PBConfig instance that allows you to edit config files.
        /// </summary>
        /// <param name="path">Path to config file.</param>
        public PBConfig(string path)
        {
            _path = path;
            _doc = new XmlDocument();
            _doc.Load(path);
            _root = _doc.DocumentElement;
        }

        /// <summary>
        /// Creates a PBConfig instance that allows you to create config files.
        /// </summary>
        public PBConfig()
        {
            _doc = new XmlDocument();
            document.AppendChild(document.CreateXmlDeclaration("1.0", null, null));
            _root = document.CreateElement("PointBlank");
            document.AppendChild(rootElement);
        }

        #region Functions
        /// <summary>
        /// Gets the text of a specific config node.
        /// </summary>
        /// <param name="nodePath">The path(or name) of the node.</param>
        /// <returns>The text of the node.</returns>
        public string getText(string nodePath)
        {
            return rootElement.SelectSingleNode(nodePath).InnerText;
        }

        /// <summary>
        /// Gets all the texts of the child config nodes.
        /// </summary>
        /// <param name="nodePath">The path to the parent config node.</param>
        /// <returns>The text of the child config nodes.</returns>
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

        /// <summary>
        /// Gets all the child config nodes of a parent config node.
        /// </summary>
        /// <param name="nodePath">The path to the parent config node.</param>
        /// <returns>List of child config nodes.</returns>
        public XmlNodeList getChildNodes(string nodePath)
        {
            return rootElement.SelectSingleNode(nodePath).ChildNodes;
        }

        /// <summary>
        /// Adds a config node.
        /// </summary>
        /// <param name="name">Config node name.</param>
        /// <param name="text">Config node text.</param>
        public void addTextElement(string name, string text)
        {
            XmlElement tmp = document.CreateElement(name);
            tmp.InnerText = text;
            rootElement.AppendChild(tmp);
        }

        /// <summary>
        /// Adds child config nodes to parent config node.
        /// </summary>
        /// <param name="arrayName">The name/path to the parent config node.</param>
        /// <param name="name">The name of all the child nodes.</param>
        /// <param name="texts">The value of each child node.</param>
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

        /// <summary>
        /// Saves the config file.
        /// </summary>
        /// <param name="path">Config file save location path.</param>
        public void save(string path)
        {
            document.Save(path);
        }
        #endregion
    }
}
