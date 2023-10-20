using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RestTestTool
{
    public class RequestModel
    {
        /// <summary>
        /// JSON戻り値
        /// </summary>
        public string json = string.Empty;

        /// <summary>
        /// PROXY設定ファイル
        /// </summary>
        public ProxyIni proxyIni;

        /// <summary>
        /// PROXY設定がある場合は設定
        /// </summary>
        public HttpClientHandler SetProxy(HttpClientHandler handler)
        {
            FileInfo proxyini = new FileInfo(Consts.PROXY_FILE);
            if (!proxyini.Exists) return handler;

            string user = proxyIni.ProxyUser;
            string password = proxyIni.ProxyPassword;
            string proxyUrl = proxyIni.ProxyUrl;

            if (!string.IsNullOrEmpty(user) && !string.IsNullOrEmpty(password) && !string.IsNullOrEmpty(proxyUrl))
            {
                handler.UseProxy = true;
                handler.Proxy = new WebProxy(proxyUrl);
                handler.Proxy.Credentials = new NetworkCredential(user, password);
            }
            return handler;
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
