using AdminConsole_Type.General;
using Newtonsoft.Json;
using ODataClientLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiKriosLib.General
{
    public class TenantKrios : TenantKriosType
    {
        #region Fields

        private static String ApiPath = "API_TenantKrios";

        /// <summary>
        /// Tenant Id.
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// The company name. Mandatory if Firstname or Lastname are null.
        /// </summary>
        public string Company { get; set; }
        /// <summary>
        /// Mandatory if Company is null.
        /// </summary>
        public string Firstname { get; set; }
        /// <summary>
        /// Mandatory if Company is null.
        /// </summary>
        public string Lastname { get; set; }

        #endregion

        #region CRUD

        /// <summary>
        /// Create a new Tenant Krios with info stored in this object.
        /// Will update Id field.
        /// </summary>
        /// <param name="api">Api object to use</param>
        public void Create(API_Proxy_BackendConsole api)
        {
            Id = api.Post<long>(ApiPath, JsonConvert.SerializeObject(this));
        }

        #endregion

    }
}
