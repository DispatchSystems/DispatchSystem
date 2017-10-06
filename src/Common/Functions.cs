using System;
using System.IO;

namespace DispatchSystem.Common
{
    public static class Functions
    {
        public static void ExceptionHandlerBackend(this Exception e, int ExitCode)
        {
            string errorFile = "error0.dump";

            {
                ushort reiteration = 0;

                while (File.Exists(errorFile))
                    errorFile = "error" + ++reiteration + ".dump";
            }

            Environment.Exit(ExitCode);
        }
    }
}