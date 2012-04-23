using Automation.Types;

namespace Automation.Tests
{
    /// <summary>bas class for tests that can be executed</summary>
    public abstract class Test
    {
        /// <summary>stores log information</summary>
        public LogEngine BaseLog { get; set; }
        /// <summary>name of the test</summary>
        abstract public string Name { get; }

        /// <summary>constructor</summary>
        public Test()
        {
            BaseLog = new LogEngine(this.GetType().FullName.Replace("Automation.Tests.","") + " (" + Name + ")");
        }

        /// <summary>sets up the test</summary>
        /// <returns>true if set up completes</returns>
        public virtual bool Setup() { return true; }

        /// <summary>executes the test</summary>
        /// <returns>true if text execution completes</returns>
        public abstract bool Run();

        /// <summary>tears down the test</summary>
        /// <returns>true if tear down completes</returns>
        public virtual bool Teardown() { return true; }
    }
}
