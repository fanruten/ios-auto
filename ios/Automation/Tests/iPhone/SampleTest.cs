namespace Automation.Tests.iPhone
{
    public class SampleTest : iPhoneTest
    {
        public override string Name { get { return "iPhone Login Test"; } }

        public override bool Run()
        {
			
			int randA = Utility.RandomNumber(0,100);
			int randB = Utility.RandomNumber(0,100);
			
			// this statement can be rewritten to not use the batch calling mechanism
			// it would look like this, but it would run much slower
			//  Client.SetValue(Hooks.iPhone.MainScreen.IntegerATextField, "2");
			//	Client.SetValue(Hooks.iPhone.MainScreen.IntegerATextField, "3");
			//	Client.Tap(Hooks.iPhone.MainScreen.ComputeSumButton);
			Client.RunBatchJob(
            	() => Client.SetValue(Hooks.iPhone.MainScreen.IntegerATextField, randA.ToString()),
				() => Client.SetValue(Hooks.iPhone.MainScreen.IntegerBTextField, randB.ToString()),
				() => Client.Tap(Hooks.iPhone.MainScreen.ComputeSumButton)
			);
			
			Log.Verify( (randA + randB).ToString(), Client.GetValue(Hooks.iPhone.MainScreen.ResultLabel), "Verifying the sum was computed correctly.");
            return Log.Success;
        }
    }
}
