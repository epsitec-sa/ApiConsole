using ConsoleAdminAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiKriosLib.Resource
{
    /// <summary>
    /// Profile of a remote user.
    /// </summary>
    public class SDXProfile : Resource, API_SDXProfile
    {
        #region Fields

        public const String OBJTYPE = API_Resource.SDX_PROFILE;
        public static new int RType_ID = Resource.GetResourceTypeId(OBJTYPE);

        public string P_JsonConfig { get; set; }
        public string I_CustomerId { get; set; }

        #endregion

        #region Constructors

        public SDXProfile() : base(OBJTYPE, "API_SDXProfile")
        {

        }

        public SDXProfile(long customerId, long poolId, UserADKomodo user) : base(OBJTYPE, "API_SDXProfile", poolId)
        {
            R_Name = user.R_Name;
            I_CustomerId = String.Format("c{0}", customerId);
        }

        public SDXProfile(long customerId, long poolId, UserADKomodo user, String jsonConfig) : this(customerId, poolId, user)
        {
            P_JsonConfig = jsonConfig;
        }

        #endregion
    }
}
