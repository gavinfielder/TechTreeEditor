using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechTreeEditor
{
    public class HexConverter
    {
        //Returns a 32-bit hex string
        static public string IntToHex(uint val)
        {
            return val.ToString("X8");
        }

        //Returns an unsigned integer
        static public uint HexToInt(string hex)
        {
            uint num = UInt32.Parse(hex, System.Globalization.NumberStyles.HexNumber);
            return num;
        }
    }
}
