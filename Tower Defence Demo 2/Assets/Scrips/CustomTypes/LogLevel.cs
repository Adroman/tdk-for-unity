using JetBrains.Annotations;

namespace Scrips.CustomTypes
{
    [PublicAPI]
    public enum LogLevel
    {
        Debug = 0,
        Warn = 100,
        Error = 200
    }
}