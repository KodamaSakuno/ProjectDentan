using System;
using System.Diagnostics;
using System.IO;

namespace Moen.KanColle.Dentan
{
    public static class DebugUtil
    {
        [Conditional("DEBUG")]
        public static void Log(string rpContent)
        {
            File.AppendAllText(@"Log\Debug.txt", string.Format("{0}: {1}{2}", DateTime.Now, rpContent, Environment.NewLine));

            Debug.WriteLine(rpContent);
        }
    }
}
