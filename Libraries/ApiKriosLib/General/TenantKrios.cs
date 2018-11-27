using ConsoleAdminAPI;
using Newtonsoft.Json;
using ODataClientLib;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace ApiKriosLib.General
{
    public class TenantKrios : API_TenantKrios
    {
        public static class AvailableApiAction
        {
            public const String SIGN_AGREEMENT = "Act.API_SignAgreement";
        }

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

        public void SignAgreement(API_Proxy_BackendConsole api, String agreementName, String version)
        {
            if (Id == 0)
                throw new Exception("You must create the client before signing agreements.");

            NameValueCollection param = new NameValueCollection();

            param.Add("agreementName", agreementName);
            param.Add("version", version);

            api.Action<string>(ApiPath, AvailableApiAction.SIGN_AGREEMENT, Id, "GET", param);
        }

        #endregion

    }
}
