using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ODataClientLib
{
    public class API_Proxy_BackendConsole : Proxy_BackEndConsole
    {
        #region Fields
        private String ApiKey { get; set; }
        #endregion

        #region Constructors

        public API_Proxy_BackendConsole(String baseUrl, String apiKey) : base(baseUrl)
        {
            ApiKey = apiKey;
        }

        #endregion

        #region EndMethods

        #region Getters

        public override List<T> List<T>(String path)
        {
            NameValueCollection queryString = new NameValueCollection();
            return List<T>(path, queryString);
        }

        public override T Get<T>(String path)
        {
            NameValueCollection queryString = new NameValueCollection();
            return base.Get<T>(path, queryString);
        }

        public override List<T> List<T>(String path, NameValueCollection queryString)
        {
            queryString.Add("apiKey", ApiKey);
            return base.List<T>(path, queryString);
        }

        public override T Get<T>(String path, NameValueCollection queryString)
        {
            queryString.Add("apiKey", ApiKey);
            return base.Get<T>(path, queryString);
        }

        #endregion

        #region CRUD

        public override T Put<T>(String path, long key, String jsonPayload, NameValueCollection queryString)
        {
            queryString.Add("apiKey", ApiKey);
            return base.Put<T>(path, key, jsonPayload, queryString);
        }

        public override T Post<T>(String path, String jsonPayload, NameValueCollection queryString)
        {
            queryString.Add("apiKey", ApiKey);
            return base.Post<T>(path, jsonPayload, queryString);
        }

        public override T Delete<T>(String path, long key, NameValueCollection queryString)
        {
            queryString.Add("apiKey", ApiKey);
            return base.Delete<T>(path, key, queryString);
        }


        public override T Put<T>(String path, long key, String jsonPayload)
        {
            NameValueCollection queryString = new NameValueCollection();
            return Put<T>(path, key, jsonPayload, queryString);
        }

        public override T Post<T>(String path, String jsonPayload)
        {
            NameValueCollection queryString = new NameValueCollection();
            return Post<T>(path, jsonPayload, queryString);
        }

        public override T Delete<T>(String path, long key)
        {
            NameValueCollection queryString = new NameValueCollection();
            return Delete<T>(path, key, queryString);
        }


        #endregion

        #region Actions

        public override T Action<T>(String path, String actionName, String httpMethod)
        {
            NameValueCollection queryString = new NameValueCollection();
            return Action<T>(path, actionName, httpMethod, queryString);
        }

        public override T Action<T>(String path, String actionName, String httpMethod, NameValueCollection queryString)
        {
            queryString.Add("apiKey", ApiKey);
            return base.Action<T>(path, actionName, httpMethod, queryString);
        }

        public override T Action<T>(String path, String actionName, long key, String httpMethod)
        {
            return Action<T>(ForgeUrl(path, key), actionName, httpMethod);
        }

        public override T Action<T>(String path, String actionName, long key, String httpMethod, NameValueCollection queryString)
        {
            return Action<T>(ForgeUrl(path, key), actionName, httpMethod, queryString);
        }

        #endregion

        #endregion


    }
}
