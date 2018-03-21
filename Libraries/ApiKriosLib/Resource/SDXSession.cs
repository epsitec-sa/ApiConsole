using AdminConsole_Type.Resources.SwissDeskX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiKriosLib.Resource
{
    public class SDXSession : Resource, SDXSessionType
    {
        #region Fields

        public const String OBJTYPE = SDXSession_Field.OBJTYPE;
        public static new int RType_ID = Resource.GetResourceTypeId(OBJTYPE);

        public static String SessionState_Testing = "Test";
        public static String SessionState_Production = "Prod";

        public static String ConnectionMode_Desktop = "DESKTOP";
        public static String ConnectionMode_RemoteApp = "REMOTE_APP";

        public string P_ModConfig_code { get; set; }
        public string P_ModConfig_customSrv { get; set; }
        public string P_ModConfig_horizonPool { get; set; }
        public string P_ModConfig_State { get; set; }
        public string I_UemDefaultConfigPath { get; set; }

        public string P_ConnectionMode { get; set; }
        public string P_JsonConfig { get; set; }
        public bool P_GenericScript { get; set; }

        #endregion

        #region Constructors

        public SDXSession() : base(SDXSession_Field.OBJTYPE, "API_SDXSession")
        {

        }

        public SDXSession(SDXProfile profile, SDXModConfig modConfig, String sessionState, String connectionMode) : base(SDXSession_Field.OBJTYPE, "API_SDXSession", profile.R_IDContract, profile.R_IDPool)
        {
            R_Name = profile.R_Name;
            R_Pointer = modConfig.PointerId;
            R_IDReference = modConfig.ReferenceId;

            P_ModConfig_code = modConfig.Code;
            P_ModConfig_customSrv = modConfig.CustomerSrv;
            P_ModConfig_horizonPool = modConfig.HorizonPool;
            I_UemDefaultConfigPath = modConfig.UemDefaultConfigPath;
            P_ModConfig_State = sessionState;

            P_ConnectionMode = connectionMode;
            P_GenericScript = false;
        }

        public SDXSession(SDXProfile profile, SDXModConfig modConfig, String sessionState, String connectionMode, String jsonConfig) : this(profile, modConfig, sessionState, connectionMode)
        {
            P_JsonConfig = jsonConfig;
        }

        public SDXSession(SDXProfile profile, SDXModConfig modConfig, String sessionState, String connectionMode, bool useGenericScript) : this(profile, modConfig, sessionState, connectionMode)
        {
            P_GenericScript = useGenericScript;
        }

        public SDXSession(SDXProfile profile, SDXModConfig modConfig, String sessionState, String connectionMode, String jsonConfig, bool useGenericScript) : this(profile, modConfig, sessionState, connectionMode, jsonConfig)
        {
            P_GenericScript = useGenericScript;
        }


        #endregion
    }
}
