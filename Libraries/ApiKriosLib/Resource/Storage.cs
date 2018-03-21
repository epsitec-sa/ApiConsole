using AdminConsole_Type.Resources.Storage;
using ODataClientLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiKriosLib.Resource
{
    public class Storage : Resource, StorageType
    {
        #region Fields

        public const String OBJTYPE = Storage_Field.OBJTYPE;
        public static new int RType_ID = Resource.GetResourceTypeId(OBJTYPE);

        public long P_EffectiveSize { get; set; }
        public string P_Name { get; set; }
        public string P_Path { get; set; }
        public long P_Size { get; set; }
        public string P_Type { get; set; }
        public string I_CustomerId { get; set; }
        #endregion

        #region Constructors

        public Storage() : base(Storage_Field.OBJTYPE, "API_Storage")
        {

        }

        public Storage(long customerId, long contractId, long poolId, StorageType type, String name) : base(Storage_Field.OBJTYPE, "API_Storage", contractId, poolId)
        {
            I_CustomerId = String.Format("c{0}", customerId);
            P_Name = name;
            R_Pointer = type.ptrId;
            R_IDReference = type.refId;
            P_Type = type.type;
        }

        #endregion

        #region Internal Class

        public class StorageType
        {
            public static String ProdType = "PROD";
            public static String ArchiveType = "ARCHIVE";
            public static String UemProfileType = "UEM_PROFILE";
            public static String ColdType = "COLD";
            public static String UemConfigType = "UEM_CONFIG";

            public String type { get; set; }
            public int ptrId { get; set; }
            public int refId { get; set; }

            public static List<StorageType> List(API_Proxy_BackendConsole api)
            {
                return api.List<StorageType>("API_StorageType");
            }

            public static StorageType Get(API_Proxy_BackendConsole api, String type)
            {
                return List(api).Find(t => t.type == type);
            }
        }

        #endregion

    }
}
