using Automation.Types;

namespace Automation.Clients
{
    /// <summary>superclass from which all test clients derive</summary>
    public abstract class TestClient
    {
        /// <summary>log engine to use</summary>
        public LogEngine Log;

        /// <summary>constructor</summary>
        /// <param name="targetLog">log engine to use</param>
        public TestClient(LogEngine targetLog)
        {
            this.Log = targetLog;
        }

        /// <summary>sleeps for the supplied number of milliseconds</summary>
        /// <param name="milliSeconds">number of milliseconds to sleep</param>
        /// <param name="writeToLog">true if info should be written to the log</param>
        public void Sleep(int milliSeconds, bool writeToLog=true)
        {
            if (writeToLog)
                Log.Verbose("Sleeping for " + milliSeconds.ToString() + " ms");
            System.Threading.Thread.Sleep(milliSeconds);
        }

        /// <summary>tears down the the test client</summary>
        public virtual void Teardown() { }
    }
}
