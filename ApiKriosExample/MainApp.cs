using ApiKriosLib.Event;
using ApiKriosLib.General;
using ApiKriosLib.Resource;
using ODataClientLib;
using System.Threading;

namespace ApiKriosExample
{
    public class MainApp
    {
        public static bool working = true;

        #region Callback

        public static void OnUserADKomodoCreated(Scope scope)
        {
            API_Proxy_BackendConsole apiKrios = scope.GetApiKrios();
            UserADKomodo createdUser = scope.GetCreatedResource<UserADKomodo>();

            scope.Add("createdUser", createdUser);

            TenantKrios tenant = scope.Get<TenantKrios>("tenant");
            Pool p = scope.Get<Pool>("pool");

            //Get production storage type. StorageTypes are templates of Storage.
            Storage.StorageType storageProdType = Storage.StorageType.Get(apiKrios, Storage.StorageType.ProdType);
            //Instantiate a storage named "Production" with the template storageProdType.
            Storage storageProd = new Storage(tenant.Id, p.Id, storageProdType, "Production");

            scope.GetEventChannel().WaitOn(storageProd.Create(apiKrios), scope, OnStorageCreated);
        }


        public static void OnStorageCreated(Scope scope)
        {
            API_Proxy_BackendConsole apiKrios = scope.GetApiKrios();
            //Get the created storage
            Storage createdStorage = scope.GetCreatedResource<Storage>();

            //Instanciate a HeadFolder on the created storage
            StorageHeadFolder hf = new StorageHeadFolder(createdStorage, "Document Crésus");

            //Create and wait
            scope.GetEventChannel().WaitOn(hf.Create(apiKrios), scope, OnHeadFolderCreated);
        }

        public static void OnHeadFolderCreated(Scope scope)
        {
            API_Proxy_BackendConsole apiKrios = scope.GetApiKrios();

            TenantKrios tenant = scope.Get<TenantKrios>("tenant");
            Pool p = scope.Get<Pool>("pool");

            //Get uem storage type.
            Storage.StorageType storageProdType = Storage.StorageType.Get(apiKrios, Storage.StorageType.UemType);
            //Instantiate a storage named "UEM" with the template storageProdType.
            Storage storageUem = new Storage(tenant.Id, p.Id, storageProdType, "UEM");

            scope.GetEventChannel().WaitOn(storageUem.Create(apiKrios), scope, OnUemStorageCreated);
        }

        public static void OnUemStorageCreated(Scope scope)
        {
            API_Proxy_BackendConsole apiKrios = scope.GetApiKrios();

            TenantKrios tenant = scope.Get<TenantKrios>("tenant");
            Pool p = scope.Get<Pool>("pool");

            UserADKomodo createdUser = scope.Get<UserADKomodo>("createdUser");

            SDXProfile sdxProfile = new SDXProfile(tenant.Id, p.Id, createdUser);
            scope.GetEventChannel().WaitOn(sdxProfile.Create(apiKrios), scope, OnProfileCreated);
        }

        public static void OnProfileCreated(Scope scope)
        {
            API_Proxy_BackendConsole apiKrios = scope.GetApiKrios();
            SDXProfile createdProfile = scope.GetCreatedResource<SDXProfile>();

            //Get SDXMutCCloud modConfig. ModConfigs are template for SwissDesk Session.
            SDXModConfig modConfig = SDXModConfig.Get(apiKrios, SDXModConfig.SDXMutCCloud);
            SDXSession sdxSession = new SDXSession(createdProfile, modConfig, SDXSession.SessionState_Production, SDXSession.ConnectionMode_Desktop, true, SDXSession.Lang_EN);

            scope.GetEventChannel().WaitOn(sdxSession.Create(apiKrios), scope, OnSessionCreated);
        }

        public static void OnSessionCreated(Scope scope)
        {
            API_Proxy_BackendConsole apiKrios = scope.GetApiKrios();
            SDXSession createdSession = scope.GetCreatedResource<SDXSession>();

            SDXModConfig modConfig = SDXModConfig.Get(apiKrios, SDXModConfig.SDXMutCCloud);

            SDXModConfig_Service template = modConfig.Services.Find(s => s.Code == SDXModConfig_Service.SDXMutCCloud_CresusSalaire);
            SDXService cresusSalaire = new SDXService(createdSession, template, "YOUR RAW JSON WITH a string field 'licence', containing the base64 licence file");

            template = modConfig.Services.Find(s => s.Code == SDXModConfig_Service.SDXMutCCloud_Office365);
            SDXService office365 = new SDXService(createdSession, template);

            cresusSalaire.Create(apiKrios);
            office365.Create(apiKrios);

            working = false;
        }

        #endregion

        public static void Main()
        {
            //Create api proxy
            API_Proxy_BackendConsole apiKrios = new API_Proxy_BackendConsole(API_Proxy_BackendConsole.BaseUrl_SandBox, "YOUR_API_KEY_HERE");
            //Open event channel
            EventChannel evtChannel = new EventChannel(EventChannel.EventHubUrl_Sandbox);
            //Create a scope used in callback
            Scope evtScope = new Scope(evtChannel, apiKrios);

            #region Create Admin stff

            //Create a TenantKrios for the end user. You will be the manager of this tenant.
            TenantKrios tenant = new TenantKrios()
            {
                Company = "SwissDesk X Test 7",
                Firstname = "",
                Lastname = ""
            };

            tenant.Create(apiKrios);

            //Sign agreement for this customer
            tenant.SignAgreement(apiKrios, "SwissDesk X", "1.0.0");


            //Create a resource pool
            Pool p = new Pool()
            {
                IdCustomer = tenant.Id,
                IsDefault = true,
                Title = "DefaultPool"
            };

            p.Create(apiKrios);

            //Grant access on this pool for the end user and the manager (your own tenant)
            p.GrantAccess(apiKrios, UserADKomodo.OBJTYPE);
            p.GrantAccessToManager(apiKrios, UserADKomodo.OBJTYPE);

            p.GrantAccess(apiKrios, Storage.OBJTYPE);
            p.GrantAccessToManager(apiKrios, Storage.OBJTYPE);

            p.GrantAccess(apiKrios, StorageHeadFolder.OBJTYPE);
            p.GrantAccessToManager(apiKrios, StorageHeadFolder.OBJTYPE);

            p.GrantAccess(apiKrios, SDXProfile.OBJTYPE);
            p.GrantAccessToManager(apiKrios, SDXProfile.OBJTYPE);

            p.GrantAccess(apiKrios, SDXSession.OBJTYPE);
            p.GrantAccessToManager(apiKrios, SDXSession.OBJTYPE);

            p.GrantAccess(apiKrios, SDXService.OBJTYPE);
            p.GrantAccessToManager(apiKrios, SDXService.OBJTYPE);

            #endregion

            //Instantiate a UserADKomodo and put it in scope. This object will be create later.
            //Pwd policy: 8 chars, at least one special char (non-alphanumeric), a number or a uppercase letter.
            UserADKomodo user = new UserADKomodo(tenant.Id, p.Id, "apiTest1")
            {
                P_DisplayName = "User TestApi 1",
                P_Pwd = "MyStrongPwd2!"
            };

            //Add some usefull variable in the scope
            evtScope.Add("user", user);
            evtScope.Add("pool", p);
            evtScope.Add("tenant", tenant);

            //Create the storage and wait util the creation is complete
            evtChannel.WaitOn(user.Create(apiKrios), evtScope, OnUserADKomodoCreated);

            while (working)
            {
                Thread.Sleep(5000);
            }

        }

    }
}
