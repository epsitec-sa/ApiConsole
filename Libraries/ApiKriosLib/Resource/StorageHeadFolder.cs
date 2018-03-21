using AdminConsole_Type.Resources.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiKriosLib.Resource
{
    public class StorageHeadFolder : Resource, StorageHeadFolderType
    {
        #region Fields

        public const String OBJTYPE = StorageHeadFolder_Field.OBJTYPE;
        public static new int RType_ID = Resource.GetResourceTypeId(OBJTYPE);

        public string P_FullPath { get; set; }
        public string P_Name { get; set; }
        #endregion

        #region Constructors

        public StorageHeadFolder() : base(StorageHeadFolder_Field.OBJTYPE, "API_StorageHeadFolder")
        {

        }

        public StorageHeadFolder(Storage parentStorage, String headFolderName) : base(StorageHeadFolder_Field.OBJTYPE, "API_StorageHeadFolder", parentStorage.R_IDContract, parentStorage.R_IDPool)
        {
            R_IDParent = parentStorage.R_IDItem;

            R_Name = P_Name = headFolderName;
            R_Pointer = parentStorage.R_Pointer;
        }

        #endregion
    }
}
