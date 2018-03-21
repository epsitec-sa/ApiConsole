using Microsoft.AspNet.SignalR.Client;
using Newtonsoft.Json;
using ODataClientLib;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiKriosLib.Event
{
    public class EventChannel
    {
        #region Fields
        private HubConnection HubConnection { get; set; }
        private IHubProxy EventProxy { get; set; }
        private Dictionary<EventMsg, EventHandler> Handlers { get; set; }

        public static String EventHubUrl_Sandbox = @"https://consolesandbox.komodo.ch/krios";
        public static String EventHubUrl_Dev = @"http://10.0.25.61/BackEnd_Console";

        #endregion

        #region Constructors

        public EventChannel(String eventHubUrl)
        {
            HubConnection = new HubConnection(eventHubUrl);
            EventProxy = HubConnection.CreateHubProxy("EventHub");
            Handlers = new Dictionary<EventMsg, EventHandler>();

            EventProxy.On("handleEvent", (EventChannel.EventMsg e) =>
            {
                Console.WriteLine(e);

                if (Handlers.ContainsKey(e))
                {
                    HandleEvent(e);
                    CleanEvent(e);
                }
            });

            HubConnection.Start().Wait();
        }

        #endregion

        #region Public methods

        public void WaitOn(long requestId, Scope scope, Action<Scope> handler)
        {
            EventChannel.EventMsg e = new EventChannel.EventMsg()
            {
                Id = requestId.ToString(),
                Type = "RequestEvent_End",
                Field = String.Empty
            };

            Handlers.Add(e, new EventHandler()
            {
                Scope = scope,
                Handler = handler
            });

            RegisterEvent(e);
        }

        #endregion

        #region Events methods

        private void RegisterEvent(EventMsg e)
        {
            EventProxy.Invoke("RegisterEventListener", e);
        }

        private void DeregisterEvent(EventMsg e)
        {
            EventProxy.Invoke("DeregisterEventListener", e);
        }

        private void HandleEvent(EventMsg e)
        {
            Handlers[e].Scope.Add("event", e);
            Handlers[e].Handler(Handlers[e].Scope);
        }

        private void CleanEvent(EventMsg e)
        {
            DeregisterEvent(e);
            Handlers[e].Scope.Remove("event");
            Handlers.Remove(e);
        }

        #endregion

        #region Internal Class
        public class EventMsg
        {
            public String Type { get; set; }
            public String Id { get; set; }
            public String Field { get; set; }
            public String Value { get; set; }

            public override bool Equals(object obj)
            {
                EventMsg m = obj as EventMsg;

                return m != null
                         && this.Type == m.Type
                         && this.Id == m.Id
                         && this.Field == m.Field;
            }

            public override int GetHashCode()
            {
                if (this.Id == null)
                    return this.Type.GetHashCode();
                else
                    return this.Type.GetHashCode()
                    ^ this.Id.GetHashCode();
            }
        }

        public class EventHandler
        {
            public Scope Scope { get; set; }
            public Action<Scope> Handler { get; set; }
        }

        #endregion
    }

    public class Scope
    {
        #region Fields

        private Dictionary<String, object> ObjScope { get; set; }
        private API_Proxy_BackendConsole ApiKrios { get; set; }
        private EventChannel EvtChannel { get; set; }

        #endregion

        #region Constructor

        public Scope(EventChannel evtChannel, API_Proxy_BackendConsole api)
        {
            EvtChannel = evtChannel;
            ApiKrios = api;
            ObjScope = new Dictionary<String, object>();
        }

        #endregion

        #region Add/Remove Object in scope

        public void Add(String key, object value)
        {
            ObjScope.Add(key, value);
        }

        public void Remove(String key)
        {
            ObjScope.Remove(key);
        }

        public void Clear()
        {
            ObjScope.Clear();
        }

        #endregion

        #region Getters

        public T GetCreatedResource<T>()
        {
            EventChannel.EventMsg e = Get<EventChannel.EventMsg>("event");
            return JsonConvert.DeserializeObject<T>(e.Value);
        }

        public API_Proxy_BackendConsole GetApiKrios()
        {
            return ApiKrios;
        }

        public EventChannel GetEventChannel()
        {
            return EvtChannel;
        }

        public T Get<T>(String key)
        {
            return (T)ObjScope[key];
        }

        #endregion
    }
}
