using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pdi.Assets
{
    public enum HOST_SHELL_TYPE
    {
        SH,
        POWERSHELL,
        CMD,
    }

    public class HostFactory
    {
        public async Task<HOST_SHELL_TYPE> ShellDetection(SshHost Host)
        {
            (var ExitStatus, var Result, _) = await Host.Execute("echo $?");
            
            if (ExitStatus != 0)
            {
                throw new PlatformNotSupportedException("Unknown Shell.");
            }

            return Result switch
            {
                "0\n"       => HOST_SHELL_TYPE.SH,
                "True\n"    => HOST_SHELL_TYPE.POWERSHELL,
                "$?\n\n"    => HOST_SHELL_TYPE.CMD,
                _           => throw new PlatformNotSupportedException("Unknown Shell.")
            };
        }
    }
}
