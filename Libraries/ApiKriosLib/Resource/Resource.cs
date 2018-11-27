using ConsoleAdminAPI;
using Newtonsoft.Json;
using ODataClientLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiKriosLib.Resource
{
    public class Resource : API_Resource
    {
        public static int RType_ID { get; set; }
        private String ApiPath { get; set; }

        public Resource(string OBJTYPE, String apiPath) : base(OBJTYPE)
        {
            ApiPath = apiPath;
        }

        public Resource(String OBJTYPE, String apiPath, long poolId) : this(OBJTYPE, apiPath)
        {
            R_IDPool = poolId;
        }

        #region CRUD

        /// <summary>
        /// Send a request to create a resource with the api
        /// </summary>
        /// <param name="api">Api object to use</param>
        /// <returns>a request ID</returns>
        public virtual long Create(API_Proxy_BackendConsole api)
        {
            return api.Post<long>(ApiPath, JsonConvert.SerializeObject(this));
        }

        /// <summary>
        /// Send a request to update a resource with the api, identified by the R_IDItem field.
        /// </summary>
        /// <param name="api">Api object to use</param>
        /// <returns>a request ID</returns>
        public virtual long Update(API_Proxy_BackendConsole api)
        {
            return api.Put<long>(ApiPath, R_IDItem, JsonConvert.SerializeObject(this));
        }

        /// <summary>
        /// Send a request to delete a resource with the api, identified by the R_IDItem field.
        /// </summary>
        /// <param name="api">Api object to use</param>
        /// <returns>a request ID</returns>
        public virtual long Delete(API_Proxy_BackendConsole api)
        {
            return api.Delete<long>(ApiPath, R_IDItem);
        }

        #endregion

        #region Tools

        //TODO Implement GetResourceType in API
        public static int GetResourceTypeId(String objType)
        {
            return 0;
        }

        #endregion
    }
}
