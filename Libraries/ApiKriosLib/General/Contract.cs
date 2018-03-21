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
    public class Contract : ContractType
    {

        public static class AvailableApiAction
        {
            public const String SIGN = "Act.Sign";
        }


        #region Fields

        private static String ApiPath = "API_Contract";

        public static String InvoiceMode_Prepaid = "PREPAID";
        public static String InvoiceMode_Postpaid = "POSTPAID";

        /// <summary>
        /// Contract Id.
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Tenant Id where the contract is.
        /// </summary>
        public long IdCustomer { get; set; }
        /// <summary>
        /// Could be PREPAID or POSTPAID.
        /// </summary>
        public string InvoiceMode { get; set; }
        /// <summary>
        /// Is the current User (the user linked with the current api key) have to be invoiced.
        /// Else, the customer setted in IdCustomer will be invoiced instead.
        /// </summary>
        public bool IsCurrentUserInvoiced { get; set; }
        /// <summary>
        /// Is the current User (the user linked with the current api key) a reseller for the customer setted in IdCustomer.
        /// </summary>
        public bool IsCurrentUserReseller { get; set; }
        /// <summary>
        /// Contract Name.
        /// </summary>
        public string Name { get; set; }

        #endregion

        #region CRUD

        /// <summary>
        /// Create a new Contract with info stored in this object.
        /// Will update Id Field
        /// </summary>
        /// <param name="api">Api object to use</param>
        public void Create(API_Proxy_BackendConsole api)
        {
            Id = api.Post<long>(ApiPath, JsonConvert.SerializeObject(this));
        }

        #endregion

        #region Actions

        /// <summary>
        /// Sign this contract with an agreement.
        /// </summary>
        /// <param name="agreementName">Name of the agreement to sign</param>
        /// <param name="version">Versionn of the agreemennt to sign. Format example: 1.0.0</param>
        public void Sign(API_Proxy_BackendConsole api, String agreementName, String version)
        {
            NameValueCollection param = new NameValueCollection();

            param.Add("agreementName", agreementName);
            param.Add("version", version);

            api.Action<string>(ApiPath, AvailableApiAction.SIGN, Id, "GET", param);
        }

        #endregion

    }
}
