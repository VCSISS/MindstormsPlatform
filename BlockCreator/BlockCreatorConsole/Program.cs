using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NDesk.Options;

using BlockCreatorLogic;

namespace BlockCreatorConsole
{
    class Program
    {
        private static string blockName = "";
        private static string family = "";
        private static string direction = "";

        private static bool showHelp = false;
        
        private static OptionSet blockOptions = new OptionSet()
                {
                    { "h|help", "show help about BlockCreator",
                        v => showHelp = true }
                };

        private static OptionSet createOptions = new OptionSet()
                {
                    { "n|name=", "the name of the block",
                        v => blockName = v },
                    { "f|family=", "the block's family",
                        v => family = v },
                    { "d|direction=", "the direction the block is configured for",
                        v => direction = v }
                };
        
        static void Main(string[] args)
        {
            if (args.Count() > 0)
            {
                switch (args[0])
                {
                    case "create":
                        {
                            string[] options = args.Skip<string>(1).ToArray<string>();
                            
                            try
                            {
                                createOptions.Parse(options);
                            }
                            catch (OptionException oe)
                            {
                                Console.WriteLine("Error: " + oe.Message);
                                Console.WriteLine("Try 'block --help' for more information");
                            }
                            
                            Console.WriteLine("Block name: " + blockName);
                            Console.WriteLine("Block family: " + family);
                            Console.WriteLine("Block direction: " + direction);
                            
                            BlockCreatorActions.CreateBlock(blockName, family, direction);

                            Console.WriteLine("Created block");

                            break;
                        }

                    default:
                        {
                            try
                            {
                                blockOptions.Parse(args);
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

            Console.WriteLine("BlockCreator - a command line utility for creating LEGO Mindstorms blocks");

            Console.WriteLine();

            Console.WriteLine("Usage: block <command> <options>");

            Console.WriteLine();

            Console.WriteLine("block");
            blockOptions.WriteOptionDescriptions(Console.Out);

            Console.WriteLine();

            Console.WriteLine("block create");
            createOptions.WriteOptionDescriptions(Console.Out);
        }
    }
}
