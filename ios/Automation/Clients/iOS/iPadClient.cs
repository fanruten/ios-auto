using System;
using Automation.Hooks;
using Automation.Types;
using System.Collections.Generic;
using System.Text;

namespace Automation.Clients
{
    /// <summary>test client which can execute ios javascript automation command</summary>
    public class iPadClient : iOSClient
    {
        /// <summary>constructor</summary>
        /// <param name="targetLog">log engine to use</param>
        public iPadClient(LogEngine targetLog) : base(targetLog, iOSDevice.iPad)
        {
			
		}
    }
}