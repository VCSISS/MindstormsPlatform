using BlockCreatorLogic.Exceptions;
using System;
using System.Diagnostics;
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
        
        /// <summary>
        /// Edits an existing module; this command assumes that the
        /// working directory is the parent directory of the entire
        /// block folder.
        /// </summary>
        /// <param name="moduleName"></param>
        /// <param name="moduleVersion"></param>
        /// <param name="moduleOldName"></param>
        public static void EditModule(string moduleName, string moduleVersion, string moduleOldName)
        {
            if (moduleName == "" && moduleVersion == "")
            {
                // TODO: figure out which parameter should be associated with this errors
                LogBlocksXmlValueError("moduleName", "You have to specify either the module name or the module version");
            }
            else if (moduleName != "" && moduleOldName != "" && moduleVersion == "")
            {
                LogBlocksXmlValueError("moduleName", "You cannot use the options --name, --old, and --version at the same time");
            }
            
            // block module --name "NewName" --old "OldName"
            if (moduleName != "" && moduleOldName != "")
            {
                if (Directory.Exists(moduleOldName))
                {
                    Directory.Move(moduleOldName, moduleName);

                    Directory.SetCurrentDirectory(moduleName);
                        
                    XmlDocument doc = new XmlDocument();
                    doc.Load("blocks.xml");

                    XmlNode root = doc.DocumentElement;

                    XmlElement polyGroups = (XmlElement)root.SelectSingleNode("//PolyGroups");
                        
                    XmlAttribute moduleNameAttr = (XmlAttribute)polyGroups.SelectSingleNode("@ModuleName");

                    if (moduleNameAttr != null)
                    {
                        moduleNameAttr.Value = moduleName;
                    }
                    else
                    {
                        // TODO: maybe let the user know that this value for some reason
                        // not defined
                        polyGroups.SetAttribute("ModuleName", moduleName);
                    }

                    doc.Save("blocks.xml");
                }
                else
                {
                    LogBlocksXmlValueError("moduleOldName", "Cannot find a module named " + moduleOldName);
                }
            }
            // block module --version "1.00" --name "Name"
            else if (moduleVersion != "")
            {
                if (moduleName == "")
                {
                    LogBlocksXmlValueError("moduleName", "You must specify the module's name to edit a module's version");
                }

                Directory.SetCurrentDirectory(moduleName);

                XmlDocument doc = new XmlDocument();
                doc.Load("blocks.xml");

                XmlElement polyGroups = (XmlElement)doc.DocumentElement.SelectSingleNode("//PolyGroups");

                XmlAttribute moduleVersionAttr = (XmlAttribute)polyGroups.SelectSingleNode("@ModuleVersion");

                // TODO: jut use SetAttribute for these
                if (moduleVersionAttr != null)
                {
                    moduleVersionAttr.Value = moduleVersion;
                }
                else
                {
                    polyGroups.SetAttribute("ModuleVersion", moduleVersion);
                }

                doc.Save("blocks.xml");
            }
            else if (moduleName != "" && moduleOldName == "")
            {
                // TODO: rather than things like moduleOldName, use
                // human readable things like Module Old Name
                LogBlocksXmlValueError("moduleOldName", "You must specify the old name of the module in order to rename it");
            }
            else
            {
                LogBlocksXmlValueError("moduleName", "Invalid usage of the command \"block module\"; try \"block module --help\" for more information");
            }
        }

        public static void CreateBlock(string blockName, string blockFamily)
        {
            // TODO: prevent creation of multiple blocks with the same name

            // Edit blocks.xml

            XmlDocument doc = new XmlDocument();
            doc.Load("blocks.xml");

            XmlElement polyGroups = (XmlElement)doc.DocumentElement.SelectSingleNode("//PolyGroups");

            XmlElement blockPolyGroup = doc.CreateElement("PolyGroup");

            blockPolyGroup.SetAttribute("Name", blockName);
            blockPolyGroup.SetAttribute("BlockFamily", blockFamily);

            blockPolyGroup.AppendChild(doc.CreateElement("Hardware"));

            polyGroups.AppendChild(blockPolyGroup);

            doc.Save("blocks.xml");

            // Edit strings blocks.xml

            // Create image files
        }

        public static void EditBlock(string blockName, string blockFamily, string blockNewName)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("blocks.xml");

            XmlElement polyGroups = (XmlElement)doc.DocumentElement.SelectSingleNode("//PolyGroups");

            XmlNodeList polyGroupList = doc.SelectNodes("//PolyGroup");

            foreach (XmlNode node in polyGroupList)
            {
                if (node.SelectSingleNode("@Name").Value == blockName)
                {
                    XmlElement blockPolyGroup = (XmlElement)node;

                    if (blockNewName != "")
                    {
                        blockPolyGroup.SetAttribute("Name", blockNewName);
                    }

                    if (blockFamily != "")
                    {
                        blockPolyGroup.SetAttribute("BlockFamily", blockFamily);
                    }
                }
            }
            
            doc.Save("blocks.xml");
        }

        public static void DeleteBlock(string blockName, string blockFamily)
        {
            throw new NotImplementedException();
        }

        public static void CreateParam(string paramName, string paramDirection, string paramDataType, string paramDefaultValue, string paramMinValue, string paramMaxValue, string paramConfig, string paramIdent, string paramCompilerDirs, string paramValueDisplay)
        {
            // TODO: prevent creating multiple parameters with the same name

            XmlDocument doc = new XmlDocument();
            doc.Load("blocks.xml");

            XmlElement polyGroups = (XmlElement)doc.DocumentElement.SelectSingleNode("//PolyGroups");

            XmlElement param = doc.CreateElement("Parameter");

            param.SetAttribute("Name", paramName);
            param.SetAttribute("Direction", paramDirection);
            param.SetAttribute("DataType", paramDataType);
            param.SetAttribute("DefaultValue", paramDefaultValue);
            param.SetAttribute("MinValue", paramMinValue);
            param.SetAttribute("MaxValue", paramMaxValue);
            param.SetAttribute("Configuration", paramConfig);
            param.SetAttribute("Identification", paramIdent);
            param.SetAttribute("CompilerDirectives", paramCompilerDirs);
            param.SetAttribute("ValueDisplay", paramValueDisplay);

            polyGroups.AppendChild(param);

            doc.Save("blocks.xml");
        }

        public static void EditParam(string paramName, string paramNewName, string paramDirection, string paramDataType, string paramDefaultValue, string paramMinValue, string paramMaxValue, string paramConfig, string paramIdent, string paramCompilerDirs, string paramValueDisplay)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("blocks.xml");

            XmlElement polyGroups = (XmlElement)doc.DocumentElement.SelectSingleNode("//PolyGroups");
    
            foreach (XmlElement element in polyGroups.GetElementsByTagName("Parameter"))
            {
                if (element.SelectSingleNode("@Name").Value == paramName)
                {
                    if (paramNewName != "")
                    {
                        element.SetAttribute("Name", paramNewName);
                    }

                    if (paramDirection != "")
                    {
                        element.SetAttribute("Direction", paramDirection);
                    }

                    if (paramDataType != "")
                    {
                        element.SetAttribute("DataType", paramDataType);
                    }

                    if (paramDefaultValue != "")
                    {
                        element.SetAttribute("DefaultValue", paramDefaultValue);
                    }

                    if (paramMinValue != "")
                    {
                        element.SetAttribute("MinValue", paramMinValue);
                    }

                    if (paramMaxValue != "")
                    {
                        element.SetAttribute("MaxValue", paramMaxValue);
                    }

                    if (paramConfig != "")
                    {
                        element.SetAttribute("Configuration", paramConfig);
                    }

                    if (paramIdent != "")
                    {
                        element.SetAttribute("Identification", paramIdent);
                    }

                    if (paramCompilerDirs != "")
                    {
                        element.SetAttribute("CompilerDirectives", paramCompilerDirs);
                    }

                    if (paramValueDisplay != "")
                    {
                        element.SetAttribute("ValueDisplay", paramValueDisplay);
                    }
                }
            }

            doc.Save("blocks.xml");
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

