using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;

namespace RestTestTool
{
    public class ProxyIni
    {
        [IniFileAttribute(Section = "PROXY", Key = "USER")]
        public string ProxyUser;

        [IniFileAttribute(Section = "PROXY", Key = "PASSWORD")]
        public string ProxyPassword;

        [IniFileAttribute(Section = "PROXY", Key = "PROXY_URL")]
        public string ProxyUrl;

    }
}
