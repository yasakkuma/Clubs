using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class IniFileAttribute : Attribute
    {
        public string Section { get; set; }
        public string Key { get; set; }

        public bool Exists { get; set; } = false;

        public bool Nullable { get; set; } = true;

    }

    public class IniFile
    {
        [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileStringW", CharSet = CharSet.Unicode, SetLastError = true)]
        static extern uint GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, uint nSize, string lpFileName);

        [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileInt", CharSet = CharSet.Unicode, SetLastError = true)]
        static extern uint GetPrivateProfileInt(string lpAppName, string lpKeyName, int nDefault, string lpFileName);
        public static T ReadIni<T>(FileInfo file)
        {
            if (!file.Exists) throw new FileNotFoundException(file.FullName + "がありません");
            T ret = (T)Activator.CreateInstance(typeof(T));

            foreach (var field in typeof(T).GetFields())
            {
                IniFileAttribute attr = IniFileAttribute.GetCustomAttribute(field, typeof(IniFileAttribute)) as IniFileAttribute;

                if (field.FieldType == typeof(int))
                {
                    field.SetValue(ret, (int)GetPrivateProfileInt(attr.Section, attr.Key, -1, file.FullName));
                }
                else if (field.FieldType == typeof(uint))
                {
                    field.SetValue(ret, GetPrivateProfileInt(attr.Section, attr.Key, -1, file.FullName));
                }
                else
                {
                    var sb = new StringBuilder(1024);
                    GetPrivateProfileString(attr.Section, attr.Key, string.Empty, sb, (uint)sb.Capacity, file.FullName);
                    field.SetValue(ret, sb.ToString());
                }
            }

            return ret;
        }
    }
}
