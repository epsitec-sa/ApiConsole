using ConsoleAdminAPI;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ODataClientLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiKriosLib.Resource
{
    public class UserADKomodo : Resource, API_UserADKomodo
    {
        #region Fields

        public const String OBJTYPE = API_Resource.USER_AD_KOMODO;
        public static new int RType_ID = Resource.GetResourceTypeId(OBJTYPE);

        public string P_ConfirmPwd { get; set; }
        public bool P_Disabled { get; set; }
        public DateTime? P_DisabledDate { get; set; }
        public string P_DisplayName { get; set; }
        public string P_Email { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public EmployeeType P_EmployeeType { get; set; }
        public string P_Firstname { get; set; }
        public bool P_FTPAccess { get; set; }
        public string P_Guid { get; set; }
        public string P_Lastname { get; set; }
        public string P_Mobile { get; set; }
        public string P_Phone { get; set; }
        public string P_Pwd { get; set; }
        public bool P_PwdNeverExpires { get; set; }
        public bool P_UsrCantChangePwd { get; set; }
        public bool P_WebDAVAccess { get; set; }
        public bool P_UserMustChangePwd { get; set; }
        #endregion

        #region Constructors

        public UserADKomodo() : base(OBJTYPE, "API_UserADKomodo")
        {

        }

        public UserADKomodo(long customerId, long poolId, String userName) : base(OBJTYPE, "API_UserADKomodo", poolId)
        {

            R_Name = String.Format("c{0}.{1}", customerId, userName);

        }

        #endregion


    }
}
