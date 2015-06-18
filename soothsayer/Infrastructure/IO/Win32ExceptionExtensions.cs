using System.ComponentModel;

namespace soothsayer.Infrastructure.IO
{
    public static class Win32ExceptionExtensions
    {
        public static bool IsFor(this Win32Exception exception, Win32ErrorCode code)
        {
            return exception.NativeErrorCode == (int)code;
        }
    }
}