using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace FL.Common.Base.Extensions
{
    public static class ObjectExtensions
    {
        public static string GetCallerMemberName(this object instance, [CallerMemberName] string memberName = "")
        {
            return memberName;
        }

        public static string GetCallerMemberName([CallerMemberName] string memberName = "")
        {
            return memberName;
        }
    }
}
