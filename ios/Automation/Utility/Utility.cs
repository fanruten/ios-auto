using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace Automation
{
    /// <summary>contains utility methods used widely across the code base</summary>
    public static partial class Utility
    {
        /// <summary>default amount of time to wait for actions</summary>
        public const int DEFAULT_WAIT_TIME = 30000;
        /// <summary>default amount of time to wait between polling actions</summary>
        public const int DEFAULT_POLL_INTERVAL = 100;
		
		/// <summary>generates a random number in the supplied range</summary>
        /// <param name="fromInt">lowest number that can be returned</param>
        /// <param name="toInt">highest number that can be returned</param>
        /// <returns>random number in the supplied range</returns>
        public static int RandomNumber(int fromInt, int toInt)
        {
            Random random = new Random();
            return random.Next(fromInt, toInt+1);
        }

        /// <summary>waits for a function to return a desired value</summary>
        /// <typeparam name="T">the return type</typeparam>
        /// <param name="F">the function to execute</param>
        /// <param name="desiredValue">the desired value that will terminate the wait</param>
        /// <param name="maxWaitInMilliseconds">maximum amount of time to wait in milliseconds</param>
        /// <param name="pollingIntervalInMilliseconds">time to wait between polling actions in milliseconds</param>
        /// <returns>true if the function evaluates the desired value within the time limit</returns>
        public static bool WaitWithTimeout<T>(Func<T> F, T desiredValue, int maxWaitInMilliseconds = DEFAULT_WAIT_TIME, int pollingIntervalInMilliseconds=DEFAULT_POLL_INTERVAL)
        {
            DateTime startTime = DateTime.Now;
            while (DateTime.Now.Subtract(startTime).TotalMilliseconds < maxWaitInMilliseconds)
            {
                // return true if the function evaluates tp the desired value
                if (F().Equals(desiredValue))
                    return true;
                // sleep before checking again
                System.Threading.Thread.Sleep(pollingIntervalInMilliseconds);
            }
            // give it one last chance
            return F().Equals(desiredValue);
        }

        /// <summary>gets the names of all the tests</summary>
        /// <param name="prefix">prefix that the test names must start with</param>
        /// <returns>array of all of the test names</returns>
        public static string[] GetAllTestNames(string prefix=null)
        {
            List<string> tests = new List<string>();
            foreach (Type t in Assembly.GetExecutingAssembly().GetTypes())
            {
                try
                {
                    string testName = t.FullName.Replace("Automation.Tests.", "");
                    if (prefix == null || testName.StartsWith(prefix))
                        if (typeof(Automation.Tests.Test).IsAssignableFrom(t) && !t.IsAbstract)
                            tests.Add(testName);
                }
                catch { }
            }
            tests.Sort();
            return tests.ToArray();
        }
    }
}
