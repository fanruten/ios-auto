using System;
using System.Collections.Generic;

namespace Automation
{
    /// <summary>manages settings used by the program</summary>
    public static class Settings
    {
        #region public settings

        /// <summary>space where command line arguments are stored</summary>
        public static List<string> Arguments { get { return (List<string>)_SettingsStorage[_ARGUMENTS]; } }

        /// <summary>setting for the environment</summary>
        public static class Environment
        {
            /// <summary>id of non dev server (e.g. core-stag-rc)</summary>
            public static string NonDevID { get; set; }
        }

        /// <summary>settings for which tests to execute and how to execute them</summary>
        public static class Test
        {
            /// <summary>true if exceptions should be thrown to the IDE</summary>
            public static bool DebugMode { get { return _GetBool(_DEBUG); } set { _SetBool(_DEBUG, value); } }
            /// <summary>list of tests to run</summary>
            public static string List { get { return _GetString(_TESTS); } set { _SetString(_TESTS, value); } }
            /// <summary>name of test suite to execute</summary>
            public static string Suite { get { return _GetString(_TEST_SUITE); } set { _SetString(_TEST_SUITE, value); } }
        }

        /// <summary>settings related to logging</summary>
        public static class Logging
        {
            /// <summary>true if verbose entries should be written to the log</summary>
            public static bool Verbose { get { return _GetBool(_LOG_VERBOSE); } set { _SetBool(_LOG_VERBOSE, value); } }
        }


        /// <summary>settings for a selenium instance</summary>
        public static class Selenium
        {
            /// <summary>url where selenium will start tests</summary>
            public static string BaseURL { get { return _GetString(_BASE_URL); } set { _SetString(_BASE_URL, value); } }
            /// <summary>desired port for selenium rc to use</summary>
            public static int Port { get { return _GetInt(_SELENIUM_PORT); } set { _SetInt(_SELENIUM_PORT, value); } }
            /// <summary>selenium browser code for the browser selenium should launch</summary>
            public static string Browser { get { return _GetString(_BROWSER); } set { _SetString(_BROWSER, value); } }
        }

        /// <summary>settings related to iOS</summary>
        public static class iOS
        {
            /// <summary>path to the xcode project for the ios application</summary>
            public static string XCodeProjectPath { get { return _GetString(_XCODE_PROJ_PATH); } set { _SetString(_XCODE_PROJ_PATH, value); } }
        }

        /// <summary>settings for user accounts to be used by tests</summary>
        public static class Users
        {
            /// <summary>default account</summary>
            public static class Default
            {
                /// <summary>email address of the default account</summary>
                public static string EmailAddress { get { return _GetString(_USER1_EMAIL); } set { _SetString(_USER1_EMAIL, value); } }
                /// <summary>password of the default account</summary>
                public static string Password { get { return _GetString(_USER1_PASSWORD); } set { _SetString(_USER1_PASSWORD, value); } }
            }
        }

        #endregion

        #region private constants
        private const string _ARGUMENTS = "ARGUMENTS";
        private const string _DEBUG = "DEBUG";
        private const string _LOG_VERBOSE = "LOG_VERBOSE";
        private const string _TEST_SUITE = "TEST_SUITE";
        private const string _TESTS = "TESTS";
        private const string _BASE_URL = "BASE_URL";
        private const string _SELENIUM_PORT = "SELENIUM_PORT";
        private const string _XCODE_PROJ_PATH = "XCODE_PROJ_PATH";
        private const string _BROWSER = "BROWSER";
        private const string _USER1_EMAIL = "USER1_EMAIL";
        private const string _USER1_PASSWORD = "USER1_PASSWORD";
        #endregion

        #region default values
        /// <summary>contains the settings values references by other elements of this class</summary>
        private static Dictionary<string, object> _SettingsStorage = new Dictionary<string, object> 
        {
            {_ARGUMENTS, (object)new List<string>()},
            {_DEBUG, false},
            {_LOG_VERBOSE, true},
            {_TEST_SUITE, "Untitled"},
            {_TESTS, "ScratchTest"},
            {_BASE_URL, "https://www.zoosk.com"},
            {_SELENIUM_PORT, 4444},
            {_XCODE_PROJ_PATH, ""},
            {_BROWSER, ""},
            {_USER1_EMAIL,"user@email.com"},
            {_USER1_PASSWORD,"password"},
        };
        #endregion

        #region public methods

        /// <summary>processes command line arguments and determines settings</summary>
        /// <param name="args">the command line arguments</param>
        public static void Initialize(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                // check for help switch
                if (args[i].ToLower() == "--help")
                    _PrintUsage();

                // check for list switch
                if (args[i].ToLower() == "--list")
                    _PrintTests();

                // split the arguments into key and value
                string[] keyValueSet = args[i].Split(new char[] { '=' }, 2);

                // grab the value if it exists
                if (keyValueSet.Length == 2)
                {
                    string key = keyValueSet[0].ToUpper();
                    string value = keyValueSet[1];
                    
                    if (key == "TEST")
                        key = "TESTS";
                    
                    if (key != "" && _SettingsStorage.ContainsKey(key) && value != "")
                    {
                        Type t = (_SettingsStorage[key]).GetType();
                        if (t == typeof(bool))
                        {
                            _SetBool(key, value);
                        }
                        else if (t == typeof(int))
                        {
                            _SetInt(key, value);
                        }
                        else
                        {
                            _SetString(key, value);
                        }
                    }
                }
                else if (keyValueSet.Length == 1)
                {
                    Settings.Arguments.Add(keyValueSet[0]);
                }
            }

            if (!Selenium.BaseURL.EndsWith("/"))
                Selenium.BaseURL = Selenium.BaseURL + "/";
        }

        #endregion

        #region private helper methods
        
        /// <summary>prints how to use the application</summary>
        private static void _PrintUsage()
        {
            // write header
            Console.WriteLine("Usage: Automation.exe [SETTING=value] ...");
            Console.WriteLine("");

            // write list of valid settings
            Console.Write("Valid Settings: ");
            foreach (string key in _SettingsStorage.Keys)
                Console.Write(key + ", ");

            // delete extra comma using backspace
            Console.Write("\b\b ");
            Console.WriteLine();

            // exit the program
            System.Environment.Exit(0);
        }

        /// <summary>prints all tests that can be run</summary>
        private static void _PrintTests()
        {
            // write header
            Console.WriteLine("Usage: Automation.exe [SETTING=value] ...");
            Console.WriteLine("");

            // get the list of tests
            string[] tests = Utility.GetAllTestNames();

            // write the list of tests
            Console.WriteLine("List Of Tests:");
            foreach (string testName in tests)
                Console.WriteLine(testName);

            // exit the program
            System.Environment.Exit(0);
        }

        /// <summary>converts a string value in the settings dictionary to a bool</summary>
        /// <param name="key">dictionary key to look up</param>
        /// <returns>bool corresponding to supplied key</returns>
        private static bool _GetBool(string key) { return (bool)_SettingsStorage[key]; }

        /// <summary>converts a string value in the settings dictionary to an int</summary>
        /// <param name="key">dictionary key to look up</param>
        /// <returns>int corresponding to supplied key</returns>
        private static int _GetInt(string key) { return (int)_SettingsStorage[key]; }

        /// <summary>gets a string value in the settings dictionary</summary>
        /// <param name="key">dictionary key to look up</param>
        /// <returns>string corresponding to supplied key</returns>
        private static string _GetString(string key) { return (string)_SettingsStorage[key]; }

        /// <summary>sets a bool value for a key in the dictionary</summary>
        /// <param name="key">dictionary key whose value will be modified</param>
        /// <param name="val">new value for the key</param>
        private static void _SetBool(string key, object val)
        {
            bool newVal;
            if (Boolean.TryParse(val.ToString(), out newVal))
                _SettingsStorage[key] = (object)newVal;
        }

        /// <summary>sets an int value for a key in the dictionary</summary>
        /// <param name="key">dictionary key whose value will be modified</param>
        /// <param name="val">new value for the key</param>
        private static void _SetInt(string key, object val)
        {
            int newVal;
            if (int.TryParse(val.ToString(), out newVal))
                _SettingsStorage[key] = (object)newVal;
        }

        /// <summary>sets a string  value for a key in the dictionary</summary>
        /// <param name="key">dictionary key whose value will be modified</param>
        /// <param name="val">new value for the key</param>
        private static void _SetString(string key, object val) { _SettingsStorage[key] = val.ToString(); }

        #endregion
    }

}
