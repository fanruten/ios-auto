using Automation.Clients;
using Automation.Types;

namespace Automation.Tests.iPad
{
    /// <summary>test that is run using the ipad</summary>
    public abstract class iPadTest : Test
    {
        /// <summary>test client used to execute the test</summary>
        public iPadClient Client { get; set; }
        /// <summary>log engine used by the client</summary>
        public LogEngine Log { get { return BaseLog; } }

        /// <summary>sets up the test</summary>
        /// <returns>true if setup completes</returns>
        public override bool Setup()
        {
            Log.Comment("Beginning Test Setup.");
            Client = new iPadClient(Log);
            Log.Comment("Completed Test Setup.");
            return true;
        }

        /// <summary>tears down the ipad test</summary>
        /// <returns>true if tear down completes</returns>
        public override bool Teardown()
        {
            Log.Comment("Beginning Test Teardown.");
            Client.StopAutomation();
            Client.Teardown();
            Log.Comment("Completed Test Teardown.");
            return true;
        }
    }
}