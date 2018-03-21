using AdminConsole_Type.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiKriosLib.Resource
{
    public class SDXService : Resource, SDXServiceType
    {
        #region Fields

        public const String OBJTYPE = SDXService_Field.OBJTYPE;
        public static new int RType_ID = Resource.GetResourceTypeId(OBJTYPE);

        public string P_JsonData { get; set; }
        public string P_Name { get; set; }

        #endregion

        #region Constructors

        public SDXService() : base(SDXService_Field.OBJTYPE, "API_SDXService")
        {

        }

        public SDXService(SDXSession parentSession, SDXModConfig_Service modConfigService) : base(SDXService_Field.OBJTYPE, "API_SDXService", parentSession.R_IDContract, parentSession.R_IDPool)
        {
            R_IDParent = parentSession.R_IDItem;
            R_IDReference = modConfigService.ReferenceId;

            R_Name = modConfigService.Code;
            P_Name = modConfigService.Name;
        }

        public SDXService(SDXSession parentSession, SDXModConfig_Service modConfigService, String jsonConfig) : this(parentSession, modConfigService)
        {
            P_JsonData = jsonConfig;
        }

        #endregion
    }
}
