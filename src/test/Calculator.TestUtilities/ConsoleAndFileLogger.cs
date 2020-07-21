// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Text;

namespace Calculator.TestUtilities
{
    /// <summary>
    /// An interface for logging messages.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Logs a string based message through the logger.
        /// </summary>
        /// <param name="message">The <see cref="message"/> to log.</param>
        void Log(string message);

        /// <summary>
        /// Flushes the cache if one exists.
        /// </summary>
        void Flush();
    }

    public class ConsoleAndFileLogger : ILogger
    {
        private readonly string _file;
        private readonly StringBuilder _buffer = new StringBuilder();

        /// <summary>
        /// Constructs a new ConsoleAndFileLogger with a default log file
        /// </summary>
        public ConsoleAndFileLogger()
        {
            _file = Directory.Exists(TestUtilities.GetTemporaryDirectoryPath()) 
                ? Path.Combine(TestUtilities.GetTemporaryDirectoryPath(), "automation-test-log.txt") 
                : "./automation-test-log.txt";
        }
        void ILogger.Log(string message)
        {
            Console.WriteLine($"{DateTime.Now} : {message}");
            _buffer.AppendLine($"{DateTime.Now} : {message}");
        }

        void ILogger.Flush()
        {
            File.AppendAllText(_file, _buffer.ToString());
            _buffer.Clear();
        }
    }
}
