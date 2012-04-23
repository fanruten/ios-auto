using System;
using Automation.Tests;

namespace Automation.Types
{
    /// <summary>stores information about a test</summary>
    public class TestInfo
    {
        /// <summary>test object</summary>
        private Test _TestObject = null;

        /// <summary>id of the test</summary>
        public int TestRailTestID;
        /// <summary>id of the test case</summary>
        public int TestRailCaseID;
        /// <summary>name of the automated test</summary>
        public string AutomatedTestName;
        /// <summary>name of the test</summary>
        public string Name { get { return _TestObject.Name; } }
        /// <summary>log used by the test</summary>
        public LogEngine Log { get { return _TestObject.BaseLog; } }
        /// <summary>place to attach a log file listener</summary>
        public LogEngine.FileSystemListener LogFileListener { get; set; }

        /// <summary>constructor</summary>
        /// <param name="automatedTestName">name of the automated test (path under Automation.Tests)</param>
        /// <param name="testRailTestID">id of the test in test link</param>
        /// <param name="testRailCaseID">id of the test case in test link</param>
        public TestInfo(string automatedTestName, int testRailTestID=-1, int testRailCaseID=-1)
        {
            AutomatedTestName = automatedTestName;
            TestRailTestID = testRailTestID;
            TestRailCaseID = testRailCaseID;
        }

        /// <summary>initializes the test so that it can be run</summary>
        /// <returns>true if successful</returns>
        public bool Initialize()
        {
            if (IsInitialized)
                return true;

            try
            {
                Type testType = Type.GetType("Automation.Tests." + AutomatedTestName);
                _TestObject = (Test)Activator.CreateInstance(testType);
                return true;
            }
            catch { return false; }
        }

        /// <summary>true if the test has been initialized</summary>
        public bool IsInitialized { get { return _TestObject != null; } }

        /// <summary>runs the test</summary>
        /// <returns>true if test set up completes</returns>
        public bool Setup() { return _TestObject.Setup(); }

        /// <summary>runs the test</summary>
        /// <returns>true if test execution completes</returns>
        public bool Run() { return _TestObject.Run(); }

        /// <summary>runs the test</summary>
        /// <returns>true if test tar down completes</returns>
        public bool Teardown() { return _TestObject.Teardown(); }
    }
}
