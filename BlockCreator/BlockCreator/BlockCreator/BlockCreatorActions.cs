using System;
using System.IO;
using System.Xml;

namespace BlockCreatorLogic
{
	public class BlockCreatorActions
	{
        public static void CreateBlock(string blockName, string family, string direction)
        {
            Directory.CreateDirectory(blockName);
            Directory.SetCurrentDirectory(blockName);

            CreateBlocksXml(blockName, family, direction, "1.1", "1.00");

            Directory.CreateDirectory("VIs");
            Directory.SetCurrentDirectory("VIs");

            Directory.CreateDirectory("PBR");
            Directory.CreateDirectory("NXT");

            Directory.SetCurrentDirectory("../");

            Directory.CreateDirectory("help");

            Directory.CreateDirectory("images");

            Directory.CreateDirectory("strings");
            Directory.SetCurrentDirectory("strings");
        }

        private static void CreateBlocksXml(string name, string family, string direction, string defaultPort, string moduleVersion)
        {
            XmlWriterSettings xmlSettings = new XmlWriterSettings();
            xmlSettings.Indent = true;

            using (XmlWriter writer = XmlWriter.Create("blocks.xml", xmlSettings))
            {
                writer.WriteStartElement("EditorDefinitions");

                writer.WriteStartElement("PolyGroups");
                writer.WriteAttributeString("ModuleName", name);
                writer.WriteAttributeString("ModuleVersion", moduleVersion);

                writer.WriteStartElement("PolyGroup");

                writer.WriteAttributeString("Name", name);
                writer.WriteAttributeString("BlockFamily", family);

                writer.WriteEndElement();

                writer.WriteStartElement("Hardware");

                writer.WriteElementString("NXTPlotColor", "#ffff3132");
                writer.WriteElementString("EV3PlotColor", "#ff785028");
                writer.WriteElementString("EV3AutoID", "-1");
                writer.WriteElementString("Direction", direction);
                writer.WriteElementString("DefaultPort", defaultPort);

                writer.WriteEndElement();

                writer.WriteEndElement();
            }
        }
	}
}

