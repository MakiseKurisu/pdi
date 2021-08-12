using HCGStudio.DistributionChecker;

using System;
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
        public async Task<HOST_SHELL_TYPE> DetectShell(IHost Host)
        {
            (var ExitStatus, var Result, _) = await Host.Execute("echo $?");

            if (ExitStatus != 0)
            {
                throw new PlatformNotSupportedException("Unknown Shell.");
            }

            return Result switch
            {
                "0\n" => HOST_SHELL_TYPE.SH,
                "True\n" => HOST_SHELL_TYPE.POWERSHELL,
                "$?\n\n" => HOST_SHELL_TYPE.CMD,
                _ => throw new PlatformNotSupportedException("Unknown Shell.")
            };
        }

        public async Task<LinuxDistribution> DetectLinuxDistribution(IHost Host)
        {
            (var ExitStatus, var Result, _) = await Host.Execute("cat /etc/os-release");

            if (ExitStatus != 0)
            {
                throw new PlatformNotSupportedException("Unknown Linux Distribution.");
            }

            return new DistributionChecker(Result).GetDistribution();
        }

        public async Task<bool> CheckPlatformSupport(IHost Host)
        {
            switch (await DetectShell(Host))
            {
                case HOST_SHELL_TYPE.SH:
                    {
                        var distro = await DetectLinuxDistribution(Host);
                        return distro.IsOrLikeDebian();
                    }
                default:
                    {
                        throw new PlatformNotSupportedException("Unknown Shell.");
                    }
            }
        }

        public async Task<IHost> CreateHost(HostRecord Record)
        {
            var host = new SshHost(new SshHostRecord(Record));
            return await CheckPlatformSupport(host) ? host : throw new PlatformNotSupportedException("Unknown Shell.");
        }
    }
}
