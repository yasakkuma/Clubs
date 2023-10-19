﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace RestTestTool
{
    public class RequestModel
    {
        public string json = string.Empty;


        [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileStringW", CharSet = CharSet.Unicode, SetLastError = true)]
        static extern uint GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, uint nSize, string lpFileName);

        public event EventHandler TaskEndEvent;
        /// <summary>
        /// PROXY設定がある場合は設定
        /// </summary>
        public HttpClientHandler SetProxy(HttpClientHandler handler)
        {
            FileInfo proxyini = new FileInfo(Consts.PROXY_FILE);
            if (!proxyini.Exists) return handler;
            
            string user = GetIniValue(proxyini, Consts.PROXY_SECTION_NAME, Consts.USER_KEY_NAME, string.Empty);
            string password = GetIniValue(proxyini, Consts.PROXY_SECTION_NAME, Consts.PASSWORD_KEY_NAME, string.Empty);
            string proxyUrl = GetIniValue(proxyini, Consts.PROXY_SECTION_NAME, Consts.PROXY_URL_KEY_NAME, string.Empty);

            if (!string.IsNullOrEmpty(user) && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(proxyUrl))
            {
                handler.UseProxy = true;
                handler.Proxy = new WebProxy(proxyUrl);
                handler.Proxy.Credentials = new NetworkCredential(user, password);
            }
            return handler;
        }

        /// <summary>
        /// INIファイルから指定したキーを取得
        /// </summary>
        /// <param name="file"></param>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        private string GetIniValue(FileInfo file , string section, string key, string defaultValue)
        {
            StringBuilder sb = new StringBuilder(256);
            GetPrivateProfileString(section, key, defaultValue, sb, 256, file.FullName);
            return sb.ToString();
        }

        /// <summary>
        /// GETリクエスト実行
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="paramsDic"></param>
        /// <returns></returns>
        public async Task<bool> DoGet(string uri, Dictionary<string, string> paramsDic)
        {
            try
            {
                json = string.Empty;
                HttpClientHandler handler = new HttpClientHandler() { UseProxy = false };
                handler = SetProxy(handler);

                using (var client = new HttpClient(handler))
                {
                    var response = await client.GetAsync(uri);
                    json = await response.Content.ReadAsStringAsync();
                }

                return true;
            } catch (Exception e)
            {
                json = e.Message + e.StackTrace;
                while (e.InnerException != null)
                {
                    json += "\n" + e.Message + e.StackTrace;
                }
                return false;
            }
        }

        /// <summary>
        /// POSTリクエスト実行
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="paramsDic"></param>
        /// <returns></returns>
        public async Task<bool> DoPost(string uri, Dictionary<string, string> paramsDic)
        {
            try
            {
                json = string.Empty;
                HttpClientHandler handler = new HttpClientHandler() { UseProxy = false };
                handler = SetProxy(handler);

                using (var client = new HttpClient(handler))
                {
                    var content = new FormUrlEncodedContent(paramsDic);
                    var response = await client.PostAsync(uri, content);
                    json = await response.Content.ReadAsStringAsync();
                }
                return true;
            }
            catch (Exception e)
            {
                json = e.Message + e.StackTrace;
                while (e.InnerException != null)
                {
                    json += "\n" + e.Message + e.StackTrace;
                }
                return false;
            }
        }
    }
}
