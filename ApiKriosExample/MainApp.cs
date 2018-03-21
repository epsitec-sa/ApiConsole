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
            //Get the instanciated user in the scope
            UserADKomodo user = scope.Get<UserADKomodo>("user");

            scope.GetEventChannel().WaitOn(user.Create(apiKrios), scope, OnUserADKomodoCreated);
        }

        public static void OnUserADKomodoCreated(Scope scope)
        {
            API_Proxy_BackendConsole apiKrios = scope.GetApiKrios();
            UserADKomodo createdUser = scope.GetCreatedResource<UserADKomodo>();

            Contract cont = scope.Get<Contract>("contract");
            Pool p = scope.Get<Pool>("pool");

            SDXProfile sdxProfile = new SDXProfile(cont.IdCustomer, cont.Id, p.Id, createdUser);
            scope.GetEventChannel().WaitOn(sdxProfile.Create(apiKrios), scope, OnProfileCreated);
        }

        public static void OnProfileCreated(Scope scope)
        {
            API_Proxy_BackendConsole apiKrios = scope.GetApiKrios();
            SDXProfile createdProfile = scope.GetCreatedResource<SDXProfile>();

            //Get SDXMutCCloud modConfig. ModConfigs are template for SwissDesk Session.
            SDXModConfig modConfig = SDXModConfig.Get(apiKrios, SDXModConfig.SDXMutCCloud);
            SDXSession sdxSession = new SDXSession(createdProfile, modConfig, SDXSession.SessionState_Production, SDXSession.ConnectionMode_Desktop);

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
            API_Proxy_BackendConsole apiKrios = new API_Proxy_BackendConsole(API_Proxy_BackendConsole.BaseUrl_SandBox, "YOUR API KEY");
            //Open event channel
            EventChannel evtChannel = new EventChannel(EventChannel.EventHubUrl_Sandbox);
            //Create a scope used in callback
            Scope evtScope = new Scope(evtChannel, apiKrios);

            #region Create Admin stff
            
            //Create a TenantKrios for the end user
            TenantKrios tenant = new TenantKrios()
            {
                Company = "FooTest Inc",
                Firstname = "Foo",
                Lastname = "Bäär"
            };

            tenant.Create(apiKrios);

            //Create a contract for this tenant. Your TenantKrios will be invoiced and setted as reseller. (Your TenantKrios is defined by your api key)
            Contract cont = new Contract()
            {
                IdCustomer = tenant.Id,
                InvoiceMode = Contract.InvoiceMode_Prepaid,
                IsCurrentUserInvoiced = true,
                IsCurrentUserReseller = true,
                Name = "Crésus Cloud"
            };

            cont.Create(apiKrios);

            //Sign the agreement named "CCloud" version 1.0.0 on this contract
            cont.Sign(apiKrios, "CCloud", "1.0.0");

            //Create a resource pool
            Pool p = new Pool()
            {
                IdCustomer = tenant.Id,
                IsDefault = true,
                Title = "DefaultPool"
            };

            p.Create(apiKrios);

            //Grant access on this pool for the end user and the reseller. 
            p.GrantAccess(apiKrios, UserADKomodo.OBJTYPE);
            p.GrantAccessToReseller(apiKrios, UserADKomodo.OBJTYPE);

            p.GrantAccess(apiKrios, Storage.OBJTYPE);
            p.GrantAccessToReseller(apiKrios, Storage.OBJTYPE);

            p.GrantAccess(apiKrios, StorageHeadFolder.OBJTYPE);
            p.GrantAccessToReseller(apiKrios, StorageHeadFolder.OBJTYPE);

            p.GrantAccess(apiKrios, SDXProfile.OBJTYPE);
            p.GrantAccessToReseller(apiKrios, SDXProfile.OBJTYPE);

            p.GrantAccess(apiKrios, SDXSession.OBJTYPE);
            p.GrantAccessToReseller(apiKrios, SDXSession.OBJTYPE);

            p.GrantAccess(apiKrios, SDXService.OBJTYPE);
            p.GrantAccessToReseller(apiKrios, SDXService.OBJTYPE);

            #endregion

            //Instantiate a UserADKomodo and put it in scope. This object will be create later.
            UserADKomodo user = new UserADKomodo(cont.IdCustomer, cont.Id, p.Id, "test")
            {
                P_DisplayName = "User TestApi",
                P_Pwd = "MyStrongPwd2!"
            };

            //Add some usefull variable in the scope
            evtScope.Add("user", user);
            evtScope.Add("contract", cont);
            evtScope.Add("pool", p);

            //Get production storage type. StorageTypes are templates of Storage.
            Storage.StorageType storageProdType = Storage.StorageType.Get(apiKrios, Storage.StorageType.ProdType);

            //Instantiate a storage named "Production" with the template storageProdType.
            Storage storageProd = new Storage(cont.IdCustomer, cont.Id, p.Id, storageProdType, "Production");
            //Create the storage and wait util the creation is complete.
            evtChannel.WaitOn(storageProd.Create(apiKrios), evtScope, OnStorageCreated);



            while (working)
            {
                Thread.Sleep(5000);
            }

        }

    }
}
