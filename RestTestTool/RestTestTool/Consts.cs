using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestTestTool
{
    enum MethodType
    {
        Get = 0,
        Post = 1,
        Put = 2,
        Delete = 3,
        Patch = 4
    }

    public static class Consts
    {
        public const string PROXY_SECTION_NAME = "PROXY";
        public const string USER_KEY_NAME = "USER";
        public const string PASSWORD_KEY_NAME = "PASSWORD";
        public const string PROXY_URL_KEY_NAME = "PROXY_URL";
        public const string PROXY_FILE = @".\proxy.ini";
    }
}
