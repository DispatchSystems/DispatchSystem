using System;
using System.Collections.Generic;
using CitizenFX.Core;

namespace DispatchSystem.Server.RequestHandling
{
    public delegate void RequestHandlerHandler(string type, Request req, string err, object[] reqInfo);

    public class RequestHandler
    {
        public string LastType { get; private set; }
        public List<Request> Types { get; set; }

        public event RequestHandlerHandler OnHandle;

        public RequestHandler(List<Request> types = null)
        {
            Types = types ?? new List<Request>();
        }

        public void Handle(string type, object[] args, object[] calArgs)
        {
            type = type.ToLower();
            calArgs = calArgs ?? new object[] { };

            Request request = Types.Find(x => x.Name == type);
            if (request == null)
            {
                OnHandle?.Invoke("invalid", null, null, null);
                SendExplicitData("invalid", calArgs, null);
                return;
            }

            LastType = type;
            RequestData sendBack;
            try
            {
                sendBack = request.Callback(args);
            }
            catch (Exception e)
            {
                Log.WriteLine("Whoops, looks like something happened while trying to handle a request, " +
                              "more information available in the log file " +
                              "-- Please contact BlockBa5her");
                Log.WriteLineSilent(e.ToString());
                return;
            }
            OnHandle?.Invoke(type, request, sendBack?.Error, sendBack?.Arguments);
            if (sendBack == null)
                throw new ArgumentNullException(nameof(sendBack), "Event handler found null device");

            SendExplicitData(type, calArgs, sendBack);
        }
        public void TriggerEvent(string eventName, Player p = null, object[] args = null, string err = null)
        {
            object[] calArgs = {};
            SendExplicitData("on_" + eventName, calArgs, new RequestData(p, err, args));
        }
        private void SendExplicitData(string type, object[] calArgs, RequestData info)
        {
            string err = info?.Error ?? "none";
            Player p = info?.Player;
            object[] args = info?.Arguments ?? new object[] { };
            calArgs = calArgs ?? new object[] { };

            Log.WriteLineSilent($"Sending Data: \"{type}\"|\"{err}\"|\"{p?.Handle ?? "-1"}\"");

            if (p == null)
                BaseScript.TriggerEvent("dispatchsystem:event", -1, type, err, args, calArgs);
            else
                BaseScript.TriggerEvent("dispatchsystem:event",
                    int.Parse(p.Handle ?? throw new InvalidOperationException()), type, err, args, calArgs);
        }
    }
}
