using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RestTestTool
{
    public class RequestModel
    {
        public string json = string.Empty;

        /// <summary>
        /// GETリクエスト実行
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="paramsDic"></param>
        /// <returns></returns>
        public bool DoGet(string uri, Dictionary<string, string> paramsDic)
        {
            try
            {
                json = string.Empty;

                var task = Task.Run(() =>
                {
                    using (var client = new HttpClient())
                    {
                        var response = client.GetAsync(uri).Result;
                        json = response.Content.ReadAsStringAsync().Result;
                    }
                });
                task.Wait();
                return true;
            } catch (Exception e)
            {
                json = e.Message + e.StackTrace;
                return false;
            }
        }

        /// <summary>
        /// POSTリクエスト実行
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="paramsDic"></param>
        /// <returns></returns>
        public bool DoPost(string uri, Dictionary<string, string> paramsDic)
        {
            try
            {
                json = string.Empty;

                var task = Task.Run(() =>
                {
                    using (var client = new HttpClient())
                    {
                        var content = new FormUrlEncodedContent(paramsDic);
                        var response = client.PostAsync(uri, content).Result;
                        json = response.Content.ReadAsStringAsync().Result;
                    }
                });
                task.Wait();
                return true;
            }
            catch (Exception e)
            {
                json = e.Message + e.StackTrace;
                return false;
            }
        }
    }
}
