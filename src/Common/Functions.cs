using System;
using System.IO;

namespace DispatchSystem.Common
{
    public static class Functions
    {
        public static string ExceptionHandlerBackend(this Exception e, int exitCode)
        {
            string errorFile = "error0.dump";

            {
                ushort reiteration = 0;

                while (File.Exists(errorFile))
                    errorFile = "error" + ++reiteration + ".dump";
            }

            return errorFile;
        }

        public static string ToSplitID(this uint rawValue)
        {
            try
            {
                char[] idC = rawValue.ToString().ToCharArray();

                return new string(new[]
                {
                    idC[0],
                    idC[1],
                    idC[2],
                    '-',
                    idC[3],
                    idC[4],
                    idC[5],
                    '-',
                    idC[6],
                    idC[7],
                    idC[8]
                });
            }
            catch
            {
                return rawValue.ToString();
            }
        }

        public static uint ToRawID(this string id)
        {
            try
            {
                char[] idC = id.ToCharArray();

                return uint.Parse(new string(new[]
                {
                    idC[0],
                    idC[1],
                    idC[2],

                    idC[4],
                    idC[5],
                    idC[6],

                    idC[8],
                    idC[9],
                    idC[10],
                }));
            }
            catch
            {
                return uint.Parse(id.Replace("-", ""));
            }
        }
    }
}
