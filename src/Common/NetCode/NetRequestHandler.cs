using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using DispatchSystem.Common.DataHolders;

namespace DispatchSystem.Common.NetCode
{
    public class NetRequestHandler
    {
        private readonly Socket S;
        private readonly Thread listenThread;
        private readonly Dictionary<string, NetRequestResult> CachedNetEvents;
        private readonly Dictionary<string, Tuple<NetRequestResult, object>> CachedNetFunctions;
        private readonly Dictionary<string, Tuple<bool, object>> CachedNetValues;
        private readonly Dictionary<int, Thread> Threads;

        public Dictionary<string, NetEvent> NetEvents { get; set; }
        public Dictionary<string, NetFunction> NetFunctions { get; set; }
        public Dictionary<string, object> NetValues { get; set; }
        public string IP { get; }
        public int Port { get; }

        public NetRequestHandler(Socket socket, bool autostart = true)
        {
            string[] vals = socket.RemoteEndPoint.ToString().Split(':');
            IP = vals[0];
            Port = int.Parse(vals[1]);

            NetEvents = new Dictionary<string, NetEvent>();
            NetFunctions = new Dictionary<string, NetFunction>();
            NetValues = new Dictionary<string, object>();

            CachedNetValues = new Dictionary<string, Tuple<bool, object>>();
            CachedNetFunctions = new Dictionary<string, Tuple<NetRequestResult, object>>();
            CachedNetEvents = new Dictionary<string, NetRequestResult>();

            S = socket;
            Threads = new Dictionary<int, Thread>();
            listenThread = new Thread(Receiver);

            if (autostart)
                listenThread.Start();
        }

        public void Receive()
        {
            if (!listenThread.IsAlive)
                listenThread.Start();
        }

        private void Receiver()
        {
            byte[] buffer = new byte[5000];
            int end;
            try
            {
                end = S.Receive(buffer);
            }
            catch (SocketException)
            {
                return;
            }
            buffer = buffer.Take(end).ToArray();

            if (buffer.Length == 0)
                return;

            NetRequest netRequest = new StorableValue<NetRequest>(buffer).Value;

            switch (netRequest.Metadata)
            {
#pragma warning disable 4014
                case NetRequestMetadata.InvocationRequest:
                    try
                    {
                        HandleInvocationRequest(netRequest).GetAwaiter().GetResult();
                    }
                    catch
                    {
                        StorableValue<NetRequest> returnNetRequest = new StorableValue<NetRequest>(new NetRequest(NetRequestMetadata.InvocationReturn, netRequest.Data.Value[1], NetRequestResult.Incompleted));
                        S.Send(returnNetRequest.Bytes);
                    }
                    break;

                case NetRequestMetadata.InvocationReturn:
                    HandleInvocationReturn(netRequest).GetAwaiter().GetResult();
                    break;

                case NetRequestMetadata.ValueRequest:
                    HandleValueRequest(netRequest).GetAwaiter().GetResult();
                    break;

                case NetRequestMetadata.ValueReturn:
                    HandleValueReturn(netRequest).GetAwaiter().GetResult();
                    break;

                case NetRequestMetadata.FunctionRequest:
                    try
                    {
                        HandleFunctionRequest(netRequest).GetAwaiter().GetResult();
                    }
                    catch
                    {
                        StorableValue<NetRequest> returnNetRequest = new StorableValue<NetRequest>(new NetRequest(NetRequestMetadata.FunctionReturn, netRequest.Data.Value[1], NetRequestResult.Incompleted));
                        S.Send(returnNetRequest.Bytes);
                    }
                    break;

                case NetRequestMetadata.FunctionReturn:
                    HandleFunctionReturn(netRequest).GetAwaiter().GetResult();
                    break;
#pragma warning restore 4014
            }
        }

        #region handlers

        private async Task<bool> HandleInvocationRequest(NetRequest request)
        {
            if (request.Metadata != NetRequestMetadata.InvocationRequest)
                return false;

            string netEvent = (string)request.Data.Value[0];

            if (!NetEvents.ContainsKey(netEvent))
            {
                try
                {
                    S.Send(new StorableValue<NetRequest>(new NetRequest(NetRequestMetadata.InvocationReturn, netEvent, NetRequestResult.Incompleted)).Bytes);
                }
                catch
                {
                    return false;
                }
            }

            object[] parameters = (object[])request.Data.Value[1];

            await NetEvents[netEvent].Invoke(this, parameters);

            try
            {
                S.Send(new StorableValue<NetRequest>(new NetRequest(NetRequestMetadata.InvocationReturn, netEvent, NetRequestResult.Completed)).Bytes);
            }
            catch (SocketException)
            {
                return false;
            }

            await Task.FromResult(0);
            return true;
        }

        private async Task<bool> HandleInvocationReturn(NetRequest request)
        {
            if (request.Metadata != NetRequestMetadata.InvocationReturn)
                return false;

            string valueName = (string)request.Data.Value[0];
            NetRequestResult result = (NetRequestResult)request.Data.Value[1];

            CachedNetEvents.Add(valueName, result);

            await Task.FromResult(0);
            return true;
        }

        private async Task<bool> HandleValueRequest(NetRequest request)
        {
            if (request.Metadata != NetRequestMetadata.ValueRequest)
                return false;

            string valueName = (string)request.Data.Value[0];

            if (!NetValues.ContainsKey(valueName))
            {
                try
                {
                    S.Send(new StorableValue<NetRequest>(new NetRequest(NetRequestMetadata.ValueReturn, valueName,
                        false)).Bytes);
                }
                catch (SocketException)
                {
                    return false;
                }

                return true;
            }

            try
            {
                S.Send(new StorableValue<NetRequest>(new NetRequest(NetRequestMetadata.ValueReturn, valueName, true,
                    NetValues[valueName])).Bytes);
            }
            catch (SocketException)
            {
                return false;
            }

            await Task.FromResult(0);
            return true;
        }

        private async Task<bool> HandleValueReturn(NetRequest request)
        {
            if (request.Metadata != NetRequestMetadata.ValueReturn)
                return false;

            string valueName = (string)request.Data.Value[0];
            bool result = (bool)request.Data.Value[1];

            if (!result)
            {
                CachedNetValues.Add(valueName, new Tuple<bool, object>(false, null));
                return true;
            }

            CachedNetValues.Add(valueName, new Tuple<bool, object>(true, request.Data.Value[2]));

            await Task.FromResult(0);
            return true;
        }

        private async Task<bool> HandleFunctionRequest(NetRequest request)
        {
            if (request.Metadata != NetRequestMetadata.FunctionRequest)
                return false;

            string functionName = (string)request.Data.Value[0];
            object[] parameters = (object[])request.Data.Value[1];

            if (!NetFunctions.ContainsKey(functionName))
            {
                try
                {
                    S.Send(new StorableValue<NetRequest>(new NetRequest(NetRequestMetadata.FunctionReturn, functionName,
                        NetRequestResult.Invalid
                    )).Bytes);
                }
                catch (SocketException)
                {
                    return false;
                }

                return true;
            }

            try
            {
                S.Send(new StorableValue<NetRequest>(new NetRequest(NetRequestMetadata.FunctionReturn, functionName, NetRequestResult.Completed,
                    NetFunctions[functionName].Invoke(this, parameters).GetAwaiter().GetResult())).Bytes);
            }
            catch (SocketException)
            {
                return false;
            }

            await Task.FromResult(0);
            return true;
        }

        private async Task<bool> HandleFunctionReturn(NetRequest request)
        {
            if (request.Metadata != NetRequestMetadata.FunctionReturn)
                return false;

            string functionName = (string)request.Data.Value[0];
            NetRequestResult result = (NetRequestResult)request.Data.Value[1];

            if (result != NetRequestResult.Completed)
            {
                CachedNetFunctions.Add(functionName, new Tuple<NetRequestResult, object>(result, null));
                return true;
            }

            CachedNetFunctions.Add(functionName, new Tuple<NetRequestResult, object>(result, request.Data.Value[2]));

            await Task.FromResult(0);
            return true;
        }

        #endregion

        public async Task TriggerNetEvent(string netEventName, params object[] parameters)
        {
            S.Send(new StorableValue<NetRequest>(new NetRequest(NetRequestMetadata.InvocationRequest, netEventName,
                parameters)).Bytes);

            while (!CachedNetEvents.ContainsKey(netEventName))
                await Task.Delay(10);

            switch (CachedNetEvents[netEventName])
            {
                case NetRequestResult.Invalid:
                    throw new InvalidOperationException("NetEvent does not exist!");
                case NetRequestResult.Incompleted:
                    throw new InvalidOperationException("NetEvent was not completed!");
            }

            CachedNetFunctions.Remove(netEventName);
        }

        public async Task<NetRequestResult> TryTriggerNetEvent(string netEventName, params object[] parameters)
        {
            S.Send(new StorableValue<NetRequest>(new NetRequest(NetRequestMetadata.InvocationRequest, netEventName,
                parameters)).Bytes);

            while (!CachedNetEvents.ContainsKey(netEventName))
                await Task.Delay(10);

            NetRequestResult result = CachedNetEvents[netEventName];
            CachedNetFunctions.Remove(netEventName);

            return result;
        }

        public async Task<T> GetNetValue<T>(string netValueName)
        {
            S.Send(new StorableValue<NetRequest>(new NetRequest(NetRequestMetadata.ValueRequest, netValueName))
                .Bytes);

            while (!CachedNetValues.ContainsKey(netValueName))
                await Task.Delay(10);

            Tuple<bool, T> tuple = new Tuple<bool, T>(CachedNetValues[netValueName].Item1, (T)CachedNetValues[netValueName].Item2);
            CachedNetFunctions.Remove(netValueName);

            if (!tuple.Item1)
                throw new InvalidOperationException("NetValue does not exist!");

            return tuple.Item2;
        }

        public async Task<Tuple<bool, T>> TryGetNetValue<T>(string netValueName)
        {
            S.Send(new StorableValue<NetRequest>(new NetRequest(NetRequestMetadata.ValueRequest, netValueName))
                .Bytes);

            while (!CachedNetValues.ContainsKey(netValueName))
                await Task.Delay(10);

            Tuple<bool, T> tuple = new Tuple<bool, T>(CachedNetValues[netValueName].Item1, (T)CachedNetValues[netValueName].Item2);
            CachedNetValues.Remove(netValueName);
            return tuple;
        }

        public async Task<T> TriggerNetFunction<T>(string netFunctionName, params object[] parameters)
        {
            S.Send(new StorableValue<NetRequest>(
                new NetRequest(NetRequestMetadata.FunctionRequest, netFunctionName, parameters)).Bytes);

            while (!CachedNetFunctions.ContainsKey(netFunctionName))
                await Task.Delay(10);

            Tuple<NetRequestResult, T> tuple = new Tuple<NetRequestResult, T>(CachedNetFunctions[netFunctionName].Item1, (T)CachedNetValues[netFunctionName].Item2);
            CachedNetFunctions.Remove(netFunctionName);

            switch (tuple.Item1)
            {
                case NetRequestResult.Invalid:
                    throw new InvalidOperationException("NetFunction does not exist!");
                case NetRequestResult.Incompleted:
                    throw new InvalidOperationException("NetFunction was not completed!");
            }

            return tuple.Item2;
        }

        public async Task<Tuple<NetRequestResult, T>> TryTriggerNetFunction<T>(string netFunctionName, params object[] parameters)
        {
            S.Send(new StorableValue<NetRequest>(
                new NetRequest(NetRequestMetadata.FunctionRequest, netFunctionName, parameters)).Bytes);

            while (!CachedNetFunctions.ContainsKey(netFunctionName))
                await Task.Delay(10);

            Tuple<NetRequestResult, T> tuple = new Tuple<NetRequestResult, T>(CachedNetFunctions[netFunctionName].Item1, (T)CachedNetFunctions[netFunctionName].Item2);
            CachedNetFunctions.Remove(netFunctionName);
            return tuple;
        }
    }
}
