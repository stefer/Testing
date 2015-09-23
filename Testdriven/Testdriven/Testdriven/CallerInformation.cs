using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Testdriven
{
    public class CallerInformation
    {
        public string GetCallerMemberName([CallerMemberName] string caller = null)
        {
            return caller;
        }

        public string GetCallerFillePath([CallerFilePath] string path = null)
        {
            return path;
        }

        public int GetCallerLineNumber([CallerLineNumber] int line = -1)
        {
            return line;
        }
    }
}
