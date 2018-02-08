using System;
using System.Collections.Generic;
using System.Linq;
using CitizenFX.Core;
using Dispatch.Common.DataHolders;

namespace DispatchSystem.Server.RequestHandling
{
    public delegate void RequestHandlerHandler(string type, Request req, string err, EventArgument[] reqInfo);

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
        public void TriggerEvent(string eventName, EventArgument[] args = null, string err = null)
        {
            object[] calArgs = {};
            SendExplicitData("on_" + eventName, calArgs, new RequestData(err, args));
        }
        private static void SendExplicitData(string type, object[] calArgs, RequestData info)
        {
            string err = info?.Error ?? "none";
            EventArgument[] args = info?.Arguments ?? new EventArgument[] { };
            calArgs = calArgs ?? new object[] { };

            Log.WriteLineSilent($"Sending Data: Type: \"{type}\" | Error: \"{err}\"");

            BaseScript.TriggerEvent("dispatchsystem:event", type, err, EventArgument.ToArray(args.AsEnumerable()), calArgs);
        }

        public void HandleMultiple(IEnumerable<object> objects)
        {
            foreach (var obj in objects)
            {
                var list = (List<object>) obj;
                if (list.Count < 3)
                    throw new InvalidOperationException("Input array was not of length 3");
                Handle((string)list[0], ((List<object>)list[1]).ToArray(), ((List<object>)list[2]).ToArray());
            }
        }
    }
}
