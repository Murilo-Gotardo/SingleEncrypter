using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.AccessControl;

namespace SingleEncrypter.Helper
{
    internal class FileHelper
    {
        public static void RestrictPermisions(string file)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                FileSecurity _fileSecurity = new();

                _fileSecurity.SetAccessRuleProtection(true, false);

                _fileSecurity.AddAccessRule(new FileSystemAccessRule("Todos",
                    FileSystemRights.FullControl, AccessControlType.Deny));

                FileInfo _fileInfo = new(file);
                _fileInfo.SetAccessControl(_fileSecurity);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ||
                 RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                ProcessStartInfo _processInfo = new("chmod", "go-rwx " + file)
                {
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                };

                Process? _process = Process.Start(_processInfo);
                _process?.WaitForExit();

                if (_process?.ExitCode != 0)
                {
                    Console.WriteLine($"""
                        SingleEncrypter could not restrict the permisions of the file: {file}
                        """);
                }
            }
        }

        public static void RestorePermissions(string file)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                FileSecurity _fileSecurity = new();
                _fileSecurity.SetAccessRuleProtection(false, false);
                FileInfo _fileInfo = new(file);
                _fileInfo.SetAccessControl(_fileSecurity);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ||
                     RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                ProcessStartInfo _processInfo = new("chmod", "original_permissions " + file)
                {
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                };

                Process? _process = Process.Start(_processInfo);
                _process?.WaitForExit();

                if (_process?.ExitCode != 0)
                {
                    Console.WriteLine($"SingleEncrypter could not restore the permissions of the file: {file}");
                }
            }
        }
    }
}
