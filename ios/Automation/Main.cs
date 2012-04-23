using System;
using System.Collections.Generic;
using System.IO;
using Automation.Types;

namespace Automation
{
	class MainClass
	{
		/// <summary>the entry point of the program, where the program control starts and ends.</summary>
		/// <param name='args'>the command-line arguments.</param>
        public static void Main(string[] args)
        {
            // initialize settings based on command line args
            Settings.Initialize(args);
            List<TestInfo> tests;
            DateTime testStartTime = DateTime.Now;

            // set the console window size
            try
            {
                Console.SetWindowSize(Math.Min(150, (Console.LargestWindowWidth * 3) / 4),
                    Math.Min(150, (Console.LargestWindowHeight * 3) / 4));
            }
            catch { }

            tests = DetectTests();

            // mount the automation logging share (if neccessary) and determine the path for the main log
            string mainLogName = "Test Suite \"" + Settings.Test.Suite + "\" @ " + testStartTime.ToShortDateString() + " " + testStartTime.ToShortTimeString();
            string mainLogPath = Path.GetTempPath();

            // add log file listener
            LogEngine.FileSystemListener _MainLogFileListener = new LogEngine.FileSystemListener(mainLogName, mainLogPath, true, true);
            LogEngine _MainLog = new LogEngine(mainLogName, _MainLogFileListener, new LogEngine.ConsoleLogListener());
            if (_MainLogFileListener.TextLogFilePath != null)
                _MainLog.Info("Main Text Log Path: " + _MainLogFileListener.TextLogFilePath);
            if (_MainLogFileListener.HTMLLogFilePath != null)
            _MainLog.Info("Main HTML Log Path: " + _MainLogFileListener.HTMLLogFilePath);

            // check that tests were found
            if (tests.Count == 0)
                _MainLog.CriticalError("No tests were found.");

            // run the tests
            _MainLog.Comment("Beginning to run tests.");
            int testNumber = 1;
            int testNumberPadding = Convert.ToInt32(Math.Floor(Math.Log10(tests.Count)) + 1);
            foreach (TestInfo test in tests)
            {
                // attempt to create the test
                if (test.Initialize())
                {
                    // add log file listener for the test
                    string testLogPath = mainLogPath != null ? Path.Combine(mainLogPath, testNumber.ToString().PadLeft(testNumberPadding, '0')) : Path.GetTempPath();
                    Directory.CreateDirectory(testLogPath);
                    test.LogFileListener = new LogEngine.FileSystemListener(test.AutomatedTestName, testLogPath, true, true);
                    if (test.LogFileListener.TextLogFilePath != null)
                        _MainLog.Info("Test Text Log Path: " + test.LogFileListener.TextLogFilePath);
                    if (test.LogFileListener.HTMLLogFilePath != null)
                        _MainLog.Info("Test HTML Log Path: " + test.LogFileListener.HTMLLogFilePath);
                    test.Log.AddListener(test.LogFileListener);
                    
                    // launch test
                    if (Settings.Test.DebugMode)
                    {
                        // throw all exceptions to the ide
                        test.Setup();
                        test.Run();
                        test.Teardown();
                    }
                    else
                    {
                        // report all exceptions in the log
                        try { test.Setup(); test.Run(); }
                        catch (Exception e) { test.Log.Error(e.ToString()); }
                        try { test.Teardown(); }
                        catch (Exception e) { test.Log.Error(e.ToString()); }
                    }
                    test.Log.CloseAllListeners();

                    // add the test results to the logs and to the automation system
                    _MainLog.Verify(true, test.Log.Success,
                        testNumber.ToString().PadLeft(testNumberPadding, '0') + ".) " + test.Log.Name,
                        false);
                }
                else
                {
                    // add results to the logs and the automation system
                    _MainLog.Fail(testNumber.ToString().PadLeft(testNumberPadding, '0') + ".) " + test.AutomatedTestName);
                }

                // increment the test counter
                testNumber++;
            }

            // wait for user to hit enter before closing console window
            if (Settings.Test.DebugMode && !Utility.SystemOperations.IsRunningOnMacOSX())
                Console.ReadLine();
        }

        /// <summary>detects which tests should be run based on command-line arguments</summary>
        /// <returns>list of tests to be executed</returns>
        private static List<TestInfo> DetectTests()
        {
            List<TestInfo> tests = new List<TestInfo>();
            tests.Add(new TestInfo("iPhone.SampleTest"));
            return tests;
        }
	}
}