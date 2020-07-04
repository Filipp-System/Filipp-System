using System;

namespace Calculator.TestUtilities
{
    /// <summary>
    /// A static class for helper functions
    /// </summary>
    public static class TestUtilities
    {
        public static string GetTemporaryDirectoryPath()
        {
            return Environment.ExpandEnvironmentVariables($@"%TEMP%\{nameof(Calculator.TestUtilities)}");
        }
    }
}
