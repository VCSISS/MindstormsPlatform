using BlockCreatorLogic;
using BlockCreatorLogic.Exceptions;
using NDesk.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockCreatorConsole
{
    /// <summary>
    /// The class that the entry point of the program delegates
    /// the responsibility of handling commands entered at the command
    /// line to; in other words, this is the code that understands
    /// and acts upon the command line instruction. This class is the
    /// controller part of the MVC  pattern implemented in
    /// BlockCreatorConsole and BlockCreatorLogic.
    /// </summary>
    class CommandLineHandler
    {
        // TODO: make way for this tool to "remember" which block/mode was being
        // edited last; so that the user won't have to specify it each time
        // a user edits things like hardware information or parameters for a mode

        // block
        private static bool showHelp = false;

        // The options you can pass when you do not specify a command
        // (such as "module" or "makeblock")
        private static OptionSet noCommandGivenOptions = new OptionSet()
        {
            { "h|help", "show help about BlockCreator",
                v => showHelp = true }
        };

        // To see how each of these parameters are used in the
        // generated blocks.xml, see README.md

        // TODO: add -h or --help option to each of these to describe
        // usage

        // block module
        private static bool createModule = false;
        private static bool editModule = false;

        private static string moduleName = "";
        private static string moduleVersion = "";
        private static string moduleOldName = "";

        private static OptionSet moduleOptions = new OptionSet()
        {
            { "c|create", "create a new module",
               v => createModule = (v != null) },
            { "e|edit", "edit an existing module",
                v => editModule = (v != null) },
            // TODO: add delete option here
            { "n|name=", "the name of the module",
                v => moduleName = v },
            { "v|version=", "the current version of the module",
                v => moduleVersion = v },
            {  "o|old=", "the current name of the module when changing a module's name",
                v => moduleOldName = v }
        };

        // block block
        private static bool createBlock = false;
        private static bool editBlock = false;
        private static bool deleteBlock = false;

        private static string blockName = "";
        private static string blockFamily = "";
        private static string blockNewName = "";

        private static OptionSet blockOptions = new OptionSet()
        {
            { "c|create", "create a new block",
               v => createBlock = (v != null) },
            { "e|edit", "edit an existing block",
               v => editBlock = (v != null) },
            { "d|delete", "deletes a block",
               v => deleteBlock = (v != null) },

            { "n|name=", "the name of the block",
              v => blockName = v },
            { "f|family=", "the family the block is in (Action, DataOperations, FlowControl, Sensor, or Advanced)",
              v => blockFamily = v },
            { "w|new=", "the new name of the block if its name is being edited",
              v => blockNewName = v }
        };

        // block param
        private static bool createParam = false;
        private static bool deleteParam = false;
        private static bool editParam = false;

        private static string paramName = "";
        private static string paramNewName = "";
        private static string paramDirection = "";
        private static string paramDataType = ""; // TODO: to keep BlockCreator simple, we may remove this
        private static string paramDefaultValue = "";
        private static string paramMinValue = "";
        private static string paramMaxValue = "";
        private static string paramConfig = "";
        private static string paramIdent = "";
        private static string paramCompilerDirs = "";
        private static string paramValueDisplay = "";

        private static OptionSet paramOptions = new OptionSet()
        {
            { "c|create", "create a new parameter",
              v => createParam = (v != null) },
            { "e|edit", "edits a parameter",
              v => editParam = (v != null) },
            { "r|delete", "delete a parameter",
              v => deleteParam = (v != null) },

            { "n|name=", "the name of the parameter",
              v => paramName = v },
            { "w|new=", "the new name of the parameter if its name is being edited",
              v => paramNewName = v },
            { "d|direction=", "the direction of the parameter; this can be Input or Output (argument or return value)",
              v => paramDirection = v },
            { "t|dataType=", "the data type of the parameter (this should usually be Single, except for special cases you can find in the EV3 Block Developer Kit)",
              v => paramDataType = v },
            { "v|defaultValue", "the default value of the parameter",
              v => paramDefaultValue = v },
            { "m|minValue=", "the minimim value of the parameter",
              v => paramMinValue = v },
            { "a|maxValue=", "the maximum value of the parameter",
              v => paramMaxValue = v },
            { "o|configuration=", "the parameter's configuration (what the parameter does when you click on it)",
              v => paramConfig = v },
            { "i|identification=", "the image associated with this parameter",
              v => paramIdent = v },
            { "l|compilerDirs=", "any special purposes that this parameter will be used for",
              v => paramCompilerDirs = v },
            // TODO: figure out what valueDisplay actually does (the documentation only mentions this one
            // purpose as an example, but doesn't actually say what it represents)
            { "p|valueDisplay=", "the images displayed at the top of the cases of a switch block (this is the only use of the value display that we currently know of)",
              v => paramValueDisplay = v }
        };

        // block hardware
        private static string hwareBlockName = "";
        private static string hwareEv3AutoId = "";
        private static string hwareOtherAutoId = "";
        private static string hwareDirection = "";
        private static string hwareDefaultPort = "";

        private static OptionSet hwareOptions = new OptionSet()
        {
            { "b|block=", "the name of the block which this hardware configuration applies to",
              v => hwareBlockName = v },
            { "i|ev3AutoId=", "the number used for the EV3's Auto ID functionality (if it does not not use this, then it should just be -1",
              v => hwareEv3AutoId = v },
            { "o|otherAutoId=", "the Auto IDs of other sensors that also use this block",
              v => hwareOtherAutoId = v },
            { "d|direction=", "the direction of the block (Input or Output)",
              v => hwareDirection = v },
            { "p|defaultPort=", "the default port of the block",
              v => hwareDefaultPort = v }
        };

        // block mode
        private static bool createMode = false;
        private static bool editMode = false;
        private static bool deleteMode = false;

        private static string modeBlockName = "";
        private static string modeName = "";
        private static string modeVIXRef = "";
        private static string modeParamRef = "";
        // The existance of this element indicates the default mode
        // for a block. Its actual purpose, however, is to tell the block's
        // weight (where it appears from left to right in the palette).
        private static string modeWeight = "";
        private static string modeType = "";
        private static string modeFlags = "";
        private static string hwareModeInfoName = "";
        private static string hwareModeInfoId = "";
        private static string hwareModeInfoRange = "";
        private static string hwareModeInfoUnit = "";

        private static OptionSet modeOptions = new OptionSet()
        {
            { "c|create", "create a new mode",
               v => createMode = (v != null) },
            { "e|edit", "edit an existing mode",
               v => editMode = (v != null) },
            { "d|delete", "delete a mode",
               v => deleteMode = (v != null) },

            { "n|name=", "the name of the mode",
               v => modeName = v },
            { "b|block=", "the block to which this mode belongs to",
               v => modeBlockName = v },
            { "v|vixRef=", "the name of the VIX file that is the source code for this mode",
               v => modeVIXRef = v },
            { "p|paramRef=", "adds a parameter to this block from the list of parameters defined with \"block param\"",
               v => modeParamRef = v },
            { "w|weight=", "the weight of this mode (where it goes from left to right on the palette); the existance of this element in a mode makes that mode the default mode for the block",
               v => modeWeight = v },
            { "t|type=", "the type of mode that this mode is (Measure, Compare, or Change)",
               v => modeType = v },
            { "f|flags=", "any flags for this particular mode",
               v => modeFlags = v },
            { "h|dataloggingName=", "the name for this mode for use in datalogging",
               v => hwareModeInfoName = v },
            { "i|dataloggingId=", "the mode of the sensor (used in mode detection and datalogging)",
               v => hwareModeInfoId = v },
            { "r|dataloggingRange=", "used in datalogging (this is all we know about this parameter at this point in time)",
               v => hwareModeInfoRange = v },
            { "u|dataloggingUnit=", "used in datalogging (this is all we know about this parameter at this point in time)",
               v => hwareModeInfoUnit = v }
        };


        public static void Handle(string[] args)
        {
            if (args.Count() > 0)
            {
                // Put one line of space before the command line
                // instruction the user typed and the output from
                // BlockCreator
                Console.WriteLine();

                string[] options = args.Skip<string>(1).ToArray<string>();

                switch (args[0])
                {
                    case "module":
                        {
                            try
                            {
                                moduleOptions.Parse(options);
                            }
                            catch (OptionException oe)
                            {
                                ErrorWithOptions(oe);
                                break;
                            }

                            try
                            {
                                if (createModule && editModule)
                                {
                                    ErrorWithOptions("You can only use one of the following options: -c or --create or -e or --edit");
                                    break;
                                }
                                else if (createModule)
                                {
                                    BlockCreatorActions.CreateModule(moduleName, moduleVersion);
                                }
                                else if (editModule)
                                {
                                    BlockCreatorActions.EditModule(moduleName, moduleVersion, moduleOldName);
                                }
                                else
                                {
                                    Console.WriteLine("block module");
                                    moduleOptions.WriteOptionDescriptions(Console.Out);
                                }
                            }
                            catch (BlocksXmlValueException bxve)
                            {
                                ErrorWithBlocksXmlValue(bxve);
                                break;
                            }

                            break;
                        }

                    case "block":
                        {
                            try
                            {
                                blockOptions.Parse(options);
                            }
                            catch (OptionException oe)
                            {
                                ErrorWithOptions(oe);
                                break;
                            }

                            try
                            {
                                if ((createBlock && editBlock)
                                    || (createBlock && deleteBlock)
                                    || (editBlock && deleteBlock))
                                {
                                    ErrorWithOptions("You can only use one of the following options: -c or --create, -e or --edit, or -d or --delete");
                                    break;
                                }
                                else if (createBlock)
                                {
                                    BlockCreatorActions.CreateBlock(blockName, blockFamily);
                                }
                                else if (editBlock)
                                {
                                    BlockCreatorActions.EditBlock(blockName, blockFamily, blockNewName);
                                }
                                else if (deleteBlock)
                                {
                                    BlockCreatorActions.DeleteBlock(blockName, blockFamily);
                                }
                                else
                                {
                                    Console.WriteLine("block block");
                                    blockOptions.WriteOptionDescriptions(Console.Out);
                                }
                            }
                            catch (BlocksXmlValueException bxve)
                            {
                                ErrorWithBlocksXmlValue(bxve);
                                break;
                            }

                            break;
                        }
                       
                    case "param":
                        {
                            try
                            {
                                paramOptions.Parse(options);
                            }
                            catch (OptionException oe)
                            {
                                ErrorWithOptions(oe);
                                break;
                            }

                            try
                            {
                                if ((createParam && editParam)
                                    || (createParam && deleteParam)
                                    || (editParam && deleteParam))
                                {
                                    ErrorWithOptions("You can only use one of the following options: -c or --create, -e or --edit, or -d or --delete");
                                    break;
                                }
                                else if (createParam)
                                {
                                    BlockCreatorActions.CreateParam(paramName, paramDirection, paramDataType, paramDefaultValue, paramMinValue, paramMaxValue, paramConfig, paramIdent, paramCompilerDirs, paramValueDisplay);
                                }
                                else if (editParam)
                                {
                                    BlockCreatorActions.EditParam(paramName, paramNewName, paramDirection, paramDataType, paramDefaultValue, paramMinValue, paramMaxValue, paramConfig, paramIdent, paramCompilerDirs, paramValueDisplay);
                                }
                                else if (deleteParam)
                                {
                                    BlockCreatorActions.DeleteParam(paramName);
                                }
                                else
                                {
                                    Console.WriteLine("block param");
                                    paramOptions.WriteOptionDescriptions(Console.Out);
                                }
                            }
                            catch (BlocksXmlValueException bxve)
                            {
                                ErrorWithBlocksXmlValue(bxve);
                                break;
                            }

                            break;
                        }

                    case "hardware":
                        {
                            try
                            {
                                hwareOptions.Parse(options);
                            }
                            catch (OptionException oe)
                            {
                                ErrorWithOptions(oe);
                                break;
                            }

                            try
                            {
                                BlockCreatorActions.EditHardwareInfo(hwareBlockName, hwareEv3AutoId, hwareOtherAutoId, hwareDirection, hwareDefaultPort);
                            }
                            catch (BlocksXmlValueException bxve)
                            {
                                ErrorWithBlocksXmlValue(bxve);
                                break;
                            }

                            break;
                        }

                    case "mode":
                        {
                            try
                            {
                                modeOptions.Parse(options);
                            }
                            catch (OptionException oe)
                            {
                                ErrorWithOptions(oe);
                                break;
                            }
                            
                            try
                            {
                                if ((createMode && editMode)
                                    || (createMode && deleteMode)
                                    || (editMode && deleteMode))
                                {
                                    ErrorWithOptions("You can only use one of the following options: -c or --create, -e or --edit, or -d or --delete");
                                    break;
                                }
                                else if (createMode)
                                {
                                    BlockCreatorActions.CreateMode(modeName, modeBlockName, modeVIXRef, modeParamRef, modeWeight, modeType, modeFlags, hwareModeInfoName, hwareModeInfoId, hwareModeInfoRange, hwareModeInfoUnit);
                                }
                                else if (editMode)
                                {
                                    BlockCreatorActions.EditMode(modeName, modeBlockName, modeVIXRef, modeParamRef, modeWeight, modeType, modeFlags, hwareModeInfoName, hwareModeInfoId, hwareModeInfoRange, hwareModeInfoUnit);
                                }
                                else if (deleteMode)
                                {
                                    BlockCreatorActions.DeleteMode(modeName);
                                }
                                else
                                {
                                    Console.WriteLine("block mode");
                                    modeOptions.WriteOptionDescriptions(Console.Out);
                                }
                            }
                            catch (BlocksXmlValueException bxve)
                            {
                                ErrorWithBlocksXmlValue(bxve);
                                break;
                            }

                            break;
                        }

                    case "finish":
                        {
                            BlockCreatorActions.Finish();

                            break;
                        }

                    default:
                        {
                            try
                            {
                                noCommandGivenOptions.Parse(args);
                            }
                            catch (OptionException oe)
                            {
                                Console.WriteLine("Error: " + oe.Message);
                                Console.WriteLine("Try 'block --help' for more information");
                            }

                            if (showHelp)
                            {
                                ShowHelp();
                            }

                            break;
                        }
                }
            }
            else
            {
                ShowHelp();
            }
        }


        private static void ShowHelp()
        {
            Console.WriteLine();

            // TODO: maybe remove this line or make it only appear in particular cases?
            Console.WriteLine("BlockCreator - a command line utility for creating LEGO Mindstorms blocks");

            Console.WriteLine();

            Console.WriteLine("Usage: block <command> <options>");

            Console.WriteLine();

            Console.WriteLine("block");
            noCommandGivenOptions.WriteOptionDescriptions(Console.Out);

            Console.WriteLine();

            Console.WriteLine("block module");
            moduleOptions.WriteOptionDescriptions(Console.Out);
        }
        
        private static void ErrorWithOptions(string error)
        {
            Console.WriteLine("Error: " + error);
            Console.WriteLine("Try 'block --help' for more information");
        }

        private static void ErrorWithOptions(OptionException oe)
        {
            Console.WriteLine("Error: " + oe.Message);
            Console.WriteLine("Try 'block --help' for more information");
        }

        private static void ErrorWithBlocksXmlValue(BlocksXmlValueException bxve)
        {
            Console.WriteLine("ERROR on argument " + bxve.ValueName + ": " + bxve.ErrorMessage);
            ShowHelp();
        }
    }
}
