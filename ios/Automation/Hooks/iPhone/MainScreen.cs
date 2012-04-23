namespace Automation.Hooks
{
    /// <summary>models the iphone app user interface</summary>
    public static partial class iPhone
    {
        /// <summary>hooks pertaining to the main screen</summary>
        public static class MainScreen
        {
            /// <summary>text field for integer a</summary>
            public static iOSHook IntegerATextField = new iOSHook("mainWindow.textFields()[0]");
			/// <summary>test field for integer b/summary>
            public static iOSHook IntegerBTextField = new iOSHook("mainWindow.textFields()[1]");
			/// <summary>button the user taps to compute the sum</summary>
            public static iOSHook ComputeSumButton = new iOSHook("mainWindow.buttons()[\"Compute Sum\"]");
			/// <summary>label which displays the sum after its computed</summary>
			public static iOSHook ResultLabel = new iOSHook("mainWindow.staticTexts()[0]");
        }
    }
}
