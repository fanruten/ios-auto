using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Automation.Clients;

namespace Automation
{
    /// <summary>used to log information</summary>
    public class LogEngine
	{
        /// <summary>denotes the type of log entry</summary>
        public enum LogEntryType { SUCCESS, FAILURE, INFO, COMMENT, ERROR, WARNING, VERBOSE, C_ERROR }
        /// <summary>list of log listeners</summary>
        public StatisticsLogListener Statistics;
        /// <summary>name of the log engine</summary>
        public string Name;
        /// <summary>true if the log has not logged any failures or errors</summary>
        public bool Success { get { return Statistics.Success; } }
        /// <summary>list of log listeners</summary>
        private List<ILogListener> _Listeners;

        /// <summary>constructor</summary>
        /// <param name="name">name of the log</param>
		public LogEngine (string name="unnamed log", params ILogListener[] logListeners)
		{
            // name the log engine
            Name = name;
            Statistics = new StatisticsLogListener();
            _Listeners = new List<ILogListener>();
            _Listeners.Add(Statistics);

            if (logListeners.Length < 1)
            {
                // add default listeners
                _Listeners.Add(new ConsoleLogListener());
            }
            else
            {
                // add supplied custom listeners
                foreach (ILogListener consumer in logListeners)
                    _Listeners.Add(consumer);
            }
		}
		
        /// <summary>adds an entry to the log</summary>
        /// <param name="entry">the entry to add</param>
		private void _AddEntry(LogEntry entry)
		{
            foreach (ILogListener logConsumer in _Listeners)
                logConsumer.AddEntry(entry);
		}

        /// <summary>logs a success</summary>
        /// <param name="remark">remark associated with the log entry</param>
		public void Pass(string remark)
		{
            _AddEntry(new LogEntry(LogEntryType.SUCCESS, remark));
		}

        /// <summary>logs a failure</summary>
        /// <param name="remark">remark associated with the log entry</param>
        public virtual void Fail(string remark)
		{
			_AddEntry(new LogEntry(LogEntryType.FAILURE, remark));
		}

        /// <summary>logs an informational entry</summary>
        /// <param name="remark">remark associated with the log entry</param>
        public void Info(string remark)
		{
            _AddEntry(new LogEntry(LogEntryType.INFO, remark));
		}

        /// <summary>logs a comment</summary>
        /// <param name="remark">remark associated with the log entry</param>
        public void Comment(string remark)
		{
            _AddEntry(new LogEntry(LogEntryType.COMMENT, remark));
		}

        /// <summary>logs an error</summary>
        /// <param name="remark">remark associated with the log entry</param>
        public virtual void Error(string remark)
		{
            _AddEntry(new LogEntry(LogEntryType.ERROR, remark));
		}

        /// <summary>logs a warning</summary>
        /// <param name="remark">remark associated with the log entry</param>
        public void Warning(string remark)
        {
            _AddEntry(new LogEntry(LogEntryType.WARNING, remark));
        }

        /// <summary>logs a verbose informational entry</summary>
        /// <param name="remark">remark associated with the log entry</param>
        public void Verbose(string remark)
        {
            if (Settings.Logging.Verbose)
                _AddEntry(new LogEntry(LogEntryType.VERBOSE, remark));
        }

        /// <summary>logs a critical error</summary>
        /// <param name="remark">remark associated with the log entry</param>
        public virtual void CriticalError(string remark)
        {
            _AddEntry(new LogEntry(LogEntryType.C_ERROR, remark));
            throw new Exception("Critical Error: " + remark);
        }
		
		#region public void Assert()
        /// <summary>logs a failure if supplied values are not equal</summary>
        /// <param name="expectedValue">expected value</param>
        /// <param name="actualValue">actual value</param>
        /// <param name="remark">remark associated with the log entry</param>
        ///<returns>true if the expected value is equal to the actual value</returns>
		public bool Assert(bool expectedValue, bool actualValue, string remark)
		{
            if (!(expectedValue == actualValue))
            {
                Fail(remark + " --- EXPECTED: " + expectedValue.ToString() + " ACTUAL: " + actualValue.ToString());
                return false;
            }
            return true;
		}
        /// <summary>logs a failure if supplied values are not equal</summary>
        /// <param name="expectedValue">expected value</param>
        /// <param name="actualValue">actual value</param>
        /// <param name="remark">remark associated with the log entry</param>
        ///<returns>true if the expected value is equal to the actual value</returns>
		public bool Assert(float expectedValue, float actualValue, string remark)
		{
			if (!(expectedValue == actualValue))
            {
				Fail(remark + " --- EXPECTED: " + expectedValue.ToString() + " ACTUAL: " + actualValue.ToString());
                return false;
            }
            return true;
		}
        /// <summary>logs a failure if supplied values are not equal</summary>
        /// <param name="expectedValue">expected value</param>
        /// <param name="actualValue">actual value</param>
        /// <param name="remark">remark associated with the log entry</param>
        ///<returns>true if the expected value is equal to the actual value</returns>
		public bool Assert(int expectedValue, int actualValue, string remark)
		{
			if (!(expectedValue == actualValue))
            {
				Fail(remark + " --- EXPECTED: " + expectedValue.ToString() + " ACTUAL: " + actualValue.ToString());
                return false;
            }
            return true;
		}
        /// <summary>logs a failure if supplied values are not equal</summary>
        /// <param name="expectedValue">expected value</param>
        /// <param name="actualValue">actual value</param>
        /// <param name="remark">remark associated with the log entry</param>
        ///<returns>true if the expected value is equal to the actual value</returns>
		public bool Assert(string expectedValue, string actualValue, string remark)
		{
			if (!(expectedValue == actualValue))
            {
				Fail(remark + " --- EXPECTED: " + expectedValue + " ACTUAL: " + actualValue);
                return false;
            }
            return true;
		}
		#endregion

        #region public void AssertCritical()
        /// <summary>logs a critical error if supplied values are not equal</summary>
        /// <param name="expectedValue">expected value</param>
        /// <param name="actualValue">actual value</param>
        /// <param name="remark">remark associated with the log entry</param>
        ///<returns>true if the expected value is equal to the actual value</returns>
        public bool AssertCritical(bool expectedValue, bool actualValue, string remark)
        {
            if (!(expectedValue == actualValue))
            {
                CriticalError(remark + " --- EXPECTED: " + expectedValue.ToString() + " ACTUAL: " + actualValue.ToString());
                return false;
            }
            return true;
        }
        /// <summary>logs a critical error if supplied values are not equal</summary>
        /// <param name="expectedValue">expected value</param>
        /// <param name="actualValue">actual value</param>
        /// <param name="remark">remark associated with the log entry</param>
        ///<returns>true if the expected value is equal to the actual value</returns>
        public bool AssertCritical(float expectedValue, float actualValue, string remark)
        {
            if (!(expectedValue == actualValue))
            {
                CriticalError(remark + " --- EXPECTED: " + expectedValue.ToString() + " ACTUAL: " + actualValue.ToString());
                return false;
            }
            return true;
        }
        /// <summary>logs a critical error if supplied values are not equal</summary>
        /// <param name="expectedValue">expected value</param>
        /// <param name="actualValue">actual value</param>
        /// <param name="remark">remark associated with the log entry</param>
        ///<returns>true if the expected value is equal to the actual value</returns>
        public bool AssertCritical(int expectedValue, int actualValue, string remark)
        {
            if (!(expectedValue == actualValue))
            {
                CriticalError(remark + " --- EXPECTED: " + expectedValue.ToString() + " ACTUAL: " + actualValue.ToString());
                return false;
            }
            return true;
        }
        /// <summary>logs a critical error if supplied values are not equal</summary>
        /// <param name="expectedValue">expected value</param>
        /// <param name="actualValue">actual value</param>
        /// <param name="remark">remark associated with the log entry</param>
        ///<returns>true if the expected value is equal to the actual value</returns>
        public bool AssertCritical(string expectedValue, string actualValue, string remark)
        {
            if (!(expectedValue == actualValue))
            {
                CriticalError(remark + " --- EXPECTED: " + expectedValue + " ACTUAL: " + actualValue);
                return false;
            }
            return true;
        }
        #endregion
		
		#region public bool Verify()
        /// <summary>logs a failure if supplied values are not equal, a pass otherwise</summary>
        /// <param name="expectedValue">expected value</param>
        /// <param name="actualValue">actual value</param>
        /// <param name="remark">remark associated with the log entry</param>
        /// <param name="includeValuesInOutput">if true, the expected and actual values will be appended to the remark if they differ</param>
        /// <returns>true if successful</returns>
		public bool Verify(bool expectedValue, bool actualValue, string remark, bool includeValuesInOutput=true)
		{
            if (expectedValue == actualValue)
            {
                Pass(remark);
                return true;
            }
            else
            {
                if (includeValuesInOutput)
                    remark += " --- EXPECTED: " + expectedValue.ToString() + " ACTUAL: " + actualValue.ToString();
                Fail(remark);
                return false;
            }
		}

        /// <summary>logs a failure if supplied values are not equal, a pass otherwise</summary>
        /// <param name="expectedValue">expected value</param>
        /// <param name="actualValue">actual value</param>
        /// <param name="remark">remark associated with the log entry</param>
        /// <param name="includeValuesInOutput">if true, the expected and actual values will be appended to the remark if they differ</param>
        /// <returns>true if successful</returns>
        public bool Verify(float expectedValue, float actualValue, string remark, bool includeValuesInOutput = true)
		{
            if (expectedValue == actualValue)
            {
                Pass(remark);
                return true;
            }
            else
            {
                if (includeValuesInOutput)
                    remark += " --- EXPECTED: " + expectedValue.ToString() + " ACTUAL: " + actualValue.ToString();
                Fail(remark);
                return false;
            }
		}

        /// <summary>logs a failure if supplied values are not equal, a pass otherwise</summary>
        /// <param name="expectedValue">expected value</param>
        /// <param name="actualValue">actual value</param>
        /// <param name="remark">remark associated with the log entry</param>
        /// <param name="includeValuesInOutput">if true, the expected and actual values will be appended to the remark if they differ</param>
        /// <returns>true if successful</returns>
        public bool Verify(int expectedValue, int actualValue, string remark, bool includeValuesInOutput = true)
		{
            if (expectedValue == actualValue)
            {
                Pass(remark);
                return true;
            }
            else
            {
                if (includeValuesInOutput)
                    remark += " --- EXPECTED: " + expectedValue.ToString() + " ACTUAL: " + actualValue.ToString();
                Fail(remark);
                return false;
            }
		}

        /// <summary>logs a failure if supplied values are not equal, a pass otherwise</summary>
        /// <param name="expectedValue">expected value</param>
        /// <param name="actualValue">actual value</param>
        /// <param name="remark">remark associated with the log entry</param>
        /// <param name="includeValuesInOutput">if true, the expected and actual values will be appended to the remark if they differ</param>
        /// <returns>true if successful</returns>
        public bool Verify(string expectedValue, string actualValue, string remark, bool includeValuesInOutput = true)
		{
            if (expectedValue == actualValue)
            {
                Pass(remark + " --- VALUE: " + expectedValue);
                return true;
            }
            else
            {
                if (includeValuesInOutput && expectedValue != null && actualValue != null)
                    remark += " --- EXPECTED: " + expectedValue.ToString() + " ACTUAL: " + actualValue.ToString();
                Fail(remark);
                return false;
            }
		}
		#endregion

        #region public bool VerifyCritical()
        /// <summary>logs a critical error if supplied values are not equal, a pass otherwise</summary>
        /// <param name="expectedValue">expected value</param>
        /// <param name="actualValue">actual value</param>
        /// <param name="remark">remark associated with the log entry</param>
        /// <param name="includeValuesInOutput">if true, the expected and actual values will be appended to the remark if they differ</param>
        /// <returns>true if successful</returns>
        public bool VerifyCritical(bool expectedValue, bool actualValue, string remark, bool includeValuesInOutput = true)
        {
            if (expectedValue == actualValue)
            {
                Pass(remark);
                return true;
            }
            else
            {
                if (includeValuesInOutput)
                    remark += " --- EXPECTED: " + expectedValue.ToString() + " ACTUAL: " + actualValue.ToString();
                CriticalError(remark);
                return false;
            }
        }

        /// <summary>logs a critical error if supplied values are not equal, a pass otherwise</summary>
        /// <param name="expectedValue">expected value</param>
        /// <param name="actualValue">actual value</param>
        /// <param name="remark">remark associated with the log entry</param>
        /// <param name="includeValuesInOutput">if true, the expected and actual values will be appended to the remark if they differ</param>
        /// <returns>true if successful</returns>
        public bool VerifyCritical(float expectedValue, float actualValue, string remark, bool includeValuesInOutput = true)
        {
            if (expectedValue == actualValue)
            {
                Pass(remark);
                return true;
            }
            else
            {
                if (includeValuesInOutput)
                    remark += " --- EXPECTED: " + expectedValue.ToString() + " ACTUAL: " + actualValue.ToString();
                CriticalError(remark);
                return false;
            }
        }

        /// <summary>logs a critical error if supplied values are not equal, a pass otherwise</summary>
        /// <param name="expectedValue">expected value</param>
        /// <param name="actualValue">actual value</param>
        /// <param name="remark">remark associated with the log entry</param>
        /// <param name="includeValuesInOutput">if true, the expected and actual values will be appended to the remark if they differ</param>
        /// <returns>true if successful</returns>
        public bool VerifyCritical(int expectedValue, int actualValue, string remark, bool includeValuesInOutput = true)
        {
            if (expectedValue == actualValue)
            {
                Pass(remark);
                return true;
            }
            else
            {
                if (includeValuesInOutput)
                    remark += " --- EXPECTED: " + expectedValue.ToString() + " ACTUAL: " + actualValue.ToString();
                CriticalError(remark);
                return false;
            }
        }

        /// <summary>logs a critical error if supplied values are not equal, a pass otherwise</summary>
        /// <param name="expectedValue">expected value</param>
        /// <param name="actualValue">actual value</param>
        /// <param name="remark">remark associated with the log entry</param>
        /// <param name="includeValuesInOutput">if true, the expected and actual values will be appended to the remark if they differ</param>
        /// <returns>true if successful</returns>
        public bool VerifyCritical(string expectedValue, string actualValue, string remark, bool includeValuesInOutput = true)
        {
            if (expectedValue == actualValue)
            {
                Pass(remark + " --- VALUE: " + expectedValue);
                return true;
            }
            else
            {
                if (includeValuesInOutput && expectedValue != null && actualValue != null)
                    remark += " --- EXPECTED: " + expectedValue.ToString() + " ACTUAL: " + actualValue.ToString();
                CriticalError(remark);
                return false;
            }
        }
        #endregion
        
        #region public void Validate()
        /// <summary>validates the expected value is equal to the actual value</summary>
        /// <param name="expectedValue">expected value</param>
        /// <param name="actualValue">actual value</param>
        /// <param name="remark">remark associated with the log entry</param>
        /// <param name="logPasses">true if passes should be logged</param>
        /// <param name="isCritical">true if failures are critical</param>
        ///<returns>true if the expected value is equal to the actual value</returns>
        public bool Validate(bool expectedValue, bool actualValue, string remark, bool logPasses, bool isCritical)
        {
            if (logPasses)
                return (isCritical) ? VerifyCritical(expectedValue, actualValue, remark) : Verify(expectedValue, actualValue, remark);
            else
                return (isCritical) ? AssertCritical(expectedValue, actualValue, remark) : Assert(expectedValue, actualValue, remark);
        }

        /// <summary>validates the expected value is equal to the actual value</summary>
        /// <param name="expectedValue">expected value</param>
        /// <param name="actualValue">actual value</param>
        /// <param name="remark">remark associated with the log entry</param>
        /// <param name="logPasses">true if passes should be logged</param>
        /// <param name="isCritical">true if failures are critical</param>
        ///<returns>true if the expected value is equal to the actual value</returns>
        public bool Validate(float expectedValue, float actualValue, string remark, bool logPasses, bool isCritical)
        {
            if (logPasses)
                return (isCritical) ? VerifyCritical(expectedValue, actualValue, remark) : Verify(expectedValue, actualValue, remark);
            else
                return (isCritical) ? AssertCritical(expectedValue, actualValue, remark) : Assert(expectedValue, actualValue, remark);
        }

        /// <summary>validates the expected value is equal to the actual value</summary>
        /// <param name="expectedValue">expected value</param>
        /// <param name="actualValue">actual value</param>
        /// <param name="remark">remark associated with the log entry</param>
        /// <param name="logPasses">true if passes should be logged</param>
        /// <param name="isCritical">true if failures are critical</param>
        ///<returns>true if the expected value is equal to the actual value</returns>
        public bool Validate(int expectedValue, int actualValue, string remark, bool logPasses, bool isCritical)
        {
            if (logPasses)
                return (isCritical) ? VerifyCritical(expectedValue, actualValue, remark) : Verify(expectedValue, actualValue, remark);
            else
                return (isCritical) ? AssertCritical(expectedValue, actualValue, remark) : Assert(expectedValue, actualValue, remark);
        }
        /// <summary>validates the expected value is equal to the actual value</summary>
        /// <param name="expectedValue">expected value</param>
        /// <param name="actualValue">actual value</param>
        /// <param name="remark">remark associated with the log entry</param>
        /// <param name="logPasses">true if passes should be logged</param>
        /// <param name="isCritical">true if failures are critical</param>
        ///<returns>true if the expected value is equal to the actual value</returns>
        public bool Validate(string expectedValue, string actualValue, string remark, bool logPasses, bool isCritical)
        {
            if (logPasses)
                return (isCritical) ? VerifyCritical(expectedValue, actualValue, remark) : Verify(expectedValue, actualValue, remark);
            else
                return (isCritical) ? AssertCritical(expectedValue, actualValue, remark) : Assert(expectedValue, actualValue, remark);
        }
        #endregion

        /// <summary>adds an attachment to the log</summary>
        /// <param name="filePath">path of the attachment</param>
        /// <param name="suffix">suffiz of the attachment</param>
        public void AddAttachment(string filePath, string suffix)
        {
            foreach (ILogListener listener in _Listeners)
                listener.AddAttachment(filePath, suffix);
        }

        /// <summary>adds a listener to the log engine</summary>
        /// <param name="listener">listener to add</param>
        public void AddListener(ILogListener listener)
        {
            _Listeners.Add(listener);
        }

        /// <summary>removes a listener from the log engine</summary>
        /// <param name="listener">listener to remove</param>
        public void RemoveListnener(ILogListener listener)
        {
            _Listeners.Remove(listener);
        }

        /// <summary>closes all log listeners</summary>
        public void CloseAllListeners()
        {
            foreach (ILogListener listener in _Listeners)
                listener.Close();
        }
	
        /// <summary>represents an entry in the log</summary>
        public class LogEntry
        {
            /// <summary>type of entry</summary>
            internal LogEntryType EntryType;
            /// <summary>time of entry</summary>
            private DateTime _EntryTime;
            /// <summary>remark associated with the log entry</summary>
            private string _EntryRemark;

            /// <summary>constrcutor</summary>
            /// <param name="type">type of the entry</param>
            /// <param name="remark">remark associated with the log entry</param>
            public LogEntry(LogEntryType type, string remark)
            {
                _EntryTime = DateTime.Now;
                EntryType = type;
                _EntryRemark = remark;
            }

            /// <summary>string representation of the log entry</summary>
            /// <returns></returns>
            public override string ToString()
            {
                string enumName = Enum.GetName(typeof(LogEntryType), EntryType);
                StringBuilder sb = new StringBuilder();
                sb.Append(_EntryTime.ToLongTimeString());
                sb.Append(" - ");
                sb.Append(enumName);
                // Add trailing whitepsace (if necessary) to line up with 7 letter log types
                for(int i = enumName.Length; i < 7; i++)
                    sb.Append(" ");
                sb.Append(" - ");
                sb.Append(_EntryRemark);
                return sb.ToString();
            }
        }

        /// <summary>interface to be able to store log data</summary>
        public interface ILogListener
        {
            /// <summary>logs an entry</summary>
            /// <param name="entry">entry to log</param>
            void AddEntry(LogEntry entry);
            
            /// <summary>logs an attachment</summary>
            /// <param name="filePath">path to the attachment</param>
            /// <param name="suffix">suffix of the attachment</param>
            void AddAttachment(string filePath, string suffix);
            
            /// <summary>closes the listener</summary>
            void Close();
        }

        /// <summary>logs info to the console</summary>
        public class ConsoleLogListener : ILogListener
        {
            /// <summary>true if the listener is listening</summary>
            private bool _Listening;

            /// <summary>constructor</summary>
            public ConsoleLogListener()
            {
                _Listening = true;
            }

            /// <summary>logs the entry to the console</summary>
            /// <param name="entry">entry to log</param>
            public void AddEntry(LogEntry entry)
            {
                if (!_Listening)
                    return;

                // note original console colors
                ConsoleColor initialForegroundColor = Console.ForegroundColor;
                ConsoleColor initialBackgroundColor = Console.BackgroundColor;

                // set console colors based on the type of information that is being written
                switch (entry.EntryType)
                {
                    case LogEntryType.SUCCESS:
                        Console.ForegroundColor = ConsoleColor.Green;
                        break;
                    case LogEntryType.FAILURE:
                        Console.ForegroundColor = ConsoleColor.Red;
                        break;
                    case LogEntryType.COMMENT:
                        Console.ForegroundColor = ConsoleColor.Cyan;
                        break;
                    case LogEntryType.ERROR:
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        break;
                    case LogEntryType.C_ERROR:
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.BackgroundColor = ConsoleColor.DarkRed;
                        break;
                    case LogEntryType.WARNING:
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        break;
                    case LogEntryType.VERBOSE:
                        Console.ForegroundColor = ConsoleColor.Gray;
                        break;
                    case LogEntryType.INFO:
                    default:
                        Console.ForegroundColor = ConsoleColor.White;
                        break;
                }

                // write the entry to the console
                Console.WriteLine(entry.ToString());

                // reset the console colors
                Console.ForegroundColor = initialForegroundColor;
                Console.BackgroundColor = initialBackgroundColor;
            }

            /// <summary>adds an attachment to the log</summary>
            /// <param name="filePath">path to the attachment</param>
            /// <param name="suffix">suffix of the attachment</param>
            public void AddAttachment(string filePath, string suffix) { return; }

            /// <summary>closes the listener</summary>
            public void Close() { _Listening = false; ; }
        }

        /// <summary>logs statistics</summary>
        public class StatisticsLogListener : ILogListener
        {
            /// <summary>true if the listener is listening</summary>
            private bool _Listening;
            /// <summary>true if the log has not logged any failures or errors</summary>
            public bool Success { get { return _NumErrors + _NumFailures == 0; } }
            /// <summary>number of passes logged</summary>
            public int Passes { get { return _NumPasses; } }
            private int _NumPasses;
            /// <summary>number of failures logged</summary>
            public int Failures { get { return _NumFailures; } }
            private int _NumFailures;
            /// <summary>number of warnings logged</summary>
            public int Warnings { get { return _NumWarnings; } }
            private int _NumWarnings;
            /// <summary>number of errors logged</summary>
            public int Errors { get { return _NumErrors; } }
            private int _NumErrors;

            /// <summary>constructor</summary>
            public StatisticsLogListener()
            {
                _NumPasses = 0;
                _NumFailures = 0;
                _NumErrors = 0;
                _NumWarnings = 0;
                _Listening = true;
            }

            /// <summary>logs an entry</summary>
            /// <param name="entry">entry to log</param>
            public void AddEntry(LogEntry entry)
            {
                if (!_Listening)
                    return;

                switch (entry.EntryType)
                {
                    case LogEntryType.SUCCESS: _NumPasses++; break;
                    case LogEntryType.FAILURE: _NumFailures++; break;
                    case LogEntryType.WARNING: _NumWarnings++; break;
                    case LogEntryType.ERROR:
                    case LogEntryType.C_ERROR: _NumErrors++; break;
                }
            }

            /// <summary>adds an attachment to the log</summary>
            /// <param name="filePath">path to the attachment</param>
            /// <param name="suffix">suffix of the attachment</param>
            public void AddAttachment(string content, string suffix) { return; }

            /// <summary>closes the listener</summary>
            public void Close() { _Listening = false; }
        }

        public class FileSystemListener : ILogListener
        {
            /// <summary>true if the listener is listening</summary>
            private bool _Listening;
            /// <summary>directory being logged to</summary>
            private string _Directory;
            /// <summary>path to the text version of the log</summary>
            public string TextLogFilePath { get { return _TextLogFilePath;}}
            private string _TextLogFilePath;
            /// <summary>stream writer for the text log</summary>
            private StreamWriter TextLogWriter;
            /// <summary>path to the html version of the log</summary>
            public string HTMLLogFilePath { get { return _HTMLLogFilePath; } }
            private string _HTMLLogFilePath;
            /// <summary>stream writer for the html log</summary>
            private StreamWriter HTMLLogWriter;
            /// <summary>true if an html log is being written</summary>
            public bool IsLoggingAsHTML { get { return HTMLLogWriter != null; } }
            /// <summary>true if an text log is being written</summary>
            public bool IsLoggingAsText { get { return TextLogWriter != null; } }
            /// <summary>true if attachments are being logged</summary>
            public bool IsLoggingAttachments { get; set; }
            /// <summary>list of attachment paths associated with this log</summary>
            public List<string> AttachmentPaths;

            /// <summary>true if the log has not logged any failures or errors</summary>
            public bool Success { get { return _NumErrors + _NumFailures == 0; } }
            /// <summary>number of passes logged</summary>
            public int Passes { get { return _NumPasses; } }
            private int _NumPasses = 0;
            /// <summary>number of failures logged</summary>
            public int Failures { get { return _NumFailures; } }
            private int _NumFailures = 0;
            /// <summary>number of warnings logged</summary>
            public int Warnings { get { return _NumWarnings; } }
            private int _NumWarnings = 0;
            /// <summary>number of errors logged</summary>
            public int Errors { get { return _NumErrors; } }
            private int _NumErrors = 0;

            /// <summary>constructor</summary>
            /// <param name="logName">name of the log</param>
            /// <param name="directory">directory where the log contents are being written</param>
            /// <param name="createTextLog">true if a text log should be created</param>
            /// <param name="createHTMLLog">true if an html log should be created</param>
            /// <param name="saveAttachments">true if attachments should be saved</param>
            public FileSystemListener(string logName, string directory=null, bool createTextLog=true, bool createHTMLLog=false, bool saveAttachments=false)
            {
                _Listening = true;

                try
                {
                    if (null == directory)
                    {
                        _Directory = Path.GetTempPath();
                        _TextLogFilePath = createTextLog ? Path.GetTempFileName() : null;
                        _HTMLLogFilePath = createHTMLLog ? Path.GetTempFileName() : null;
                    }
                    else
                    {
                        _Directory = directory;
                        _TextLogFilePath = createTextLog ? Path.Combine(_Directory, "automation_log.txt") : null;
                        _HTMLLogFilePath = createHTMLLog ? Path.Combine(_Directory, "automation_log.html") : null;
                    }

                    if (!Directory.Exists(_Directory))
                        Directory.CreateDirectory(_Directory);

                    if (createTextLog)
                    {
                        TextLogWriter = new StreamWriter(TextLogFilePath);
                    }
                    if (createHTMLLog)
                    {
                        HTMLLogWriter = new StreamWriter(HTMLLogFilePath);

                        StringBuilder sb = new StringBuilder();
                        sb.Append("<html>\n<head>\n<title>");
                        sb.Append(logName);
                        sb.Append("</title>\n</head>\n<body>\n<span style=\"font:Courier; font-family:Courier\">\n<h1>");
                        sb.Append(logName);
                        sb.Append("</h1>\n<p>\n");
                        HTMLLogWriter.Write(sb.ToString());
                        HTMLLogWriter.Flush();
                    }

                    IsLoggingAttachments = saveAttachments;
                    AttachmentPaths = new List<string>();
                }
                catch { Close(); }
            }

            /// <summary>adds an entry to the log</summary>
            /// <param name="entry">entry to add</param>
            public void AddEntry(LogEntry entry)
            {
                if (!_Listening)
                    return;
                try
                {
                    if (IsLoggingAsText)
                    {
                        TextLogWriter.WriteLine(entry.ToString());
                        TextLogWriter.Flush();
                    }

                    if (IsLoggingAsHTML)
                    {
                        string color;
                        switch (entry.EntryType)
                        {
                            case LogEntryType.VERBOSE: color = "gray"; break;
                            case LogEntryType.SUCCESS: color = "green"; _NumPasses++; break;
                            case LogEntryType.FAILURE: color = "red"; _NumFailures++; break;
                            case LogEntryType.ERROR:
                            case LogEntryType.C_ERROR: color = "red"; _NumErrors++; break;
                            case LogEntryType.WARNING: color = "orange"; _NumWarnings++; break;
                            case LogEntryType.COMMENT: color = "blue"; break;
                            default: color = "black"; break;
                        }

                        // write the entry's remark
                        StringBuilder sb = new StringBuilder();
                        sb.Append("<span style=\"color:");
                        sb.Append(color);
                        sb.Append("; white-space:pre\">");
                        sb.Append(entry.ToString());
                        sb.Append("</span><br />\n");
                        HTMLLogWriter.Write(sb.ToString());
                        HTMLLogWriter.Flush();
                    }
                }
                catch { Close(); }
            }

            /// <summary>adds an attachment to the log</summary>
            /// <param name="filePath">path to the attachment</param>
            /// <param name="suffix">suffix of the attachment</param>
            public void AddAttachment(string filePath, string suffix)
            {
                if (!_Listening)
                    return;
                try
                {
                    string attachmentPath = Path.Combine(_Directory, (AttachmentPaths.Count + 1).ToString() + suffix);
                    AttachmentPaths.Add(attachmentPath);
                    File.Copy(filePath, attachmentPath);
                }
                catch { Close(); }
            }

            /// <summary>closes the listener</summary>
            public void Close()
            {
                try
                {
                    if (IsLoggingAsText)
                    {
                        TextLogWriter.Flush();
                        TextLogWriter.Close();
                        TextLogWriter = null;
                    }

                    if (IsLoggingAsHTML)
                    {
                        // write a statistical summary
                        StringBuilder sb = new StringBuilder();
                        sb.Append("<br />Passes: " + _NumPasses.ToString() + "<br />\n");
                        sb.Append("Failures: " + _NumFailures.ToString() + "<br />\n");
                        sb.Append("Warnings: " + _NumWarnings.ToString() + "<br />\n");
                        sb.Append("Errors: " + _NumErrors.ToString() + "<br />\n");
                        sb.Append("</p>\n<p>\n");
                        sb.Append("</p>\n</span>\n</body>\n</html>");
                        HTMLLogWriter.Write(sb.ToString());
                        HTMLLogWriter.Flush();
                        HTMLLogWriter.Close();
                        HTMLLogWriter = null;
                    }
                }
                catch { }
                _Listening = false;
            }
        }
	}
}