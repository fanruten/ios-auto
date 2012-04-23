using Automation.Types;

namespace Automation.Clients
{
    /// <summary>test client which can execute ios javascript automation command</summary>
    public class iPhoneClient : iOSClient
    {
        /// <summary>constructor</summary>
        /// <param name="targetLog">log engine to use</param>
        public iPhoneClient(LogEngine targetLog) : base(targetLog, iOSDevice.iPhone)
        {
			
        }
    }
}