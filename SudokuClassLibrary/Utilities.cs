using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SudokuClassLibrary
{
    public enum CallerNameFormat
    {
        CallingMemberNameOnly = 0,
        IncludeClassName,
        IncludeClassNameAndNamespace,
    }

    public static class Utilities
    {
        /// <summary>
        /// Returns the name of the method or property that called this method.
        /// </summary>
        /// <param name="caller">A dummy parameter, used only to annotate with the [CallerMemberName] 
        /// attribute.  When calling this method do not specify a value for this argument, the value 
        /// will be provided via the attribute.</param>
        /// <returns>The name of the method or property that called this method.</returns>
        /// <remarks>
        /// From answer to Stackoverflow question "Using nameof to get name of current method", 
        /// https://stackoverflow.com/a/38098931/216440
        /// 
        /// The <see cref="CallerMemberNameAttribute">CallerMemberNameAttribute</see> is available 
        /// from .NET Framework 4.5 onwards.
        /// 
        /// The reason for setting the default value of <paramref name="caller"/> to a string, rather 
        /// than to null, is to allow the return type to be <see cref="String">string</see>, rather 
        /// than string?, for .NET 6+/C# 10+.
        /// </remarks>
        /// <example>
        /// The following example shows how to retrieve the name of the executing method.
        /// <code>
        /// string thisMethodName = Utilities.GetCallerName();
        /// </code>
        /// </example>
        public static string GetCallerName(
            [CallerMemberName] string caller = "[DO NOT PASS A VALUE TO THIS PARAMETER]")
        {
            return caller;
        }

        /// <summary>
        /// Extension method that works with any class to return the name of the method or property 
        /// that called this method.
        /// </summary>
        /// <param name="type">Represents the type instance the extension method is applied to.
        /// </param>
        /// <param name="caller">A dummy parameter, only used to be annotated with the 
        /// [CallerMemberName] attribute.  When calling this method do not specify a value for 
        /// this argument, the value will be provided via the attribute.</param>
        /// <param name="callerNameFormat">Determines the format of the caller name that is 
        /// returned: Caller member name only, Caller member name prefixed by caller class 
        /// name, or Caller member name prefixed by namespace and caller class name.</param>
        /// <returns>The name of the method or property that called this method, in the format 
        /// determined by <paramref name="callerNameFormat"/>.</returns>
        /// <remarks>
        /// Based on answer to Stackoverflow question "Using nameof to get name of current method", 
        /// https://stackoverflow.com/a/58919663/216440
        /// 
        /// The <see cref="CallerMemberNameAttribute">CallerMemberNameAttribute</see> is available 
        /// from .NET Framework 4.5 onwards.
        /// 
        /// The reason for setting the default value of <paramref name="caller"/> to a string, rather 
        /// than to null, is to allow the return type to be <see cref="String">string</see>, rather 
        /// than string? (nullable string), for .NET 6+/C# 10+.
        /// </remarks>
        /// <example>
        /// The following examples shows how to retrieve the name of the executing method.  
        /// The first example returns {method name}.  
        /// The second example returns {class name}.{method name}.  
        /// The third example returns {namespace}.{class name}.{method name}.
        /// <code>
        /// string thisMethodName = this.GetCallerName();
        /// string thisMethodWithClassName = 
        ///     this.GetCallerName(callerNameFormat: CallerNameFormat.IncludeClassName);
        /// string thisMethodWithFullClassName = 
        ///     this.GetCallerName(callerNameFormat: CallerNameFormat.IncludeClassNameAndNamespace);
        /// </code>
        /// </example>
        public static string GetCallerName(this object type,
            [CallerMemberName] string caller = "[DO NOT PASS A VALUE TO THIS PARAMETER]",
            CallerNameFormat callerNameFormat = CallerNameFormat.CallingMemberNameOnly)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            switch (callerNameFormat)
            {
                case CallerNameFormat.IncludeClassName:
                    return $"{type.GetType().Name}.{caller}";
                case CallerNameFormat.IncludeClassNameAndNamespace:
                    return $"{type.GetType().FullName}.{caller}";
                case CallerNameFormat.CallingMemberNameOnly:
                default:
                    return caller;
            }
        }

        /// <summary>
        /// Blocks while condition is true or timeout occurs.
        /// </summary>
        /// <param name="condition">The condition that will perpetuate the block.</param>
        /// <param name="frequency">The frequency at which the condition will be checked, in milliseconds.</param>
        /// <param name="timeout">The timeout in milliseconds.</param>
        /// <exception cref="TimeoutException"></exception>
        /// <remarks>
        /// Copied from answer to Stackoverflow question "C# Wait until condition is true", 
        /// https://stackoverflow.com/a/52357854/216440
        /// 
        /// See https://dotnetfiddle.net/Vy8GbV for example of usage.
        /// </remarks>
        public static async Task WaitWhile(Func<bool> condition, int frequency = 25, int timeout = -1)
        {
            var waitTask = Task.Run(async () =>
            {
                while (condition()) await Task.Delay(frequency);
            });

            if (waitTask != await Task.WhenAny(waitTask, Task.Delay(timeout)))
                throw new TimeoutException();
        }

        /// <summary>
        /// Blocks until condition is true or timeout occurs.
        /// </summary>
        /// <param name="condition">The break condition.</param>
        /// <param name="frequency">The frequency at which the condition will be checked, in milliseconds.</param>
        /// <param name="timeout">The timeout in milliseconds.</param>
        /// <remarks>
        /// Copied from answer to Stackoverflow question "C# Wait until condition is true", 
        /// https://stackoverflow.com/a/52357854/216440
        /// 
        /// See https://dotnetfiddle.net/Vy8GbV for example of usage.
        /// </remarks>
        public static async Task WaitUntil(Func<bool> condition, int frequency = 25, int timeout = -1)
        {
            var waitTask = Task.Run(async () =>
            {
                while (!condition()) await Task.Delay(frequency);
            });

            if (waitTask != await Task.WhenAny(waitTask,
                    Task.Delay(timeout)))
                throw new TimeoutException();
        }
    }
}
