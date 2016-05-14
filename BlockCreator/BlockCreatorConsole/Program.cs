using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NDesk.Options;

using BlockCreatorLogic;

namespace BlockCreatorConsole
{
    /// <summary>
    /// The class containing the entry point into BlockCreator
    /// </summary>
    class Program
    {   
        static void Main(string[] args)
        {
            CommandLineHandler.Handle(args);
        }
    }
}
