using ConsoleAdminAPI;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiKriosLib.Resource
{
    public class SDXSession : Resource, API_SDXSession
    {

        #region Inner Classes

        public class SDXSession_JsonConfig
        {
            public String lang { get; set; }
        }

        #endregion

        #region Fields

        public const String OBJTYPE = API_Resource.SDX_SESSION;
        public static new int RType_ID = Resource.GetResourceTypeId(OBJTYPE);

        public static String SessionState_Testing = "Test";
        public static String SessionState_Production = "Prod";

        public static String ConnectionMode_Desktop = "DESKTOP";
        public static String ConnectionMode_RemoteApp = "REMOTE_APP";

        public static String Lang_FR = "FR";
        public static String Lang_EN = "EN";
        public static String Lang_DE = "DE";
        public static String LANG_IT = "IT";

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

        public SDXSession() : base(OBJTYPE, "API_SDXSession")
        {

        }

        public SDXSession(SDXProfile profile, SDXModConfig modConfig, String sessionState, String connectionMode) : base(OBJTYPE, "API_SDXSession", profile.R_IDPool)
        {
            R_Name = profile.R_Name;
            R_Pointer = modConfig.PointerId;

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

        public SDXSession(SDXProfile profile, SDXModConfig modConfig, String sessionState, String connectionMode, bool useGenericScript, String lang) : this(profile, modConfig, sessionState, connectionMode, useGenericScript)
        {
            SDXSession_JsonConfig configs = new SDXSession_JsonConfig()
            {
                lang = lang
            };

            P_JsonConfig = JsonConvert.SerializeObject(configs);
        }

        #endregion
    }
}
