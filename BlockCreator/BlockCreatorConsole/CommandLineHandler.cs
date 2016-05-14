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

        // block
        private static bool showHelp = false;

        // The options you can pass when you do not specify a command
        // (such as "module" or "makeblock")
        private static OptionSet noCommandOptions = new OptionSet()
        {
            { "h|help", "show help about BlockCreator",
                v => showHelp = true }
        };

        // To see how each of these parameters are used in the
        // generated blocks.xml, see README.md

        // block module
        private static string moduleName = "";
        private static string moduleVersion = "";
        
        private static OptionSet moduleOptions = new OptionSet()
        {
            { "n|name=", "the name of the module",
                v => moduleName = v },
            { "v|version=", "the current version of the module",
                v => moduleVersion = v }
        };
        
        // block editmodule
        private static string editModuleName = "";
        private static string editModuleVersion = "";

        private static OptionSet editModuleOptions = new OptionSet()
        {
            { "n|name=", "the name of the module",
                v => moduleName = v },
            { "v|version=", "the current version of the module",
                v => moduleVersion = v }
        };

        // block makeblock
        private static string blockName = "";
        private static string blockFamily = "";

        private static OptionSet makeBlockOptions = new OptionSet()
        {
            { "n|name=", "the name of the block",
              v => blockName = v },
            { "f|family=", "the family the block is in (Action, DataOperations, FlowControl, Sensor, or Advanced)",
              v => blockFamily = v }
        };

        // block removeblock
        private static string removeBlockName = "";

        private static OptionSet removeBlockOptions = new OptionSet()
        {
            { "n|name=", "the name of the block to remove",
              v => removeBlockName = v }
        };

        // block param
        private static string paramName = "";
        private static string paramDirection = "";
        private static string paramDataType = ""; // TODO: to keep BlockCreator simple, we may remove this
        private static string paramDefaultValue = "";
        private static string paramMinValue = "";
        private static string paramMaxValue = "";
        private static string paramConfig = "";
        private static string paramIdent = "";
        private static string paramCompilerDirs = "";
        private static string paramValueDisplay = "";
        private static bool removeParam = false;

        private static OptionSet paramOptions = new OptionSet()
        {
            { "n|name=", "the name of the parameter",
              v => paramName = v },
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
            { "c|configuration=", "the parameter's configuration (what the parameter does when you click on it)",
              v => paramConfig = v },
            { "i|identification=", "the image associated with this parameter",
              v => paramIdent = v },
            { "i|compilerDirs=", "any special purposes that this parameter will be used for",
              v => paramCompilerDirs = v },
            // TODO: figure out what valueDisplay actually does (the documentation only mentions this one
            // purpose as an example, but doesn't actually say what it represents)
            { "p|valueDisplay=", "the images displayed at the top of the cases of a switch block (this is the only use of the value display that we currently know of)",
              v => paramValueDisplay = v },
            { "r|remove", "if present, deletes the parameter specified by the \"name\" option",
              v => removeParam = (v != null) }
        };

        // block hardware
        private static string hwareEv3AutoId = "";
        private static string hwareOtherAutoId = "";
        private static string hwareDirection = "";
        private static string hwareDefaultPort = "";

        private static OptionSet hwareOptions = new OptionSet()
        {
            { "i|ev3AutoId=", "the number used for the EV3's Auto ID functionality (if it does not not use this, then it should just be -1",
              v => hwareEv3AutoId = v },
            { "o|otherAutoId=", "the Auto IDs of other sensors that also use this block",
              v => hwareOtherAutoId = v },
            { "d|direction=", "the direction of the block (Input or Output)",
              v => hwareDirection = v },
            { "p|defaultPort=", "the default port of the block",
              v => hwareDefaultPort = v }
        };

        // block makemode
        private static string modeName = "";

        private static OptionSet makeModeOptions = new OptionSet()
        {
            { "n|name=", "the name of the mode",
               v => modeName = v }
        };

        // block editmode
        private static string editModeName = "";
        private static string modeRef = "";
        private static string modeParamRef = "";
        // The existance of this element indicates the default mode
        // for a block. Its actual purpose, however, is to tell the block's
        // weight (where it appears from left to right in the palette).
        private static string modeDefaultAndWeight = "";
        private static string modeType = "";
        private static string modeFlags = "";
        private static string hwareModeInfoName = "";
        private static string hwareModeInfoId = "";
        private static string hwareModeInfoRange = "";
        private static string hwareModeInfoUnit = "";
        private static bool editModeRemove = false;

        private static OptionSet editModeOptions = new OptionSet()
        {
            { "n|name=", "the name of the mode",
               v => editModeName = v },
            { "v|vixRef=", "the name of the VIX file that is the source code for this mode",
               v => modeRef = v },
            { "p|paramRef=", "adds a parameter to this block from the list of parameters defined with \"block param\"",
               v => modeParamRef = v },
            { "w|weight=", "the weight of this mode (where it goes from left to right on the palette); the existance of this element in a mode makes that mode the default mode for the block",
               v => modeDefaultAndWeight = v },
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
               v => hwareModeInfoUnit = v },
            { "r|remove", "removes the elements pointed to by the other options (use this to remove elements)",
               v => editModeRemove = (v != null) }
        };

        // block removemode
        private static string removeModeName = "";

        private static OptionSet removeModeOptions = new OptionSet()
        {
            { "n|name=", "the name of the mode to remove",
               v => removeModeName = v }
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
                                BlockCreatorActions.CreateModule(moduleName, moduleVersion);
                            }
                            catch (BlocksXmlValueException bxve)
                            {
                                ErrorWithBlocksXmlValue(bxve);
                                break;
                            }

                            Console.WriteLine("Module name: " + moduleName);
                            Console.WriteLine("Module version: " + moduleVersion);

                            break;
                        }

                    case "editmodule":
                        {
                            try
                            {
                                editModuleOptions.Parse(options);
                            }
                            catch (OptionException oe)
                            {
                                ErrorWithOptions(oe);
                                break;
                            }

                            try
                            {
                                BlockCreatorActions.EditModule(editModuleName, editModuleVersion);
                            }
                            catch (BlocksXmlValueException bxve)
                            {
                                ErrorWithBlocksXmlValue(bxve);
                                break;
                            }

                            Console.WriteLine("Module name: " + moduleName);
                            Console.WriteLine("Module version: " + moduleVersion);

                            break;
                        }

                    case "makeblock":
                        {
                            try
                            {
                                makeBlockOptions.Parse(options);
                            }
                            catch (OptionException oe)
                            {
                                ErrorWithOptions(oe);
                                break;
                            }

                            try
                            {
                                BlockCreatorActions.CreateBlock(blockName, blockFamily);
                            }
                            catch (BlocksXmlValueException bxve)
                            {
                                ErrorWithBlocksXmlValue(bxve);
                                break;
                            }

                            break;
                        }

                    case "removeblock":
                        {
                            try
                            {
                                removeBlockOptions.Parse(options);
                            }
                            catch (OptionException oe)
                            {
                                ErrorWithOptions(oe);
                                break;
                            }

                            try
                            {
                                BlockCreatorActions.RemoveBlock(removeBlockName);
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
                                BlockCreatorActions.Parameter(paramName, paramDirection, paramDataType, paramDefaultValue, paramMinValue, paramMaxValue, paramConfig, paramIdent, paramCompilerDirs, paramValueDisplay, removeParam);
                            }
                            catch (BlocksXmlValueException bxve)
                            {
                                ErrorWithBlocksXmlValue(bxve);
                                break;
                            }

                            break;
                        }

                    default:
                        {
                            try
                            {
                                noCommandOptions.Parse(args);
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
            noCommandOptions.WriteOptionDescriptions(Console.Out);

            Console.WriteLine();

            Console.WriteLine("block module");
            moduleOptions.WriteOptionDescriptions(Console.Out);
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
