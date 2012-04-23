using System;
using System.IO;
using System.Net;
using System.Text;
using Automation.Types;

namespace Automation
{
    public static partial class Utility
    {
        /// <summary>performs utility tasks pertaining to the web</summary>
        public static class Web
        {

            /// <summary>makes an http post request</summary>
            /// <param name="uri">uri to post to</param>
            /// <param name="postContent">content to post</param>
            /// <param name="acceptHeader">if null, the default is used, otherwise the accep hedaer will be set to this value</param>
            /// <returns>command response</returns>
            public static CommandResult MakeHttpPostRequest(string uri, string postContent, string acceptHeader = null)
            {
                try
                {
                    // generate the request
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                    request.AllowAutoRedirect = true;
                    request.Credentials = CredentialCache.DefaultCredentials;
                    request.UserAgent = "Automation System";
                    if (null != acceptHeader)
                        request.Accept = acceptHeader;
                    request.Method = "POST";
                    byte[] byteArray = Encoding.UTF8.GetBytes(postContent);
                    request.ContentLength = byteArray.Length;
                    request.ContentType = "application/x-www-form-urlencoded";

                    // post the request
                    Stream dataStream = request.GetRequestStream();
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();

                    // receive the response
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    dataStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);
                    string responseFromServer = reader.ReadToEnd();
                    reader.Close();
                    dataStream.Close();
                    response.Close();
                    return new CommandResult(0, responseFromServer);
                }
                catch (Exception e) { return new CommandResult(-1, e.ToString()); }
            }

            /// <summary>concatenates keys and values to the format required for post parameters</summary>
            /// <param name="paramArray">keys and values to post</param>
            /// <returns>valid string to be used by an http post request</returns>
            public static string GeneratePostParamString(params String[] paramArray)
            {
                StringBuilder sb = new StringBuilder();
                int paramCount = 0;
                foreach (string param in paramArray)
                {
                    if (paramCount == 0)
                    {
                        // first parameter name, do not add ? for now
                        sb.Append(param);
                    }
                    else if (paramCount % 2 == 1)
                    {
                        // write parameter value as escaped string
                        if (param.Length > 0)
                        {
                            sb.Append("=");
                            // add the data in chunks to avoid the URI exceeding the 65535-ish characters
                            for (int i = 0; i < param.Length; i+=60000)
                                sb.Append(Uri.EscapeDataString(param.Substring(i, System.Math.Min(60000,param.Length - i) )));
                        }
                    }
                    else
                    {
                        // write parameter name
                        sb.Append("&");
                        sb.Append(param);
                    }

                    paramCount++;
                }

                return sb.ToString();

            }

            /// <summary>gets the public ipv4 address of the machine</summary>
            /// <returns>the public ipv4 address of the machine</returns>
            public static IPAddress GetPublicIPv4Address()
            {
                IPAddress _ip = IPAddress.Parse("127.0.0.1");
                if (SystemOperations.IsRunningOnMacOSX())
                {
                    // loop through all 802.x devices
                    for (int i = 0; i < 3; i++)
                    {
                        // get the ip of the device
                        CommandResult result = Utility.SystemOperations.RunShellCommand("/usr/sbin/ipconfig", "getifaddr en" + i.ToString());
                        if (IPAddress.TryParse(result.CommandOutput.Replace("\n", ""), out _ip))
                            return _ip;
                    }
                }
                else
                {
                    IPHostEntry ips = Dns.GetHostEntry(Dns.GetHostName());
                    _ip = ips.AddressList[0];
                    foreach (IPAddress ip in ips.AddressList)
                    {
                        // only use IPv4 addresses (bug in WCF for mono it appears)
                        if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                        {
                            _ip = ip;
                            break;
                        }
                    }
                }
                return _ip;
            }
        }
    }
}
