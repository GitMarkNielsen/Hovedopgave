using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WritingOutput
{
    internal static class ConsoleWriter
    {
        /// <summary>
        /// Custom writer method to write text to console with changing color in the middle.
        /// Used primarily for highlighting folder, files, and errors.
        /// </summary>
        /// <param name="text"></param>
        /// <param name="seperator"></param>
        /// <param name="colour"></param>
        public static void Write(string text, char seperator, ConsoleColor colour = ConsoleColor.White)
        {
            Console.ForegroundColor = ConsoleColor.White;
            string[] splitStrings = text.Split(seperator);


            Console.Write(splitStrings[0]);

            Console.ForegroundColor = colour;
            Console.Write(splitStrings[1]);
            Console.ForegroundColor = ConsoleColor.White;
            
            Console.Write(splitStrings[2] + "\n");

        }


    }
}
