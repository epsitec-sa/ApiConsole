using AdminConsole_Type.Resources.SwissDeskX;
using ODataClientLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiKriosLib.Resource
{
    /// <summary>
    /// SDX Session template.
    /// </summary>
    public class SDXModConfig : SDXModConfigType
    {
        #region Fields

        public static String SDXMutCCloud = "SDXMutCC";

        public int ID { get; set; }
        /// <summary>
        /// Identification code
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Set a specific server where the remote session will be executed
        /// </summary>
        public string CustomerSrv { get; set; }
        /// <summary>
        /// HorizonPool where the remote session will be executed.
        /// </summary>
        public string HorizonPool { get; set; }
        public int PointerId { get; set; }
        public int ReferenceId { get; set; }
        public string UemDefaultConfigPath { get; set; }
        /// <summary>
        /// All available template services with this modConfig.
        /// </summary>
        public List<SDXModConfig_Service> Services { get; set; }

        #endregion

        #region Getters

        public static List<SDXModConfig>List(API_Proxy_BackendConsole api)
        {
            return api.List<SDXModConfig>("API_SDXModConfig");
        }

        public static SDXModConfig Get(API_Proxy_BackendConsole api, String code)
        {
            return List(api).Find(m => m.Code == code);
        }

        #endregion
    }

    /// <summary>
    /// SDX Service template.
    /// </summary>
    public class SDXModConfig_Service : SDXModConfig_ServiceType
    {
        #region Fields

        public static String SDXMutCCloud_CresusSalaire = "SDX_CCLOUD_SAL";
        public static String SDXMutCCloud_Office365 = "SDX_O2016_E3";

        public int ID { get; set; }
        /// <summary>
        /// Identification code
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Agreement to sign in order to enable the service
        /// </summary>
        public int AgreementId { get; set; }
        public string GroupAD { get; set; }
        public int ModConfigId { get; set; }
        public int ReferenceId { get; set; }
        public int ResourceTypeId { get; set; }
        public string Name { get; set; }

        #endregion

    }

}
