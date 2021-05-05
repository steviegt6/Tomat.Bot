using System.Diagnostics;

namespace TomatBot.Utilities
{
    public static class ShellHelper
    {
        public static string Bash(this string cmd)
        {
            string escapedArgs = cmd.Replace("\"", "\\\"");
            
            Process process = new()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardInput = false
                }
            };

            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            return result;
        }
    }
}