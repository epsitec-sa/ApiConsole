using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;

namespace ODataClientLib
{
    public class Proxy_BackEndConsole
    {
        #region Fields

        private static Mutex mut = new Mutex();
        private String BaseUrl { get; set; }

        public static String BaseUrl_Dev = @"http://10.0.25.61/BackEnd_Console/odata/";
        public static String BaseUrl_Prod = @"https://console.komodo.ch/krios/odata/";
        public static String BaseUrl_SandBox = @"https://consolesandbox.komodo.ch/krios/odata/";

        #endregion

        #region Constructors

        public Proxy_BackEndConsole(String baseUrl)
        {
            BaseUrl = baseUrl;
        }

        #endregion

        #region ODataResponse Classes

        public class ODataResponse_list<T>
        {
            [JsonProperty("odata.metadata")]
            public String Metadata { get; set; }
            public List<T> Value { get; set; }
        }

        public class ODataResponse_action<T>
        {
            [JsonProperty("@odata.context")]
            public String Metadata { get; set; }
            public T value { get; set; }
        }

        public class ODataResponse_post<T>
        {
            [JsonProperty("@odata.context")]
            public String Metadata { get; set; }
            public T value { get; set; }
        }

        public class ODataResponse_put<T>
        {
            [JsonProperty("@odata.context")]
            public String Metadata { get; set; }
            public T value { get; set; }
        }

        public class ODataResponse_delete<T>
        {
            [JsonProperty("@odata.context")]
            public String Metadata { get; set; }
            public T value { get; set; }
        }

        #endregion

        #region EndMethods

        #region Getters

        public virtual List<T> List<T>(String path)
        {
            String payload_raw = DoHttpRequest(path, "GET");
            ODataResponse_list<T> payload = JsonConvert.DeserializeObject<ODataResponse_list<T>>(payload_raw);

            return payload.Value;
        }

        public virtual T Get<T>(String path)
        {
            String payload_raw = DoHttpRequest(path, "GET");
            return JsonConvert.DeserializeObject<T>(payload_raw);
        }

        public virtual List<T> List<T>(String path, NameValueCollection queryString)
        {
            String payload_raw = DoHttpRequest(ForgeUrl(path, queryString), "GET");
            ODataResponse_list<T> payload = JsonConvert.DeserializeObject<ODataResponse_list<T>>(payload_raw);

            return payload.Value;
        }

        public virtual T Get<T>(String path, NameValueCollection queryString)
        {
            String payload_raw = DoHttpRequest(ForgeUrl(path, queryString), "GET");
            return JsonConvert.DeserializeObject<T>(payload_raw);
        }

        #endregion

        #region CRUD

        public virtual T Put<T>(String path, long key, String jsonPayload, NameValueCollection queryString)
        {
            String resp_raw = DoHttpRequest(ForgeUrl(path, key, queryString), jsonPayload, "PUT");

            if (String.IsNullOrEmpty(resp_raw))
                return default(T);

            return JsonConvert.DeserializeObject<ODataResponse_post<T>>(resp_raw).value;
        }

        public virtual T Post<T>(String path, String jsonPayload, NameValueCollection queryString)
        {
            String resp_raw = DoHttpRequest(ForgeUrl(path, queryString), jsonPayload, "POST");

            if (String.IsNullOrEmpty(resp_raw))
                return default(T);

            return JsonConvert.DeserializeObject<ODataResponse_put<T>>(resp_raw).value;
        }

        public virtual T Delete<T>(String path, long key, NameValueCollection queryString)
        {
            String resp_raw = DoHttpRequest(ForgeUrl(path, key, queryString), "DELETE");

            if (String.IsNullOrEmpty(resp_raw))
                return default(T);

            return JsonConvert.DeserializeObject<ODataResponse_delete<T>>(resp_raw).value;
        }


        public virtual T Put<T>(String path, long key, String jsonPayload)
        {
            String resp_raw = DoHttpRequest(ForgeUrl(path, key), jsonPayload, "PUT");

            if (String.IsNullOrEmpty(resp_raw))
                return default(T);

            return JsonConvert.DeserializeObject<ODataResponse_post<T>>(resp_raw).value;
        }

        public virtual T Post<T>(String path, String jsonPayload)
        {
            String resp_raw = DoHttpRequest(path, jsonPayload, "POST");

            if (String.IsNullOrEmpty(resp_raw))
                return default(T);

            return JsonConvert.DeserializeObject<ODataResponse_put<T>>(resp_raw).value;
        }

        public virtual T Delete<T>(String path, long key)
        {
            String resp_raw = DoHttpRequest(ForgeUrl(path, key), "DELETE");

            if (String.IsNullOrEmpty(resp_raw))
                return default(T);

            return JsonConvert.DeserializeObject<ODataResponse_delete<T>>(resp_raw).value;
        }


        #endregion

        #region Actions

        public virtual T Action<T>(String path, String actionName, String httpMethod)
        {
            String p = path + "/" + actionName;

            String resp_raw = DoHttpRequest(p, httpMethod);

            if (String.IsNullOrEmpty(resp_raw))
                return default(T);

            return JsonConvert.DeserializeObject<ODataResponse_action<T>>(resp_raw).value;
        }

        public virtual T Action<T>(String path, String actionName, String httpMethod, NameValueCollection queryString)
        {
            String p = path + "/" + actionName;

            String resp_raw = DoHttpRequest(ForgeUrl(p, queryString), httpMethod);

            if (String.IsNullOrEmpty(resp_raw))
                return default(T);

            return JsonConvert.DeserializeObject<ODataResponse_action<T>>(resp_raw).value;
        }

        public virtual T Action<T>(String path, String actionName, long key, String httpMethod)
        {
            String p = ForgeUrl(path, key) + "/" + actionName;

            return Action<T>(p, actionName, httpMethod);
        }

        public virtual T Action<T>(String path, String actionName, long key, String httpMethod, NameValueCollection queryString)
        {
            String p = ForgeUrl(path, key) + "/" + actionName;

            return Action<T>(p, actionName, httpMethod, queryString);
        }

        #endregion

        #endregion

        #region Tools

        #region UrlForgers

        internal String ForgeUrl(String path, long key, NameValueCollection queryString)
        {
            return ForgeUrl(ForgeUrl(path, key), queryString);
        }

        internal String ForgeUrl(String path, NameValueCollection queryString)
        {
            return String.Format("{0}/{1}", path, ToQueryString(queryString));
        }

        internal String ForgeUrl(String path, long key)
        {
            return String.Format("{0}({1})", path, key);
        }

        //From https://stackoverflow.com/questions/829080/how-to-build-a-query-string-for-a-url-in-c
        internal string ToQueryString(NameValueCollection nvc)
        {
            var array = (from key in nvc.AllKeys
                         from value in nvc.GetValues(key)
                         select string.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(value)))
                .ToArray();
            return "?" + string.Join("&", array);
        }

        #endregion

        internal String DoHttpRequest(String path, String httpMethod)
        {
            mut.WaitOne();
            HttpWebRequest req = WebRequest.Create(BaseUrl + path) as HttpWebRequest;

            req.Method = httpMethod;
            req.Accept = "application/json";

            HttpWebResponse resp = null;

            String toReturn;

            try
            {
                resp = req.GetResponse() as HttpWebResponse;
            }
            catch (WebException ex)
            {
                // 500 ERROR
                using (var stream = ex.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    mut.ReleaseMutex();
                    throw new Exception(reader.ReadToEnd());
                }
            }

            if ((int)resp.StatusCode >= 400)
            {
                // 400 ERROR
                using (var reader = new StreamReader(resp.GetResponseStream(), ASCIIEncoding.ASCII))
                {
                    mut.ReleaseMutex();
                    throw new Exception(reader.ReadToEnd());
                }
            }

            using (var reader = new StreamReader(resp.GetResponseStream(), ASCIIEncoding.ASCII))
            {
                toReturn = reader.ReadToEnd();
                mut.ReleaseMutex();
                return toReturn;
            }
        }

        internal String DoHttpRequest(String path, String jsonPayload, String httpMethod)
        {
            mut.WaitOne();
            HttpWebRequest req = WebRequest.Create(BaseUrl + path) as HttpWebRequest;

            req.Method = httpMethod;
            req.Accept = "application/json";
            req.ContentType = "application/json";

            HttpWebResponse resp = null;

            String toReturn;


            using (var streamWriter = new StreamWriter(req.GetRequestStream()))
            {
                streamWriter.Write(jsonPayload);
                streamWriter.Flush();
                streamWriter.Close();
            }

            try
            {
                resp = req.GetResponse() as HttpWebResponse;
            }
            catch (WebException ex)
            {
                // 500 ERROR
                using (var stream = ex.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    mut.ReleaseMutex();
                    throw new Exception(reader.ReadToEnd());
                }
            }

            if ((int)resp.StatusCode >= 400)
            {
                // 400 ERROR
                using (var reader = new StreamReader(resp.GetResponseStream(), ASCIIEncoding.ASCII))
                {
                    mut.ReleaseMutex();
                    throw new Exception(reader.ReadToEnd());
                }
            }

            using (var reader = new StreamReader(resp.GetResponseStream(), ASCIIEncoding.ASCII))
            {
                toReturn = reader.ReadToEnd();
                mut.ReleaseMutex();
                return toReturn;
            }
        }

        public bool Ping()
        {
            try
            {
                DoHttpRequest("/", "GET");
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

    }
}
