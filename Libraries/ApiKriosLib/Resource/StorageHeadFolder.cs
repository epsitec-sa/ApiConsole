using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleAdminAPI;

namespace ApiKriosLib.Resource
{
    public class StorageHeadFolder : Resource, API_StorageHeadFolder
    {
        #region Fields

        public const String OBJTYPE = API_Resource.STORAGE_HEADFOLDER;
        public static new int RType_ID = Resource.GetResourceTypeId(OBJTYPE);

        public string P_FullPath { get; set; }
        public string P_Name { get; set; }
        public String I_CustomerId { get; set; }
        #endregion

        #region Constructors

        public StorageHeadFolder() : base(OBJTYPE, "API_StorageHeadFolder")
        {

        }

        public StorageHeadFolder(Storage parentStorage, String headFolderName) : base(OBJTYPE, "API_StorageHeadFolder", parentStorage.R_IDPool)
        {
            R_IDParent = parentStorage.R_IDItem;

            R_Name = P_Name = headFolderName;
            R_Pointer = parentStorage.R_Pointer;
            I_CustomerId = parentStorage.I_CustomerId;
        }

        #endregion
    }
}
