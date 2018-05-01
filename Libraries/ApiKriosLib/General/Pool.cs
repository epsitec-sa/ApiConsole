using AdminConsole_Type.General;
using Newtonsoft.Json;
using ODataClientLib;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace ApiKriosLib.General
{
    public class Pool : PoolType
    {

        public static class AvailableApiAction
        {
            public const String GRANT_ACCESS = "Act.GrantAccess";
            public const String GRANT_ACCESS_TO_MANAGER = "Act.GrantAccessToManager";
        }

        #region Fields

        private static String ApiPath = "API_Pool";

        /// <summary>
        /// The pool Id
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// The Pool's owner. 
        /// </summary>
        public long IdCustomer { get; set; }
        /// <summary>
        /// Is the default pool
        /// </summary>
        public bool IsDefault { get; set; }
        /// <summary>
        /// Pool name
        /// </summary>
        public string Title { get; set; }

        #endregion

        #region CRUD

        /// <summary>
        /// Create a new pool with info setted in this object.
        /// </summary>
        /// <param name="api">Api object to use.</param>
        public void Create(API_Proxy_BackendConsole api)
        {
            Id = api.Post<long>(ApiPath, JsonConvert.SerializeObject(this));
        }

        #endregion

        #region Actions

        /// <summary>
        /// Grant Access to pool's owner for a specific resourceType.
        /// Resource types can be found on Resources object in the static OBJTYPE field. Ex: UserADKomodo.OBJTYPE
        /// </summary>
        /// <param name="api">Api object to use.</param>
        /// <param name="resourceType">The specific resourceType.</param>
        public void GrantAccess(API_Proxy_BackendConsole api, String resourceType)
        {
            NameValueCollection param = new NameValueCollection();

            param.Add("resourceType", resourceType);

            api.Action<string>(ApiPath, AvailableApiAction.GRANT_ACCESS, Id, "GET", param);

        }

        /// <summary>
        /// Grant Access to reseller 
        /// </summary>
        /// <param name="api"></param>
        /// <param name="resourceType"></param>
        public void GrantAccessToManager(API_Proxy_BackendConsole api, String resourceType)
        {
            NameValueCollection param = new NameValueCollection();

            param.Add("resourceType", resourceType);

            api.Action<string>(ApiPath, AvailableApiAction.GRANT_ACCESS_TO_MANAGER, Id, "GET", param);
        }

        #endregion

    }
}
