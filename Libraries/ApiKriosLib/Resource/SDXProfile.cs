using AdminConsole_Type.Resources.SwissDeskX;
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
    public class SDXProfile : Resource, SDXProfileType
    {
        #region Fields

        public const String OBJTYPE = SDXProfile_Field.OBJTYPE;
        public static new int RType_ID = Resource.GetResourceTypeId(OBJTYPE);

        public string P_JsonConfig { get; set; }
        public string I_CustomerId { get; set; }

        #endregion

        #region Constructors

        public SDXProfile() : base(SDXProfile_Field.OBJTYPE, "API_SDXProfile")
        {

        }

        public SDXProfile(long customerId, long contractId, long poolId, UserADKomodo user) : base(SDXProfile_Field.OBJTYPE, "API_SDXProfile", contractId, poolId)
        {
            R_Name = user.R_Name;
            I_CustomerId = String.Format("c{0}", customerId);
        }

        public SDXProfile(long customerId, long contractId, long poolId, UserADKomodo user, String jsonConfig) : this(customerId, contractId, poolId, user)
        {
            P_JsonConfig = jsonConfig;
        }

        #endregion
    }
}
