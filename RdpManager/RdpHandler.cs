using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace RdpManager
{
    public class RdpHandler
    {
        public static string Process(LogInfo info)
        {
            if (string.IsNullOrEmpty(info.Username) || string.IsNullOrEmpty(info.Password))
            {
                throw new ArgumentNullException($"username and password can't be empty");
            }

            var pwstr = BitConverter.ToString(DataProtection.ProtectData(Encoding.Unicode.GetBytes(info.Password), ""))
                .Replace("-", "");
            var str = string.Format(File.ReadAllText(Main.DataPath + "\\TemplateRDP.txt"), info.IpAddress, info.Username, pwstr, info.Domain,info.MultiMonitor);
            return str;

        }

        public static void Mstsc(string cmd)
        {
            var p = new Process
            {
                StartInfo =
                {
                    FileName = "cmd.exe",
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    CreateNoWindow = true
                }
            };
            p.Start();
            p.StandardInput.WriteLine(cmd);
        }
    }

    public class LogInfo
    {
        public string IpAddress { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Domain { get; set; }
        public int MultiMonitor { get; set; }
    }
}
