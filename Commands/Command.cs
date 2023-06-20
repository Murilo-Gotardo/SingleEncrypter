using SingleEncrypter.Helper;
using System.Reflection;

namespace SingleEncrypter.Commands
{
    internal abstract class Command
    {
        public abstract string? CommandName { get; set; }

        public abstract string? Option { get; set; }

        public abstract void ExecuteCommand(string[] args);

        public abstract Task ExecuteCommandAsync(string[] args);

        public abstract bool VerifyCommand(string[] args);
    }
}
