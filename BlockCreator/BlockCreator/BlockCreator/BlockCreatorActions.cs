using BlockCreatorLogic.Exceptions;
using System;
using System.IO;
using System.Xml;

namespace BlockCreatorLogic
{
    /// <summary>
    /// The model, or app logic, for all of different BlockCreator
    /// user interfaces (command line, Windows Forms, etc.)
    /// </summary>
	public class BlockCreatorActions
	{
        /// <summary>
        /// Create a module; in other words, create the directories and
        /// files for an empty module. A module is like a namespace for
        /// EV3 blocks.
        /// </summary>
        /// <param name="moduleName">The name of the module</param>
        /// <param name="moduleVersion">The current version of the module</param>
        public static void CreateModule(string moduleName, string moduleVersion)
        {
            if (moduleName == "")
            {
                LogBlocksXmlValueError("moduleName", "No module name was specified");
            }

            if (moduleVersion == "")
            {
                LogBlocksXmlValueError("moduleVersion", "No module version was specified");
            }

            Directory.CreateDirectory(moduleName);
            Directory.SetCurrentDirectory(moduleName);

            CreateBlocksXml(moduleName, moduleVersion);

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
        
        /// <summary>
        /// Generate a blocks.xml file. The blocks.xml file contains
        /// all of the information for a module and any blocks inside
        /// of it; such as parameters and references to VIX files that
        /// the block uses.
        /// </summary>
        /// <param name="moduleName">The name of the module that the blocks.xml file will be created for</param>
        /// <param name="moduleVersion">The current version of the module that the blocks.xml file will be created for</param>
        private static void CreateBlocksXml(string moduleName, string moduleVersion)
        {
            XmlWriterSettings xmlSettings = new XmlWriterSettings();
            xmlSettings.Indent = true;

            using (XmlWriter writer = XmlWriter.Create("blocks.xml", xmlSettings))
            {
                // Write the Polygroups element (the module)
               
                writer.WriteStartElement("EditorDefinitions");

                writer.WriteStartElement("PolyGroups");
                writer.WriteAttributeString("ModuleName", moduleName);
                writer.WriteAttributeString("ModuleVersion", moduleVersion);
               
                writer.WriteEndElement();
            }
        }
        
        public static void EditModule(string moduleName, string moduleVersion)
        {
            throw new NotImplementedException();
        }

        public static void CreateBlock(string blockName, string blockFamily)
        {
            throw new NotImplementedException();
        }

        public static void EditBlock(string blockName, string blockFamily)
        {
            throw new NotImplementedException();
        }

        public static void DeleteBlock(string blockName, string blockFamily)
        {
            throw new NotImplementedException();
        }

        public static void CreateParam(string paramName, string paramDirection, string paramDataType, string paramDefaultValue, string paramMinValue, string paramMaxValue, string paramConfig, string paramIdent, string paramCompilerDirs, string paramValueDisplay)
        {
            throw new NotImplementedException();
        }

        public static void EditParam(string paramName, string paramDirection, string paramDataType, string paramDefaultValue, string paramMinValue, string paramMaxValue, string paramConfig, string paramIdent, string paramCompilerDirs, string paramValueDisplay)
        {
            throw new NotImplementedException();
        }

        public static void DeleteParam(string paramName)
        {
            throw new NotImplementedException();
        }

        public static void EditHardwareInfo(string hwareBlockName, string hwareEv3AutoId, string hwareOtherAutoId, string hwareDirection, string hwareDefaultPort)
        {
            throw new NotImplementedException();
        }

        public static void CreateMode(string modeName, string modeBlockName, string modeVIXRef, string modeParamRef, object modeWeight, string modeType, string modeFlags, string hwareModeInfoName, string hwareModeInfoId, string hwareModeInfoRange, string hwareModeInfoUnit)
        {
            throw new NotImplementedException();
        }

        public static void EditMode(string modeName, string modeBlockName, string modeVIXRef, string modeParamRef, object modeWeight, string modeType, string modeFlags, string hwareModeInfoName, string hwareModeInfoId, string hwareModeInfoRange, string hwareModeInfoUnit)
        {
            throw new NotImplementedException();
        }

        public static void DeleteMode(string modeName)
        {
            throw new NotImplementedException();
        }
        
        public static void Finish()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Throws an error that occurred because of an invalid value
        /// given to be used in the blocks.xml file.
        /// </summary>
        /// <param name="value">The invalid value's name</param>
        /// <param name="error">A string describing the error</param>
        private static void LogBlocksXmlValueError(string value, string error)
        {
            throw new BlocksXmlValueException(value, error);
        }
    }
}

