using Modding;
using System.IO;
using System.Runtime.CompilerServices;

namespace RandoStats.Util
{
    internal static class ScopedLoggers
    {
        public static Loggable GetLogger([CallerFilePath] string callingFile = "")
        {
            return new SimpleLogger($"RandoStats.{Path.GetFileNameWithoutExtension(callingFile)}");
        }
    }
}
